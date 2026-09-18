using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinancialMonitor.Application.DTOs;
using FinancialMonitor.Domain.Entities;

namespace FinancialMonitor.Application.Interfaces;

public interface ITransactionService
{
    Task<Transaction> CreateAsync(CreateTransactionRequest request);

    Task<IReadOnlyCollection<Transaction>> GetLatestAsync(int count);
}
