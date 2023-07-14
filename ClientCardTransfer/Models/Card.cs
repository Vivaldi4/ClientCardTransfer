using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ClientCardTransfer.Models
{
    /// <summary>
    /// Класс сущности Card
    /// </summary>
    public class Card
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string ClientExtenalId { get; set; }//номер договора
        [ForeignKey("Client")]
        public int ClientId { get; set; } // Внешний ключ на клиента
        public Client Client { get; set; } // Ссылка на клиента
        public string CardNumber { get; set; }
        public string CardType { get; set; }
        public string BankName { get; set; }
        public DateTime? DeleteDate { get; set; }
    }
}
