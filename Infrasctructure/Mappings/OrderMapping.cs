using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities;

namespace Infrasctructure.Mappings
{
    public class OrderMapping : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(u => u.Id);
            builder.Property(u => u.ExternalId);
            builder.Property(u => u.Requestor).IsRequired();
            builder.Property(u => u.Total).IsRequired();
            builder.Property(u => u.DateCreated).IsRequired();
            builder.Property(u => u.Status).IsRequired();

            builder.HasIndex(u => u.Id)
                .IsUnique()
                .HasDatabaseName("IX_Order_Id");

            builder.HasIndex(u => u.ExternalId)
                .IsUnique()
                .HasDatabaseName("IX_Order_ExternalId");
        }
    }
}
