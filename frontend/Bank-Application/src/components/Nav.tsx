import { Link } from 'react-router-dom'

const Nav = () => {
  return (
    
    <nav className = "w-full flex justify-between items-center text-white bg-amber-950">
        <div className='flex justify-between px-20 py-4 gap-10'>
            <Link to="/home"    className='hover:text-yellow-400'>Home</Link>
            <Link to="/account" className='hover:text-yellow-400'>Account</Link>
            <Link to="/support" className='hover:text-yellow-400'>Support</Link>
        </div>
    </nav>
  )
}

export default Nav