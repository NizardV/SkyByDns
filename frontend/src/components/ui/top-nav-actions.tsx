import { useNavigate, useLocation } from "react-router-dom";
import { Button } from "@/components/ui/button";
import { useAuth } from "@/contexts/AuthContext";
import { toast } from "sonner";
import { AppTour } from "@/components/tour";
import { useState } from "react";

export default function TopNavActions() {
  const { user, logout } = useAuth();
  const navigate = useNavigate();
  const location = useLocation();

  const [isTourOpen, setIsTourOpen] = useState(false);

  const isAdmin = user?.role === "Admin";
  const isOnAdminPage = location.pathname.startsWith("/admin");
  const isOnDashboard = location.pathname.startsWith("/dashboard");

  const handleSwitch = () => {
    if (isOnAdminPage) {
      navigate("/dashboard");
    } else {
      navigate("/admin");
    }
  };

  const handleLogout = () => {
    logout();
    toast.success("Logged out successfully");
    navigate("/login");
  };

  return (
    <>
      <AppTour open={isTourOpen} setOpen={setIsTourOpen} />
      <div className="flex gap-3 absolute top-4 right-4">
        {/* 🔁 Switch between dashboards (Admin only) */}
        {isAdmin && (isOnAdminPage || isOnDashboard) && (
          <Button variant="secondary" onClick={handleSwitch}>
            {isOnAdminPage ? "Go to Dashboard" : "Go to Admin"}
          </Button>
        )}

        {/* 🚪 Logout */}
        <Button variant="destructive" onClick={handleLogout} id="feature-4">
          Logout
        </Button>

        {/* 🧭 App Tour */}
        {isOnDashboard && (
          <Button variant="outline" onClick={() => setIsTourOpen(true)} id="feature-5" >
            Take a Tour
          </Button>
        )}
      </div>
    </>
  );
}
