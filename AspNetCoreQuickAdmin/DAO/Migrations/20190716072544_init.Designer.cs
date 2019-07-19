﻿// <auto-generated />
using System;
using DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DAO.Migrations
{
    [DbContext(typeof(QuickDbContext))]
    [Migration("20190716072544_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Model.Entities.AuditLog", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<string>("ActionName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("CreateUserId")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("DeleteTime");

                    b.Property<string>("DeleteUserId")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<int>("Duration");

                    b.Property<string>("ExceptionContent")
                        .IsRequired()
                        .HasMaxLength(4000);

                    b.Property<string>("Ip")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastModifyTime");

                    b.Property<string>("LastModifyUserId")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("RequestParam")
                        .IsRequired()
                        .HasMaxLength(4000);

                    b.Property<string>("RequestUrl")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.Property<string>("ResponseParam")
                        .IsRequired()
                        .HasMaxLength(4000);

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<int>("Type");

                    b.Property<string>("UserAgent")
                        .IsRequired()
                        .HasMaxLength(1000);

                    b.HasKey("Id");

                    b.ToTable("AuditLog");
                });

            modelBuilder.Entity("Model.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(36);

                    b.Property<DateTime>("CreateTime");

                    b.Property<string>("CreateUserId")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<DateTime>("DeleteTime");

                    b.Property<string>("DeleteUserId")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("EnUserName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<bool>("IsDeleted");

                    b.Property<DateTime>("LastModifyTime");

                    b.Property<string>("LastModifyUserId")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(100);

                    b.HasKey("Id");

                    b.ToTable("User");
                });
#pragma warning restore 612, 618
        }
    }
}