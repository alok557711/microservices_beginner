﻿// <auto-generated />
using Cartservice.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Cartservice.Migrations
{
    [DbContext(typeof(CartserviceContext))]
    [Migration("20200510134124_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.3-rtm-10026")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Cartservice.Models.Cart", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Color");

                    b.Property<decimal>("Price");

                    b.Property<string>("ProductId");

                    b.Property<string>("ProductName");

                    b.Property<int>("Quantity");

                    b.HasKey("Id");

                    b.ToTable("Cart");
                });
#pragma warning restore 612, 618
        }
    }
}
