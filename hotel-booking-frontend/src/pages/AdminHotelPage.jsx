import { useEffect, useState } from "react";

const API_BASE = "http://localhost:5009/api/v1";

function AdminHotelPage() {
  const [hotels, setHotels] = useState([]);
  const [rooms, setRooms] = useState([]);
  const [successMessage, setSuccessMessage] = useState(""); // ✅ başarı mesajı için
  const [form, setForm] = useState({
    roomNumber: "",
    capacity: 1,
    pricePerNight: 1000,
    availableFrom: "",
    availableTo: "",
    hotelId: ""
  });
  const [editingRoomId, setEditingRoomId] = useState(null);

  useEffect(() => {
    fetchHotels();
    fetchRooms();
  }, []);

  const fetchHotels = async () => {
    const res = await fetch(`${API_BASE}/Hotel`);
    const data = await res.json();
    setHotels(data); // Eğer `data.items` varsa ona göre düzelt
  };

  const fetchRooms = async () => {
    const res = await fetch(`${API_BASE}/Room?page=1&pageSize=100`);
    const data = await res.json();
    setRooms((data.items || []).filter(room => room.capacity > 0));
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const token = localStorage.getItem("token");
  
    
    if (new Date(form.availableTo) <= new Date(form.availableFrom)) {
      alert("❌ Bitiş tarihi, başlangıç tarihinden önce olamaz.");
      return;
    }
  
    const method = editingRoomId ? "PUT" : "POST";
    const endpoint = editingRoomId ? `${API_BASE}/Room/${editingRoomId}` : `${API_BASE}/Room`;
  
    const res = await fetch(endpoint, {
      method,
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`
      },
      body: JSON.stringify({
        ...form,
        capacity: parseInt(form.capacity),
        pricePerNight: parseFloat(form.pricePerNight),
        hotelId: parseInt(form.hotelId)
      })
    });
  
    if (res.ok) {
      await fetchRooms();
      setForm({
        roomNumber: "",
        capacity: 1,
        pricePerNight: 1000,
        availableFrom: "",
        availableTo: "",
        hotelId: ""
      });
      setEditingRoomId(null);
      setSuccessMessage("✅ Oda başarıyla eklendi!");
      setTimeout(() => setSuccessMessage(""), 3000);
    } else {
      alert("❌ Kayıt işlemi başarısız oldu.");
    }
  };
  

  const handleEdit = (room) => {
    setForm({
      roomNumber: room.roomNumber,
      capacity: room.capacity,
      pricePerNight: room.pricePerNight,
      availableFrom: room.availableFrom?.split("T")[0],
      availableTo: room.availableTo?.split("T")[0],
      hotelId: room.hotel?.id
    });
    setEditingRoomId(room.id);
  };

  return (
    <div className="max-w-3xl mx-auto">
      <h2 className="text-2xl font-bold mb-4">
        {editingRoomId ? "📝 Odayı Güncelle" : "➕ Yeni Oda Ekle"}
      </h2>

      {/* ✅ Başarı mesajı */}
      {successMessage && (
        <div className="bg-green-100 text-green-800 border border-green-300 px-4 py-2 rounded mb-4">
          {successMessage}
        </div>
      )}

      <form onSubmit={handleSubmit} className="space-y-4 bg-white p-4 rounded shadow">
        <div className="grid grid-cols-2 gap-4">
          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1" htmlFor="roomNumber">Oda Numarası</label>
            <input
              type="text"
              id="roomNumber"
              name="roomNumber"
              value={form.roomNumber}
              onChange={handleChange}
              className="border p-2 rounded"
              required
            />
          </div>

          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1" htmlFor="capacity">Kapasite (Kişi)</label>
            <input
              type="number"
              id="capacity"
              name="capacity"
              value={form.capacity}
              onChange={handleChange}
              className="border p-2 rounded"
              required
            />
          </div>

          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1" htmlFor="pricePerNight">Gecelik Ücret (₺)</label>
            <input
              type="number"
              id="pricePerNight"
              name="pricePerNight"
              value={form.pricePerNight}
              onChange={handleChange}
              className="border p-2 rounded"
              required
            />
          </div>

          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1" htmlFor="availableFrom">Başlangıç Tarihi</label>
            <input
              type="date"
              id="availableFrom"
              name="availableFrom"
              value={form.availableFrom}
              onChange={handleChange}
              className="border p-2 rounded"
              required
            />
          </div>

          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1" htmlFor="availableTo">Bitiş Tarihi</label>
            <input
              type="date"
              id="availableTo"
              name="availableTo"
              value={form.availableTo}
              onChange={handleChange}
              className="border p-2 rounded"
              required
            />
          </div>

          <div className="flex flex-col">
            <label className="text-sm font-medium mb-1" htmlFor="hotelId">Bağlı Otel</label>
            <select
              id="hotelId"
              name="hotelId"
              value={form.hotelId}
              onChange={handleChange}
              className="border p-2 rounded"
              required
            >
              <option value="">Otel Seç</option>
              {hotels.map((hotel) => (
                <option key={hotel.id} value={hotel.id}>
                  {hotel.name}
                </option>
              ))}
            </select>
          </div>
        </div>

        <button
          type="submit"
          className="bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          {editingRoomId ? "Güncelle" : "Ekle"}
        </button>
      </form>

      <h2 className="text-2xl font-semibold mt-8 mb-4">🛏️ Mevcut Odalar</h2>
      <div className="space-y-3">
        {rooms.map((room) => (
          <div
            key={room.id}
            className="border p-3 rounded shadow flex justify-between items-center"
          >
            <div>
              <p className="font-semibold">
                Oda: {room.roomNumber} | Otel: {room.hotel?.name || "Bilinmiyor"}
              </p>
              <p>
                Kapasite: {room.capacity} kişi | Fiyat: {room.pricePerNight} ₺
              </p>
              <p>
                Tarih Aralığı: {room.availableFrom?.split("T")[0]} –{" "}
                {room.availableTo?.split("T")[0]}
              </p>
            </div>
            <button
              onClick={() => handleEdit(room)}
              className="bg-green-600 text-white px-3 py-1 rounded hover:bg-green-700"
            >
              Düzenle
            </button>
          </div>
        ))}
      </div>
    </div>
  );
}

export default AdminHotelPage;
