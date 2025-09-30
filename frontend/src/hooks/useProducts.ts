import { useCallback } from 'react';
import { useProductsStore } from '../stores/productsStore';
import { CreateProductDto, UpdateProductDto } from '../types/api';

export const useProducts = () => {
  const store = useProductsStore();
  
  const loadProducts = useCallback(async (page: number = 1, pageSize: number = 10) => {
    try {
      await store.loadProducts(page, pageSize);
      return { success: true };
    } catch (error) {
      console.error('Failed to load products:', error);
      return { 
        success: false, 
        error: error instanceof Error ? error.message : 'Failed to load products' 
      };
    }
  }, [store.loadProducts]);

  const loadMoreProducts = useCallback(async () => {
    try {
      await store.loadMoreProducts();
      return { success: true };
    } catch (error) {
      console.error('Failed to load more products:', error);
      return { 
        success: false, 
        error: error instanceof Error ? error.message : 'Failed to load more products' 
      };
    }
  }, [store.loadMoreProducts]);

  const loadMyProducts = useCallback(async (page: number = 1, pageSize: number = 10) => {
    try {
      await store.loadMyProducts(page, pageSize);
      return { success: true };
    } catch (error) {
      console.error('Failed to load my products:', error);
      return { 
        success: false, 
        error: error instanceof Error ? error.message : 'Failed to load my products' 
      };
    }
  }, [store.loadMyProducts]);

  const createProduct = useCallback(async (productData: CreateProductDto) => {
    try {
      const result = await store.createProduct(productData);
      return { success: !!result, data: result };
    } catch (error) {
      return { 
        success: false, 
        error: error instanceof Error ? error.message : 'Failed to create product' 
      };
    }
  }, [store.createProduct]);

  const updateProduct = useCallback(async (id: number, productData: UpdateProductDto) => {
    try {
      const result = await store.updateProduct(id, productData);
      return { success: !!result, data: result };
    } catch (error) {
      return { 
        success: false, 
        error: error instanceof Error ? error.message : 'Failed to update product' 
      };
    }
  }, [store.updateProduct]);

  const deleteProduct = useCallback(async (id: number) => {
    try {
      const result = await store.deleteProduct(id);
      return { success: result };
    } catch (error) {
      return { 
        success: false, 
        error: error instanceof Error ? error.message : 'Failed to delete product' 
      };
    }
  }, [store.deleteProduct]);

  const getProductById = useCallback(async (id: number) => {
    try {
      const result = await store.getProductById(id);
      return { success: !!result, data: result };
    } catch (error) {
      return { 
        success: false, 
        error: error instanceof Error ? error.message : 'Failed to get product' 
      };
    }
  }, [store.getProductById]);

  const clearError = useCallback(() => {
    store.clearError();
  }, [store.clearError]);

  const resetProducts = useCallback(() => {
    store.resetProducts();
  }, [store.resetProducts]);

  return {
    // State
    products: store.products,
    myProducts: store.myProducts,
    currentProduct: store.currentProduct,
    isLoading: store.isLoading,
    error: store.error,
    hasMore: store.hasMore,
    currentPage: store.currentPage,
    totalCount: store.totalCount,
    
    // Actions
    loadProducts,
    loadMoreProducts,
    loadMyProducts,
    createProduct,
    updateProduct,
    deleteProduct,
    getProductById,
    clearError,
    resetProducts,
  };
};