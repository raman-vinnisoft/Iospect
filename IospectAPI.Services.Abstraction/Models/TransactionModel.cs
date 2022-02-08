using System;
using System.Collections.Generic;
using System.Text;

namespace IospectAPI.Services.Abstraction.Models
{
    public class TransactionModel
    {
        public int Id { get; set; }
        public int  AccountId { get; set; }
        public string AccountNumber { get; set; }
        public decimal Amount { get; set; }
        public int TransactionType { get; set; }

        public string TransactionTypeName { get; set; }

        public DateTime CreatedDate { get; set; }
        public string Remarks { get; set; }

    }
}
