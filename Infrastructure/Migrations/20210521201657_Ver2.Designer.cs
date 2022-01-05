
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20210521201657_Ver2")]
    partial class Ver2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("Infrastructure.Server", b =>
                {
                    b.Property<ulong>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint unsigned");

                    b.Property<int>("CommitmentNumber")
                        .HasColumnType("int");

                    b.Property<string>("Prefix")
                        .HasColumnType("longtext");

                    b.Property<ulong>("UserId")
                        .HasColumnType("bigint unsigned");

                    b.HasKey("Id");

                    b.ToTable("Servers");
                });
#pragma warning restore 612, 618
        }
    }
}
