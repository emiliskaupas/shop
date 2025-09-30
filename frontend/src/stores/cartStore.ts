import { create } from 'zustand';
import { CartItemDto, LocalCartItem, ProductDto, AddToCartRequestDto, UserDto } from '../types/api';
import { cartApi } from '../services/api';
import { localStorageService } from '../services/localStorage';

interface CartState {
  items: (CartItemDto | LocalCartItem)[];
  isLoading: boolean;
  isInitialLoading: boolean;
  error: string | null;
  total: number;
  itemCount: number;
}

interface CartActions {
  addToCart: (productId: number, quantity: number, product?: ProductDto) => Promise<void>;
  updateQuantity: (productId: number, quantity: number) => Promise<void>;
  removeFromCart: (productId: number) => Promise<void>;
  clearCart: () => Promise<void>;
  syncCartToApi: (userId: number) => Promise<void>;
  loadCart: (userId?: number) => Promise<void>;
  calculateTotals: () => void;
  clearError: () => void;
}

const isCartItemDto = (item: CartItemDto | LocalCartItem): item is CartItemDto => {
  return 'userId' in item;
};

const getProductId = (item: CartItemDto | LocalCartItem): number => {
  if (isCartItemDto(item)) {
    return item.product?.id ?? item.productId;
  }
  return item.productId;
};

