export default function Content({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return <main className="flex min-h-[400px] flex-col items-center justify-between p-12 sm:p-24 pb-[100px] border-timberwolf-700 border-y-2">
    <div className="z-10 w-full max-w-5xl items-center justify-between font-mono text-sm">
      <div className="text-lg">
        {children}
      </div>
    </div>
  </main>
}