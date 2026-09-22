using FinancialMonitor.Domain.Entities;
using FinancialMonitor.Infrastructure.Persistence;

namespace FinancialMonitor.Tests;

public class InMemoryTransactionRepositoryTests
{
    [Fact]
    public async Task AddAsync_ShouldStoreTransaction()
    {
        // Arrange
        var repository = new InMemoryTransactionRepository();

        var transaction = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Amount = 100,
            Currency = "USD",
            Status = TransactionStatus.Completed,
            Timestamp = DateTime.UtcNow
        };

        // Act
        await repository.AddAsync(transaction);

        // Assert
        var result = await repository.GetLatestAsync(10);

        Assert.Single(result);
        Assert.Equal(transaction.TransactionId, result.First().TransactionId);
    }

    [Fact]
    public async Task AddAsync_ShouldHandleConcurrentWrites()
    {
        // Arrange
        var repository = new InMemoryTransactionRepository();

        var transactions = Enumerable.Range(1, 100)
            .Select(_ => new Transaction
            {
                TransactionId = Guid.NewGuid(),
                Amount = 100,
                Currency = "USD",
                Status = TransactionStatus.Completed,
                Timestamp = DateTime.UtcNow
            })
            .ToList();

        // Act
        var tasks = transactions
            .Select(transaction => repository.AddAsync(transaction));

        await Task.WhenAll(tasks);

        // Assert
        var result = await repository.GetLatestAsync(100);

        Assert.Equal(100, result.Count);
        Assert.Equal(
             transactions.Select(transaction => transaction.TransactionId).OrderBy(id => id),
             result.Select(transaction => transaction.TransactionId).OrderBy(id => id));
    }

    [Fact]
    public async Task GetLatestAsync_ShouldReturnRequestedCountInDescendingTimestampOrder()
    {
        // Arrange
        var repository = new InMemoryTransactionRepository();

        var oldestTransaction = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Amount = 100,
            Currency = "USD",
            Status = TransactionStatus.Completed,
            Timestamp = DateTime.UtcNow.AddMinutes(-3)
        };

        var middleTransaction = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Amount = 200,
            Currency = "EUR",
            Status = TransactionStatus.Pending,
            Timestamp = DateTime.UtcNow.AddMinutes(-2)
        };

        var latestTransaction = new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Amount = 300,
            Currency = "GBP",
            Status = TransactionStatus.Failed,
            Timestamp = DateTime.UtcNow.AddMinutes(-1)
        };

        await repository.AddAsync(oldestTransaction);
        await repository.AddAsync(middleTransaction);
        await repository.AddAsync(latestTransaction);

        // Act
        var result = await repository.GetLatestAsync(2);

        // Assert
        Assert.Equal(2, result.Count);

        Assert.Equal(
            latestTransaction.TransactionId,
            result.ElementAt(0).TransactionId);

        Assert.Equal(
            middleTransaction.TransactionId,
            result.ElementAt(1).TransactionId);
    }

    [Fact]
    public async Task Repository_ShouldHandleConcurrentReadsAndWrites()
    {
        // Arrange
        var repository = new InMemoryTransactionRepository();

        var initialTransactions = Enumerable.Range(1, 50)
            .Select(_ => new Transaction
            {
                TransactionId = Guid.NewGuid(),
                Amount = 100,
                Currency = "USD",
                Status = TransactionStatus.Completed,
                Timestamp = DateTime.UtcNow
            })
            .ToList();

        foreach (var transaction in initialTransactions)
        {
            await repository.AddAsync(transaction);
        }

        // Act
        var writeTasks = Enumerable.Range(1, 50)
            .Select(_ => repository.AddAsync(new Transaction
            {
                TransactionId = Guid.NewGuid(),
                Amount = 200,
                Currency = "EUR",
                Status = TransactionStatus.Pending,
                Timestamp = DateTime.UtcNow
            }));

        var readTasks = Enumerable.Range(1, 50)
            .Select(_ => repository.GetLatestAsync(100));

        await Task.WhenAll(
            writeTasks.Concat(readTasks));

        // Assert
        var finalResult = await repository.GetLatestAsync(100);

        Assert.Equal(100, finalResult.Count);
    }
}
