﻿// <auto-generated />
using System;
using ChatApplicationAuthen.Entities.DTO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ChatApplicationAuthen.Migrations
{
    [DbContext(typeof(ChatContext))]
    partial class ChatContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("ChatApplicationAuthen.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("VARCHAR(255)");

                    b.Property<string>("ContactMobile")
                        .HasColumnType("VARCHAR(50)");

                    b.Property<string>("Email")
                        .HasColumnType("VARCHAR(100)");

                    b.Property<string>("FirstName")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("LastName")
                        .HasColumnType("VARCHAR(250)");

                    b.Property<string>("Password")
                        .HasColumnType("VARCHAR(32)");

                    b.Property<string>("UserName")
                        .HasColumnType("VARCHAR(50)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
