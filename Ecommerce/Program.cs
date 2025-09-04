
using Core.Identity;
using Core.Interfaces;
using Infrastrucure.Data;
using Infrastrucure.Identity;
using Infrastrucure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;
using System.Text;

namespace Ecommerce
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

                // ?  ⁄—Ì› ‰Ê⁄ «· Êﬂ‰: Bearer JWT
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "√œŒ· «· Êﬂ‰ ›Ì «·›Ê—„«  œÂ: Bearer {token}",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey, // ? Œ·ÌÂ« ApiKey „‘ Http!
                    Scheme = "Bearer"
                });

                // ?  ›⁄Ì· ≈—”«· «· Êﬂ‰ „⁄ ﬂ· Request
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            In = ParameterLocation.Header
                        },
                        new string[] {}
                    }
                });
            });


            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            
            
            //DataBase
            builder.Services.AddDbContext<StoreContext>(options => {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<AppIdentityDBContext>(options => {
                options.UseSqlite(builder.Configuration.GetConnectionString("IdentityConnection"));
            });

            //idetity
            builder.Services.AddIdentityCore<AppUser>(opt =>
            {
              //  opt.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppIdentityDBContext>()
              .AddSignInManager<SignInManager<AppUser>>();

            //JWT Authentication
            var jwt =builder.Configuration.GetSection("Token");
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options=>
                options.TokenValidationParameters=new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwt["Issuer"],
                    ValidAudience = jwt["Audience"],
                    IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]))
                });
            builder.Services.AddAuthorization();

            //registeration
            builder.Services.AddScoped<IProductRepository,ProductRepository>();
            builder.Services.AddScoped<IBasketRepository,BasketRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //Token Service
            builder.Services.AddScoped<ITokenService, TokenService>();

            // AutoMapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            //Redis Cache
            builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });

            //Cors
            // ⁄‘«‰  ”„Õ ·Õ«ÃÂ ÊÕœÂ »”  «ﬂ”” «· API
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader()
                          .AllowAnyMethod()
                          .WithOrigins("https://localhost:4200")  //  «··Ì‰ﬂ œ« «··Ï «·«‰ÃÊ·«— »Ì—‰ ⁄·ÌÂ 
                          .AllowCredentials();
                });
            });
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseStatusCodePagesWithReExecute("/errors/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");

            app.UseAuthorization();

            app.MapControllers();


            //  ⁄‘«‰ «Õÿ œ« « »«ÌœÏ
            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<StoreContext>();
            var IdentityContext = services.GetRequiredService<AppIdentityDBContext>();
            var UserManager = services.GetRequiredService<UserManager<AppUser>>();
            var logger = services.GetRequiredService<ILogger<Program>>();
            try
            {
                await context.Database.MigrateAsync();
                await IdentityContext.Database.MigrateAsync();
                await StoreContextSeed.SeedAsync(context);
                await AppIdentityDbContextSeed.SeedUserAsync(UserManager);
            }
            catch(Exception ex)
            {

                logger.LogError(ex, "Error occared while migrating process");
            }
            app.Run();
        }
    }
}
