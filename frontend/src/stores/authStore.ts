import { create } from 'zustand';
import { UserDto, LoginDto, RegisterDto, LoginResponseDto } from '../types/api';
import { authApi } from '../services/api';
import { localStorageService } from '../services/localStorage';

interface AuthState {
  user: UserDto | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  error: string | null;
  isInitialized: boolean;
}

interface AuthActions {
  login: (credentials: LoginDto) => Promise<boolean>;
  register: (userData: RegisterDto) => Promise<boolean>;
  logout: () => void;
  clearError: () => void;
  initializeAuth: () => void;
}

export const useAuthStore = create<AuthState & AuthActions>((set, get) => ({
  // State
  user: null,
  isAuthenticated: false,
  isLoading: false,
  error: null,
  isInitialized: false, // NEW

  // Actions
  login: async (credentials: LoginDto): Promise<boolean> => {
    set({ isLoading: true, error: null });

    try {
      const response = await authApi.login(credentials);

      if (response.success && 'data' in response) {
        const loginResponse = response.data as LoginResponseDto;

        // Store token and user
        localStorageService.setAuthToken(loginResponse.token);
        localStorageService.setUser(loginResponse.user);

        set({
          user: loginResponse.user,
          isAuthenticated: true,
          isLoading: false,
          error: null,
          isInitialized: true, //ensure we’re initialized after login
        });

        return true;
      } else {
        const errorMessage = 'error' in response ? response.error : 'Login failed';
        set({
          error: errorMessage,
          isLoading: false,
          isInitialized: true,
        });
        return false;
      }
    } catch (error) {
      set({
        error: 'An unexpected error occurred during login',
        isLoading: false,
        isInitialized: true,
      });
      return false;
    }
  },

  register: async (userData: RegisterDto): Promise<boolean> => {
    set({ isLoading: true, error: null });

    try {
      const response = await authApi.register(userData);

      if (response.success && 'data' in response) {
        set({
          isLoading: false,
          error: null,
          isInitialized: true,
        });
        return true;
      } else {
        const errorMessage = 'error' in response ? response.error : 'Registration failed';
        set({
          error: errorMessage,
          isLoading: false,
          isInitialized: true,
        });
        return false;
      }
    } catch (error) {
      set({
        error: 'An unexpected error occurred during registration',
        isLoading: false,
        isInitialized: true,
      });
      return false;
    }
  },

  logout: () => {
    localStorageService.clearAll();
    set({
      user: null,
      isAuthenticated: false,
      error: null,
      isInitialized: true, // mark initialized so routes render
    });
  },

  clearError: () => {
    set({ error: null });
  },

  initializeAuth: () => {
    const token = localStorageService.getAuthToken();
    const user = localStorageService.getUser<UserDto>();
    if (token && user) {
      set({
        user,
        isAuthenticated: true,
        isInitialized: true,
      });
    } else {
      set({
        user: null,
        isAuthenticated: false,
        isInitialized: true,
      });
    }
  },
}));
