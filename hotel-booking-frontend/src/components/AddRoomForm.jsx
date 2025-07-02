import React, { useState, useEffect } from 'react';
import axios from 'axios';

const AddRoomForm = () => {
  const [hotels, setHotels] = useState([]);
  const [roomData, setRoomData] = useState({
    roomNumber: '',
    capacity: 1,
    pricePerNight: 100,
    availableFrom: '',
    availableTo: '',
    hotelId: ''
  });

  const token = localStorage.getItem('token');

  useEffect(() => {
    const fetchHotels = async () => {
      try {
        const response = await axios.get('http://localhost:5009/api/v1/Hotel/paged?page=1&pageSize=100');
        setHotels(response.data.items);
      } catch (err) {
        console.error('❌ Error fetching hotels:', err);
      }
    };
    fetchHotels();
  }, []);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setRoomData((prev) => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    try {
      await axios.post('http://localhost:5009/api/v1/Room', roomData, {
        headers: { Authorization: `Bearer ${token}` }
      });
      alert('✅ Room added successfully!');
      setRoomData({
        roomNumber: '',
        capacity: 1,
        pricePerNight: 100,
        availableFrom: '',
        availableTo: '',
        hotelId: ''
      });
    } catch (err) {
      console.error('❌ Error adding room:', err);
      alert('Error adding room');
    }
  };

  return (
    <div className="p-6 bg-white rounded-xl shadow-md max-w-xl mx-auto mt-8">
      <h2 className="text-2xl font-semibold mb-4">➕ Add New Room</h2>
      <form onSubmit={handleSubmit} className="grid gap-4">
        <input
          type="text"
          name="roomNumber"
          placeholder="Room Number"
          value={roomData.roomNumber}
          onChange={handleChange}
          required
          className="border p-2 rounded"
        />
        <input
          type="number"
          name="capacity"
          placeholder="Capacity"
          value={roomData.capacity}
          onChange={handleChange}
          required
          className="border p-2 rounded"
        />
        <input
          type="number"
          name="pricePerNight"
          placeholder="Price Per Night"
          value={roomData.pricePerNight}
          onChange={handleChange}
          required
          className="border p-2 rounded"
        />
        <input
          type="date"
          name="availableFrom"
          value={roomData.availableFrom}
          onChange={handleChange}
          required
          className="border p-2 rounded"
        />
        <input
          type="date"
          name="availableTo"
          value={roomData.availableTo}
          onChange={handleChange}
          required
          className="border p-2 rounded"
        />
        <select
          name="hotelId"
          value={roomData.hotelId}
          onChange={handleChange}
          required
          className="border p-2 rounded"
        >
          <option value="">Select Hotel</option>
          {hotels.map((hotel) => (
            <option key={hotel.id} value={hotel.id}>
              {hotel.name} – {hotel.location}
            </option>
          ))}
        </select>
        <button
          type="submit"
          className="bg-blue-600 text-white p-2 rounded hover:bg-blue-700"
        >
          Add Room
        </button>
      </form>
    </div>
  );
};

export default AddRoomForm;
