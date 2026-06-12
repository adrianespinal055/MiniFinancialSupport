using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MiniFinancialSupport.Domain.Entities
{
    public class Customer
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string DocumentNumber { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;    // typo corregido: Eimail -> Email (si no, EF crearía la columna mal)
        public bool IsActive { get; set; } = true;            // = true: un cliente nace activo por defecto

        public DateTime CreatedAt { get; set; }

    }
}
