'use server';
import { cookies } from 'next/headers';
import { LoginFormDataInputs } from '@/components/authentification/login-dialog';
import { parseSetCookie } from 'next/dist/compiled/@edge-runtime/cookies';
import setCookieParser from 'set-cookie-parser';
import { serialize } from 'v8';

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
			// get the cookies from the response

			//var setCookies = Response.Headers["Set-Cookie"];
			response.headers.forEach(function (value, key) {
				console.log(key + ': ' + value);
			});

			const setCookieHeader = response.headers.getSetCookie();
			console.log('setCookieHeader: ', setCookieHeader);

			proxyServerCookies(['jwt'], response);

			var jwtValue = cookies().get('jwt')?.value;
			console.log('jwtValue:', jwtValue);

			//response.headers.set('set-cookie', 'cookieString');

			//return encryptedSessionData ? JSON.parse(decrypt(encryptedSessionData)) : null;
			//var data = await response.json();
			//console.log('Login successful: ', data);

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

export const proxyServerCookies = async (cookieNames: string[], response: Response) => {
	if (response.headers.has('set-cookie')) {
		const cookieString = response.headers.get('set-cookie')!;

		const cookieObject = setCookieParser.parse(setCookieParser.splitCookiesString(cookieString), {
			map: true,
		});

		cookieNames.forEach((cookieName) => {
			if (cookieObject[cookieName]) {
				const cookie = cookieObject[cookieName];

				console.debug(`[API Request] Proxying cookie ${cookieName} to client.`);
				console.log('[API Request] Proxying cookie: ', cookie);

				cookies().set(cookieName, cookie.value, {
					path: cookie.path,
					domain: cookie.domain,
					maxAge: cookie.maxAge,
					sameSite: cookie.sameSite as 'lax' | 'strict' | 'none' | boolean | undefined,
					//					expires: cookie.expires,
					secure: cookie.secure,
					httpOnly: cookie.httpOnly,
				});
			}
		});
	}

	return response;
};

// auth.ts

export async function checkAuth() {
	try {
		const response = await fetch('http://localhost:5115/api/auth/check-auth', {
			credentials: 'include',
		});

		if (response.ok) {
			console.log('test: is authenticated!');
			return { isAuthenticated: true };
		} else {
			console.log('test: is NOT authenticated!');

			return { isAuthenticated: false };
		}
	} catch (error) {
		console.error('Error checking authentication:', error);
		return { isAuthenticated: false };
	}
}

/*
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
	*/

/*
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
	*/
