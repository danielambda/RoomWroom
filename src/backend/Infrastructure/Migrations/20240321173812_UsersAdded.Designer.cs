﻿// <auto-generated />
using System;
using Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Infrastructure.Migrations
{
    [DbContext(typeof(RoomWroomDbContext))]
    [Migration("20240321173812_UsersAdded")]
    partial class UsersAdded
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Domain.ReceiptAggregate.Receipt", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Qr")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Receipts", (string)null);
                });

            modelBuilder.Entity("Domain.ReceiptAggregate.ValueObjects.ShopItemAssociation", b =>
                {
                    b.Property<string>("OriginalName")
                        .HasColumnType("text");

                    b.Property<Guid>("ShopItemId")
                        .HasColumnType("uuid");

                    b.HasKey("OriginalName");

                    b.ToTable("ShopItemAssociation", (string)null);
                });

            modelBuilder.Entity("Domain.RoomAggregate.Room", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<decimal>("BudgetLowerBound")
                        .HasColumnType("numeric");

                    b.Property<bool>("MoneyRoundingRequired")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.ToTable("Rooms", (string)null);
                });

            modelBuilder.Entity("Domain.UserAggregate.User", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.Property<Guid?>("RoomId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Domain.ReceiptAggregate.Receipt", b =>
                {
                    b.OwnsMany("Domain.ReceiptAggregate.ValueObjects.ReceiptItem", "Items", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("ReceiptId")
                                .HasColumnType("uuid");

                            b1.Property<Guid?>("AssociatedShopItemId")
                                .HasColumnType("uuid");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("character varying(100)");

                            b1.Property<decimal>("Quantity")
                                .HasColumnType("numeric");

                            b1.HasKey("Id", "ReceiptId");

                            b1.HasIndex("ReceiptId");

                            b1.ToTable("ReceiptItems", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("ReceiptId");

                            b1.OwnsOne("Domain.Common.ValueObjects.Money", "Price", b2 =>
                                {
                                    b2.Property<int>("ReceiptItemId")
                                        .HasColumnType("integer");

                                    b2.Property<Guid>("ReceiptItemReceiptId")
                                        .HasColumnType("uuid");

                                    b2.Property<decimal>("Amount")
                                        .HasColumnType("numeric")
                                        .HasColumnName("PriceAmount");

                                    b2.Property<int>("Currency")
                                        .ValueGeneratedOnUpdateSometimes()
                                        .HasColumnType("integer")
                                        .HasColumnName("Currency");

                                    b2.HasKey("ReceiptItemId", "ReceiptItemReceiptId");

                                    b2.ToTable("ReceiptItems");

                                    b2.WithOwner()
                                        .HasForeignKey("ReceiptItemId", "ReceiptItemReceiptId");
                                });

                            b1.OwnsOne("Domain.Common.ValueObjects.Money", "Sum", b2 =>
                                {
                                    b2.Property<int>("ReceiptItemId")
                                        .HasColumnType("integer");

                                    b2.Property<Guid>("ReceiptItemReceiptId")
                                        .HasColumnType("uuid");

                                    b2.Property<decimal>("Amount")
                                        .HasColumnType("numeric")
                                        .HasColumnName("SumAmount");

                                    b2.Property<int>("Currency")
                                        .ValueGeneratedOnUpdateSometimes()
                                        .HasColumnType("integer")
                                        .HasColumnName("Currency");

                                    b2.HasKey("ReceiptItemId", "ReceiptItemReceiptId");

                                    b2.ToTable("ReceiptItems");

                                    b2.WithOwner()
                                        .HasForeignKey("ReceiptItemId", "ReceiptItemReceiptId");
                                });

                            b1.Navigation("Price")
                                .IsRequired();

                            b1.Navigation("Sum")
                                .IsRequired();
                        });

                    b.Navigation("Items");
                });

            modelBuilder.Entity("Domain.RoomAggregate.Room", b =>
                {
                    b.OwnsOne("Domain.Common.ValueObjects.Money", "Budget", b1 =>
                        {
                            b1.Property<Guid>("RoomId")
                                .HasColumnType("uuid");

                            b1.Property<decimal>("Amount")
                                .HasColumnType("numeric")
                                .HasColumnName("BudgetAmount");

                            b1.Property<int>("Currency")
                                .HasColumnType("integer")
                                .HasColumnName("BudgetCurrency");

                            b1.HasKey("RoomId");

                            b1.ToTable("Rooms");

                            b1.WithOwner()
                                .HasForeignKey("RoomId");
                        });

                    b.OwnsMany("Domain.RoomAggregate.ValueObjects.OwnedShopItem", "OwnedShopItems", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("RoomId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("ShopItemId")
                                .HasColumnType("uuid")
                                .HasColumnName("ShopItemId");

                            b1.HasKey("Id");

                            b1.HasIndex("RoomId");

                            b1.ToTable("RoomsOwnedShopItems", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RoomId");

                            b1.OwnsOne("Domain.Common.ValueObjects.Money", "Price", b2 =>
                                {
                                    b2.Property<int>("OwnedShopItemId")
                                        .HasColumnType("integer");

                                    b2.HasKey("OwnedShopItemId");

                                    b2.ToTable("RoomsOwnedShopItems");

                                    b2.WithOwner()
                                        .HasForeignKey("OwnedShopItemId");
                                });

                            b1.Navigation("Price")
                                .IsRequired();
                        });

                    b.OwnsMany("Domain.UserAggregate.ValueObjects.UserId", "UserIds", b1 =>
                        {
                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("RoomId")
                                .HasColumnType("uuid");

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid")
                                .HasColumnName("UserId");

                            b1.HasKey("Id", "RoomId");

                            b1.HasIndex("RoomId");

                            b1.ToTable("RoomUserIds", (string)null);

                            b1.WithOwner()
                                .HasForeignKey("RoomId");
                        });

                    b.Navigation("Budget")
                        .IsRequired();

                    b.Navigation("OwnedShopItems");

                    b.Navigation("UserIds");
                });

            modelBuilder.Entity("Domain.UserAggregate.User", b =>
                {
                    b.OwnsMany("Domain.ReceiptAggregate.ValueObjects.ReceiptId", "ScannedReceiptsIds", b1 =>
                        {
                            b1.Property<Guid>("UserId")
                                .HasColumnType("uuid");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("integer");

                            NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b1.Property<int>("Id"));

                            b1.Property<Guid>("Value")
                                .HasColumnType("uuid");

                            b1.HasKey("UserId", "Id");

                            b1.ToTable("ReceiptId");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("ScannedReceiptsIds");
                });
#pragma warning restore 612, 618
        }
    }
}
