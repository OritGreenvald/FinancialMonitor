using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinancialMonitor.Domain.Entities
{
    
    public class Transaction
    {
        public Guid TransactionId { get; set; }

        public decimal Amount { get; set; }

        public string Currency { get; set; } = string.Empty;

        public TransactionStatus Status { get; set; }

        public DateTime Timestamp { get; set; }
    }
}
