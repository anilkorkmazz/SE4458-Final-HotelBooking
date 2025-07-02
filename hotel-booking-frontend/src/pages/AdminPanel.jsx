import { Link, Outlet } from "react-router-dom";

function AdminPanel() {
  return (
    <div className="min-h-screen flex flex-col md:flex-row bg-gray-100">
      {/* Sidebar */}
      <aside className="w-full md:w-64 bg-white shadow-md md:min-h-screen p-6">
        <h2 className="text-2xl font-bold text-gray-800 mb-8 border-b pb-2">⚙️ Admin Panel</h2>
        <nav className="flex flex-col gap-4">
          <Link
            to="hotels"
            className="py-2 px-4 rounded-md bg-blue-600 text-white text-center hover:bg-blue-700 transition"
          >
            🏨 Manage Hotels
          </Link>
          <Link
            to="add-hotel"
            className="py-2 px-4 rounded-md bg-green-600 text-white text-center hover:bg-green-700 transition"
          >
            ➕ Add New Hotel
          </Link>
        </nav>
      </aside>

      {/* Content */}
      <main className="flex-1 p-6">
        <Outlet />
      </main>
    </div>
  );
}

export default AdminPanel;
