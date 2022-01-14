﻿// <auto-generated />
using System;
using DiyProjectCalc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DiyProjectCalc.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("DiyProjectCalc.Models.BasicShape", b =>
                {
                    b.Property<int>("BasicShapeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BasicShapeId"), 1L, 1);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Number1")
                        .HasColumnType("float");

                    b.Property<double>("Number2")
                        .HasColumnType("float");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<int>("ShapeType")
                        .HasColumnType("int");

                    b.HasKey("BasicShapeId");

                    b.HasIndex("ProjectId");

                    b.ToTable("BasicShape", (string)null);
                });

            modelBuilder.Entity("DiyProjectCalc.Models.Material", b =>
                {
                    b.Property<int>("MaterialId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MaterialId"), 1L, 1);

                    b.Property<double?>("Depth")
                        .HasColumnType("float");

                    b.Property<double?>("DepthNeeded")
                        .HasColumnType("float");

                    b.Property<double?>("Length")
                        .HasColumnType("float");

                    b.Property<int>("MeasurementType")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<double?>("Width")
                        .HasColumnType("float");

                    b.HasKey("MaterialId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Material", (string)null);
                });

            modelBuilder.Entity("DiyProjectCalc.Models.Project", b =>
                {
                    b.Property<int>("ProjectId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ProjectId"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ProjectId");

                    b.ToTable("Project", (string)null);
                });

            modelBuilder.Entity("MaterialBasicShape", b =>
                {
                    b.Property<int>("BasicShapeId")
                        .HasColumnType("int");

                    b.Property<int>("MaterialId")
                        .HasColumnType("int");

                    b.HasKey("BasicShapeId", "MaterialId");

                    b.HasIndex("MaterialId");

                    b.ToTable("MaterialBasicShape");
                });

            modelBuilder.Entity("DiyProjectCalc.Models.BasicShape", b =>
                {
                    b.HasOne("DiyProjectCalc.Models.Project", "Project")
                        .WithMany("BasicShapes")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("DiyProjectCalc.Models.Material", b =>
                {
                    b.HasOne("DiyProjectCalc.Models.Project", "Project")
                        .WithMany("Materials")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("MaterialBasicShape", b =>
                {
                    b.HasOne("DiyProjectCalc.Models.BasicShape", null)
                        .WithMany()
                        .HasForeignKey("BasicShapeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK_MaterialBasicShape_BasicShape_BasicShapeId");

                    b.HasOne("DiyProjectCalc.Models.Material", null)
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired()
                        .HasConstraintName("FK_MaterialBasicShape_Material_MaterialId");
                });

            modelBuilder.Entity("DiyProjectCalc.Models.Project", b =>
                {
                    b.Navigation("BasicShapes");

                    b.Navigation("Materials");
                });
#pragma warning restore 612, 618
        }
    }
}
