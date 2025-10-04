import { Link } from "react-router-dom"

interface FeatureCardProps {
  icon: string
  title: string
  text: string
  to?: string
}

const FeatureCard = ({icon, title, text, to} : FeatureCardProps) => {
  const CardContent = (
    <div className="p-6 rounded-2xl shadow-sm hover:shadow-md transition bg-gray-50 text-center cursor-pointer">
      <div className="text-5xl mb-4">{icon}</div>
      <h3 className="text-xl font-semibold mb-2">{title}</h3>
      <p className="text-gray-600">{text}</p>
    </div>
  )

  return to ? <Link to={to}>{CardContent}</Link> : CardContent
}

export default FeatureCard