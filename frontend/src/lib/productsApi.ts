export function fetchAllProducts() {
	return fetch(`http://localhost:5115/api/products`).then((res) => res.json());
}