export const useCartStore = create<CartState & CartActions>((set, get) => ({
  // State
  items: [],
  isLoading: false,
  isInitialLoading: true,
  error: null,
  total: 0,
  itemCount: 0,

  // Actions
  addToCart: async (productId: number, quantity: number, product?: ProductDto) => {
    const { items } = get();
    set({ error: null });
    
    // Find existing item
    const existingItemIndex = items.findIndex(item => getProductId(item) === productId);
    let updatedItems;
    let originalItems = [...items];
    
    if (existingItemIndex >= 0) {
      // Update existing item optimistically
      updatedItems = [...items];
      const existingItem = updatedItems[existingItemIndex];
      updatedItems[existingItemIndex] = { 
        ...existingItem, 
        quantity: existingItem.quantity + quantity 
      };
    } else if (product) {
      // Add new item optimistically
      const user = localStorageService.getUser<UserDto>();
      const newItem = user && user.id 
        ? {
            id: Date.now(), // Temporary ID
            userId: user.id,
            productId,
            quantity,
            product,
            createdAt: new Date().toISOString(),
          } as CartItemDto
        : {
            productId,
            quantity,
            product,
          } as LocalCartItem;
      
      updatedItems = [...items, newItem];
    } else {
      // Can't add without product info
      set({ error: 'Product information required' });
      return;
    }
    
    // Update UI immediately
    set({ items: updatedItems });
    get().calculateTotals();
    
    try {
      // For authenticated users, use API
      const user = localStorageService.getUser<UserDto>();
      if (user && user.id) {
        const request: AddToCartRequestDto = { productId, quantity };
        const response = await cartApi.addToCart(user.id, request);
        
        if (response.success && 'data' in response) {
          // Reload cart to get correct server data (with real IDs, etc.)
          await get().loadCart(user.id);
        } else {
          // Rollback on failure
          set({ items: originalItems });
          get().calculateTotals();
          set({ error: 'error' in response ? response.error : 'Failed to add to cart' });
        }
      } else {
        // For guests, use localStorage
        localStorageService.addToCart(productId, quantity, product);
      }
    } catch (error) {
      // Rollback on error
      set({ items: originalItems });
      get().calculateTotals();
      set({ error: 'Failed to add item to cart' });
    }
  },

  updateQuantity: async (productId: number, quantity: number) => {
    const { items } = get();
    set({ error: null });
    
    if (quantity <= 0) {
      await get().removeFromCart(productId);
      return;
    }
    
    // Find the item to update
    const itemIndex = items.findIndex(item => getProductId(item) === productId);
    if (itemIndex === -1) return;
    
    // Store original item for rollback
    const originalItem = items[itemIndex];
    
    // Optimistic update - update UI immediately
    const updatedItems = [...items];
    updatedItems[itemIndex] = { ...originalItem, quantity };
    set({ items: updatedItems });
    get().calculateTotals();
    
    try {
      const user = localStorageService.getUser<UserDto>();
      
      if (user && user.id) {
        // For authenticated users, update via API
        const cartItem = originalItem as CartItemDto;
        
        const response = await cartApi.updateCartItemQuantity(
          user.id,
          cartItem.id,
          { quantity }
        );
        
        if (!response.success) {
          // Rollback on failure
          set({ items });
          get().calculateTotals();
          set({ error: 'error' in response ? response.error : 'Failed to update quantity' });
        }
      } else {
        // For guests, update localStorage
        localStorageService.updateCartItemQuantity(productId, quantity);
      }
    } catch (error) {
      // Rollback on error
      set({ items });
      get().calculateTotals();
      set({ error: 'Failed to update item quantity' });
    }
  },

  removeFromCart: async (productId: number) => {
    const { items } = get();
    set({ error: null });
    
    // Find the item to remove
    const itemIndex = items.findIndex(item => getProductId(item) === productId);
    if (itemIndex === -1) return;
    
    // Store original items for rollback
    const originalItems = [...items];
    
    // Optimistic update - remove from UI immediately
    const updatedItems = items.filter(item => getProductId(item) !== productId);
    set({ items: updatedItems });
    get().calculateTotals();
    
    try {
      const user = localStorageService.getUser<UserDto>();
      
      if (user && user.id) {
        // For authenticated users, remove via API
        const cartItem = originalItems[itemIndex] as CartItemDto;
        
        const response = await cartApi.removeFromCart(user.id, cartItem.id);
        
        if (!response.success) {
          // Rollback on failure
          set({ items: originalItems });
          get().calculateTotals();
          set({ error: 'error' in response ? response.error : 'Failed to remove from cart' });
        }
      } else {
        // For guests, remove from localStorage
        localStorageService.removeFromCart(productId);
      }
    } catch (error) {
      // Rollback on error
      set({ items: originalItems });
      get().calculateTotals();
      set({ error: 'Failed to remove item from cart' });
    }
  },

  clearCart: async () => {
    const { items } = get();
    set({ error: null });
    
    // Store original items for rollback
    const originalItems = [...items];
    
    // Optimistic update - clear UI immediately
    set({ items: [], total: 0, itemCount: 0 });
    
    try {
      const user = localStorageService.getUser<UserDto>();
      
      if (user && user.id) {
        const response = await cartApi.clearCart(user.id);
        if (!response.success) {
          // Rollback on failure
          set({ items: originalItems });
          get().calculateTotals();
          set({ error: 'error' in response ? response.error : 'Failed to clear cart' });
        }
      } else {
        localStorageService.clearCart();
      }
    } catch (error) {
      // Rollback on error
      set({ items: originalItems });
      get().calculateTotals();
      set({ error: 'Failed to clear cart' });
    }
  },

  syncCartToApi: async (userId: number) => {
    const localItems = localStorageService.getCartItems();

    if (localItems.length === 0) return;

    try {
      let skippedNames: string[] = [];
      // Add each local cart item to API, skipping items where product.createdByUserId === userId
      for (const localItem of localItems) {
        // Only add if product is not created by the logged-in user
        if (localItem.product && localItem.product.createdByUserId === userId) {
          if (localItem.product.name) {
            skippedNames.push(localItem.product.name);
          }
          continue;
        }
        await cartApi.addToCart(userId, {
          productId: localItem.productId,
          quantity: localItem.quantity,
        });
      }

      // Show error popup if any items were skipped
      if (skippedNames.length > 0) {
        set({ error: `The following products were not added to your cart because you are their creator: ${skippedNames.join(', ')}` });
      }

      // Clear local cart after sync
      localStorageService.clearCart();

      // Reload cart from API
      await get().loadCart(userId);
    } catch (error) {
      console.error('Failed to sync cart to API:', error);
    }
  },

  loadCart: async (userId?: number) => {
    set({ isLoading: true, isInitialLoading: false, error: null });
    
    try {
      if (userId) {
        // Load from API for authenticated users
        const response = await cartApi.getCartItems(userId);
        
        if (response.success && 'data' in response) {
          const items = response.data as CartItemDto[];
          set({ items });
        } else {
          set({ error: 'error' in response ? response.error : 'Failed to load cart' });
        }
      } else {
        // Load from localStorage for guests
        const localItems = localStorageService.getCartItems();
        set({ items: localItems });
      }
      
      get().calculateTotals();
    } catch (error) {
      set({ error: 'Failed to load cart' });
    } finally {
      set({ isLoading: false });
    }
  },

  calculateTotals: () => {
    const { items } = get();
    
    let total = 0;
    let itemCount = 0;
    
    items.forEach(item => {
      const quantity = item.quantity;
      itemCount += quantity;
      
      let price = 0;
      if (isCartItemDto(item)) {
        price = item.product?.price ?? 0;
      } else {
        price = item.product?.price ?? 0;
      }
      total += price * quantity;
    });
    
    set({ total, itemCount });
  },

  clearError: () => {
    set({ error: null });
  },
}));