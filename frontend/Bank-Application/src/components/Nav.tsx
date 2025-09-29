import { Link } from 'react-router-dom'

const Nav = () => {
  return (
    
    <nav className = "w-full flex justify-between items-center text-[var(--secondary)] bg-[var(--primary)] font-semibold">
        <div className='flex justify-between px-30 py-4 gap-10'>
            <Link to="/home"    className='hover:text-[var(--accent)]'>Home</Link>
            <Link to="/account" className='hover:text-[var(--accent)]'>Account</Link>
            <Link to="/support" className='hover:text-[var(--accent)]'>Support</Link>
        </div>
    </nav>
  )
}

export default Nav