import { Route, Routes} from 'react-router-dom'
import HomePage from './pages/HomePage.tsx'
import AccountPage from './pages/AccountPage.tsx'
import SupportPage from './pages/SupportPage.tsx'
import Nav from './components/Nav.tsx'
import './App.css'

function App() {
  return(
    <div>
      <Nav />

      <Routes>
        <Route path="/home" element={<HomePage />}></Route>
        <Route path="/account" element={<AccountPage />}></Route>
        <Route path="/support" element={<SupportPage />}></Route>
      </Routes>
    </div>
  )
}

export default App
