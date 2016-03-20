using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using ConsumptionWatchList.Data;

namespace ConsumptionWatchList.Migrations
{
    [DbContext(typeof(ConsumptionWLContext))]
    partial class ConsumptionWLContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("ConsumptionWatchList.Models.Item", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<float>("Amount");

                    b.Property<string>("Name");

                    b.Property<string>("Symbol");

                    b.HasKey("ID");
                });
        }
    }
}
