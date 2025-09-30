import { create } from 'zustand';
import { ProductDto, CreateProductDto, UpdateProductDto, PagedResult, PaginationRequest, UserDto } from '../types/api';
import { productsApi } from '../services/api';
import { localStorageService } from '../services/localStorage';

interface ProductsState {
  products: ProductDto[];
  myProducts: ProductDto[];
  currentProduct: ProductDto | null;
  isLoading: boolean;
  error: string | null;
  hasMore: boolean;
  currentPage: number;
  totalCount: number;
}

interface ProductsActions {
  loadProducts: (page?: number, pageSize?: number) => Promise<void>;
  loadMoreProducts: () => Promise<void>;
  loadMyProducts: (page?: number, pageSize?: number) => Promise<void>;
  getProductById: (id: number) => Promise<ProductDto | null>;
  createProduct: (productDto: CreateProductDto) => Promise<ProductDto | null>;
  updateProduct: (id: number, productDto: UpdateProductDto) => Promise<ProductDto | null>;
  deleteProduct: (id: number) => Promise<boolean>;
  clearError: () => void;
  resetProducts: () => void;
}

export const useProductsStore = create<ProductsState & ProductsActions>((set, get) => ({
  // State
  products: [],
  myProducts: [],
  currentProduct: null,
  isLoading: false,
  error: null,
  hasMore: true,
  currentPage: 1,
  totalCount: 0,

  // Actions
  loadProducts: async (page = 1, pageSize = 10) => {
    set({ isLoading: true, error: null });
    
    try {
      const params: PaginationRequest = { page, pageSize };
      const response = await productsApi.getProducts(params);
      
      if (response.success && 'data' in response) {
        const pagedResult = response.data as PagedResult<ProductDto>;
        
        if (page === 1) {
          // First page - replace products
          set({
            products: pagedResult.items,
            currentPage: page,
            totalCount: pagedResult.totalCount,
            hasMore: pagedResult.hasNextPage,
          });
        } else {
          // Additional pages - append products
          const { products } = get();
          set({
            products: [...products, ...pagedResult.items],
            currentPage: page,
            hasMore: pagedResult.hasNextPage,
          });
        }
      } else {
        set({ error: 'error' in response ? response.error : 'Failed to load products' });
      }
    } catch (error) {
      set({ error: 'Failed to load products' });
    } finally {
      set({ isLoading: false });
    }
  },

  loadMoreProducts: async () => {
    const { currentPage, hasMore, isLoading } = get();
    
    if (!hasMore || isLoading) return;
    
    await get().loadProducts(currentPage + 1, 10);
  },

  loadMyProducts: async (page = 1, pageSize = 10) => {
    set({ isLoading: true, error: null });
    
    try {
      const user = localStorageService.getUser<UserDto>();
      if (!user || !user.id) {
        set({ error: 'User not authenticated' });
        return;
      }

      const params: PaginationRequest = { page, pageSize };
      const response = await productsApi.getMyProducts(params);
      
      if (response.success && 'data' in response) {
        const pagedResult = response.data as PagedResult<ProductDto>;
        set({ myProducts: pagedResult.items });
      } else {
        set({ error: 'error' in response ? response.error : 'Failed to load your products' });
      }
    } catch (error) {
      set({ error: 'Failed to load your products' });
    } finally {
      set({ isLoading: false });
    }
  },

  getProductById: async (id: number): Promise<ProductDto | null> => {
    set({ isLoading: true, error: null });
    
    try {
      const response = await productsApi.getProductById(id);
      
      if (response.success && 'data' in response) {
        const product = response.data as ProductDto;
        set({ currentProduct: product });
        return product;
      } else {
        set({ error: 'error' in response ? response.error : 'Product not found' });
        return null;
      }
    } catch (error) {
      set({ error: 'Failed to load product' });
      return null;
    } finally {
      set({ isLoading: false });
    }
  },

  createProduct: async (productDto: CreateProductDto): Promise<ProductDto | null> => {
    set({ isLoading: true, error: null });
    
    try {
      const response = await productsApi.createProduct(productDto);
      
      if (response.success && 'data' in response) {
        const newProduct = response.data as ProductDto;
        
        // Add to products list
        const { products } = get();
        set({ products: [newProduct, ...products] });
        
        // Refresh my products
        await get().loadMyProducts();
        
        return newProduct;
      } else {
        set({ error: 'error' in response ? response.error : 'Failed to create product' });
        return null;
      }
    } catch (error) {
      set({ error: 'Failed to create product' });
      return null;
    } finally {
      set({ isLoading: false });
    }
  },

  updateProduct: async (id: number, productDto: UpdateProductDto): Promise<ProductDto | null> => {
    set({ isLoading: true, error: null });
    
    try {
      const response = await productsApi.updateProduct(id, productDto);
      
      if (response.success && 'data' in response) {
        const updatedProduct = response.data as ProductDto;
        
        // Update in products list
        const { products, myProducts } = get();
        set({
          products: products.map(p => p.id === id ? updatedProduct : p),
          myProducts: myProducts.map(p => p.id === id ? updatedProduct : p),
          currentProduct: updatedProduct,
        });
        
        return updatedProduct;
      } else {
        set({ error: 'error' in response ? response.error : 'Failed to update product' });
        return null;
      }
    } catch (error) {
      set({ error: 'Failed to update product' });
      return null;
    } finally {
      set({ isLoading: false });
    }
  },

  deleteProduct: async (id: number): Promise<boolean> => {
    set({ isLoading: true, error: null });
    
    try {
      const response = await productsApi.deleteProduct(id);
      
      if (response.success) {
        // Remove from all lists
        const { products, myProducts } = get();
        set({
          products: products.filter(p => p.id !== id),
          myProducts: myProducts.filter(p => p.id !== id),
          currentProduct: null,
        });
        
        return true;
      } else {
        set({ error: 'error' in response ? response.error : 'Failed to delete product' });
        return false;
      }
    } catch (error) {
      set({ error: 'Failed to delete product' });
      return false;
    } finally {
      set({ isLoading: false });
    }
  },

  clearError: () => {
    set({ error: null });
  },

  resetProducts: () => {
    set({
      products: [],
      currentPage: 1,
      hasMore: true,
      totalCount: 0,
    });
  },
}));