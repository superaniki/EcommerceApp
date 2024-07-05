"use client"

import { createContext } from 'react';
import { apiValidateJWT } from '@/lib/validateJWT';
import { useContext, useEffect, useState } from 'react';
import { LoginFormDataInputs } from '@/components/authentification/login-dialog';
import { apiLogin } from '@/lib/authApi';
import axios from 'axios';

interface AuthContextProps {
  login: (data: LoginFormDataInputs) => Promise<{ success: boolean, message?: string, error?: string }>;
  logout: () => void;
  isAuthenticated: boolean;
  loading: boolean;
  data: JwtData;
}

const AuthContext = createContext<AuthContextProps | undefined>(undefined);

type JwtData = {
  email: string;
  role: string;
}

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({ children }) => {
  const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
  const [loading, setLoading] = useState<boolean>(true);
  const [data, setData] = useState<JwtData>({
    email: "",
    role: ""
  });

  /*
  function validateJWT(){


  }*/

  const validateJwt = async () => {
    console.log("useEffect to validateJWT()");
    const result = await apiValidateJWT();
    if (result.success == true) {
      console.log("loggin in, and setting authentification TRUE");
      setIsAuthenticated(true);
      setData({
        email: result.data!.email,
        role: result.data!.role
      })

    } else {
      console.log("fail and setting authentification FALSE");

      setIsAuthenticated(false);
      setData({
        email: "",
        role: ""
      });
    }
    setLoading(false);
  };

  useEffect(() => {
    validateJwt();
  }, []);

  async function login(data: LoginFormDataInputs) {//email: string, password: string) {
    var result = await apiLogin(data.email, data.password);
    validateJwt();
    return result;
  }

  async function logout() {
    try {
      const response = await axios.post('/api/logout');
      if (response.status === 200) {
        setIsAuthenticated(false);
        setData({
          email: "",
          role: ""
        });
        console.log('Successfully logged out');
        // Perform any additional logout actions, such as redirecting to the login page
      } else {
        console.error('Logout failed');
      }
    } catch (error) {
      console.error('Request error:', error);
    }
  }

  return (
    <AuthContext.Provider value={{ login, logout, isAuthenticated, loading, data }}>
      {children}
    </AuthContext.Provider>
  );
};

export const useAuth = (): AuthContextProps => {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
};
