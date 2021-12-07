using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.CartItem
{
    public class CartItemEntityConfiguration : IEntityTypeConfiguration<CartItemEntity>
    {
        public void Configure(EntityTypeBuilder<CartItemEntity> builder)
        {
            builder.ToTable("CatrItem");
            builder.HasKey(i => i.ID);
            builder.Property(i => i.ID).ValueGeneratedOnAdd();
            builder.Property(i => i.Amount).IsRequired();
            builder.Property(i => i.TotalPrice).IsRequired();
            builder.Property(i => i.Date).IsRequired();
        }
    }
}
