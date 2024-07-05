import setCookieParser from 'set-cookie-parser';
import { cookies } from 'next/headers';

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
