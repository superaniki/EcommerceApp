import Image from "next/image";
import { checkAuth } from "@/lib/authApi";


export default async function Dashboard() {
  const authStatus = await checkAuth();

  if (!authStatus.isAuthenticated) {
    // You might want to redirect here, or show a login prompt
    return <div>Please log in to view this page.</div>;
  }

  return (
    <div>
      Logged in!
      {/*
      <h1>Welcome, {authStatus.userEmail}</h1>
      <p>Your roles: {authStatus.userRoles?.join(', ') || ''}</p>
*/}
    </div>
  );
}