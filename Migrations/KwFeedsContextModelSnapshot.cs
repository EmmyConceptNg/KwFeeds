﻿// <auto-generated />
using System;
using KwFeeds.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DancingGoat.Migrations
{
    [DbContext(typeof(KwFeedsContext))]
    partial class KwFeedsContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("CMS.ContentEngine.ContentItemFields", b =>
                {
                    b.Property<int>("ContentItemCommonDataContentLanguageID")
                        .HasColumnType("int");

                    b.Property<int>("ContentItemCommonDataVersionStatus")
                        .HasColumnType("int");

                    b.Property<int>("ContentItemContentTypeID")
                        .HasColumnType("int");

                    b.Property<Guid>("ContentItemGUID")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ContentItemID")
                        .HasColumnType("int");

                    b.Property<bool>("ContentItemIsSecured")
                        .HasColumnType("bit");

                    b.Property<string>("ContentItemName")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("ContentItemFields");
                });

            modelBuilder.Entity("CMS.MediaLibrary.AssetDimensions", b =>
                {
                    b.Property<int>("Height")
                        .HasColumnType("int");

                    b.Property<int>("Width")
                        .HasColumnType("int");

                    b.ToTable("AssetDimensions");
                });

            modelBuilder.Entity("CMS.MediaLibrary.AssetRelatedItem", b =>
                {
                    b.Property<Guid>("Identifier")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.ToTable("AssetRelatedItem");
                });

            modelBuilder.Entity("KwFeeds.About", b =>
                {
                    b.Property<string>("button_text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("tag")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("Abouts");
                });

            modelBuilder.Entity("KwFeeds.HomePageBanner", b =>
                {
                    b.Property<string>("button_text")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("tag")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("title")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("HomePageBanners");
                });

            modelBuilder.Entity("KwFeeds.Models.SiteSetting", b =>
                {
                    b.Property<string>("HomePageTitle")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("SiteSettings");
                });

            modelBuilder.Entity("KwFeeds.SingleProduct", b =>
                {
                    b.Property<bool>("IsProductOfTheMonth")
                        .HasColumnType("bit");

                    b.Property<string>("feeding_guidelines")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("health_benefits")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ingredients")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("nutritional_information")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("storage_instructions")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("usage_tips")
                        .HasColumnType("nvarchar(max)");

                    b.ToTable("SingleProducts");
                });
#pragma warning restore 612, 618
        }
    }
}
