import Button from "../components/Button"
import FeatureCard from "../components/FeatureCard"

export default function HomePage() {
  return (
    <div className="min-h-screen text-[var(--primary)]">
      
      {/* Hero Section */}
      <section className="bg-gray-100 text-center py-16 px-4 sm:py-20 md:py-24">
        <h1 className="text-3xl sm:text-4xl font-bold mb-4">
          Controlează-ți finanțele cu <span className="text-[var(--cta)]">SmartBank</span>
        </h1>
        <p className="text-lg sm:text-xl mb-8 max-w-2xl mx-auto">
          O singură aplicație pentru conturi, carduri, plăți și investiții. Simplu, sigur și rapid.
        </p>
        <div className="flex flex-col sm:flex-row justify-center items-center gap-4 max-w-sm mx-auto">
          <Button to="/account" variant="primary" className="w-full">Creeaza cont</Button>
          <Button to="/" variant="secondary" className="w-full">Afla mai multe</Button>
        </div>

      </section>

      <section id="features" className="py-10 px-6 sm:px-8">
        <h2 className="text-2xl sm:text-3xl font-semibold text-center mb-8 sm:mb-12">Funcționalități cheie</h2>
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6 sm:gap-8 md:gap-10 max-w-5xl mx-auto">
          <FeatureCard
            icon="💳"
            title="Carduri instant"
            text="Generează carduri virtuale și controlează-le în timp real."
          />
          <FeatureCard
            icon="📈"
            title="Analize inteligente"
            text="Primești rapoarte despre cheltuieli și recomandări personalizate."
          />
          <FeatureCard
            icon="🔒"
            title="Securitate totală"
            text="Protecție biometrică și criptare de nivel bancar."
          />
        </div>
      </section>

      <section id="how-it-works" className="py-10 px-6 sm:px-8 bg-gray-100">
        <h2 className="text-2xl sm:text-3xl font-semibold text-center mb-8 sm:mb-12">Cum funcționează</h2>
        <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-3 gap-6 sm:gap-8 md:gap-10 max-w-5xl mx-auto text-center">
          <FeatureCard
            icon="📝"
            title="Deschide contul"
            text="Completează rapid formularul și activează contul în câteva minute."
          />
          <FeatureCard
            icon="💰"
            title="Adaugă fonduri"
            text="Încarcă bani în cont folosind cardul sau transfer bancar."
          />
          <FeatureCard
            icon="📊"
            title="Controlează cheltuielile"
            text="Urmărește cheltuielile și vezi statistici clare în aplicație."
          />
        </div>
      </section>
    </div>
  )
}


