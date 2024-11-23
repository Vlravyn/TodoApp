﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoApp.Core.EntityFramework;

#nullable disable

namespace TodoApp.Core.Migrations
{
    [DbContext(typeof(UserTasksDbContext))]
    [Migration("20241001125706_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.8");

            modelBuilder.Entity("TodoApp.Core.DataModels.UserTask", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAtUtc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Description")
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("DueDateUtc")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCompleted")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsImportant")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasDefaultValue(false);

                    b.Property<DateTime?>("RemindUserUtc")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("UserTasks");
                });

            modelBuilder.Entity("TodoApp.Core.DataModels.UserTask", b =>
                {
                    b.OwnsMany("TodoApp.Core.DataModels.Step", "Steps", b1 =>
                        {
                            b1.Property<Guid>("UserTaskId")
                                .HasColumnType("TEXT");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("TEXT");

                            b1.Property<bool>("IsCompleted")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("INTEGER")
                                .HasDefaultValue(false);

                            b1.Property<string>("Title")
                                .IsRequired()
                                .HasColumnType("TEXT");

                            b1.HasKey("UserTaskId", "Id");

                            b1.ToTable("Step");

                            b1.WithOwner()
                                .HasForeignKey("UserTaskId");
                        });

                    b.Navigation("Steps");
                });
#pragma warning restore 612, 618
        }
    }
}
