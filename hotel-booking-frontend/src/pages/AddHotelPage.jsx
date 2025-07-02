import { useState } from "react";
import axios from "axios";

function AddHotelPage() {
  const [hotel, setHotel] = useState({
    name: "",
    location: "",
    description: "",
  });

  const handleChange = (e) => {
    setHotel({ ...hotel, [e.target.name]: e.target.value });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios.post("http://localhost:5009/api/v1/Hotel", hotel, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("token")}`,
        },
      });
      alert("✅ Hotel added!");
      setHotel({ name: "", location: "", description: "" });
    } catch (err) {
      alert("❌ Failed to add hotel.");
      console.error(err);
    }
  };

  return (
    <div className="max-w-xl mx-auto mt-8">
      <h2 className="text-2xl font-bold mb-4">🛎️ Add New Hotel</h2>
      <form onSubmit={handleSubmit} className="space-y-4">
        <input
          name="name"
          value={hotel.name}
          onChange={handleChange}
          placeholder="Hotel Name"
          className="w-full border p-2 rounded"
          required
        />
        <input
          name="location"
          value={hotel.location}
          onChange={handleChange}
          placeholder="Location"
          className="w-full border p-2 rounded"
          required
        />
        <textarea
          name="description"
          value={hotel.description}
          onChange={handleChange}
          placeholder="Description"
          className="w-full border p-2 rounded"
          rows="4"
        />
        <button type="submit" className="px-4 py-2 bg-blue-600 text-white rounded">
          ➕ Add Hotel
        </button>
      </form>
    </div>
  );
}

export default AddHotelPage;
