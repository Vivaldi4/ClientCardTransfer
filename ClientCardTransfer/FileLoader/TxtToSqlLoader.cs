using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using System.IO;
using ClientCardTransfer.Models;
using System.Collections.Generic;
using System.Linq;
using ClientCardTransfer.Data;
using ClientCardTransfer.Repositories;
using System.Threading.Tasks;
using System;
using ClientCardTransfer.Service;
using Microsoft.Extensions.Logging;

namespace ClientCardTransfer.FileLoader
{
    public class TxtToSqlLoader
    {
        private readonly ILogger<TxtToSqlLoader> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IClientRepository _clientRepository;
        private readonly ICardRepository _cardRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TxtToSqlLoader(ILogger<TxtToSqlLoader> logger, ApplicationDbContext context, IClientRepository clientRepository, ICardRepository cardRepository, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _context = context;
            _clientRepository = clientRepository;
            _cardRepository = cardRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task LoadFilesToSql(string clientFilePath, string cardFilePath)
        {
            try
            {
                List<Client> clients = LoadClientsFromTextFile(clientFilePath); // Загрузка клиентов из файла
                List<Card> cards = LoadCardsFromTextFile(cardFilePath, _clientRepository); // Загрузка карт из файла

                // Очистка существующих данных перед загрузкой новых данных
                await _clientRepository.ClearAllClients();
                await _cardRepository.ClearAllCards();

                // Сохранение клиентов и карт через репозитории
                await _clientRepository.AddRange(clients);
                await _cardRepository.AddRange(cards);

                // Применение изменений в базе данных через Unit of Work
                await _unitOfWork.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while loading files to the database.");
                throw; // Или выполните другую обработку ошибки, в зависимости от требований
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

        private static List<Card> LoadCardsFromTextFile(string filePath, IClientRepository _clientRepository)
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
                    var client = _clientRepository.GetClientByExtenalId(card.ClientExtenalId).Result;

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
