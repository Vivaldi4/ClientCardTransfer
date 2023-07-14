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
            // Определение конфигураций сущностей (если есть)
            //modelBuilder.ApplyConfiguration(new CardConfiguration());
            // modelBuilder.ApplyConfiguration(new ClientConfiguration());
            // Определение отношений между моделями Client и Card
            modelBuilder.Entity<Card>()
                .HasOne(c => c.Client)
                .WithMany(c => c.Cards)
                .HasForeignKey(c => c.ClientId);

            base.OnModelCreating(modelBuilder);
        }
    }
}
