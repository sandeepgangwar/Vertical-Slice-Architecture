﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VerticalSliceArchitecture.Infrastructure.Data;

namespace VerticalSliceArchitecture.Infrastructure.Migrations
{
    [DbContext(typeof(AppDb))]
    partial class AppDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("VerticalSliceArchitecture.Core.Domain.Games.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<DateTime?>("ReleaseDate");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<Guid>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("VerticalSliceArchitecture.Core.Domain.Identity.Session", b =>
                {
                    b.Property<Guid>("UserId");

                    b.Property<string>("DeviceId")
                        .HasMaxLength(64);

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("DeviceName")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<DateTime>("ExpiresAt");

                    b.Property<Guid>("Id");

                    b.Property<int>("NumberOfRefreshes");

                    b.Property<int>("Platform");

                    b.Property<string>("RefreshToken")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<DateTime?>("RefreshedAt");

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("UserId", "DeviceId");

                    b.ToTable("Sessions");
                });

            modelBuilder.Entity("VerticalSliceArchitecture.Core.Domain.Identity.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255);

                    b.Property<bool>("IsDeactivated");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<string>("PasswordSalt")
                        .IsRequired()
                        .HasMaxLength(64);

                    b.Property<DateTime>("UpdatedAt");

                    b.Property<int>("UserRole");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("Login")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("VerticalSliceArchitecture.Core.Domain.Games.Game", b =>
                {
                    b.HasOne("VerticalSliceArchitecture.Core.Domain.Identity.User", "User")
                        .WithMany("Games")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("VerticalSliceArchitecture.Core.Domain.Identity.Session", b =>
                {
                    b.HasOne("VerticalSliceArchitecture.Core.Domain.Identity.User", "User")
                        .WithMany("Sessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
