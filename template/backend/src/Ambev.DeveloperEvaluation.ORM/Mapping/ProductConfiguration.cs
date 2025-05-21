using Ambev.DeveloperEvaluation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ambev.DeveloperEvaluation.ORM.Mapping;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable("Products");

        builder.HasKey(u => u.Id);
        builder.Property(u => u.Id).HasColumnType("uuid").HasDefaultValueSql("gen_random_uuid()");

        builder.Property(u => u.Title).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Description).IsRequired().HasMaxLength(300);
        builder.Property(u => u.Category).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Price).IsRequired().HasPrecision(10, 2);
        builder.Property(u => u.Image).IsRequired().HasMaxLength(100);

        builder.Property(u => u.CreatedAt).IsRequired();
        builder.Property(u => u.UpdatedAt).IsRequired(false);

        builder.OwnsOne(u => u.Rating, n =>
        {
            n.Property(_ => _.Rate).HasColumnName("Rate").IsRequired().HasPrecision(10, 2);
            n.Property(_ => _.Count).HasColumnName("RateCount").IsRequired();
        });
    }
}