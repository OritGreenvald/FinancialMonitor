using FinancialMonitor.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace FinancialMonitor.Application.Interfaces
{
    public interface ITransactionRepository
    {
        Task AddAsync(Transaction transaction);

        Task<IReadOnlyCollection<Transaction>> GetLatestAsync(int count);
    }
}
