import React, { useEffect, useState } from "react";
import { Link } from "react-router-dom";

const HotelListPage = () => {
  const [hotels, setHotels] = useState([]);

  useEffect(() => {
    fetch("http://localhost:5009/api/v1/Hotel")
      .then((res) => res.json())
      .then((data) => setHotels(data))
      .catch((err) => console.error("Hotel fetch failed:", err));
  }, []);

  return (
    <div className="max-w-5xl mx-auto p-6">
      <h1 className="text-3xl font-bold mb-6">Available Hotels</h1>
      <div className="space-y-6">
        {hotels.map((hotel) => (
          <div key={hotel.id} className="border rounded p-4 shadow">
            <h2 className="text-2xl font-semibold">{hotel.name}</h2>
            <p className="text-gray-600">{hotel.location}</p>
            <p className="mb-2">{hotel.description}</p>
            <Link
              to={`/hotels/${hotel.id}`}
              className="text-blue-600 underline"
            >
              View Details →
            </Link>
          </div>
        ))}
      </div>
    </div>
  );
};

export default HotelListPage;
