"use client";
import { useState } from 'react'
import { Transition } from '@headlessui/react'
import Link from 'next/link'
import { LoginDialog } from './login-dialog';

export function AuthNav() {
	const [isOpen, setIsOpen] = useState(false);

	function closeModal() {
		setIsOpen(false)
	}

	function openModal() {
		setIsOpen(true)
	}

	return (
		<>
			<Link href="#" className="m-auto" onClick={openModal}>
				Log in
			</Link>

			<Transition appear show={isOpen} as="div">
				<LoginDialog closeModal={closeModal} />
			</Transition>
		</>
	)
}

