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
  }
};

export default nextConfig;
