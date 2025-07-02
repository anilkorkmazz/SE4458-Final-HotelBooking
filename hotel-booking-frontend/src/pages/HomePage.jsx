import { useNavigate } from "react-router-dom";

export default function HomePage() {
  const navigate = useNavigate();

  return (
    <div className="w-full min-h-screen bg-gradient-to-br from-blue-100 to-indigo-100 flex items-center justify-center px-4 py-10">
      <div className="w-full max-w-2xl bg-white rounded-2xl shadow-2xl p-10 text-center">
        <h1 className="text-4xl font-extrabold text-gray-800 mb-4">
          🏨 Welcome to <span className="text-indigo-600">SmartHotel</span>
        </h1>
        <p className="text-gray-600 mb-8 text-lg">
          Search, explore, and book hotels easily. <br /> Use our AI assistant or start searching manually.
        </p>

        <div className="flex flex-col sm:flex-row gap-4 justify-center">
          <button
            onClick={() => navigate("/agent")}
            className="bg-indigo-600 hover:bg-indigo-700 text-white font-semibold py-3 px-6 rounded-xl transition duration-200"
          >
            🤖 Use AI Assistant
          </button>

          <button
            onClick={() => navigate("/hotels")}
            className="bg-gray-200 hover:bg-gray-300 text-gray-800 font-semibold py-3 px-6 rounded-xl transition duration-200"
          >
            🔍 Browse Hotels
          </button>
        </div>

        <div className="mt-8 text-sm text-gray-500">
          Already have an account? <a href="/login" className="text-indigo-600 underline">Login</a>
        </div>
      </div>
    </div>
  );
}
