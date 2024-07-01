import Link from 'next/link';

export function HeaderLink({ href, children }: { href: string; children: React.ReactNode }) {
	return (
		<Link className="m-auto" href={href}>
			{children}
		</Link>
	);
}
