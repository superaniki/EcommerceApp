import Image from "next/image";
import Link from "next/link";


function HeaderLink({ href, children }: { href: string, children: React.ReactNode }) {
  return <Link className="m-auto" href={href}>{children}</Link>
}

export default function Header() {
  return <div className="flex h-100px p-5 justify-center bg-cakery_timberwolf-900">
    <div className="block w-full mx-8 md:flex">
      <Link className="table m-auto sm:m-0 sm:block italic font-extrabold" href="/">
        WEBSHOP DELUX!
        {/*
        <Image className="flex justify-center" alt="Munamii cakery" src="/logo.png" width={150} height={150}></Image>
        */}
      </Link>
      <div className="text-md grid grid-cols-2 sm:ml-10 sm:items-end sm:flex sm:justify-between sm:gap-10 text-cakery_white-200">
        <HeaderLink href="/">Home</HeaderLink>
        <HeaderLink href="/about">About</HeaderLink>
        <HeaderLink href="/products">Products</HeaderLink>
        <HeaderLink href="contact">Contact</HeaderLink>
      </div>
    </div>
  </div>
}