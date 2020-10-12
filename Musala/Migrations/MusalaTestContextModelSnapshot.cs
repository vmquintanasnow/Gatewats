﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Musala.Models;

namespace Musala.Migrations
{
    [DbContext(typeof(MusalaTestContext))]
    partial class MusalaTestContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Musala.Models.Gateway", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("varchar(64)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetime");

                    b.Property<string>("Ipv4")
                        .HasColumnName("ipv4")
                        .HasColumnType("varchar(15)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<string>("Name")
                        .HasColumnName("name")
                        .HasColumnType("varchar(64)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<bool>("isDeleted")
                        .HasColumnName("is_deleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("gateway");
                });

            modelBuilder.Entity("Musala.Models.Peripheral", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetime");

                    b.Property<DateTime?>("DateCreation")
                        .HasColumnName("date_creation")
                        .HasColumnType("datetime");

                    b.Property<string>("GatewayId")
                        .IsRequired()
                        .HasColumnName("gateway_id")
                        .HasColumnType("varchar(64)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<bool?>("Status")
                        .HasColumnName("status")
                        .HasColumnType("bit");

                    b.Property<string>("Vendor")
                        .HasColumnName("vendor")
                        .HasColumnType("varchar(64)")
                        .HasAnnotation("MySql:CharSet", "utf8mb4")
                        .HasAnnotation("MySql:Collation", "utf8mb4_0900_ai_ci");

                    b.Property<bool>("isDeleted")
                        .HasColumnName("is_deleted")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("GatewayId")
                        .HasName("Gateway_Peripheral");

                    b.ToTable("peripheral");
                });

            modelBuilder.Entity("Musala.Models.Peripheral", b =>
                {
                    b.HasOne("Musala.Models.Gateway", "Gateway")
                        .WithMany("Peripherals")
                        .HasForeignKey("GatewayId")
                        .HasConstraintName("Gateway_Peripheral")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}