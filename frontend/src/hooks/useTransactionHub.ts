import { useEffect, useState } from 'react'
import {
    HubConnectionBuilder,
    HubConnectionState,
} from '@microsoft/signalr'
import type { Transaction } from '../types/transaction'

const HUB_URL = 'https://localhost:7147/hubs/transactions'

export type ConnectionStatus =
    | 'Connected'
    | 'Reconnecting'
    | 'Disconnected'

export function useTransactionHub(
    onTransactionCreated: (transaction: Transaction) => void
) {
    const [connectionStatus, setConnectionStatus] =
        useState<ConnectionStatus>('Disconnected')

    useEffect(() => {
        const connection = new HubConnectionBuilder()
            .withUrl(HUB_URL)
            .withAutomaticReconnect()
            .build()

        connection.on('TransactionCreated', (transaction: Transaction) => {
            onTransactionCreated(transaction)
        })

        connection.onreconnecting(() => {
            setConnectionStatus('Reconnecting')
        })

        connection.onreconnected(() => {
            setConnectionStatus('Connected')
        })

        connection.onclose(() => {
            setConnectionStatus('Disconnected')
        })

        async function startConnection() {
            if (connection.state === HubConnectionState.Disconnected) {
                try {
                    setConnectionStatus('Reconnecting')

                    await connection.start()

                    setConnectionStatus('Connected')
                } catch (error) {
                    console.error('SignalR connection failed:', error)
                    setConnectionStatus('Disconnected')
                }
            }
        }

        startConnection()

        return () => {
            connection.stop()
        }
    }, [onTransactionCreated])

    return connectionStatus
}
