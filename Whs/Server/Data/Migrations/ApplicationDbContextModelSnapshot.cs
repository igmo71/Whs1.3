﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Whs.Server.Data;

namespace Whs.Server.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Whs.Shared.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Barcode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FullName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("WarehouseId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.HasIndex("WarehouseId");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Whs.Shared.Models.MngrOrderIn", b =>
                {
                    b.Property<string>("Документ_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Распоряжение_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Документ_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Распоряжение_Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Документ_Id", "Распоряжение_Id");

                    b.HasIndex("Документ_Id", "Распоряжение_Id")
                        .HasName("IX_MngOrdersIn_ДокументIdРаспоряжениеId");

                    b.ToTable("MngrOrdersIn");
                });

            modelBuilder.Entity("Whs.Shared.Models.MngrOrderOut", b =>
                {
                    b.Property<string>("Документ_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Распоряжение_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Документ_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Распоряжение_Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Документ_Id", "Распоряжение_Id");

                    b.HasIndex("Документ_Id", "Распоряжение_Id")
                        .HasName("IX_MngOrdersOut_ДокументIdРаспоряжениеId");

                    b.ToTable("MngrOrdersOut");
                });

            modelBuilder.Entity("Whs.Shared.Models.ProductIn", b =>
                {
                    b.Property<string>("Документ_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("НомерСтроки")
                        .HasColumnType("int");

                    b.Property<string>("Артикул")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Вес")
                        .HasColumnType("real");

                    b.Property<string>("Документ_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("КоличествоПлан")
                        .HasColumnType("real");

                    b.Property<float>("КоличествоФакт")
                        .HasColumnType("real");

                    b.Property<string>("Номенклатура_Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Номенклатура_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Упаковка_Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Упаковка_Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Документ_Id", "НомерСтроки");

                    b.HasIndex("Документ_Id", "НомерСтроки")
                        .HasName("IX_ProductsIn_ДокументIdНомерСтроки");

                    b.ToTable("ProductsIn");
                });

            modelBuilder.Entity("Whs.Shared.Models.ProductOut", b =>
                {
                    b.Property<string>("Документ_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("НомерСтроки")
                        .HasColumnType("int");

                    b.Property<string>("Артикул")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("Вес")
                        .HasColumnType("real");

                    b.Property<string>("Документ_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<float>("КоличествоПлан")
                        .HasColumnType("real");

                    b.Property<float>("КоличествоФакт")
                        .HasColumnType("real");

                    b.Property<string>("Номенклатура_Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Номенклатура_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Упаковка_Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Упаковка_Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Документ_Id", "НомерСтроки");

                    b.HasIndex("Документ_Id", "НомерСтроки")
                        .HasName("IX_ProductsOut_ДокументIdНомерСтроки");

                    b.ToTable("ProductsOut");
                });

            modelBuilder.Entity("Whs.Shared.Models.Warehouse", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Warehouses");
                });

            modelBuilder.Entity("Whs.Shared.Models.WhsOrderDataIn", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NewStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StatusSwitchDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("WhsOrderInId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("WhsOrderInId");

                    b.ToTable("WhsOrdersDataIn");
                });

            modelBuilder.Entity("Whs.Shared.Models.WhsOrderDataOut", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ApplicationUserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("NewStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OldStatus")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("StatusSwitchDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("WhsOrderOutId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ApplicationUserId");

                    b.HasIndex("WhsOrderOutId");

                    b.ToTable("WhsOrdersDataOut");
                });

            modelBuilder.Entity("Whs.Shared.Models.WhsOrderIn", b =>
                {
                    b.Property<string>("Документ_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("Вес")
                        .HasColumnType("real");

                    b.Property<int>("ВесовойКоэффициент")
                        .HasColumnType("int");

                    b.Property<DateTime>("Дата")
                        .HasColumnType("datetime2");

                    b.Property<string>("Документ_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("КоличествоСтрок")
                        .HasColumnType("int");

                    b.Property<string>("Комментарий")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Номер")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ответственный_Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ответственный_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ОтправительПолучатель_Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ОтправительПолучатель_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Проведен")
                        .HasColumnType("bit");

                    b.Property<string>("Склад_Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Склад_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("СрокВыполнения")
                        .HasColumnType("datetime2");

                    b.Property<string>("Статус")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ТипОчереди")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ЭтоПеремещение")
                        .HasColumnType("bit");

                    b.HasKey("Документ_Id");

                    b.HasIndex("Документ_Id")
                        .HasName("IX_WhsOrdersIn_ДокументId");

                    b.ToTable("WhsOrdersIn");
                });

            modelBuilder.Entity("Whs.Shared.Models.WhsOrderOut", b =>
                {
                    b.Property<string>("Документ_Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<float>("Вес")
                        .HasColumnType("real");

                    b.Property<int>("ВесовойКоэффициент")
                        .HasColumnType("int");

                    b.Property<DateTime>("Дата")
                        .HasColumnType("datetime2");

                    b.Property<string>("Документ_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("КоличествоСтрок")
                        .HasColumnType("int");

                    b.Property<string>("Комментарий")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("НаправлениеДоставки_Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("НаправлениеДоставки_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Номер")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("НомерОчереди")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ответственный_Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Ответственный_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ОтправительПолучатель_Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ОтправительПолучатель_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Проведен")
                        .HasColumnType("bit");

                    b.Property<string>("Склад_Id")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Склад_Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("СрокВыполнения")
                        .HasColumnType("datetime2");

                    b.Property<string>("Статус")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ТипОчереди")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ЭтоПеремещение")
                        .HasColumnType("bit");

                    b.HasKey("Документ_Id");

                    b.HasIndex("Документ_Id")
                        .HasName("IX_WhsOrdersOut_ДокументId");

                    b.ToTable("WhsOrdersOut");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Whs.Shared.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Whs.Shared.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Whs.Shared.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Whs.Shared.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Whs.Shared.Models.ApplicationUser", b =>
                {
                    b.HasOne("Whs.Shared.Models.Warehouse", "Warehouse")
                        .WithMany("Users")
                        .HasForeignKey("WarehouseId");
                });

            modelBuilder.Entity("Whs.Shared.Models.MngrOrderIn", b =>
                {
                    b.HasOne("Whs.Shared.Models.WhsOrderIn", "WhsOrder")
                        .WithMany("Распоряжения")
                        .HasForeignKey("Документ_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Whs.Shared.Models.MngrOrderOut", b =>
                {
                    b.HasOne("Whs.Shared.Models.WhsOrderOut", "WhsOrder")
                        .WithMany("Распоряжения")
                        .HasForeignKey("Документ_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Whs.Shared.Models.ProductIn", b =>
                {
                    b.HasOne("Whs.Shared.Models.WhsOrderIn", "WhsOrder")
                        .WithMany("Товары")
                        .HasForeignKey("Документ_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Whs.Shared.Models.ProductOut", b =>
                {
                    b.HasOne("Whs.Shared.Models.WhsOrderOut", "WhsOrder")
                        .WithMany("Товары")
                        .HasForeignKey("Документ_Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Whs.Shared.Models.WhsOrderDataIn", b =>
                {
                    b.HasOne("Whs.Shared.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("Whs.Shared.Models.WhsOrderIn", "WhsOrderIn")
                        .WithMany("Data")
                        .HasForeignKey("WhsOrderInId");
                });

            modelBuilder.Entity("Whs.Shared.Models.WhsOrderDataOut", b =>
                {
                    b.HasOne("Whs.Shared.Models.ApplicationUser", "ApplicationUser")
                        .WithMany()
                        .HasForeignKey("ApplicationUserId");

                    b.HasOne("Whs.Shared.Models.WhsOrderOut", "WhsOrderOut")
                        .WithMany("Data")
                        .HasForeignKey("WhsOrderOutId");
                });
#pragma warning restore 612, 618
        }
    }
}
