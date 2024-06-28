import ProductGrid from '@/components/product-grid/product-grid';

export function fetchAllProducts() {
	return fetch(`http://localhost:5115/api/products`).then((res) => res.json());

	/*
	return [
		{
			name: 'Sloyd',
			details:
				'Sloyd is a 100% natural, 100% organic, 100% pure, 100% delicious, 100% healthy, 100% delicious, 100% healthy, 100% delicious, 100% healthy, 100% delicious, 100% healthy',
			imageUrl: '',
			price: '100',
			slug: 'sloyd-1',
		},
	];
	*/
}
/*
	return fetch(
		`https://api.contentful.com/spaces/${process.env.CONTENTFUL_SPACE_ID}/environments/master/entries?access_token=${process.env.CONTENTFUL_ACCESS_TOKEN}`
	).then((res) => res.json());

*/
