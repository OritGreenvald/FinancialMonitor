using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinancialMonitor.Application.DTOs;
using FinancialMonitor.Application.Interfaces;
using FinancialMonitor.Domain.Entities;

namespace FinancialMonitor.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;

    public TransactionService(ITransactionRepository repository)
    {
        _repository = repository;
    }

    public async Task<Transaction> CreateAsync(CreateTransactionRequest request)
    {
        var transaction = new Transaction
        {
            TransactionId = request.TransactionId,
            Amount = request.Amount,
            Currency = request.Currency,
            Status = request.Status,
            Timestamp = request.Timestamp
        };

        await _repository.AddAsync(transaction);

        return transaction;
    }

    public async Task<IReadOnlyCollection<Transaction>> GetLatestAsync(int count)
    {
        return await _repository.GetLatestAsync(count);
    }
}
