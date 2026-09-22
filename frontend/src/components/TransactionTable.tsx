import type { Transaction } from '../types/transaction'
import './TransactionTable.css'

interface TransactionTableProps {
    transactions: Transaction[]
}
function getStatusLabel(status: Transaction['status']) {
    switch (status) {
        case 'Pending':
            return {
                label: 'Pending',
                className: 'status-pending',
            }

        case 'Completed':
            return {
                label: 'Completed',
                className: 'status-completed',
            }

        case 'Failed':
            return {
                label: 'Failed',
                className: 'status-failed',
            }
    }
}
function TransactionTable({ transactions }: TransactionTableProps) {
    if (transactions.length === 0) {
        return (
            <div className="empty-state">
                <h2>No transactions yet</h2>
                <p>
                    Transactions will appear here when they are created.
                </p>
            </div>
        )
    }

    return (
        <table>
            <thead>
                <tr>
                    <th>Transaction ID</th>
                    <th>Amount</th>
                    <th>Currency</th>
                    <th>Status</th>
                    <th>Timestamp</th>
                </tr>
            </thead>

            <tbody>
                {transactions.map((transaction) => {
                    const status = getStatusLabel(transaction.status)

                    return (
                        <tr key={transaction.transactionId}>
                            <td>{transaction.transactionId}</td>
                            <td>{transaction.amount.toFixed(2)}</td>
                            <td>{transaction.currency}</td>
                            <td>
                                <span
                                    className={`status-badge ${status.className}`}
                                >
                                    {status.label}
                                </span>
                            </td>
                            <td>
                                {new Date(
                                    transaction.timestamp
                                ).toLocaleString()}
                            </td>
                        </tr>
                    )
                })}
            </tbody>
        </table>
    )
}

export default TransactionTable