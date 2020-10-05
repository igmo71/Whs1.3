using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Whs.Shared.Models;

namespace Whs.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
            : base(dbContextOptions)
        { }

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WhsOrderIn> WhsOrdersIn { get; set; }
        public DbSet<WhsOrderOut> WhsOrdersOut { get; set; }
        public DbSet<ProductIn> ProductsIn { get; set; }
        public DbSet<ProductOut> ProductsOut { get; set; }
        public DbSet<MngOrderIn> MngOrdersIn { get; set; }
        public DbSet<MngOrderOut> MngOrdersOut { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WhsOrderIn>().HasKey(e => e.Документ_Id);
            builder.Entity<WhsOrderOut>().HasKey(e => e.Документ_Id);

            builder.Entity<ProductIn>().HasKey(e => new { e.Документ_Id, e.НомерСтроки });
            builder.Entity<ProductIn>().HasOne(e => e.WhsOrder).WithMany(e => e.Товары).HasForeignKey(e => e.Документ_Id).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<ProductOut>().HasKey(e => new { e.Документ_Id, e.НомерСтроки });
            builder.Entity<ProductOut>().HasOne(e => e.WhsOrder).WithMany(e => e.Товары).HasForeignKey(e => e.Документ_Id).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MngOrderIn>().HasKey(e => new { e.Документ_Id, e.Распоряжение_Id });
            builder.Entity<MngOrderIn>().HasOne(e => e.WhsOrder).WithMany(e => e.Распоряжения).HasForeignKey(e => e.Документ_Id).OnDelete(DeleteBehavior.Cascade);
            builder.Entity<MngOrderOut>().HasKey(e => new { e.Документ_Id, e.Распоряжение_Id });
            builder.Entity<MngOrderOut>().HasOne(e => e.WhsOrder).WithMany(e => e.Распоряжения).HasForeignKey(e => e.Документ_Id).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
