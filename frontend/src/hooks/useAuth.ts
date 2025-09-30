import { useCallback, useEffect } from 'react';
import { useAuthStore } from '../stores/authStore';
import { LoginDto, RegisterDto } from '../types/api';

export const useAuth = () => {
  const store = useAuthStore();

  // Initialize auth on mount
  useEffect(() => {
    store.initializeAuth();
  }, [store.initializeAuth]);

  const login = useCallback(async (credentials: LoginDto) => {
    try {
      const success = await store.login(credentials);
      return { success };
    } catch (error) {
      return {
        success: false,
        error: error instanceof Error ? error.message : 'Login failed',
      };
    }
  }, [store.login]);

  const register = useCallback(async (userData: RegisterDto) => {
    try {
      const success = await store.register(userData);
      return { success };
    } catch (error) {
      return {
        success: false,
        error: error instanceof Error ? error.message : 'Registration failed',
      };
    }
  }, [store.register]);

  const logout = useCallback(() => {
    try {
      store.logout();
      return { success: true };
    } catch (error) {
      return {
        success: false,
        error: error instanceof Error ? error.message : 'Logout failed',
      };
    }
  }, [store.logout]);

  const clearError = useCallback(() => {
    store.clearError();
  }, [store.clearError]);

  return {
    // State
    user: store.user,
    isLoading: store.isLoading,
    error: store.error,
    isAuthenticated: store.isAuthenticated, 
    isInitialized: store.isInitialized,   

    // Actions
    login,
    register,
    logout,
    clearError,
  };
};
