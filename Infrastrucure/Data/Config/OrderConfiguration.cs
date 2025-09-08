using Core.Entities.OrederAggregat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastrucure.Data.Config
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.OwnsOne(o => o.ShipToAddress, a => { a.WithOwner(); });
            builder.Navigation(a=>a.ShipToAddress).IsRequired();
            builder.Property(a => a.Statues)
                .HasConversion(
                o => o.ToString(), 
                o => (OrderStatues)Enum.Parse(typeof(OrderStatues), o)
                );
            builder.HasMany(o=>o.OrderItems).WithOne().OnDelete(DeleteBehavior.Cascade);    
        }
    }
}
