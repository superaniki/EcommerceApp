/** @type {import('next').NextConfig} */
const nextConfig = {
  images: {
    remotePatterns: [
      {
        protocol: 'https',
        hostname: 'images.unsplash.com',
        pathname: '/photo-**'
      },
      {
        protocol: 'https',
        hostname: 'plus.unsplash.com',
        pathname: '/premium_photo-**'
      }
    ]
  },
  async rewrites() {
    return [
      {
        source: '/api/:path*',
        destination: 'http://localhost:5115/api/:path*',
      },
    ];
  },
};

export default nextConfig;
