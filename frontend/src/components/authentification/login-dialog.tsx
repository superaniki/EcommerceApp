import { DialogTitle } from "@headlessui/react";
import { checkAuth, login } from '@/lib/authApi';
import { useState } from 'react';
import { useForm } from "react-hook-form";
import { NiceDialog } from "../UI/NiceDialog";
import { redirect } from "next/dist/server/api-utils";
//import { signIn } from "next-auth/react";

export type LoginFormDataInputs = {
    email: string
    password: string
}

export function LoginDialog({ closeModal }: { closeModal: () => void }) {
    const [error, setError] = useState('');
    const { register, handleSubmit, formState: { errors } } = useForm<LoginFormDataInputs>();

    const onSubmit = async (data: LoginFormDataInputs) => {
        try {
            var response = await login(data);

            if (response.success) {
                console.log(response.message);
                // httponly cookie should be set now.
                closeModal();
            } else {
                setError(response.error!);
            }
        } catch (error) {
            console.log(error);
            setError('Invalid email or password');
        }
    };

    return <NiceDialog closeModal={closeModal}>
        <DialogTitle as="h3" className="text-lg font-medium leading-6 text-gray-900">
            Login
        </DialogTitle>
        <div className="mt-2">
            <p className="text-sm text-gray-500">
                Please enter your login credentials.
            </p>
        </div>

        <form className="mt-4" onSubmit={handleSubmit(onSubmit)}>
            {error && <p className="text-red-500 mb-2">{error}</p>}
            <input
                {...register("email", { required: true })}
                type="email"
                placeholder="Email"
                className="w-full border border-gray-300 rounded-md py-2 px-3 mb-2"
            />
            {errors.email && <p className="text-red-500 mb-2">Email is required</p>}
            <input
                {...register("password", { required: true })}
                type="password"
                placeholder="Password"
                className="w-full border border-gray-300 rounded-md py-2 px-3 mb-4"
            />
            {errors.password && <p className="text-red-500 mb-2">Password is required</p>}
            <button
                type="submit"
                className="inline-flex justify-center rounded-md border border-transparent bg-blue-100 px-4 py-2 text-sm font-medium text-blue-900 hover:bg-blue-200 focus:outline-none focus-visible:ring-2 focus-visible:ring-blue-500 focus-visible:ring-offset-2"
            >
                Login
            </button>
        </form>
    </NiceDialog>

}