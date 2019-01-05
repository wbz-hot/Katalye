﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Katalye.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Katalye.Data.Migrations
{
    [DbContext(typeof(KatalyeContext))]
    [Migration("20190105081636_V4")]
    partial class V4
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.0-rtm-35687")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Katalye.Data.Entities.Job", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Arguments")
                        .IsRequired();

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("Function")
                        .IsRequired();

                    b.Property<string>("Jid")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<DateTimeOffset>("ModifiedOn");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("Jid")
                        .IsUnique();

                    b.ToTable("Jobs");
                });

            modelBuilder.Entity("Katalye.Data.Entities.JobCreationEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<Guid?>("JobId")
                        .IsRequired();

                    b.Property<List<string>>("Minions");

                    b.Property<List<string>>("MissingMinions");

                    b.Property<DateTimeOffset>("ModifiedOn");

                    b.Property<string>("TargetType")
                        .IsRequired();

                    b.Property<List<string>>("Targets");

                    b.Property<DateTimeOffset>("Timestamp");

                    b.Property<string>("User")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("JobId")
                        .IsUnique();

                    b.ToTable("JobCreationEvents");
                });

            modelBuilder.Entity("Katalye.Data.Entities.Minion", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<DateTimeOffset?>("LastAuthentication");

                    b.Property<DateTimeOffset?>("LastSeen");

                    b.Property<string>("MinionSlug")
                        .IsRequired();

                    b.Property<DateTimeOffset>("ModifiedOn");

                    b.Property<int>("Version");

                    b.HasKey("Id");

                    b.HasIndex("MinionSlug")
                        .IsUnique();

                    b.ToTable("Minions");
                });

            modelBuilder.Entity("Katalye.Data.Entities.MinionAuthenticationEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Action")
                        .IsRequired();

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<Guid?>("MinionId")
                        .IsRequired();

                    b.Property<DateTimeOffset>("ModifiedOn");

                    b.Property<string>("PublicKey")
                        .IsRequired();

                    b.Property<string>("PublicKeyHash")
                        .IsRequired();

                    b.Property<bool>("Success");

                    b.Property<DateTimeOffset>("Timestamp");

                    b.HasKey("Id");

                    b.HasIndex("MinionId");

                    b.HasIndex("PublicKeyHash");

                    b.ToTable("MinionAuthenticationEvents");
                });

            modelBuilder.Entity("Katalye.Data.Entities.MinionReturnEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<Guid?>("JobId")
                        .IsRequired();

                    b.Property<Guid?>("MinionId")
                        .IsRequired();

                    b.Property<DateTimeOffset>("ModifiedOn");

                    b.Property<long>("ReturnCode");

                    b.Property<string>("ReturnData")
                        .IsRequired();

                    b.Property<bool>("Success");

                    b.Property<DateTimeOffset>("Timestamp");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("MinionId", "JobId")
                        .IsUnique();

                    b.ToTable("MinionReturnEvents");
                });

            modelBuilder.Entity("Katalye.Data.Entities.UnknownEvent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTimeOffset>("CreatedOn");

                    b.Property<string>("Data")
                        .IsRequired();

                    b.Property<DateTimeOffset>("ModifiedOn");

                    b.Property<string>("Tag")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("UnknownEvents");
                });

            modelBuilder.Entity("Katalye.Data.Entities.JobCreationEvent", b =>
                {
                    b.HasOne("Katalye.Data.Entities.Job", "Job")
                        .WithMany()
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Katalye.Data.Entities.MinionAuthenticationEvent", b =>
                {
                    b.HasOne("Katalye.Data.Entities.Minion", "Minion")
                        .WithMany()
                        .HasForeignKey("MinionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Katalye.Data.Entities.MinionReturnEvent", b =>
                {
                    b.HasOne("Katalye.Data.Entities.Job", "Job")
                        .WithMany()
                        .HasForeignKey("JobId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Katalye.Data.Entities.Minion", "Minion")
                        .WithMany()
                        .HasForeignKey("MinionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
