using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IospectAPI.Repositories.Context.Entities
{
    public class BankAccount
    {
        [Key]
        public int Id { get; set; }
        public string AccountHolder { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public int AccountType { get; set; }
        public int AccountStatus { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [ForeignKey("AccountId")]
        public virtual List<Transaction> Transaction { get; set; }
    }
}
