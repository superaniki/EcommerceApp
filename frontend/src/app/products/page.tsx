import ProductGrid from "@/components/product-grid/product-grid";
import { fetchAllProducts } from "@/lib/productsApi";

export default async function Page() {
  const products = await fetchAllProducts();

  return <>
    <ProductGrid type={"Sloyd"} products={products} />

  </>
}
