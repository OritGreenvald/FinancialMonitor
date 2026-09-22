using FinancialMonitor.Application.DTOs;
using FinancialMonitor.Application.Interfaces;
using FinancialMonitor.Application.Services;
using FinancialMonitor.Domain.Entities;
using Moq;

namespace FinancialMonitor.Tests;

public class TransactionServiceTests
{
    [Fact]
    public async Task CreateAsync_ShouldCreateAndSaveTransaction()
    {
        // Arrange
        var repositoryMock = new Mock<ITransactionRepository>();
        var notifierMock = new Mock<ITransactionNotifier>();

        var service = new TransactionService(
            repositoryMock.Object,
            notifierMock.Object);

        var request = new CreateTransactionRequest
        {
            Amount = 1500.50m,
            Currency = "USD",
            Status = TransactionStatus.Completed
        };

        // Act
        var result = await service.CreateAsync(request);

        // Assert
        Assert.NotEqual(Guid.Empty, result.TransactionId);
        Assert.Equal(request.Amount, result.Amount);
        Assert.Equal(request.Currency, result.Currency);
        Assert.Equal(request.Status, result.Status);
        Assert.NotEqual(default, result.Timestamp);

        repositoryMock.Verify(
            repository => repository.AddAsync(
                It.Is<Transaction>(transaction =>
                    transaction.TransactionId == result.TransactionId &&
                    transaction.Amount == request.Amount &&
                    transaction.Currency == request.Currency &&
                    transaction.Status == request.Status)),
            Times.Once);

        notifierMock.Verify(
            notifier => notifier.NotifyTransactionCreatedAsync(
                It.Is<Transaction>(transaction =>
                    transaction.TransactionId == result.TransactionId)),
            Times.Once);
    }

    [Fact]
    public async Task GetLatestAsync_ShouldReturnLatestTransactions()
    {
        // Arrange
        var repositoryMock = new Mock<ITransactionRepository>();
        var notifierMock = new Mock<ITransactionNotifier>();

        var expectedTransactions = new List<Transaction>
    {
        new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Amount = 100,
            Currency = "USD",
            Status = TransactionStatus.Completed,
            Timestamp = DateTime.UtcNow
        },
        new Transaction
        {
            TransactionId = Guid.NewGuid(),
            Amount = 200,
            Currency = "EUR",
            Status = TransactionStatus.Pending,
            Timestamp = DateTime.UtcNow
        }
    };

        repositoryMock
            .Setup(repository => repository.GetLatestAsync(2))
            .ReturnsAsync(expectedTransactions);

        var service = new TransactionService(
            repositoryMock.Object,
            notifierMock.Object);

        // Act
        var result = await service.GetLatestAsync(2);

        // Assert
        Assert.Equal(expectedTransactions.Count, result.Count);
        Assert.Equal(expectedTransactions, result);

        repositoryMock.Verify(
            repository => repository.GetLatestAsync(2),
            Times.Once);
    }
}