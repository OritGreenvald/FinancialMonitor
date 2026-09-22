using FinancialMonitor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinancialMonitor.Application.DTOs
{
    public class CreateTransactionRequest
    {

        //[Required]
        //public Guid TransactionId { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }

        [Required]
        [StringLength(3, MinimumLength = 3)]
        public string Currency { get; set; } = string.Empty;

        [Required]
        public TransactionStatus Status { get; set; }

        //[Required]
        //public DateTime Timestamp { get; set; }
    }
}
