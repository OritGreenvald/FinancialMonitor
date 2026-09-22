# Real-Time Financial Monitor

A full-stack MVP for monitoring financial transactions in real time.

The system allows transactions to be created through a REST API and immediately broadcast to connected dashboard clients using SignalR.

## Overview

The application consists of:

- A .NET 8 backend
- A React + TypeScript frontend
- REST API for transaction creation and retrieval
- SignalR for real-time transaction updates
- Thread-safe in-memory transaction storage
- A dashboard for monitoring transactions
- A transaction simulator for creating new transactions

## Architecture

The backend follows a layered architecture:

```
Controller
    ?
Application Service
    ?
Repository
    ?
In-Memory Storage
```

Real-time notifications are handled separately:

```
TransactionService
        ?
ITransactionNotifier
        ?
SignalR
        ?
Connected Dashboard Clients
```

### Backend Projects

```
FinancialMonitor
??? FinancialMonitor.Api
?   ??? Controllers
?   ??? Hubs
?   ??? Middleware
?   ??? Program.cs
?
??? FinancialMonitor.Application
?   ??? DTOs
?   ??? Interfaces
?   ??? Services
?
??? FinancialMonitor.Domain
?   ??? Entities
?
??? FinancialMonitor.Infrastructure.Persistence
    ??? InMemoryTransactionRepository
```

This separation keeps HTTP concerns, business logic, domain models, and persistence concerns independent from each other.

## Tech Stack

### Backend

- .NET 8
- ASP.NET Core Web API
- C#
- SignalR
- Swagger / OpenAPI
- `ConcurrentDictionary`

### Frontend

- React
- TypeScript
- Vite
- React Router
- `@microsoft/signalr`

## Backend

### Transaction Creation

Transactions are created through:

```
POST /api/Transactions
```

The client sends:

```json
{
  "amount": 1500.50,
  "currency": "USD",
  "status": "Pending"
}
```

The backend generates:

- `TransactionId`
- `Timestamp`

This prevents clients from controlling server-generated transaction metadata.

### Latest Transactions

Transactions can be retrieved through:

```
GET /api/Transactions?count=10
```

The API returns the latest transactions ordered by timestamp in descending order.

The `count` parameter is validated to accept values between 1 and 100.

## Thread Safety

The application uses `ConcurrentDictionary<Guid, Transaction>` for in-memory storage.

This allows multiple concurrent requests to add transactions safely without requiring explicit locking around the collection.

The repository is registered as a singleton so the in-memory transaction collection is shared across requests.

## Real-Time Communication

SignalR is used to broadcast newly created transactions to connected dashboard clients.

The SignalR hub is available at:

```
/hubs/transactions
```

When a transaction is successfully stored:

```
TransactionService
        ?
Repository.AddAsync()
        ?
Notifier.NotifyTransactionCreatedAsync()
        ?
SignalR
        ?
Connected clients
```

The frontend uses automatic reconnect and displays the current connection state:

- Connected
- Reconnecting
- Disconnected

## Frontend

The frontend contains two main routes:

### Dashboard

```
/monitor
```

The dashboard displays:

- Transaction ID
- Amount
- Currency
- Status
- Timestamp
- Connection status
- Transaction count
- Status filtering

New transactions received through SignalR are added to the dashboard without requiring a page refresh.

### Add Transaction

```
/add
```

The transaction simulator allows users to create transactions with:

- Amount
- Currency
- Status

Client-side validation is applied before submitting the request.

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| POST | `/api/Transactions` | Creates a transaction |
| GET | `/api/Transactions?count=10` | Returns the latest transactions |

## Tests

The project includes backend unit tests covering the main transaction processing and storage logic.

The tests cover:

- Transaction creation and persistence
- Transaction retrieval
- Repository storage behavior
- Concurrent transaction writes
- Concurrent reads and writes
- Latest transaction ordering

The test suite uses xUnit and Moq.

Run the tests with:

```bash
dotnet test .\FinancialMonitor.Tests\FinancialMonitor.Tests.csproj
```

All tests should pass successfully.

## Running the Project

### Backend

1. Open the solution in Visual Studio.
2. Run the `FinancialMonitor.Api` project.
3. The API will start on the configured HTTPS/HTTP development ports.
4. Swagger is available in the Development environment.

### Frontend

Open a terminal in the `frontend` directory and run:

```bash
npm.cmd install
npm.cmd run dev
```

The Vite development server will start the React application.

## Design Decisions

### Layered Architecture

The application uses a Controller ? Service ? Repository structure.

This keeps responsibilities separated and makes individual components easier to test and replace.

### In-Memory Storage

An in-memory repository was selected for the MVP to keep the implementation simple and focused on the real-time requirements.

A persistent database can be introduced later without changing the controller or frontend contract.

### ConcurrentDictionary

`ConcurrentDictionary` provides thread-safe access to the in-memory transaction collection and supports concurrent transaction creation.

### SignalR

SignalR was selected because the dashboard requires immediate server-to-client updates when new transactions are created.

### Server-Generated Metadata

Transaction IDs and timestamps are generated by the backend rather than supplied by the client.

This keeps server-controlled transaction metadata consistent.

### Thin SignalR Hub

The SignalR hub contains no business logic.

Transaction processing remains inside the application service, while the notifier abstraction handles communication with connected clients.

## Future Improvements

Possible future improvements include:

- Persistent database storage
- Transaction statistics and summary widgets
- Additional sorting and filtering options
- Authentication and authorization
- Additional frontend tests
- Production configuration and deployment
- More detailed monitoring and logging