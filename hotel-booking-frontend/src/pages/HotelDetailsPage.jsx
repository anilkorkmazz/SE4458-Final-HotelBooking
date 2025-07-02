import React, { useEffect, useState } from "react";
import { useParams, Link } from "react-router-dom";

const HotelDetailsPage = () => {
  const { hotelId } = useParams();
  const [hotel, setHotel] = useState(null);
  const [rooms, setRooms] = useState([]);
  const [bookingData, setBookingData] = useState({});
  const storedUsername = localStorage.getItem("username") || "";
  const isLoggedIn = !!storedUsername;

  useEffect(() => {
    fetch(`http://localhost:5009/api/v1/Hotel/${hotelId}`)
      .then((res) => res.json())
      .then((data) => setHotel(data))
      .catch((err) => console.error("Hotel fetch failed:", err));

    reloadRooms();
  }, [hotelId]);

  const reloadRooms = () => {
    fetch(`http://localhost:5009/api/v1/Room?hotelId=${hotelId}`)
      .then((res) => res.json())
      .then((data) => setRooms((data.items || []).filter(r => r.capacity > 0)))
      .catch((err) => console.error("Rooms fetch failed:", err));
  };

  const handleInputChange = (roomId, field, value) => {
    setBookingData((prev) => ({
      ...prev,
      [roomId]: {
        ...prev[roomId],
        [field]: value,
      },
    }));
  };

  const handleBooking = (roomId) => {
    const data = bookingData[roomId];
    if (!data?.startDate || !data?.endDate || !data?.peopleCount) {
      alert("Please fill all fields.");
      return;
    }

    fetch("http://localhost:5009/api/v1/Reservation", {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify({
        roomId,
        username: storedUsername,
        ...data,
      }),
    })
      .then((res) => {
        if (!res.ok) throw new Error("Reservation failed");
        return res.json();
      })
      .then(() => {
        alert("✅ Reservation successful!");
        setBookingData((prev) => ({ ...prev, [roomId]: {} }));
        reloadRooms();
      })
      .catch(() => alert("❌ Reservation failed."));
  };

  if (!hotel) return <div className="p-8 text-center">Loading...</div>;

  return (
    <div className="p-8 max-w-4xl mx-auto">
      <h1 className="text-3xl font-bold mb-2">{hotel.name}</h1>
      <p className="text-gray-600 mb-1">{hotel.location}</p>
      <p className="mb-4">{hotel.description}</p>

      {/* 💬 Yorumları Gör butonu */}
      <Link
        to={`/comments/${hotel.id}`}
        className="inline-block bg-blue-100 text-blue-800 px-4 py-2 rounded hover:bg-blue-200 transition mb-8"
      >
        💬 Yorumları Gör
      </Link>

      <h2 className="text-2xl font-semibold mb-4">Available Rooms</h2>
      <div className="space-y-8">
        {rooms.map((room) => (
          <div key={room.id} className="border p-4 rounded shadow">
            <p><strong>Room:</strong> {room.roomNumber}</p>
            <p><strong>Capacity:</strong> {room.capacity}</p>
            <p>
              <strong>Price:</strong>{" "}
              {isLoggedIn
                ? `${(room.pricePerNight * 0.85).toFixed(2)}₺ (15% indirimli)`
                : `${room.pricePerNight}₺`}
            </p>
            <p>
              <strong>Available:</strong>{" "}
              {room.availableFrom.split("T")[0]} → {room.availableTo.split("T")[0]}
            </p>

            <div className="grid grid-cols-2 gap-2 mt-4">
              <input
                type="text"
                value={storedUsername}
                readOnly
                className="border px-2 py-1 bg-gray-100 text-gray-500"
              />
              <input
                type="number"
                placeholder="People Count"
                className="border px-2 py-1"
                value={bookingData[room.id]?.peopleCount || ""}
                onChange={(e) =>
                  handleInputChange(room.id, "peopleCount", parseInt(e.target.value))
                }
              />



              <input
                type="date"
                className="border px-2 py-1"
                value={bookingData[room.id]?.startDate || ""}
                min={room.availableFrom.split("T")[0]}
                max={room.availableTo.split("T")[0]}
                onChange={(e) =>
                  handleInputChange(room.id, "startDate", e.target.value)
                }
              />
              <input
                type="date"
                className="border px-2 py-1"
                value={bookingData[room.id]?.endDate || ""}
                min={room.availableFrom.split("T")[0]}
                max={room.availableTo.split("T")[0]}
                onChange={(e) =>
                  handleInputChange(room.id, "endDate", e.target.value)
                }
              />
            </div>

            <button
              disabled={
                !bookingData[room.id]?.startDate ||
                !bookingData[room.id]?.endDate ||
                !bookingData[room.id]?.peopleCount
              }
              onClick={() => handleBooking(room.id)}
              className="mt-4 px-4 py-2 bg-blue-600 text-white rounded hover:bg-blue-700 disabled:opacity-50"
            >
              Book this Room
            </button>
          </div>
        ))}
      </div>
    </div>
  );
};

export default HotelDetailsPage;
