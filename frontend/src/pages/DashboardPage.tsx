import { useCallback, useEffect, useState } from 'react'
import { getLatestTransactions } from '../api/transactionsApi'
import type { Transaction } from '../types/transaction'
import TransactionTable from '../components/TransactionTable'
import { useTransactionHub } from '../hooks/useTransactionHub'
import './DashboardPage.css'

function DashboardPage() {
    const [transactions, setTransactions] = useState<Transaction[]>([])
    const [statusFilter, setStatusFilter] = useState<'All' | Transaction['status']>('All')
    const [loading, setLoading] = useState(true)
    const [error, setError] = useState<string | null>(null)

    useEffect(() => {
        async function loadTransactions() {
            try {
                const data = await getLatestTransactions()
                setTransactions(data)
            } catch {
                setError('Failed to load transactions')
            } finally {
                setLoading(false)
            }
        }

        loadTransactions()
    }, [])

    const handleTransactionCreated = useCallback(
        (transaction: Transaction) => {
            setTransactions((currentTransactions) => {
                const alreadyExists = currentTransactions.some(
                    (currentTransaction) =>
                        currentTransaction.transactionId === transaction.transactionId
                )

                if (alreadyExists) {
                    return currentTransactions
                }

                return [transaction, ...currentTransactions]
            })
        },
        []
    )

    const connectionStatus = useTransactionHub(handleTransactionCreated)

    const filteredTransactions =
        statusFilter === 'All'
            ? transactions
            : transactions.filter(
                (transaction) => transaction.status === statusFilter
            )

    if (loading) {
        return <h1>Loading transactions...</h1>
    }

    if (error) {
        return <h1>{error}</h1>
    }

    return (
        <div>
            <h1>Financial Monitor Dashboard</h1>

            <div className={`connection-status connection-${connectionStatus.toLowerCase()}`}>
                <span className="connection-dot"></span>
                {connectionStatus}
            </div>

            <div>
                <label htmlFor="status-filter">Filter by status: </label>

                <select
                    id="status-filter"
                    value={statusFilter}
                    onChange={(event) =>
                        setStatusFilter(
                            event.target.value as 'All' | Transaction['status']
                        )
                    }
                >
                    <option value="All">All</option>
                    <option value="Pending">Pending</option>
                    <option value="Completed">Completed</option>
                    <option value="Failed">Failed</option>
                </select>
            </div>

            <p>Total transactions: {transactions.length}</p>

            <TransactionTable transactions={filteredTransactions} />
        </div>
    )
}

export default DashboardPage