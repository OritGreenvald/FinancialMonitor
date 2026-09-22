import type { Transaction, CreateTransactionRequest, } from '../types/transaction'


const API_BASE_URL = 'https://localhost:7147'

export async function getLatestTransactions(
    count: number = 10
): Promise<Transaction[]> {
    const response = await fetch(
        `${API_BASE_URL}/api/Transactions?count=${count}`
    )

    if (!response.ok) {
        throw new Error('Failed to fetch transactions')
    }

    return response.json()
}

export async function createTransaction(
    request: CreateTransactionRequest
): Promise<Transaction> {
    const transaction = {
        transactionId: crypto.randomUUID(),
        amount: request.amount,
        currency: request.currency,
        status: request.status,
        timestamp: new Date().toISOString(),
    }

    const response = await fetch(
        `${API_BASE_URL}/api/Transactions`,
        {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(transaction),
        }
    )

    if (!response.ok) {
        throw new Error('Failed to create transaction')
    }

    return response.json()
}