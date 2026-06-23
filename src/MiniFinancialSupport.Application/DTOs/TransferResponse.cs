using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MiniFinancialSupport.Application.DTOs
{
    public class TransferResponse
    {
        public int FromAccountId { get; set; }
        public int ToAccountId { get; set; }
        public decimal Amount { get; set; }
        public string Description { get; set; } = string.Empty;
        public decimal FromBalance { get; set; }
        public decimal ToBalance { get; set; }

    }
}
