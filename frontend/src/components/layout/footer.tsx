import Link from "next/link";

export default function Footer({ }) {
  return <div className="h-100px w-full p-10 static bottom-0 bg-cakery_timberwolf-900">
    <div className="text-zinc-500">
      <ul className="flex gap-10">
        <li>
          <Link href="">
            <i className="fab fa-facebook" /> Facebook
          </Link>
        </li>
        <li>
          <Link href="">
            <i className="fab fa-instagram" /> Instagram
          </Link>
        </li>
      </ul>
    </div>
    <br />

    <div className="text-zinc-500 text-xs flex gap-10">
      <div>Disclaimers</div>
      <div className="w-72">
        1. I have personal permission for the store owner to use her company name,
        logotype and product images for learning purposes.
      </div>
      <div className="w-72">
        2. This is not spec work (work done for free in hopes to get elected from a pool of
        contestant and only 1 get paid)
      </div>
    </div>
  </div>
}