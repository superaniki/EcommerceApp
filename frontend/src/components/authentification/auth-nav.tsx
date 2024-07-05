"use client";
import { useState } from 'react'
import { Transition } from '@headlessui/react'
import Link from 'next/link'
import { LoginDialog } from './login-dialog';
import { useAuth } from '@/context/AuthContext';
import axios from 'axios';


export function AuthNav() {
	const [isOpen, setIsOpen] = useState(false);
	const { isAuthenticated, loading, data } = useAuth();

	console.log("data:" + data.email);

	function closeModal() {
		setIsOpen(false)
	}

	function openModal() {
		setIsOpen(true)
	}
	if (loading) {
		return <div>Loading...</div>
	}
	async function logout(): Promise<void> {
		try {
			const response = await axios.post('/api/logout');
			if (response.status === 200) {
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
		<>
			{!isAuthenticated && <Link href="#" className="m-auto" onClick={openModal}> Log in </Link>}
			{isAuthenticated && <>
				<div>email: {data.email}</div>
				<Link href="#" className="m-auto" onClick={logout}> Log out </Link>
			</>}


			<Transition appear show={isOpen} as="div">
				<LoginDialog closeModal={closeModal} />
			</Transition>
		</>
	)
}

