import { useState } from 'react'
import { createTransaction } from '../api/transactionsApi'
import type {
    CreateTransactionRequest,
    TransactionStatus,
} from '../types/transaction'
import './AddTransactionPage.css'

function AddTransactionPage() {
    const [formData, setFormData] = useState<CreateTransactionRequest>({
        amount: 0,
        currency: 'USD',
        status: 'Pending',
    })

    const [isSubmitting, setIsSubmitting] = useState(false)
    const [error, setError] = useState<string | null>(null)
    const [success, setSuccess] = useState<string | null>(null)

    const handleSubmit = async (event: React.FormEvent) => {
        event.preventDefault()

        setError(null)
        setSuccess(null)

        if (formData.amount <= 0) {
            setError('Amount must be greater than 0')
            return
        }

        if (!/^[A-Z]{3}$/.test(formData.currency)) {
            setError('Currency must contain exactly 3 letters')
            return
        }

        try {
            setIsSubmitting(true)

            await createTransaction(formData)

            setSuccess('Transaction created successfully')

            setFormData({
                amount: 0,
                currency: 'USD',
                status: 'Pending',
            })
        } catch {
            setError('Failed to create transaction')
        } finally {
            setIsSubmitting(false)
        }
    }

    return (
        <div className="add-transaction-page">
            <h1>Add Transaction</h1>

            <form className="transaction-form" onSubmit={handleSubmit}>
                <div className="form-field">
                    <label htmlFor="amount">Amount</label>
                    <input
                        id="amount"
                        type="number"
                        step="0.01"
                        min="0"
                        value={formData.amount}
                        onChange={(event) =>
                            setFormData({
                                ...formData,
                                amount: Number(event.target.value),
                            })
                        }
                    />
                </div>

                <div className="form-field">
                    <label htmlFor="currency">Currency</label>
                    <input
                        id="currency"
                        type="text"
                        maxLength={3}
                        value={formData.currency}
                        onChange={(event) =>
                            setFormData({
                                ...formData,
                                currency: event.target.value.toUpperCase(),
                            })
                        }
                    />
                </div>

                <div className="form-field">
                    <label htmlFor="status">Status</label>
                    <select
                        id="status"
                        value={formData.status}
                        onChange={(event) =>
                            setFormData({
                                ...formData,
                                status: event.target.value as TransactionStatus,
                            })
                        }
                    >
                        <option value="Pending">Pending</option>
                        <option value="Completed">Completed</option>
                        <option value="Failed">Failed</option>
                    </select>
                </div>

                <button type="submit" disabled={isSubmitting}>
                    {isSubmitting ? 'Adding...' : 'Add Transaction'}
                </button>
            </form>

            {error && (
                <p className="form-error">
                    {error}
                </p>
            )}

            {success && (
                <p className="form-success">
                    {success}
                </p>
            )}
        </div>
    )
}

export default AddTransactionPage