import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import HomePage from "./pages/HomePage";
import SearchPage from "./pages/SearchPage";
import BookingPage from "./pages/BookingPage";
import CommentsPage from "./pages/CommentsPage";
import AgentPage from "./pages/AgentPage";
import HotelListPage from './pages/HotelListPage';
import HotelDetailsPage from './pages/HotelDetailsPage';
import AdminHotelPage from './pages/AdminHotelPage';
import Navbar from './components/Navbar';
import LoginPage from "./pages/LoginPage";
import AdminPanel from './pages/AdminPanel';
import RegisterPage from './pages/RegisterPage';

import AddHotelPage from './pages/AddHotelPage';



function App() {
  return (
    <Router>
      <Navbar />
      <div className="px-4 py-6">
        <Routes>
          <Route path="/" element={<HomePage />} />
          <Route path="/search" element={<SearchPage />} />
          <Route path="/book" element={<BookingPage />} />
          <Route path="/comments/:hotelId" element={<CommentsPage />} />
          <Route path="/agent" element={<AgentPage />} />
          <Route path="/hotels" element={<HotelListPage />} />
          <Route path="/hotels/:hotelId" element={<HotelDetailsPage />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          
          {/* 🧩 Nested Admin Routes */}
          <Route path="/admin/" element={<AdminPanel />}>
            <Route path="hotels" element={<AdminHotelPage />} />
            <Route path="add-hotel" element={<AddHotelPage />} /> 
          </Route>
        </Routes>
      </div>
    </Router>
  );
}

export default App;
