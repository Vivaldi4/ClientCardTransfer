using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;
using ClientCardTransfer.Models;
using System.Collections.Generic;
using System.Linq;

namespace ClientCardTransfer.FileLoader
{
    public class TxtToSqlLoader
    {
        private readonly string _connectionString;

        public TxtToSqlLoader(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void LoadFilesToSql(string clientFilePath, string cardFilePath)
        {                                  
            using (var context = new MyDbContext(_connectionString))
            {
                

                //context.SaveChanges();
                List<Client> clients = LoadClientsFromTextFile(clientFilePath); // Загрузка клиентов из файла
                // Загрузка данных из файла Clients_.txt в таблицу Clients
                foreach (var client in clients)
                {
                    context.Clients.Add(client);
                }

                context.SaveChanges();

                List<Card> cards = LoadCardsFromTextFile(cardFilePath, _connectionString); // Загрузка карт из файла
                // Загрузка данных из файла Cards_.txt в таблицу Cards
                foreach (var card in cards)
                {
                    context.Cards.Add(card);
                }

                context.SaveChanges();
            }


        }
        private static List<Client> LoadClientsFromTextFile(string filePath)
        {
            List<Client> clients = new List<Client>();
            int id = 1;
            foreach (string line in File.ReadAllLines(filePath))
            {
                string[] values = line.Split('|');
                string name = values[0];
                string extenalId= values[1];

                Client client = new Client()
                {
                    Id = id,
                    Name = name,
                    ExtenalId = extenalId
                };
                id++;
                clients.Add(client);
            }

            return clients;
        }

        private static List<Card> LoadCardsFromTextFile(string filePath,string connectionstring)
        {
            using (var context = new MyDbContext(connectionstring))
            {
                List<Card> cards = new List<Card>();
                int id = 1;
                foreach (string line in File.ReadAllLines(filePath))
                {
                    string[] values = line.Split('|');



                    Card card = new Card()
                    {
                        Id =id ,
                        ClientExtenalId = values[3].Trim(),
                        CardNumber = values[0].Trim(),
                        CardType = values[1].Trim(),
                        BankName = values[2].Trim()
                    };
                    // Находим клиента по внешнему идентификатору (номеру договора)
                    var client = context.Clients.FirstOrDefault(c => c.ExtenalId == card.ClientExtenalId);

                    if (client != null)
                    {
                        card.ClientId = client.Id;
                        card.Client = client;
                    }
                    id++;
                    cards.Add(card);
                }

                return cards;
            }
        }
      
    }
    
    // Контекст базы данных
    public class MyDbContext : DbContext
    {
        private readonly string _connectionString;
        public MyDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }
        // Определите наборы моделей, соответствующие таблицам базы данных
        public DbSet<Client> Clients { get; set; }
        public DbSet<Card> Cards { get; set; }

        
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Определение отношений между моделями Client и Card
            modelBuilder.Entity<Card>()
                .HasOne(c => c.Client)
                .WithMany(c => c.Cards)
                .HasForeignKey(c => c.ClientId);
        }
    }
  
}
