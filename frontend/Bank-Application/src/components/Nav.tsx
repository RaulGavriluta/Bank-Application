import { useState, useEffect, useRef } from "react"
import { Link } from "react-router-dom"
import { HiMenu, HiX } from "react-icons/hi"

const Nav = () => {
  const [isOpen, setIsOpen] = useState(false)
  const menuRef = useRef<HTMLDivElement>(null)

  useEffect(() => {
    const handleClickOutside = (event: MouseEvent) => {
      if (menuRef.current && !menuRef.current.contains(event.target as Node)) {
        setIsOpen(false)
      }
    }
    document.addEventListener("mousedown", handleClickOutside)
    return () => document.removeEventListener("mousedown", handleClickOutside)
  }, [])

  const links = [
    { name: "Home", to: "/" },
    { name: "Account", to: "/account" },
    { name: "Support", to: "/support" },
  ]

  return (
    <>
      {/* Navbar */}
      <nav className="w-full bg-[var(--primary)] text-[var(--accent)] font-semibold sticky top-0 left-0 z-50 shadow-md">
        <div className="max-w-7xl mx-auto flex justify-between items-center px-6 lg:px-12 py-4">
          {/* Logo */}
          <div className="text-xl font-bold">
            <Link to="/home">SmartBank</Link>
          </div>

          {/* Desktop Links */}
          <div className="hidden lg:flex gap-10">
            {links.map((link) => (
              <Link
                key={link.to}
                to={link.to}
                className="hover:text-[var(--secondary)] transition-colors"
              >
                {link.name}
              </Link>
            ))}
          </div>

          {/* Hamburger */}
          <button
            onClick={() => setIsOpen(!isOpen)}
            className="lg:hidden text-3xl p-2 focus:outline-none transition-transform duration-300"
          >
            <span
              className={`transform transition-transform duration-300 ${
                isOpen ? "rotate-90" : ""
              }`}
            >
              {isOpen ? <HiX /> : <HiMenu />}
            </span>
          </button>
        </div>
      </nav>

      

      {/* Mobile Menu Slide-in */}
      <div
        ref={menuRef}
        className={`fixed top-10 right-0 h-full w-64 bg-[var(--primary)] shadow-lg transform transition-transform duration-300 z-40 ${
          isOpen ? "translate-x-0" : "translate-x-full"
        } lg:hidden flex flex-col px-6 pt-20 gap-6`}
      >
        {links.map((link) => (
          <Link
            key={link.to}
            to={link.to}
            className="py-3 border-b border-[var(--accent)] text-[var(--accent)] font-semibold hover:text-[var(--secondary)] transition-all"
            onClick={() => setIsOpen(false)}
          >
            {link.name}
          </Link>
        ))}
      </div>
    </>
  )
}

export default Nav
