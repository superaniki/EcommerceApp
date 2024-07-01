'use server';
import { cookies } from 'next/headers';
import jwt from 'jsonwebtoken';
import { parseCookies } from 'nookies';
import { LoginFormDataInputs } from '@/components/authentification/login-dialog';

export async function login(formData: LoginFormDataInputs) {
	const email = formData.email;
	const password = formData.password;

	if (!email || !password) {
		console.error('Email or password is missing');
		return { success: false, error: 'Email and password are required' };
	}

	const requestBody = JSON.stringify({ email, password });
	console.log('requestBody:', requestBody);

	try {
		console.log('login()');
		const response = await fetch('http://localhost:5115/api/auth/login', {
			method: 'POST',
			headers: {
				'Content-Type': 'application/json',
			},
			body: requestBody,
			credentials: 'include',
		});

		if (response.ok) {
			console.log('Login successful');
			return { success: true };
		} else {
			console.error('Login failed');
			return { success: false, error: 'Invalid credentials' };
		}
	} catch (error) {
		console.error('Error during login:', error);
		return { success: false, error: 'Server Error' };
	}
}

// auth.ts

export async function checkAuth() {
	const cookieStore = cookies();
	const jwtCookie = cookieStore.get('jwt')?.value;
	console.log('checkauth()');

	if (!jwtCookie) {
		-console.log('has no jwt cookie');

		return { isAuthenticated: false };
	}

	try {
		const decodedToken: any = jwt.verify(jwtCookie, process.env.JWT_SECRET || '');
		const userEmail = decodedToken.sub;
		const role = decodedToken?.role || 'logged_out';

		// Fetch user roles or additional data from your backend API if needed
		const userRoles = ['Customer']; // Replace with actual role fetching logic

		return { isAuthenticated: true, userRoles, userEmail };
	} catch (error) {
		console.error('Error verifying JWT token:', error);
		return { isAuthenticated: false };
	}
}

export async function checkAuthNookie(ctx?: any) {
	const cookies = parseCookies(ctx);
	const jwtCookie = cookies['jwt'];

	console.log('checkauth()');

	if (!jwtCookie) {
		console.log('has no jwt cookie');
		return { isAuthenticated: false };
	}

	try {
		const decodedToken: any = jwt.verify(jwtCookie, process.env.JWT_SECRET || '');
		const userEmail = decodedToken.sub;
		const role = decodedToken?.role || 'logged_out';

		// Fetch user roles or additional data from your backend API if needed
		const userRoles = ['Customer']; // Replace with actual role fetching logic

		return { isAuthenticated: true, userRoles, userEmail };
	} catch (error) {
		console.error('Error verifying JWT token:', error);
		return { isAuthenticated: false };
	}
}
