﻿// <auto-generated />
using System;
using GeekShopping.Email.Model.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GeekShopping.EMail.Migrations
{
    [DbContext(typeof(MySqlContext))]
    partial class MySqlContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("GeekShopping.Email.Model.EmailLog", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("email");

                    b.Property<string>("Log")
                        .IsRequired()
                        .HasColumnType("longtext")
                        .HasColumnName("log");

                    b.Property<DateTime>("SentDate")
                        .HasColumnType("datetime(6)")
                        .HasColumnName("sent_date");

                    b.HasKey("Id");

                    b.ToTable("email_logs");
                });
#pragma warning restore 612, 618
        }
    }
}