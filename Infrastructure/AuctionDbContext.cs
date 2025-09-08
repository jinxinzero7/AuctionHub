using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Infrastructure.AuctionDbContext;

namespace Infrastructure
{
    public class AuctionDbContext(DbContextOptions<AuctionDbContext> options) : DbContext(options)
    {
        // Таблицы сущностей
        public DbSet<Lot> Lots { get; set; }
        public DbSet<Bid> Bids { get; set; }
        public DbSet<User> Users { get; set; }

        // Применение конфигураций (см. ниже)
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new LotConfiguration());
            modelBuilder.ApplyConfiguration(new BidConfiguration());
        }
    }

    // Конфигурации сущностей
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);

            // Связи

            builder.HasMany(u => u.CreatedLots)
                .WithOne(l => l.Creator)
                .HasForeignKey(l => l.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(u => u.Bids)
                .WithOne(b => b.Bidder)
                .HasForeignKey(b => b.BidderId)
                .OnDelete(DeleteBehavior.Restrict);

            // Настройка property

            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e => e.Username)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(u => u.PasswordHash)
            .IsRequired();

            builder.Property(u => u.PasswordSalt)
                .IsRequired();

            // Индексы для часто используемых полей
            builder.HasIndex(u => u.Username).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
    public class LotConfiguration : IEntityTypeConfiguration<Lot>
    {
        public void Configure(EntityTypeBuilder<Lot> builder)
        {
            builder.HasKey(l => l.Id);

            // Связи

            builder.HasMany(l => l.Bids)
                .WithOne(b => b.Lot)
                .HasForeignKey(b => b.LotId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(l => l.Winner)
                .WithMany(u => u.WonLots)
                .HasForeignKey(l => l.WinnerId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            builder.HasOne(l => l.Creator)
                .WithMany(u => u.CreatedLots)
                .HasForeignKey(l => l.CreatorId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Настройка property

            builder.Property(l => l.Title)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(l => l.Description)
                .HasMaxLength(1000);

            builder.Property(l => l.StartingPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(l => l.BidIncrement)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(l => l.CurrentPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(l => l.StartDate)
                .IsRequired();

            builder.Property(l => l.EndDate)
                .IsRequired();

            builder.Property(l => l.IsCompleted)
                .IsRequired()
                .HasDefaultValue(false);

            // Индексы для часто используемых полей

            builder.HasIndex(l => l.CreatorId);
            builder.HasIndex(l => l.WinnerId);
            builder.HasIndex(l => l.IsCompleted);
            builder.HasIndex(l => l.EndDate);
        }
    }
    public class BidConfiguration : IEntityTypeConfiguration<Bid>
    {
        public void Configure(EntityTypeBuilder<Bid> builder)
        {
            builder.HasKey(b => b.Id);

            // Связи

            builder.HasOne(b => b.Lot)
                .WithMany(l => l.Bids)
                .HasForeignKey(b => b.LotId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            builder.HasOne(b => b.Bidder)
                .WithMany(b => b.Bids)
                .HasForeignKey(b => b.BidderId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired();

            // Настройка property

            builder.Property(b => b.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired();

            builder.Property(b => b.Timestamp)
                .IsRequired()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Индексы для часто используемых полей

            builder.HasIndex(b => b.LotId);
            builder.HasIndex(b => b.BidderId);
            builder.HasIndex(b => b.Timestamp);
        }
    }

}

