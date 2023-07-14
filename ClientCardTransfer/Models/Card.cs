using System;

namespace ClientCardTransfer.Models
{

    public class Card
    {
        public int Id { get; set; }
        public string ClientExtenalId { get; set; }//номер договора
        public int ClientId { get; set; } // Внешний ключ на клиента
        public Client Client { get; set; } // Ссылка на клиента
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public string BankName { get; set; }
        public DateTime DeleteDate { get; set; }
    }
}
