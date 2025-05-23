using Domain.Entities;
using Infrasctructure.Mappings;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infrasctructure.Context
{
    public class TesteAmbevContext : DbContext
    {
        public TesteAmbevContext(DbContextOptions<TesteAmbevContext> options) : base(options)
        {
        }

        public DbSet<Order> Orders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Order>(new OrderMapping().Configure);
        }
    }
}
