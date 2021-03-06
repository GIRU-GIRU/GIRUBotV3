﻿// <auto-generated />
using GIRUBotV3.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GIRUBotV3.Migrations
{
    [DbContext(typeof(Memestorage))]
    [Migration("20201128035830_initial")]
    partial class initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("GIRUBotV3.Data.MemeStoreModel", b =>
                {
                    b.Property<int>("MemeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Author")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("AuthorID")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<string>("Date")
                        .HasColumnType("TEXT");

                    b.Property<ulong>("MemeUses")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Time")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("MemeId");

                    b.ToTable("Memestore");
                });
#pragma warning restore 612, 618
        }
    }
}
