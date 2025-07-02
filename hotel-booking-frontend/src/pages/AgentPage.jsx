import { useState } from "react";
import { sendAgentMessage } from "../services/agentService";
import { parseAgentResponse } from "../utils/parseAgentResponse";

export default function AgentPage() {
  const [message, setMessage] = useState("");
  const [response, setResponse] = useState("");
  const [cards, setCards] = useState([]);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!message.trim()) return;

    setLoading(true);
    setResponse("");
    const result = await sendAgentMessage(message);
    setResponse(result);
    setLoading(false);

    if (result.includes("Hotel:")) {
      const parsed = parseAgentResponse(result);
      setCards(parsed);
    } else {
      setCards([]);
    }
  };

  return (
    <div className="min-h-screen bg-gradient-to-br from-indigo-100 via-white to-blue-100 flex items-center justify-center px-4 py-10">
      <div className="w-full max-w-4xl bg-white/60 backdrop-blur-md rounded-3xl shadow-xl p-8 border border-white/30">
        <h2 className="text-3xl font-extrabold text-center text-gray-800 mb-8">
          💬 Ask AI to Search or Book a Room
        </h2>

        <form onSubmit={handleSubmit} className="space-y-6">
          <textarea
            rows={4}
            className="w-full p-4 border border-gray-300 rounded-2xl text-base focus:outline-none focus:ring-2 focus:ring-indigo-500 resize-none shadow-inner bg-white/80 backdrop-blur"
            placeholder="Example: Search a room in Rome for 2 people from July 10 to July 15"
            value={message}
            onChange={(e) => setMessage(e.target.value)}
          />
          <div className="flex justify-center">
            <button
              type="submit"
              className="px-8 py-3 bg-indigo-600 text-white font-semibold rounded-2xl hover:bg-indigo-700 transition-all shadow-lg"
            >
               Send
            </button>
          </div>
        </form>

        {loading && (
          <div className="mt-6 text-center text-gray-500 animate-pulse">
            ⏳ Thinking...
          </div>
        )}

        {!loading && cards.length > 0 && (
          <div className="mt-10 grid sm:grid-cols-2 lg:grid-cols-3 gap-6">
            {cards.map((item, index) => (
              <div
                key={index}
                className="bg-white rounded-2xl p-6 shadow-md border border-gray-200 hover:shadow-xl transition-transform hover:-translate-y-1"
              >
                <h3 className="text-xl font-bold text-indigo-700 mb-2">{item.hotel}</h3>
                <p className="text-gray-700"><strong>📍 Location:</strong> {item.location}</p>
                <p className="text-gray-700"><strong>🛏️ Room:</strong> {item.room}</p>
                <p className="text-gray-700"><strong>💰 Price:</strong> {item.price}</p>
                <p className="text-gray-700"><strong>📅 Availability:</strong> {item.availability}</p>
              </div>
            ))}
          </div>
        )}

        {!loading && !cards.length && response && (
          <div className="mt-6 text-center text-gray-600 whitespace-pre-line bg-white rounded-xl p-4 shadow-sm">
            {response}
          </div>
        )}
      </div>
    </div>
  );
}
