using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
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
        public DbSet<MngrOrderIn> MngrOrdersIn { get; set; }
        public DbSet<MngrOrderOut> MngrOrdersOut { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WhsOrderIn>().HasKey(e => e.Документ_Id);
            builder.Entity<WhsOrderIn>().HasIndex(e => e.Документ_Id).HasName("IX_WhsOrdersIn_ДокументId");

            builder.Entity<WhsOrderOut>().HasKey(e => e.Документ_Id);
            builder.Entity<WhsOrderOut>().HasIndex(e => e.Документ_Id).HasName("IX_WhsOrdersOut_ДокументId");

            builder.Entity<ProductIn>().HasKey(e => new { e.Документ_Id, e.НомерСтроки });
            builder.Entity<ProductIn>().HasIndex(e => new { e.Документ_Id, e.НомерСтроки }).HasName("IX_ProductsIn_ДокументIdНомерСтроки");
            builder.Entity<ProductIn>().HasOne(e => e.WhsOrder).WithMany(e => e.Товары).HasForeignKey(e => e.Документ_Id).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductOut>().HasKey(e => new { e.Документ_Id, e.НомерСтроки });
            builder.Entity<ProductOut>().HasIndex(e => new { e.Документ_Id, e.НомерСтроки }).HasName("IX_ProductsOut_ДокументIdНомерСтроки");
            builder.Entity<ProductOut>().HasOne(e => e.WhsOrder).WithMany(e => e.Товары).HasForeignKey(e => e.Документ_Id).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MngrOrderIn>().HasKey(e => new { e.Документ_Id, e.Распоряжение_Id });
            builder.Entity<MngrOrderIn>().HasIndex(e => new { e.Документ_Id, e.Распоряжение_Id }).HasName("IX_MngOrdersIn_ДокументIdРаспоряжениеId");
            builder.Entity<MngrOrderIn>().HasOne(e => e.WhsOrder).WithMany(e => e.Распоряжения).HasForeignKey(e => e.Документ_Id).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<MngrOrderOut>().HasKey(e => new { e.Документ_Id, e.Распоряжение_Id });
            builder.Entity<MngrOrderOut>().HasIndex(e => new { e.Документ_Id, e.Распоряжение_Id }).HasName("IX_MngOrdersOut_ДокументIdРаспоряжениеId");
            builder.Entity<MngrOrderOut>().HasOne(e => e.WhsOrder).WithMany(e => e.Распоряжения).HasForeignKey(e => e.Документ_Id).OnDelete(DeleteBehavior.Cascade);
        }
    }
}
