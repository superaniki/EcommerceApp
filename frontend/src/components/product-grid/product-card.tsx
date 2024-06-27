"use client";
import Image from "next/image";
import { useState } from "react";

export default function ProductCard({ imageUrl, name, price, type }: { imageUrl: string, name: string, price: number, type: string }) {
  const [imageLoaded, setImageLoaded] = useState(true);

  const handleImageError = () => {
    setImageLoaded(false);
  };

  return (
    <div className="w-72 bg-white shadow-md rounded-md duration-300 hover:scale-102 hover:shadow-xl">
      {imageLoaded ? (
        <Image
          src={imageUrl}
          height={320}
          width={288}
          alt="Product"
          className="h-80 w-72 object-cover rounded-t-xl"
          onError={handleImageError}
        />
      ) : (
        <div className="h-80 w-72 bg-gray-200 rounded-t-xl flex items-center justify-center">
          <div className="text-gray-400">
            <svg
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 24 24"
              strokeWidth={1.5}
              stroke="currentColor"
              className="w-12 h-12"
            >
              <path
                strokeLinecap="round"
                strokeLinejoin="round"
                d="M6 18L18 6M6 6l12 12"
              />
            </svg>
          </div>
        </div>
      )}
      <div className="px-4 py-3 w-72 pb-5">
        <span className="text-gray-400 mr-3 uppercase text-xs">{type}</span>
        <p className="text-lg text-black truncate block capitalize">{name}</p>
        <div className="flex items-center">
          <p className="text-lg text-black cursor-auto my-0">{price}SEK</p>
          <del>
            <p className="text-sm text-gray-600 cursor-auto ml-2">{price + 50}SEK</p>
          </del>
        </div>
      </div>
    </div>
  );
}