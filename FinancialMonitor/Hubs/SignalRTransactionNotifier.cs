using FinancialMonitor.Application.Interfaces;
using FinancialMonitor.Domain.Entities;
using Microsoft.AspNetCore.SignalR;

namespace FinancialMonitor.Hubs;

public class SignalRTransactionNotifier : ITransactionNotifier
{
    private readonly IHubContext<TransactionHub> _hubContext;

    public SignalRTransactionNotifier(IHubContext<TransactionHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task NotifyTransactionCreatedAsync(Transaction transaction)
    {
        await _hubContext.Clients.All.SendAsync(
            "TransactionCreated",
            transaction);
    }
}