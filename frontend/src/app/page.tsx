//"use client";

import Image from "next/image";
import { checkAuth } from "@/lib/authApi";
//import { useEffect, useState } from "react";
//import { useAuth } from "@/context/AuthContext";

export default function Page() {

  //const { isAuthenticated, loading } = useAuth();

  /*
    if (!isAuthenticated) {
      // You might want to redirect here, or show a login prompt
      return <div>Please log in to view this page.</div>;
    }
      */

  return (
    <div>
      Welcome to all time shopping!
    </div>
  );
}