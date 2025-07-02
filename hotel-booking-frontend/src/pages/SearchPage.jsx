import React, { useState } from "react";

const SearchPage = () => {
  const [formData, setFormData] = useState({
    location: "",
    startDate: "",
    endDate: "",
    peopleCount: 1,
  });

  const [results, setResults] = useState([]);
  const [message, setMessage] = useState("");

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
  };

  const handleSearch = async () => {
    const { location, startDate, endDate, peopleCount } = formData;
    if (!location || !startDate || !endDate || !peopleCount) {
      alert("Lütfen tüm alanları doldurun.");
      return;
    }

    const params = new URLSearchParams({
      location,
      startDate,
      endDate,
      peopleCount,
      page: 1,
      pageSize: 10,
    });

    try {
      const res = await fetch(`http://localhost:5009/api/v1/Room/search?${params}`);
      const data = await res.json();

      if (data.message === "Uygun oda bulunamadi.") {
        setResults([]);
        setMessage(data.message);
      } else {
        setResults(data.items || []);
        setMessage("");
      }
    } catch (error) {
      console.error("Arama hatası:", error);
      setMessage("Arama sırasında hata oluştu.");
    }
  };

  return (
    <div className="max-w-4xl mx-auto p-6">
      <h1 className="text-3xl font-bold mb-6">🔍 Otel Arama</h1>



      <div className="grid grid-cols-1 md:grid-cols-2 gap-4 mb-6">
        <div className="flex flex-col">
          <label className="mb-1 text-sm font-medium text-gray-700" htmlFor="location">
            📍 Lokasyon
          </label>
          <input
            type="text"
            id="location"
            name="location"
            placeholder="Örn: İstanbul, Rome, Paris"
            value={formData.location}
            onChange={handleChange}
            className="border px-3 py-2 rounded"
          />
        </div>

        <div className="flex flex-col">
          <label className="mb-1 text-sm font-medium text-gray-700" htmlFor="peopleCount">
            👥 Kişi Sayısı
          </label>
          <input
            type="number"
            id="peopleCount"
            name="peopleCount"
            placeholder="Örn: 2"
            value={formData.peopleCount}
            onChange={handleChange}
            className="border px-3 py-2 rounded"
          />
        </div>

        <div className="flex flex-col">
          <label className="mb-1 text-sm font-medium text-gray-700" htmlFor="startDate">
            🗓️ Giriş Tarihi
          </label>
          <input
            type="date"
            id="startDate"
            name="startDate"
            value={formData.startDate}
            onChange={handleChange}
            className="border px-3 py-2 rounded"
          />
        </div>

        <div className="flex flex-col">
          <label className="mb-1 text-sm font-medium text-gray-700" htmlFor="endDate">
            🗓️ Çıkış Tarihi
          </label>
          <input
            type="date"
            id="endDate"
            name="endDate"
            value={formData.endDate}
            onChange={handleChange}
            className="border px-3 py-2 rounded"
          />
        </div>
      </div>







      <button
        onClick={handleSearch}
        className="bg-blue-600 text-white px-6 py-2 rounded hover:bg-blue-700"
      >
        Ara
      </button>

      {message && <p className="mt-4 text-red-600">{message}</p>}

      <div className="mt-8 space-y-4">
        {results.map((room) => (
          <div key={room.id} className="border p-4 rounded shadow">
            <p><strong>Hotel:</strong> {room.hotel.name}</p>
            <p><strong>Location:</strong> {room.hotel.location}</p>
            <p><strong>Room:</strong> {room.roomNumber}</p>
            <p><strong>Capacity:</strong> {room.capacity}</p>
            <p><strong>Price:</strong> {room.pricePerNight}₺ / night</p>
            <p>
              <strong>Available:</strong> {room.availableFrom.split("T")[0]} →{" "}
              {room.availableTo.split("T")[0]}
            </p>
          </div>
        ))}
      </div>
    </div>
  );
};

export default SearchPage;
