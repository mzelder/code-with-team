import { useState } from "react";
import { Search } from "lucide-react"; // ikona z lucide-react

function Dashboard() {
  const [loading, setLoading] = useState(false);

  const handleClick = () => {
    setLoading(true);
    // tutaj możesz wywołać API do dobierania drużyny
    setTimeout(() => setLoading(false), 2000); // symulacja
  };

  return (
    <button
      onClick={handleClick}
      disabled={loading}
      className="flex items-center justify-center gap-2 px-6 py-3 
                 bg-gradient-to-r from-indigo-500 to-purple-600 
                 text-white font-semibold text-lg rounded-2xl 
                 shadow-lg hover:scale-105 hover:shadow-xl 
                 transition-all duration-200 disabled:opacity-70"
    >
      {loading ? (
        <span className="flex items-center gap-2">
          <svg
            className="animate-spin h-5 w-5 text-white"
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
          >
            <circle
              className="opacity-25"
              cx="12"
              cy="12"
              r="10"
              stroke="currentColor"
              strokeWidth="4"
            ></circle>
            <path
              className="opacity-75"
              fill="currentColor"
              d="M4 12a8 8 0 018-8v4l3-3-3-3v4a8 8 0 100 16v-4l-3 3 3 3v-4a8 8 0 01-8-8z"
            ></path>
          </svg>
          Szukam drużyny...
        </span>
      ) : (
        <span className="flex items-center gap-2">
          <Search className="w-5 h-5" />
          Znajdź swój zespół 🚀
        </span>
      )}
    </button>
  );
}

export default Dashboard;