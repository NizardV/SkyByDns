import { createContext, useContext, useState, type ReactNode } from 'react';
import { auth } from '@/lib/api';
import { decodeToken } from '@/lib/jwt';
import type { LoginCredentials, RegisterData, AuthResponse, User } from '@/types/auth';

interface AuthContextType {
  isAuthenticated: boolean;
  isLoading: boolean;
  token: string | null;
  user: User | null;
  login: (credentials: LoginCredentials) => Promise<AuthResponse>;
  register: (userData: RegisterData) => Promise<AuthResponse>;
  logout: () => void;
  setToken: (token: string) => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};

interface AuthProviderProps {
  children: ReactNode;
}

export const AuthProvider: React.FC<AuthProviderProps> = ({ children }) => {
  // Initialize all auth state from localStorage token (decode only once)
  const initialAuth = (() => {
    const storedToken = localStorage.getItem('auth_token');
    if (storedToken) {
      const decodedUser = decodeToken(storedToken);
      if (decodedUser) {
        return {
          token: storedToken,
          user: decodedUser,
          isAuthenticated: true,
        };
      }
    }
    return {
      token: null,
      user: null,
      isAuthenticated: false,
    };
  })();
  
  const [tokenState, setTokenState] = useState<string | null>(initialAuth.token);
  const [userState, setUserState] = useState<User | null>(initialAuth.user);
  const [isAuthenticatedState, setIsAuthenticated] = useState<boolean>(initialAuth.isAuthenticated);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  // Function to update token in both context and localStorage
  const setToken = (newToken: string) => {
    localStorage.setItem('auth_token', newToken);
    setTokenState(newToken);
    
    // Decode token to get user information
    const decodedUser = decodeToken(newToken);
    if (decodedUser) {
      setUserState(decodedUser);
      setIsAuthenticated(true);
    } else {
      // Token is invalid or expired
      removeToken();
    }
  };

  // Function to remove token
  const removeToken = () => {
    localStorage.removeItem('auth_token');
    setTokenState(null);
    setUserState(null);
    setIsAuthenticated(false);
  };

  const login = async (credentials: LoginCredentials): Promise<AuthResponse> => {
    setIsLoading(true);
    try {
      const result = await auth.login(credentials);
      
      if (result.success && result.token) {
        setToken(result.token);
      }
      
      setIsLoading(false);
      return result;
    } catch {
      setIsLoading(false);
      return {
        success: false,
        token: null,
        errorMessage: 'An unexpected error occurred',
      };
    }
  };

  const register = async (userData: RegisterData): Promise<AuthResponse> => {
    setIsLoading(true);
    try {
      const result = await auth.register(userData);
      
      if (result.success && result.token) {
        setToken(result.token);
      }
      
      setIsLoading(false);
      return result;
    } catch {
      setIsLoading(false);
      return {
        success: false,
        token: null,
        errorMessage: 'An unexpected error occurred',
      };
    }
  };

  const logout = () => {
    removeToken();
  };

  const value = {
    isAuthenticated: isAuthenticatedState,
    isLoading,
    token: tokenState,
    user: userState,
    login,
    register,
    logout,
    setToken,
  };

  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  );
};