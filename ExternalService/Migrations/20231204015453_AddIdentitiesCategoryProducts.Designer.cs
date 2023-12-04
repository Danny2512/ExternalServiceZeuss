﻿// <auto-generated />
using System;
using ExternalService.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ExternalService.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20231204015453_AddIdentitiesCategoryProducts")]
    partial class AddIdentitiesCategoryProducts
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.25")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ExternalService.Data.Entities.Category", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("BiActive")
                        .HasColumnType("bit");

                    b.Property<string>("StrName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("StrName")
                        .IsUnique();

                    b.ToTable("tblCategory");
                });

            modelBuilder.Entity("ExternalService.Data.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<bool>("BiActive")
                        .HasColumnType("bit");

                    b.Property<Guid>("CategoryFK")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("DePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("StrImageUrl")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("StrName")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.HasKey("Id");

                    b.HasIndex("CategoryFK");

                    b.ToTable("tblProduct");
                });

            modelBuilder.Entity("ExternalService.Data.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<byte[]>("HsPassword")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("StrUserName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("StrUserName")
                        .IsUnique();

                    b.ToTable("tblUser");
                });

            modelBuilder.Entity("ExternalService.Data.Entities.Product", b =>
                {
                    b.HasOne("ExternalService.Data.Entities.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryFK")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });
#pragma warning restore 612, 618
        }
    }
}