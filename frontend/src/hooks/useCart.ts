import { useCallback } from 'react';
import { useCartStore } from '../stores/cartStore';
import { ProductDto } from '../types/api';

export const useCart = () => {
  const store = useCartStore();
  
  const addToCart = useCallback(async (productId: number, quantity: number, product?: ProductDto) => {
    try {
      await store.addToCart(productId, quantity, product);
      return { success: true };
    } catch (error) {
      return { 
        success: false, 
        error: error instanceof Error ? error.message : 'Failed to add item to cart' 
      };
    }
  }, [store.addToCart]);

  const updateQuantity = useCallback(async (productId: number, quantity: number) => {
    try {
      await store.updateQuantity(productId, quantity);
      return { success: true };
    } catch (error) {
      return { 
        success: false, 
        error: error instanceof Error ? error.message : 'Failed to update quantity' 
      };
    }
  }, [store.updateQuantity]);

  const removeFromCart = useCallback(async (productId: number) => {
    try {
      await store.removeFromCart(productId);
      return { success: true };
    } catch (error) {
      return { 
        success: false, 
        error: error instanceof Error ? error.message : 'Failed to remove item from cart' 
      };
    }
  }, [store.removeFromCart]);

  const clearCart = useCallback(async () => {
    try {
      await store.clearCart();
      return { success: true };
    } catch (error) {
      return { 
        success: false, 
        error: error instanceof Error ? error.message : 'Failed to clear cart' 
      };
    }
  }, [store.clearCart]);

  const loadCart = useCallback(async (userId?: number) => {
    try {
      await store.loadCart(userId);
      return { success: true };
    } catch (error) {
      return { 
        success: false, 
        error: error instanceof Error ? error.message : 'Failed to load cart' 
      };
    }
  }, [store.loadCart]);

  const syncCartToApi = useCallback(async (userId: number) => {
    try {
      await store.syncCartToApi(userId);
      return { success: true };
    } catch (error) {
      return { 
        success: false, 
        error: error instanceof Error ? error.message : 'Failed to sync cart' 
      };
    }
  }, [store.syncCartToApi]);

  const clearError = useCallback(() => {
    store.clearError();
  }, [store.clearError]);

  return {
    // State
    items: store.items,
    isLoading: store.isLoading,
    isInitialLoading: store.isInitialLoading,
    error: store.error,
    total: store.total,
    itemCount: store.itemCount,
    
    // Actions
    addToCart,
    updateQuantity,
    removeFromCart,
    clearCart,
    loadCart,
    syncCartToApi,
    clearError,
  };
};