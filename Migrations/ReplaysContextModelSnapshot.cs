using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using ParseTsgReplays;

namespace ParseTsgReplays.Migrations
{
    [DbContext(typeof(ReplaysContext))]
    partial class ReplaysContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752");

            modelBuilder.Entity("ParseTsgReplays.EnterExitEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsEnter");

                    b.Property<int>("ReplayId");

                    b.Property<int>("Time");

                    b.Property<int>("UnitId");

                    b.Property<string>("User")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.HasIndex("ReplayId");

                    b.ToTable("EnterExitEvents");
                });

            modelBuilder.Entity("ParseTsgReplays.Kill", b =>
                {
                    b.Property<int>("KillId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Ammo")
                        .HasMaxLength(50);

                    b.Property<double>("Distance");

                    b.Property<int>("KillerId");

                    b.Property<int?>("KillerVehicleId");

                    b.Property<int>("ReplayId");

                    b.Property<int>("TargetId");

                    b.Property<int>("Time");

                    b.Property<string>("Weapon")
                        .HasMaxLength(50);

                    b.HasKey("KillId");

                    b.HasIndex("ReplayId");

                    b.ToTable("Kills");
                });

            modelBuilder.Entity("ParseTsgReplays.Replay", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsFinished");

                    b.Property<string>("Island")
                        .HasMaxLength(50);

                    b.Property<string>("Mission")
                        .HasMaxLength(50);

                    b.Property<string>("Server")
                        .HasMaxLength(2);

                    b.Property<DateTime>("Timestamp");

                    b.HasKey("Id");

                    b.ToTable("Replays");
                });

            modelBuilder.Entity("ParseTsgReplays.Unit", b =>
                {
                    b.Property<int>("ReplayId");

                    b.Property<int>("Id");

                    b.Property<string>("Class")
                        .HasMaxLength(50);

                    b.Property<double>("Damage");

                    b.Property<string>("Icon")
                        .HasMaxLength(50);

                    b.Property<bool>("IsVehicle");

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<int>("Side");

                    b.Property<string>("Squad")
                        .HasMaxLength(50);

                    b.Property<int?>("TimeOfDeath");

                    b.Property<string>("Title")
                        .HasMaxLength(50);

                    b.Property<int?>("VehicleOrDriverId");

                    b.HasKey("ReplayId", "Id");

                    b.ToTable("Units");
                });

            modelBuilder.Entity("ParseTsgReplays.EnterExitEvent", b =>
                {
                    b.HasOne("ParseTsgReplays.Replay")
                        .WithMany("Events")
                        .HasForeignKey("ReplayId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ParseTsgReplays.Kill", b =>
                {
                    b.HasOne("ParseTsgReplays.Replay")
                        .WithMany("Kills")
                        .HasForeignKey("ReplayId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("ParseTsgReplays.Unit", b =>
                {
                    b.HasOne("ParseTsgReplays.Replay")
                        .WithMany("Units")
                        .HasForeignKey("ReplayId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
