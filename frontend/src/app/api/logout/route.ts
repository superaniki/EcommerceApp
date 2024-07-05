/*
import { NextApiRequest, NextApiResponse } from 'next';

export default function logout(req: NextApiRequest, res: NextApiResponse) {
	// Clear the JWT cookie by setting it to a past date
	console.log('logout()');
	res.setHeader('Set-Cookie', 'jwt=; HttpOnly; Path=/; Max-Age=0; SameSite=Strict; Secure');
	res.status(200).json({ message: 'Logged out successfully' });
}
*/

import { NextRequest, NextResponse } from 'next/server';

export async function POST(req: NextRequest) {
	// Clear the JWT cookie by setting it to a past date
	const response = NextResponse.json({ message: 'Logged out successfully' });
	response.cookies.set('jwt', '', { httpOnly: true, path: '/', maxAge: 0, sameSite: 'strict', secure: true });
	return response;
}
