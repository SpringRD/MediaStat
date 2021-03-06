﻿// <auto-generated />
using System;
using MediaStat.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MediaStat.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210416111644_CreateInitialDB")]
    partial class CreateInitialDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MediaStat.Data.AccountInfo", b =>
                {
                    b.Property<int>("AccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("AccountType")
                        .HasColumnType("int");

                    b.Property<string>("AccountUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AllLinks")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Classification1")
                        .HasColumnType("int");

                    b.Property<int?>("Classification2")
                        .HasColumnType("int");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Followers")
                        .HasColumnType("int");

                    b.Property<int?>("Following")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Joined")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("LastChanged")
                        .HasColumnType("datetime2");

                    b.Property<string>("Link")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Location")
                        .HasColumnType("int");

                    b.Property<string>("LocationDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("Party")
                        .HasColumnType("int");

                    b.Property<string>("ProfileImageURL")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfileName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ScreenName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SpecialAccountId")
                        .HasColumnType("nvarchar(4000)");

                    b.HasKey("AccountId");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("MediaStat.Data.AccountLink", b =>
                {
                    b.Property<int>("AccountLinkId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountInfoId")
                        .HasColumnType("int");

                    b.Property<string>("LinkDescription")
                        .HasColumnType("nvarchar(4000)");

                    b.HasKey("AccountLinkId");

                    b.HasIndex("AccountInfoId");

                    b.ToTable("AccountLinks");
                });

            modelBuilder.Entity("MediaStat.Data.LoginRequest", b =>
                {
                    b.Property<int>("LoginRequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AccessToken")
                        .HasColumnType("nvarchar(4000)")
                        .HasMaxLength(4000);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(4000)")
                        .HasMaxLength(4000);

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("bit");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("LoginRequestId");

                    b.ToTable("LoginRequests");
                });

            modelBuilder.Entity("MediaStat.Data.LookupCode", b =>
                {
                    b.Property<int>("LookupCodeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LookupCodeDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LookupCodeId");

                    b.ToTable("LookupCodes");
                });

            modelBuilder.Entity("MediaStat.Data.LookupDescription", b =>
                {
                    b.Property<int>("LookupDescriptionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("LookupCodeId")
                        .HasColumnType("int");

                    b.Property<string>("LookupDescriptionDetail")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LookupDescriptionId");

                    b.HasIndex("LookupCodeId");

                    b.ToTable("LookupDescriptions");
                });

            modelBuilder.Entity("MediaStat.Data.TweetHashtagDim", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("HashtagText")
                        .HasColumnType("nvarchar(4000)")
                        .HasMaxLength(4000);

                    b.HasKey("Id");

                    b.ToTable("TweetHashtagDim");
                });

            modelBuilder.Entity("MediaStat.Data.AccountLink", b =>
                {
                    b.HasOne("MediaStat.Data.AccountInfo", "AccountInfo")
                        .WithMany("AccountLinks")
                        .HasForeignKey("AccountInfoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MediaStat.Data.LookupDescription", b =>
                {
                    b.HasOne("MediaStat.Data.LookupCode", "LokkupCode")
                        .WithMany("LookupDescriptions")
                        .HasForeignKey("LookupCodeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
