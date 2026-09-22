using FinancialMonitor.Application.DTOs;
using FinancialMonitor.Application.Interfaces;
using FinancialMonitor.Domain.Entities;

namespace FinancialMonitor.Application.Services;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _repository;
    private readonly ITransactionNotifier _notifier;

    public TransactionService(
        ITransactionRepository repository,
        ITransactionNotifier notifier)
    {
        _repository = repository;
        _notifier = notifier;
    }

    public async Task<Transaction> CreateAsync(CreateTransactionRequest request)
    {
        var transaction = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Amount = request.Amount,
            Currency = request.Currency,
            Status = request.Status,
            Timestamp = DateTime.UtcNow
        };

        await _repository.AddAsync(transaction);

        await _notifier.NotifyTransactionCreatedAsync(transaction);

        return transaction;
    }

    public async Task<IReadOnlyCollection<Transaction>> GetLatestAsync(int count)
    {
        return await _repository.GetLatestAsync(count);
    }
}