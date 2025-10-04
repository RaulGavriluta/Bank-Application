import { Link } from "react-router-dom"
import type { ReactNode } from "react"
import clsx from "clsx"

interface ButtonProps {
    to: string
    children: ReactNode
    variant?: "primary" | "secondary"
    className?: string
}

const Button = ({to, children, variant = "primary", className} : ButtonProps) => {

    const baseClasses = "px-8 py-4 rounded-xl transition font-semibold text-center"
    const variantClasses = 
    variant === "primary" 
    ? "bg-[var(--cta)] text-white border border-transparent hover:bg-white hover:text-[var(--cta)] hover:border-[var(--cta)]" 
    : "border border-[var(--cta)] text-[var(--cta)] hover:bg-[var(--cta)] hover:text-white"

  return (
    <Link to={to} className={clsx(baseClasses, variantClasses, className)}>
      {children}
    </Link>
)
}

export default Button