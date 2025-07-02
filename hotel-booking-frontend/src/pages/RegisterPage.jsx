// pages/RegisterPage.jsx
import { useState } from "react";
import { useNavigate } from "react-router-dom";

const API_URL = "http://localhost:5009/api/v1/Auth/register";

function RegisterPage() {
  const [formData, setFormData] = useState({
    username: "",
    password: "",
  });
  const [error, setError] = useState("");
  const navigate = useNavigate();

  const handleChange = (e) => {
    setFormData((prev) => ({
      ...prev,
      [e.target.name]: e.target.value,
    }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    const res = await fetch(API_URL, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: JSON.stringify(formData),
    });

    if (res.ok) {
      alert("✅ Kayıt başarılı! Giriş yapabilirsiniz.");
      navigate("/login");
    } else {
      setError("❌ Kayıt başarısız. Kullanıcı adı alınmış olabilir.");
    }
  };

  return (
    <div className="max-w-md mx-auto p-6 border rounded mt-12 shadow">
      <h2 className="text-2xl font-bold mb-4 text-center">📝 Kayıt Ol</h2>

      <form onSubmit={handleSubmit} className="space-y-4">
        <input
          type="text"
          name="username"
          placeholder="Kullanıcı Adı"
          value={formData.username}
          onChange={handleChange}
          required
          className="w-full border px-4 py-2 rounded"
        />
        <input
          type="password"
          name="password"
          placeholder="Şifre"
          value={formData.password}
          onChange={handleChange}
          required
          className="w-full border px-4 py-2 rounded"
        />
        {error && <p className="text-red-600">{error}</p>}

        <button
          type="submit"
          className="w-full bg-blue-600 text-white px-4 py-2 rounded hover:bg-blue-700"
        >
          Kayıt Ol
        </button>
      </form>
    </div>
  );
}

export default RegisterPage;
