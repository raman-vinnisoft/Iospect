using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IospectAPI.Repositories.Context.Entities
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public int AccountId { get; set; }
        public decimal Amount { get; set; }
        public int TransactionType { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public string Remarks { get; set; }

    }
}
