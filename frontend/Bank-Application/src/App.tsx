import { Route, Routes} from 'react-router-dom'
import HomePage from './pages/HomePage.tsx'
import AccountPage from './pages/AccountPage.tsx'
import SupportPage from './pages/SupportPage.tsx'
import Nav from './components/Nav.tsx'
import './App.css'
import Footer from './components/Footer.tsx'

function App() {
  return(
    <div>
      <Nav />

      <Routes>
        <Route path="/" element={<HomePage />}></Route>
        <Route path="/account" element={<AccountPage />}></Route>
        <Route path="/support" element={<SupportPage />}></Route>
      </Routes>

      <Footer />
    </div>
  )
}

export default App
