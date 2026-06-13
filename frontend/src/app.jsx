import { BrowserRouter, Route, Routes } from 'react-router-dom';
import { Suspense } from 'react';
import { ThemeProvider } from "@/components/theme-provider";
import { AuthProvider } from '@/contexts/AuthContext';
import { ProtectedRoute } from '@/components/ProtectedRoute';
import { Login } from './pages/login/login';
import Dashboard from './pages/dashboard/dashboard';
import DashboardRecords from './pages/dashboard/domains/[domainId]/dashboard-records';
import AdminDashboard from './pages/dashboard/admin/admin-dashboard';
import NotFound from './pages/not-found';
import { Unauthorized } from './pages/unauthorized';
import Loading from './pages/loading';
import Hero from './pages/hero';

const App = () => {
  return (
    <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
      <AuthProvider>
        <BrowserRouter>
          <Routes>
            <Route path="/login" element={
              <Suspense fallback={<Loading />}>
                <Login />
              </Suspense>
            } />
            
            <Route path="/dashboard" redirectTo="/login" element={
              <ProtectedRoute>
                <Suspense fallback={<Loading />}>
                  <Dashboard />
                </Suspense>
              </ProtectedRoute>
            } />

            <Route path="/dashboard/domains/:domainId" redirectTo="/login" element={
              <ProtectedRoute>
                <Suspense fallback={<Loading />}>
                  <DashboardRecords />
                </Suspense>
              </ProtectedRoute>
            } />
            
            <Route path="/admin" element={
              <ProtectedRoute requireAdmin redirectTo="/dashboard">
                <Suspense fallback={<Loading />}>
                  <AdminDashboard />
                </Suspense>
              </ProtectedRoute>
            } />
            
            <Route path="/unauthorized" element={
              <Suspense fallback={<Loading />}>
                <Unauthorized />
              </Suspense>
            } />

            <Route path="/" element={
              <Suspense fallback={<Loading />}>
                <Hero />
              </Suspense>
            } />
            
            <Route path="*" element={
              <Suspense fallback={<Loading />}>
                <NotFound />
              </Suspense>
            } />
          </Routes>
        </BrowserRouter>
      </AuthProvider>
    </ThemeProvider>
  );
}

export default App;