"use client"

import { createContext } from 'react';
import { validateJWT } from '@/lib/validateJWT';
import { useContext, useEffect, useState } from 'react';

interface AuthContextProps {
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

  useEffect(() => {
    const checkAuth = async () => {
      console.log("useEffect to validateJWT()");
      const result = await validateJWT();
      if (result.success == true) {
        setIsAuthenticated(true);
        setData({
          email: result.data!.email,
          role: result.data!.role
        })

      } else {
        setIsAuthenticated(false);
        setData({
          email: "",
          role: ""
        });
      }
      setLoading(false);
    };

    checkAuth();
  }, []);

  return (
    <AuthContext.Provider value={{ isAuthenticated, loading, data }}>
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
