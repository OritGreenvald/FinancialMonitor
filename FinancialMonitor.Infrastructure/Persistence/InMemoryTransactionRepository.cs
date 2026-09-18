using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinancialMonitor.Application.Interfaces;
using FinancialMonitor.Domain.Entities;

namespace FinancialMonitor.Infrastructure.Persistence
{
    public class InMemoryTransactionRepository : ITransactionRepository
    {
        private readonly ConcurrentDictionary<Guid, Transaction> _transactions = new();

        public Task AddAsync(Transaction transaction)
        {
            _transactions[transaction.TransactionId] = transaction;

            return Task.CompletedTask;
        }

        public Task<IReadOnlyCollection<Transaction>> GetLatestAsync(int count)
        {
            var latestTransactions = _transactions.Values
                .OrderByDescending(transaction => transaction.Timestamp)
                .Take(count)
                .ToList();

            return Task.FromResult<IReadOnlyCollection<Transaction>>(
                latestTransactions);
        }
    }
}
