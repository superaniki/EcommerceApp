//'use client';

// utils/auth.js
import axios from 'axios';

export const validateJWT = async () => {
	try {
		console.log('validateJWT()');
		const response = await axios.get('http://localhost:5115/api/auth/check-auth', {
			withCredentials: true, // Ensure cookies are included
			headers: {
				'Content-Type': 'application/json',
			},
		});

		console.log('response status:', response.status);

		if (response.status !== 200) {
			console.log('Error validating JWT:', response.status, response.statusText);
			return { success: false };
		} else {
			const claims = response.data.claims;
			console.log('User claims:', claims);

			return { success: true, data: { email: claims.Email, role: claims.Role } }; // Optionally, you can return await response.data if there is JSON data to be processed
		}
	} catch (error) {
		console.log('Error validating JWT:', error);
		return { success: false };
	}
};
