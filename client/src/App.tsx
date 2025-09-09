import { Route, Routes, Link } from 'react-router-dom'
import { Toaster } from 'react-hot-toast'
import { LoginPage, RegisterPage, DashboardPage} from './pages'
import ProtectedRoutes from './utils/ProtectedRoutes'
import './App.css'

function App() {
  return (
    <div>
        <Toaster position="top-center" />
        <Routes>
            <Route
            path="/"
            element={
            <div className="flex flex-col items-center justify-center min-h-screen gap-4 bg-gray-900 text-white">
                <div className="flex gap-4 mt-6">
                <Link
                    to="/signin"
                    className="px-4 py-2 rounded-md bg-indigo-500 hover:bg-indigo-400 font-semibold"
                >
                    Sign in
                </Link>
                <Link
                    to="/signup"
                    className="px-4 py-2 rounded-md bg-green-500 hover:bg-green-400 font-semibold"
                >
                    Sign up
                </Link>
                </div>
            </div>
            }
        />
            
            
            <Route path="/signin" element={<LoginPage />} />
            <Route path="/signup" element={<RegisterPage />} />
            
            <Route element={<ProtectedRoutes />}>
                <Route path="/dashboard" element={<DashboardPage />} />
            </Route>
        </Routes>
    </div>
  )
}

export default App
