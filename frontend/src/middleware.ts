import { NextResponse } from 'next/server';
import type { NextRequest } from 'next/server';
import jwt from 'jsonwebtoken';
import { checkAuth } from './lib/authApi';

// List of paths that don't require authentication
const publicPaths = ['/login', '/register', '/', '/api/auth/login'];

export async function middleware(request: NextRequest) {
	const path = request.nextUrl.pathname;

	// Allow access to public paths without authentication
	if (publicPaths.includes(path)) {
		return NextResponse.next();
	}

	var response = await checkAuth();
	if (response.isAuthenticated) {
		return NextResponse.next();
	} else {
		return NextResponse.redirect(new URL('/login', request.url));
	}

	// Check for the presence of the JWT token in cookies
	const jwtToken = request.cookies.get('jwt')?.value;

	if (!jwtToken) {
		console.log('No jwtToken somehow....');
		// Redirect to login page if no token is present
		return NextResponse.redirect(new URL('/login', request.url));
	}

	try {
		// Verify the JWT token
		const secret = process.env.JWT_SECRET || 'your-secret-key';
		jwt.verify(jwtToken, secret);

		// Token is valid, allow the request to proceed
		return NextResponse.next();
	} catch (error) {
		console.error('Invalid token:', error);
		// Redirect to login page if token is invalid
		return NextResponse.redirect(new URL('/login', request.url));
	}
}

// Configure which routes to run the middleware on
export const config = {
	matcher: ['/((?!api|_next/static|_next/image|favicon.ico).*)'],
};
