import type { User } from '@/types/auth';

interface JwtPayload {
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier': string;
  'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress': string;
  'http://schemas.microsoft.com/ws/2008/06/identity/claims/role': string;
  exp: number;
  iss: string;
  aud: string;
}

export const decodeToken = (token: string): User | null => {
  try {
    // Split the token and decode the payload (second part)
    const parts = token.split('.');
    if (parts.length !== 3) {
      console.error('Invalid JWT token: expected 3 parts, got', parts.length);
      return null;
    }

    // Decode base64url
    const payload = parts[1];
    const base64 = payload.replace(/-/g, '+').replace(/_/g, '/');
    let jsonPayload: string;
    try {
      jsonPayload = decodeURIComponent(
        atob(base64)
          .split('')
          .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
          .join('')
      );
    } catch (err) {
      console.error('Failed to decode JWT payload from base64:', err);
      return null;
    }

    let decoded: JwtPayload;
    try {
      decoded = JSON.parse(jsonPayload) as JwtPayload;
    } catch (err) {
      console.error('Failed to parse JWT payload as JSON:', err);
      return null;
    }

    // Check if token is expired
    if (decoded.exp && decoded.exp * 1000 < Date.now()) {
      console.error('JWT token has expired');
      return null;
    }

    return {
      id: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'],
      email: decoded['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'],
      role: decoded['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'],
    };
  } catch (error) {
    console.error('Unexpected error decoding JWT token:', error);
    return null;
  }
};
