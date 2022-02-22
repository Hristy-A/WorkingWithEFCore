﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Store.Data;

namespace Store.Data.Migrations
{
    [DbContext(typeof(StoreDbContext))]
    [Migration("20220222115804_AddAccountHistory")]
    partial class AddAccountHistory
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.14")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.Property<int>("RolesId")
                        .HasColumnType("integer")
                        .HasColumnName("rolesId");

                    b.Property<int>("UsersId")
                        .HasColumnType("integer")
                        .HasColumnName("usersId");

                    b.HasKey("RolesId", "UsersId")
                        .HasName("pK_roleUser");

                    b.HasIndex("UsersId")
                        .HasDatabaseName("iX_roleUser_usersId");

                    b.ToTable("roleUser");
                });

            modelBuilder.Entity("Store.Data.Entities.AccountHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset>("DateTimeOffset")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("dateTimeOffset");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("text")
                        .HasColumnName("errorMessage");

                    b.Property<int>("EventType")
                        .HasColumnType("integer")
                        .HasColumnName("eventType");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("userId");

                    b.HasKey("Id")
                        .HasName("pK_accountHistory");

                    b.HasIndex("UserId")
                        .HasDatabaseName("iX_accountHistory_userId");

                    b.ToTable("accountHistory");
                });

            modelBuilder.Entity("Store.Data.Entities.Manufacturer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address")
                        .HasColumnType("text")
                        .HasColumnName("address");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pK_manufacturers");

                    b.ToTable("manufacturers");
                });

            modelBuilder.Entity("Store.Data.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int>("ManufacturerId")
                        .HasColumnType("integer")
                        .HasColumnName("manufacturerId");

                    b.Property<string>("Name")
                        .HasMaxLength(255)
                        .HasColumnType("character varying(255)")
                        .HasColumnName("name");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.HasKey("Id")
                        .HasName("pK_products");

                    b.HasIndex("ManufacturerId")
                        .HasDatabaseName("iX_products_manufacturerId");

                    b.ToTable("products");
                });

            modelBuilder.Entity("Store.Data.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ShortName")
                        .HasColumnType("text")
                        .HasColumnName("shortName");

                    b.HasKey("Id")
                        .HasName("pK_role");

                    b.ToTable("role");
                });

            modelBuilder.Entity("Store.Data.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdOn");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("isActive");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("login");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.HasKey("Id")
                        .HasName("pK_users");

                    b.HasIndex("Login")
                        .IsUnique()
                        .HasDatabaseName("iX_users_login");

                    b.ToTable("users");
                });

            modelBuilder.Entity("RoleUser", b =>
                {
                    b.HasOne("Store.Data.Entities.Role", null)
                        .WithMany()
                        .HasForeignKey("RolesId")
                        .HasConstraintName("fK_roleUser_role_rolesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Store.Data.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UsersId")
                        .HasConstraintName("fK_roleUser_users_usersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Store.Data.Entities.AccountHistory", b =>
                {
                    b.HasOne("Store.Data.Entities.User", "User")
                        .WithMany("AccountHistory")
                        .HasForeignKey("UserId")
                        .HasConstraintName("fK_accountHistory_users_userId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Store.Data.Entities.Product", b =>
                {
                    b.HasOne("Store.Data.Entities.Manufacturer", "Manufacturer")
                        .WithMany()
                        .HasForeignKey("ManufacturerId")
                        .HasConstraintName("fK_products_manufacturers_manufacturerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Manufacturer");
                });

            modelBuilder.Entity("Store.Data.Entities.User", b =>
                {
                    b.Navigation("AccountHistory");
                });
#pragma warning restore 612, 618
        }
    }
}
