import React from 'react';
import { Link, useNavigate } from 'react-router-dom';

const Navbar = () => {
  const username = localStorage.getItem('username');
  const role = localStorage.getItem('role');
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.clear();
    navigate('/login');
  };

  return (
    <nav className="bg-gray-800 p-4 text-white flex justify-between items-center">
      <div className="space-x-4">
        <Link to="/" className="hover:underline">Home</Link>
        <Link to="/hotels" className="hover:underline">Hotels</Link>
        <Link to="/search" className="hover:underline">Search</Link>
        <Link to="/agent" className="hover:underline">Agent</Link> {/* Yeni eklendi */}
        {role === 'Admin' && (
          <Link to="/admin" className="hover:underline">Admin Panel</Link>
        )}
      </div>

      <div className="space-x-4 flex items-center">
        {username ? (
          <>
            <span className="font-semibold">👤 {username}</span>
            <button onClick={handleLogout} className="hover:underline">
              Logout
            </button>
          </>
        ) : (
          <>
            <Link to="/login" className="hover:underline">Login</Link>
            <Link to="/register" className="hover:underline">Register</Link>
          </>
        )}
      </div>
    </nav>
  );
};

export default Navbar;
