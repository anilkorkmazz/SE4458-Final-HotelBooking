import { useParams } from "react-router-dom";
import { useEffect, useState } from "react";
import axios from "axios";
import { Bar } from "react-chartjs-2";
import {
  Chart as ChartJS,
  BarElement,
  CategoryScale,
  LinearScale,
  Tooltip,
  Legend,
} from "chart.js";

ChartJS.register(BarElement, CategoryScale, LinearScale, Tooltip, Legend);

function CommentsPage() {
  const { hotelId } = useParams();
  const [comments, setComments] = useState([]);
  const [distribution, setDistribution] = useState({});
  const [newComment, setNewComment] = useState({ text: "", rating: 5 });
  const username = localStorage.getItem("username");

  useEffect(() => {
    fetchComments();
    fetchDistribution();
  }, [hotelId]);

  const fetchComments = async () => {
    try {
      const res = await axios.get(`http://localhost:5009/api/v1/Comments/hotel/${hotelId}`);
      setComments(res.data);
    } catch (err) {
      console.error("Yorumlar alınamadı:", err);
    }
  };

  const fetchDistribution = async () => {
    try {
      const res = await axios.get(`http://localhost:5009/api/v1/Comments/distribution/${hotelId}`);
      setDistribution(res.data);
    } catch (err) {
      console.error("Dağılım alınamadı:", err);
    }
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (!username) {
      alert("Yorum yapabilmek için giriş yapmalısınız.");
      return;
    }

    try {
      await axios.post("http://localhost:5009/api/v1/Comments", {
        userName: username,
        text: newComment.text,
        rating: newComment.rating,
        hotelId: parseInt(hotelId),
      });

      setNewComment({ text: "", rating: 5 });
      fetchComments();
      fetchDistribution();
      alert("✅ Yorum başarıyla eklendi!");
    } catch (err) {
      console.error("Yorum eklenemedi:", err);
      alert("❌ Yorum eklenirken hata oluştu.");
    }
  };

  const chartData = {
    labels: Object.keys(distribution),
    datasets: [
      {
        label: "Yorum Sayısı",
        data: Object.values(distribution),
        backgroundColor: "#60a5fa",
        borderRadius: 6,
      },
    ],
  };

  return (
    <div className="max-w-4xl mx-auto px-4 py-8">
      <h2 className="text-2xl font-semibold mb-6 text-gray-800">📊 Yorum Dağılımı</h2>
      <Bar data={chartData} />

      <h3 className="text-xl font-semibold mt-10 mb-4 text-gray-700">📝 Tüm Yorumlar</h3>
      <div className="space-y-4 mb-8">
        {comments.map((c, index) => (
          <div key={index} className="p-4 bg-gray-100 rounded-xl shadow-sm">
            <p className="font-semibold text-gray-800">{c.username}</p>
            <p className="text-gray-600">{c.text}</p>
            <p className="text-yellow-600">⭐ {c.stars || c.rating} yıldız</p>
          </div>
        ))}
      </div>

      {username ? (
        <form onSubmit={handleSubmit} className="bg-white border rounded-xl p-6 shadow-sm">
          <h4 className="text-lg font-semibold mb-4 text-gray-700">💬 Yorum Ekle</h4>
          <textarea
            value={newComment.text}
            onChange={(e) => setNewComment({ ...newComment, text: e.target.value })}
            rows="4"
            className="w-full border p-2 mb-4 rounded"
            placeholder="Yorumunuzu yazın..."
            required
          />
          <div className="mb-4">
            <label className="block mb-1 font-medium">Puan (1-5):</label>
            <select
              value={newComment.rating}
              onChange={(e) => setNewComment({ ...newComment, rating: parseInt(e.target.value) })}
              className="border p-2 rounded"
            >
              {[1, 2, 3, 4, 5].map((star) => (
                <option key={star} value={star}>{star}</option>
              ))}
            </select>
          </div>
          <button
            type="submit"
            className="px-6 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
          >
            Gönder
          </button>
        </form>
      ) : (
        <p className="text-gray-500">Yorum yapabilmek için giriş yapmalısınız.</p>
      )}
    </div>
  );
}

export default CommentsPage;
