using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ExampleCheck
{
    public partial class DBLibraryContext : DbContext
    {
        public DBLibraryContext()
        {
        }

        public DBLibraryContext(DbContextOptions<DBLibraryContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Currency> Currencies { get; set; } = null!;
        public virtual DbSet<CurrencyPair> CurrencyPairs { get; set; } = null!;
        public virtual DbSet<Customer> Customers { get; set; } = null!;
        public virtual DbSet<Exchange> Exchanges { get; set; } = null!;
        public virtual DbSet<ExchangeClient> ExchangeClients { get; set; } = null!;
        public virtual DbSet<Order> Orders { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=DESKTOP-NCQ36ON\\MSSQLSERVER02; Database=DBLibrary; Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<CurrencyPair>(entity =>
            {
                entity.ToTable("Currency_Pairs");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.FirstCurrency).HasColumnName("First_currency");

                entity.Property(e => e.PairType)
                    .HasMaxLength(10)
                    .HasColumnName("Pair_type")
                    .IsFixedLength();

                entity.Property(e => e.Price)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.SecondCurrency).HasColumnName("Second_currency");

                entity.HasOne(d => d.FirstCurrencyNavigation)
                    .WithMany(p => p.CurrencyPairFirstCurrencyNavigations)
                    .HasForeignKey(d => d.FirstCurrency)
                    .HasConstraintName("FK_Currency_Pairs_Currency_Pairs");

                entity.HasOne(d => d.SecondCurrencyNavigation)
                    .WithMany(p => p.CurrencyPairSecondCurrencyNavigations)
                    .HasForeignKey(d => d.SecondCurrency)
                    .HasConstraintName("FK_Currency_Pairs_Currencies");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Birthday)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<Exchange>(entity =>
            {
                entity.ToTable("Exchange");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Fee)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.Property(e => e.Name)
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<ExchangeClient>(entity =>
            {
                entity.ToTable("Exchange_Clients");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ExchangeClient1).HasColumnName("Exchange_Client");

                entity.HasOne(d => d.ExchangeNavigation)
                    .WithMany(p => p.ExchangeClient1)
                    .HasForeignKey(d => d.Exchange)
                    .HasConstraintName("FK_Exchange_Clients_Exchange");

                entity.HasOne(d => d.ExchangeClient1Navigation)
                    .WithMany(p => p.ExchangeClient1)
                    .HasForeignKey(d => d.ExchangeClient1)
                    .HasConstraintName("FK_Exchange_Clients_Customer");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.PairId).HasColumnName("Pair_ID");

                entity.Property(e => e.Value)
                    .HasMaxLength(10)
                    .IsFixedLength();

                entity.HasOne(d => d.ExchangeNavigation)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.Exchange)
                    .HasConstraintName("FK_Orders_Exchange");

                entity.HasOne(d => d.Pair)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PairId)
                    .HasConstraintName("FK_Orders_Currency_Pairs");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
