using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ProductImage
{
    public class ProductImageEntityConfiguration : IEntityTypeConfiguration<ProductImageEntity>
    {
        public void Configure(EntityTypeBuilder<ProductImageEntity> builder)
        {
            builder.ToTable("ProductImage");
            builder.HasKey(i => i.ID);
            builder.Property(i => i.ID).ValueGeneratedOnAdd();
            builder.Property(i => i.ImageURL).IsRequired();
        }
    }
}
