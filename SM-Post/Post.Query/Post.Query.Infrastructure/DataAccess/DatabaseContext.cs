using Microsoft.EntityFrameworkCore;
using Post.Query.Domain.Entities;

namespace Post.Query.Infrastructure.DataAccess
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BalanceByTransactionTypeEntity>()
                .HasKey(nameof(BalanceByTransactionTypeEntity.FinAccountId), nameof(BalanceByTransactionTypeEntity.TransactionTypeId));


            modelBuilder.Entity<BalanceByTransactionTypeEntity>()
                .HasOne(e => e.FinAccount)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity<BalanceByTransactionTypeEntity>()
                .HasOne(e => e.TransactionType)
                .WithMany()
                .OnDelete(DeleteBehavior.NoAction)
                .IsRequired();

            modelBuilder.Entity<FinAccountEntity>()
                .HasMany(f => f.Balances)
                .WithOne(e => e.FinAccount)
                .HasForeignKey(e => e.FinAccountId);
        }

        public DbSet<PostEntity> Posts { get; set; }
        public DbSet<CommentEntity> Comments { get; set; }
        public DbSet<FinAccountEntity> FinAccounts { get; set; }
        public DbSet<TransactionTypeEntity> TransactionTypes { get; set; }
        public DbSet<BalanceByTransactionTypeEntity> Balances { get; set; }
    }
}