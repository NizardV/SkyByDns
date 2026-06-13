export interface LoginCredentials {
  email: string;
  password: string;
}

export interface RegisterData {
  firstName: string;
  lastName: string;
  email: string;
  password: string;
  role?: string;
}

export interface AuthResponse {
  token: string;
  success: boolean;
  errorMessage?: string;
}

export interface User {
  id: string;
  email: string;
  role: string;
}
