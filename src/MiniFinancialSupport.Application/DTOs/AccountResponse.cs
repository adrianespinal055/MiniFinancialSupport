using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniFinancialSupport.Application.DTOs
{
    public class AccountResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string AccountNumber { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public bool  IsBlocked { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
