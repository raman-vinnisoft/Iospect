using System;
using System.Collections.Generic;
using System.Text;

namespace IospectAPI.Services.Abstraction.Models
{
    public class AccountModel
    {
        public int Id { get; set; }
        public string AccountHolder { get; set; }
        public string AccountNumber { get; set; }
        public decimal Balance { get; set; }
        public int AccountType { get; set; }
        public string AccountTypeName { get; set; }

        public int AccountStatus { get; set; }

        public string AccountStatusName { get; set; }

        public DateTime CreatedDate { get; set; }

    }
}
