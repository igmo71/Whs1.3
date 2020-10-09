using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Whs.Shared.Models;
using Whs.Shared.Models.Accounts;

namespace Whs.Server.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContextOptions)
            : base(dbContextOptions)
        { }

        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<WhsOrderIn> WhsOrdersIn { get; set; }
        public DbSet<WhsOrderDataIn> WhsOrdersDataIn{ get; set; }
        public DbSet<WhsOrderOut> WhsOrdersOut { get; set; }
        public DbSet<WhsOrderDataOut> WhsOrdersDataOut { get; set; }
        public DbSet<ProductIn> ProductsIn { get; set; }
        public DbSet<ProductDataIn> ProductsDataIn { get; set; }
        public DbSet<ProductOut> ProductsOut { get; set; }
        public DbSet<ProductDataOut> ProductsDataOut { get; set; }
        public DbSet<MngrOrderIn> MngrOrdersIn { get; set; }
        public DbSet<MngrOrderOut> MngrOrdersOut { get; set; }

        public DbSet<EditingCause> EditingCauses { get; set; }
        public DbSet<EditingCauseIn> EditingCausesIn { get; set; }
        public DbSet<EditingCauseOut> EditingCausesOut { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<WhsOrderIn>().HasKey(e => e.Документ_Id);
            builder.Entity<WhsOrderIn>().HasIndex(e => e.Документ_Id).HasName("IX_WhsOrdersIn_ДокументId");
            //builder.Entity<WhsOrderIn>().HasIndex(e => e.Статус).HasName("IX_WhsOrdersIn_Статус");

            //builder.Entity<WhsOrderDataIn>().HasKey(e => e.Id);
            //builder.Entity<WhsOrderDataIn>().HasIndex(e => e.Id).HasName("IX_WhsOrdersIn_Id");
            //builder.Entity<WhsOrderDataIn>().HasOne(e => e.WhsOrderIn).WithMany().HasForeignKey(e => e.Документ_Id).HasPrincipalKey(e => e.Документ_Id).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductIn>().HasKey(e => new { e.Документ_Id, e.НомерСтроки });
            builder.Entity<ProductIn>().HasIndex(e => new { e.Документ_Id, e.НомерСтроки }).HasName("IX_ProductsIn_ДокументIdНомерСтроки");
            builder.Entity<ProductIn>().HasOne(e => e.WhsOrder).WithMany(e => e.Товары).HasForeignKey(e => e.Документ_Id).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductDataIn>().HasKey(e => e.Id);
            //builder.Entity<ProductDataIn>().HasOne(e => e.Product).WithOne(e => e.Data).HasForeignKey(typeof(ProductDataIn), "Документ_Id", "НомерСтроки");
            //builder.Entity<ProductDataIn>().HasOne(e => e.EditingCause).WithMany().HasForeignKey(e => e.EditingCauseId);

            builder.Entity<MngrOrderIn>().HasKey(e => new { e.Документ_Id, e.Распоряжение_Id });
            builder.Entity<MngrOrderIn>().HasIndex(e => new { e.Документ_Id, e.Распоряжение_Id }).HasName("IX_MngOrdersIn_ДокументIdРаспоряжениеId");
            builder.Entity<MngrOrderIn>().HasOne(e => e.WhsOrder).WithMany(e => e.Распоряжения).HasForeignKey(e => e.Документ_Id).OnDelete(DeleteBehavior.Cascade);


            builder.Entity<WhsOrderOut>().HasKey(e => e.Документ_Id);
            builder.Entity<WhsOrderOut>().HasIndex(e => e.Документ_Id).HasName("IX_WhsOrdersOut_ДокументId");
            //builder.Entity<WhsOrderOut>().HasIndex(e => e.Статус).HasName("IX_WhsOrdersOut_Статус");

            //builder.Entity<WhsOrderDataOut>().HasKey(e => e.Id);
            //builder.Entity<WhsOrderDataOut>().HasIndex(e => e.Id).HasName("IX_WhsOrdersOut_Id");
            //builder.Entity<WhsOrderDataOut>().HasOne(e => e.WhsOrderOut).WithMany().HasForeignKey(e => e.Документ_Id).HasPrincipalKey(e => e.Документ_Id).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductOut>().HasKey(e => new { e.Документ_Id, e.НомерСтроки });
            builder.Entity<ProductOut>().HasIndex(e => new { e.Документ_Id, e.НомерСтроки }).HasName("IX_ProductsOut_ДокументIdНомерСтроки");
            builder.Entity<ProductOut>().HasOne(e => e.WhsOrder).WithMany(e => e.Товары).HasForeignKey(e => e.Документ_Id).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ProductDataOut>().HasKey(e => e.Id);
            //builder.Entity<ProductDataOut>().HasOne(e => e.Product).WithOne(e => e.Data).HasForeignKey(typeof(ProductDataOut), "Документ_Id", "НомерСтроки");
            //builder.Entity<ProductDataOut>().HasOne(e => e.EditingCause).WithMany().HasForeignKey(e => e.EditingCauseId);

            builder.Entity<MngrOrderOut>().HasKey(e => new { e.Документ_Id, e.Распоряжение_Id });
            builder.Entity<MngrOrderOut>().HasIndex(e => new { e.Документ_Id, e.Распоряжение_Id }).HasName("IX_MngOrdersOut_ДокументIdРаспоряжениеId");
            builder.Entity<MngrOrderOut>().HasOne(e => e.WhsOrder).WithMany(e => e.Распоряжения).HasForeignKey(e => e.Документ_Id).OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Warehouse>().HasMany(e => e.Users).WithOne(e => e.Warehouse).OnDelete(DeleteBehavior.SetNull);
        }
    }
}
