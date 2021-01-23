﻿// <auto-generated />
using System;
using BlogManagement.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace BlogManagement.Migrations
{
    [DbContext(typeof(BlogDbContext))]
    partial class BlogDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityByDefaultColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.2");

            modelBuilder.Entity("BlogManagement.Models.Blog", b =>
                {
                    b.Property<int>("BlogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("BlogId");

                    b.ToTable("Blogs");
                });

            modelBuilder.Entity("BlogManagement.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<int>("BlogId")
                        .HasColumnType("integer");

                    b.Property<string>("Content")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("CreatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("PostId");

                    b.HasIndex("BlogId");

                    b.HasIndex("UserId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("BlogManagement.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .UseIdentityByDefaultColumn();

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BlogManagement.Models.Post", b =>
                {
                    b.HasOne("BlogManagement.Models.Blog", "Blog")
                        .WithMany("BlogPosts")
                        .HasForeignKey("BlogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("BlogManagement.Models.User", "User")
                        .WithMany("Posts")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Blog");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BlogManagement.Models.Blog", b =>
                {
                    b.Navigation("BlogPosts");
                });

            modelBuilder.Entity("BlogManagement.Models.User", b =>
                {
                    b.Navigation("Posts");
                });
#pragma warning restore 612, 618
        }
    }
}
