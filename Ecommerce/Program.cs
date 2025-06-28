
using Core.Identity;
using Core.Interfaces;
using Infrastrucure.Data;
using Infrastrucure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace Ecommerce
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<StoreContext>(options => {
                options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            builder.Services.AddDbContext<AppIdentityDBContext>(options => {
                options.UseSqlite(builder.Configuration.GetConnectionString("IdentityConnection"));
            });
            builder.Services.AddIdentityCore<AppUser>(opt =>
            {
              //  opt.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<AppIdentityDBContext>()
              .AddSignInManager<SignInManager<AppUser>>();
            builder.Services.AddAuthentication();
            builder.Services.AddAuthorization();

            builder.Services.AddScoped<IProductRepository,ProductRepository>();
            builder.Services.AddScoped<IBasketRepository,BasketRepository>();
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.Services.AddSingleton<IConnectionMultiplexer>(c =>
            {
                var configuration = ConfigurationOptions.Parse(builder.Configuration.GetConnectionString("Redis"), true);
                return ConnectionMultiplexer.Connect(configuration);
            });
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
