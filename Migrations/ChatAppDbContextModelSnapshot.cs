﻿// <auto-generated />
using System;
using ChatAppServer.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChatAppServer.Migrations
{
    [DbContext(typeof(ChatAppDbContext))]
    partial class ChatAppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.11")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("AccountDirectChat", b =>
                {
                    b.Property<int>("DirectChatsId")
                        .HasColumnType("int");

                    b.Property<int>("ParticipantsId")
                        .HasColumnType("int");

                    b.HasKey("DirectChatsId", "ParticipantsId");

                    b.HasIndex("ParticipantsId");

                    b.ToTable("DirectChatParticipants", (string)null);
                });

            modelBuilder.Entity("AccountGroupChat", b =>
                {
                    b.Property<int>("GroupChatsId")
                        .HasColumnType("int");

                    b.Property<int>("MembersId")
                        .HasColumnType("int");

                    b.HasKey("GroupChatsId", "MembersId");

                    b.HasIndex("MembersId");

                    b.ToTable("GroupChatMembers", (string)null);
                });

            modelBuilder.Entity("ChatAppServer.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("LastLogin")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Accounts");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            FirstName = "adam",
                            LastName = "baruch",
                            Password = "$2a$11$QFSF5h8ZS0FEr2.w2gLNxuAN/9fwoyYXSShJfi4DOsMusjBTKKKvW",
                            Username = "tsotso123"
                        },
                        new
                        {
                            Id = 2,
                            FirstName = "adam2",
                            LastName = "baruch",
                            Password = "$2a$11$QFSF5h8ZS0FEr2.w2gLNxuAN/9fwoyYXSShJfi4DOsMusjBTKKKvW",
                            Username = "tsotso"
                        },
                        new
                        {
                            Id = 3,
                            FirstName = "adam3",
                            LastName = "baruch",
                            Password = "$2a$11$QFSF5h8ZS0FEr2.w2gLNxuAN/9fwoyYXSShJfi4DOsMusjBTKKKvW",
                            Username = "tsotso9"
                        },
                        new
                        {
                            Id = 4,
                            FirstName = "adam4",
                            LastName = "baruch",
                            Password = "$2a$11$QFSF5h8ZS0FEr2.w2gLNxuAN/9fwoyYXSShJfi4DOsMusjBTKKKvW",
                            Username = "tso"
                        });
                });

            modelBuilder.Entity("ChatAppServer.Models.DirectChat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("LastMessageSentAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("DirectChats");
                });

            modelBuilder.Entity("ChatAppServer.Models.GroupChat", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<int?>("ManagerId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ManagerId");

                    b.ToTable("GroupChats");
                });

            modelBuilder.Entity("ChatAppServer.Models.MessageReceipt", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("MessageId")
                        .HasColumnType("int");

                    b.Property<bool>("Received")
                        .HasColumnType("bit");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("MessageId");

                    b.ToTable("Receipts");
                });

            modelBuilder.Entity("ChatAppServer.Models.Messege", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DirectChatId")
                        .HasColumnType("int");

                    b.Property<int?>("GroupChatId")
                        .HasColumnType("int");

                    b.Property<bool>("Received")
                        .HasColumnType("bit");

                    b.Property<string>("RecipientUsername")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Seen")
                        .HasColumnType("bit");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("SentAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("UnifiedId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("DirectChatId");

                    b.HasIndex("GroupChatId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messeges");
                });

            modelBuilder.Entity("AccountDirectChat", b =>
                {
                    b.HasOne("ChatAppServer.Models.DirectChat", null)
                        .WithMany()
                        .HasForeignKey("DirectChatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatAppServer.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("ParticipantsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("AccountGroupChat", b =>
                {
                    b.HasOne("ChatAppServer.Models.GroupChat", null)
                        .WithMany()
                        .HasForeignKey("GroupChatsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatAppServer.Models.Account", null)
                        .WithMany()
                        .HasForeignKey("MembersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ChatAppServer.Models.GroupChat", b =>
                {
                    b.HasOne("ChatAppServer.Models.Account", "Manager")
                        .WithMany()
                        .HasForeignKey("ManagerId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("ChatAppServer.Models.MessageReceipt", b =>
                {
                    b.HasOne("ChatAppServer.Models.Messege", "Message")
                        .WithMany("Receipts")
                        .HasForeignKey("MessageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Message");
                });

            modelBuilder.Entity("ChatAppServer.Models.Messege", b =>
                {
                    b.HasOne("ChatAppServer.Models.DirectChat", "DirectChat")
                        .WithMany("Messages")
                        .HasForeignKey("DirectChatId");

                    b.HasOne("ChatAppServer.Models.GroupChat", "GroupChat")
                        .WithMany("Messages")
                        .HasForeignKey("GroupChatId");

                    b.HasOne("ChatAppServer.Models.Account", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("DirectChat");

                    b.Navigation("GroupChat");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("ChatAppServer.Models.DirectChat", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("ChatAppServer.Models.GroupChat", b =>
                {
                    b.Navigation("Messages");
                });

            modelBuilder.Entity("ChatAppServer.Models.Messege", b =>
                {
                    b.Navigation("Receipts");
                });
#pragma warning restore 612, 618
        }
    }
}
