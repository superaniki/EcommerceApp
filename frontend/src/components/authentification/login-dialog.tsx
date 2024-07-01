import { Dialog, DialogPanel, DialogTitle, TransitionChild } from "@headlessui/react";
import { login } from '@/lib/authApi';
import { useState } from 'react';
import { useForm } from "react-hook-form";



export type LoginFormDataInputs = {
    email: string
    password: string
}

export function LoginDialog({ closeModal }: { closeModal: () => void }) {
    const [error, setError] = useState('');
    const { register, handleSubmit, formState: { errors } } = useForm<Inputs>();

    const onSubmit = async (data: LoginFormDataInputs) => {
        try {
            var response = await login(data);
            if (response.success) {
                console.log("login successful");

                closeModal();
            } else {
                setError(response.error!);
            }
        } catch (error) {
            console.log(error);
            setError('Invalid email or password');
        }
    };

    return <Dialog as="div" className="relative z-10" onClose={closeModal}>
        <TransitionChild
            as="div"
            enter="ease-out duration-300"
            enterFrom="opacity-0"
            enterTo="opacity-100"
            leave="ease-in duration-200"
            leaveFrom="opacity-100"
            leaveTo="opacity-0"
        >
            <div className="fixed inset-0 bg-black bg-opacity-25" />
        </TransitionChild>

        <div className="fixed inset-0 overflow-y-auto">
            <div className="flex min-h-full items-center justify-center p-4 text-center">
                <TransitionChild
                    as="div"
                    enter="ease-out duration-300"
                    enterFrom="opacity-0 scale-95"
                    enterTo="opacity-100 scale-100"
                    leave="ease-in duration-200"
                    leaveFrom="opacity-100 scale-100"
                    leaveTo="opacity-0 scale-95"
                >
                    <DialogPanel className="w-full max-w-md transform overflow-hidden rounded-2xl bg-white p-6 text-left align-middle shadow-xl transition-all">
                        <DialogTitle
                            as="h3"
                            className="text-lg font-medium leading-6 text-gray-900"
                        >
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
                    </DialogPanel>
                </TransitionChild>
            </div>
        </div>
    </Dialog>
}