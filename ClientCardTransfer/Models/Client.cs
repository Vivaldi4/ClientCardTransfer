using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClientCardTransfer.Models
{/// <summary>
/// Класс сущности Client
/// </summary>
    public class Client
    {
        public Client()
        {
            Cards = new List<Card>();
        }
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string ExtenalId { get; set; }//номер договора
        public List<Card> Cards { get; set; } // Список карт клиента
        public DateTime? DeleteDate { get; set; }
    }
}
