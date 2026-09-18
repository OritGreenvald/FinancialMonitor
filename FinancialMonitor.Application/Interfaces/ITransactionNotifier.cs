using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FinancialMonitor.Domain.Entities;

namespace FinancialMonitor.Application.Interfaces;

public interface ITransactionNotifier
{
    Task NotifyTransactionCreatedAsync(Transaction transaction);
}
