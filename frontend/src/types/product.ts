/*
Old product
export type Product = {
  title: string;
  slug: string;
  content: string;
  price: string;
  typeOfCake: TypeOfCake[];
  coverImage: string;
}
  */

export type Product = {
	name: string;
	slug: string;
	description: string;
	price: number;
	imageUrl: string;
	quantity: number;
	vectorData: string;
};
/*
export type TypeOfCake = {
  label: string;
  value:*/
