import { useState } from "react";

const SupportPage = () => {
  const [messages, setMessages] = useState<{ sender: string; text: string }[]>([]);
  const [input, setInput] = useState("");

  const sendMessage = async () => {
  if (!input.trim()) return;

  const newMessage = { sender: 'user' as const, text: input };
  setMessages(prev => [...prev, newMessage]);
  setInput('');

  try {
    const res = await fetch("http://127.0.0.1:8000/chat", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({ question: input }),
    });
    const data = await res.json();
    const botMessage = { sender: 'bot' as const, text: data.answer };
    setMessages(prev => [...prev, botMessage]);
  } catch (err) {
    const botMessage = { sender: 'bot' as const, text: "Eroare la server" };
    setMessages(prev => [...prev, botMessage]);
  }
};


  return (
    <main className="flex p-20 justify-center items-center h-screen">
      <div className="shadow-[0_0_20px_rgba(0,0,0,0.2)] w-full h-full max-w-2xl flex flex-col bg-white rounded-lg overflow-hidden">
        {/* Header */}
        <div className="bg-blue-600 text-white p-4 text-xl font-semibold">
          Support Chat
        </div>

        {/* Zona de mesaje */}
        <div className="flex-1 p-4 overflow-y-auto space-y-3">
          {messages.map((msg, i) => (
            <div
              key={i}
              className={`p-3 rounded-lg max-w-[75%] ${
                msg.sender === "user"
                  ? "bg-blue-500 text-white self-end ml-auto"
                  : "bg-gray-200 text-gray-800 self-start mr-auto"
              }`}
            >
              {msg.text}
            </div>
          ))}
        </div>

        {/* Input + buton */}
        <div className="flex p-4 border-t gap-2">
          <input
            className="flex-1 border rounded-lg p-2 focus:outline-none focus:ring-2 focus:ring-blue-500"
            value={input}
            onChange={(e) => setInput(e.target.value)}
            placeholder="Scrie un mesaj..."
            onKeyDown={(e) => e.key === "Enter" && sendMessage()}
          />
          <button
            onClick={sendMessage}
            className="bg-blue-600 text-white px-4 py-2 rounded-lg hover:bg-blue-700 cursor-pointer"
          >
            Trimite
          </button>
        </div>
      </div>
    </main>
  );
};

export default SupportPage;
