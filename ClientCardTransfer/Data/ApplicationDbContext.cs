using ClientCardTransfer.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientCardTransfer.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Card> Cards { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            // Определение отношений между моделями Client и Card
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.Name).IsRequired();
                entity.Property(e => e.ExtenalId).IsRequired();
                entity.HasMany(c => c.Cards)
                    .WithOne(c => c.Client)
                    .HasForeignKey(c => c.ClientId)
                    .OnDelete(DeleteBehavior.Restrict); // Опционально указываете правило удаления записей связанных таблиц
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).ValueGeneratedOnAdd();
                entity.Property(e => e.ClientExtenalId).IsRequired();
                entity.Property(e => e.CardNumber).IsRequired();
                entity.Property(e => e.CardType).IsRequired();
                entity.Property(e => e.BankName).IsRequired();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
