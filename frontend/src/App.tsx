import React, { useEffect } from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { ThemeProvider, createTheme, CssBaseline } from '@mui/material';
import { Layout } from './components/layout/Layout';
import { ProtectedRoute } from './components/common/ProtectedRoute';
import { ErrorBoundary } from './components/common/ErrorBoundary';
import { ToastProvider } from './contexts/ToastContext';
import { ErrorProvider } from './contexts/ErrorContext';
import { ProductList } from './components/products/ProductList';
import { Cart } from './components/cart/Cart';
import { Login } from './components/auth/Login';
import { Register } from './components/auth/Register';
import { Profile } from './components/profile/Profile';
import { useAuth, useCart } from './hooks';
import { NotFound } from './components/common/NotFound';

// Create Material-UI theme
const theme = createTheme({
  palette: {
    primary: { main: '#1976d2' },
    secondary: { main: '#dc004e' },
  },
  typography: {
    h4: { fontWeight: 600 },
  },
});

function App() {
  const { user, isAuthenticated, isInitialized } = useAuth();
  const { loadCart } = useCart();

  useEffect(() => {
    loadCart(user?.id);
  }, [user, loadCart]);

  if (!isInitialized) {
    // Show spinner until auth state is known
    return <div>Loading...</div>;
  }

  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <ToastProvider>
        <ErrorProvider>
          <ErrorBoundary showErrorDetails={process.env.NODE_ENV === 'development'}>
            <Router>
              <Layout>
                <Routes>
                  {/* Always redirect root */}
                  <Route path="/" element={<Navigate to="/products" replace />} />

                  {/* Public routes (always exist) */}
                  <Route path="/products" element={<ProductList />} />
                  <Route path="/cart" element={<Cart />} />
                  <Route
                    path="/login"
                    element={isAuthenticated ? <Navigate to="/products" replace /> : <Login />}
                  />
                  <Route
                    path="/register"
                    element={isAuthenticated ? <Navigate to="/products" replace /> : <Register />}
                  />

                  {/* Protected route */}
                  <Route
                    path="/profile"
                    element={
                      <ProtectedRoute>
                        <Profile />
                      </ProtectedRoute>
                    }
                  />

                  {/* Fallbacks */}
                  <Route
                    path="*"
                    element={
                      isAuthenticated ? <NotFound /> : <Navigate to="/login" replace />
                    }
                  />
                </Routes>
              </Layout>
            </Router>
          </ErrorBoundary>
        </ErrorProvider>
      </ToastProvider>
    </ThemeProvider>
  );
}

export default App;