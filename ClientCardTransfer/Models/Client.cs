using System;
using System.Collections.Generic;

namespace ClientCardTransfer.Models
{
    public class Client
    {
        public Client()
        {
            Cards = new List<Card>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string ExtenalId { get; set; }//номер договора
        public List<Card> Cards { get; set; } // Список карт клиента
        public DateTime DeleteDate { get; set; }
    }
}
