import { BrowserRouter, Routes, Route, Link } from 'react-router-dom'
import DashboardPage from './pages/DashboardPage'
import AddTransactionPage from './pages/AddTransactionPage'

function App() {
    return (
        <BrowserRouter>
            <nav>
                <Link to="/monitor">Dashboard</Link>
                {' | '}
                <Link to="/add">Add Transaction</Link>
            </nav>

            <Routes>
                <Route path="/monitor" element={<DashboardPage />} />
                <Route path="/add" element={<AddTransactionPage />} />
            </Routes>
        </BrowserRouter>
    )
}

export default App