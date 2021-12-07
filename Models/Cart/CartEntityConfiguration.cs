using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Cart
{
    public class CartEntityConfiguration : IEntityTypeConfiguration<CartEntity>
    {
        public void Configure(EntityTypeBuilder<CartEntity> builder)
        {
            builder.ToTable("Cart");
            builder.HasKey(i => i.ID);
            builder.Property(i => i.ID).ValueGeneratedOnAdd();
            builder.Property(i => i.Status).HasDefaultValue(CartStatus.Cleared);
            builder.Property(i => i.TotalPrice).IsRequired();
        }
    }
}
