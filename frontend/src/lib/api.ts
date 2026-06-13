import type { LoginCredentials, RegisterData, AuthResponse } from "@/types/auth";
import type { CreateDomainDto, UpdateDomainDto, DomainAvailabilityDto, DomainsDto } from "@/types/domain";
import type { CreateRecordsDto, PutRecordsDto, RecordsDto } from "@/types/records";
import { toast } from "sonner";

const API_URL = import.meta.env.VITE_API_URL;

function getAuthHeaders(token?: string): HeadersInit {
  const headers: HeadersInit = {
    "Content-Type": "application/json",
  };
  
  if (token) {
    headers["Authorization"] = `Bearer ${token}`;
  }
  
  return headers;
}

export const auth = {
  login: async (credentials: LoginCredentials): Promise<AuthResponse> => {
    try {
      const response = await fetch(`${API_URL}/auth/login`, {
        method: "POST",
        headers: getAuthHeaders(),
        body: JSON.stringify(credentials),
      });

      const data = await response.json();
      
      if (!response.ok) {
        const errorMessage = data.errorMessage || data.error || "Login failed";
        toast.error(errorMessage);
        return {
          success: false,
          token: null,
          errorMessage,
        };
      }

      if (!data.success || !data.token) {
        const errorMsg = data.errorMessage || "Login failed";
        toast.error(errorMsg);
        return {
          success: false,
          token: null,
          errorMessage: errorMsg,
        };
      }

      return {
        success: true,
        token: data.token,
        errorMessage: null,
      };

    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    } catch (error) {
      toast.error("Network error during login");
      return {
        success: false,
        token: null,
        errorMessage: "Network error during login",
      };
    }
  },

  register: async (userInfo: RegisterData): Promise<AuthResponse> => {
    try {
      const payload = { ...userInfo };

      const response = await fetch(`${API_URL}/auth/register`, {
        method: "POST",
        headers: getAuthHeaders(),
        body: JSON.stringify(payload),
      });

      const data = await response.json();
      
      if (!response.ok) {
        const errorMessage = data.errorMessage || data.error || "Registration failed";
        toast.error(errorMessage);
        return {
          success: false,
          token: null,
          errorMessage,
        };
      }

      if (!data.success || !data.token) {
        const errorMsg = data.errorMessage || "Registration failed";
        toast.error(errorMsg);
        return {
          success: false,
          token: null,
          errorMessage: errorMsg,
        };
      }

      return {
        success: true,
        token: data.token,
        errorMessage: null,
      };
    // eslint-disable-next-line @typescript-eslint/no-unused-vars
    } catch (error) {
      toast.error("Network error during registration");
      return {
        success: false,
        token: null,
        errorMessage: "Network error during registration",
      };
    }
  },
};

// Create a function to make authenticated requests
export const makeAuthenticatedRequest = async (
  endpoint: string,
  options: RequestInit = {},
  token: string
) => {
  const response = await fetch(`${API_URL}${endpoint}`, {
    ...options,
    headers: {
      ...getAuthHeaders(token),
      ...options.headers,
    },
  });

  if (!response.ok) {
    const error = await response.json().catch(() => ({}));
    throw new Error(error.errorMessage || 'Request failed');
  }

  return response.json();
};

export const domain = {
  // Get All Domains
  getAll: async (): Promise<DomainsDto[]> => {
    const response = await fetch(`${API_URL}/domains`, {
      method: "GET",
      headers: getAuthHeaders(),
    });
    if (!response.ok) {
      throw new Error("Failed to fetch domains");
    }
    return response.json();
  },
  // get by id
  getById: async (id: number): Promise<DomainsDto> => {
    const response = await fetch(`${API_URL}/domains/${id}`, {
      method: "GET",
      headers: getAuthHeaders(),
    });
    if (!response.ok) {
      throw new Error("Failed to fetch domain");
    }
    return response.json();
  },
  // Create Domain
  create: async (domainData: CreateDomainDto): Promise<DomainsDto> => {
    const response = await fetch(`${API_URL}/domains`, {
      method: "POST",
      headers: getAuthHeaders(),
      body: JSON.stringify(domainData),
    });
    if (!response.ok) {
      throw new Error("Failed to create domain");
    }
    return response.json();
  },
  // Update Domain
  update: async (id: number, domainData: UpdateDomainDto): Promise<DomainsDto> => {
    const response = await fetch(`${API_URL}/domains/${id}`, {
      method: "PUT",
      headers: getAuthHeaders(),
      body: JSON.stringify(domainData),
    });
    if (!response.ok) {
      throw new Error("Failed to update domain");
    }
    return response.json();
  },
  // Check Domain Availability
  checkAvailability: async (domainName: string): Promise<boolean> => {
    const response = await fetch(`${API_URL}/domains/check?domainName=${encodeURIComponent(domainName)}`, {
      method: "GET",
      headers: getAuthHeaders(),
    });
    if (!response.ok) {
      throw new Error("Failed to check domain availability");
    }
    return (await response.json()).isAvailable;
  },
  delete: async (id: number): Promise<boolean> => {
    const response = await fetch(`${API_URL}/domains/${id}`, {
      method: "DELETE",
      headers: getAuthHeaders(),
    });
    if (!response.ok) {
      throw new Error("Failed to delete domain");
    }
    return true;
  }
};

export const record = {
  getAll : async (domainId: number): Promise<RecordsDto[]> => {
    const response = await fetch(`${API_URL}/records?domainId=${domainId}`, {
      method: "GET",
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      if (response.status === 404) {
        toast.error("No records found for this domain.");
        return [];
      }
      throw new Error("Failed to fetch records");
    }

    return response.json();
  },
  getById: async (recordId: number): Promise<RecordsDto> => {
    const response = await fetch(`${API_URL}/records/${recordId}`, {
      method: "GET",
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      throw new Error("Failed to fetch record");
    }
    
    return response.json();
  },
  create: async (recordData: CreateRecordsDto): Promise<boolean> => {
    const response = await fetch(`${API_URL}/records`, {
      method: "POST",
      headers: getAuthHeaders(),
      body: JSON.stringify(recordData),
    });

    if (!response.ok) {
      toast.error("Failed to create record");
      return false;
    }

    return true;
  },
  delete: async (recordId: number): Promise<boolean> => {
    const response = await fetch(`${API_URL}/records/${recordId}`, {
      method: "DELETE",
      headers: getAuthHeaders(),
    });

    if (!response.ok) {
      toast.error("Failed to delete record");
      return false;
    }

    return true;
  },
  put: async (recordId: number, recordData: PutRecordsDto): Promise<boolean> => {
    const response = await fetch(`${API_URL}/records/${recordId}`, {
      method: "PUT",
      headers: getAuthHeaders(),
      body: JSON.stringify(recordData),
    });

    console.log(response);
    if (!response.ok) {
      toast.error("Failed to update record");
      return false;
    }

    return true;
  }
}