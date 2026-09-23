# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY FinancialMonitor/FinancialMonitor.csproj FinancialMonitor/
COPY FinancialMonitor.Application/FinancialMonitor.Application.csproj FinancialMonitor.Application/
COPY FinancialMonitor.Domain/FinancialMonitor.Domain.csproj FinancialMonitor.Domain/
COPY FinancialMonitor.Infrastructure/FinancialMonitor.Infrastructure.csproj FinancialMonitor.Infrastructure/

RUN dotnet restore FinancialMonitor/FinancialMonitor.csproj

COPY . .

RUN dotnet publish FinancialMonitor/FinancialMonitor.csproj \
    -c Release \
    -o /app/publish

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final

WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

USER app

ENTRYPOINT ["dotnet", "FinancialMonitor.dll"]