"use client";
import { useState } from 'react'
import { Transition } from '@headlessui/react'
import Link from 'next/link'
import { LoginDialog } from './login-dialog';
import { useAuth } from '@/context/AuthContext';
import axios from 'axios';


export function AuthNav() {
	const [isOpen, setIsOpen] = useState(false);
	const { isAuthenticated, loading, data, logout } = useAuth();

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

	return (
		<>
			{!isAuthenticated && <Link href="#" className="m-auto" onClick={openModal}> Log in </Link>}
			{isAuthenticated && <>
				<div>Hello {data.email}!</div>
				<Link href="#" className="m-auto border px-3 rounded-md bg-gray-100" onClick={logout}> Log out </Link>
			</>}


			<Transition appear show={isOpen} as="div">
				<LoginDialog closeModal={closeModal} />
			</Transition>
		</>
	)
}

