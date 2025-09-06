import { Route, Routes, Link } from 'react-router-dom'
import { LoginForm, RegistrationForm, SideMenu} from './components'
import ProtectedRoutes from './utils/ProtectedRoutes'
import './App.css'

function App() {
  return (
    <Routes>
        <Route
        path="/"
        element={
          <div className="flex flex-col items-center justify-center min-h-screen gap-4 bg-gray-900 text-white">
            <h1 className="text-3xl font-bold">Hello world</h1>
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
        
        
        <Route path="/signin" element={<LoginForm />} />
        <Route path="/signup" element={<RegistrationForm />} />
        
        <Route element={<ProtectedRoutes />}>
            <Route path="/dashboard" element={<SideMenu />} />
        </Route>
    </Routes>
  )
}

export default App
