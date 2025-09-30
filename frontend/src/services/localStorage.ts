import { LocalCartItem, ProductDto } from '../types/api';

const CART_STORAGE_KEY = 'guestCart';

export const localStorageService = {
  // Cart operations for guests
  getCartItems: (): LocalCartItem[] => {
    try {
      const cartData = localStorage.getItem(CART_STORAGE_KEY);
      return cartData ? JSON.parse(cartData) : [];
    } catch (error) {
      console.error('Error reading cart from localStorage:', error);
      return [];
    }
  },

  addToCart: (productId: number, quantity: number, product?: ProductDto): void => {
    try {
      const currentCart = localStorageService.getCartItems();
      const existingItemIndex = currentCart.findIndex(item => item.productId === productId);

      if (existingItemIndex > -1) {
        // Update existing item quantity
        currentCart[existingItemIndex].quantity += quantity;
      } else {
        // Add new item
        currentCart.push({ productId, quantity, product });
      }

      localStorage.setItem(CART_STORAGE_KEY, JSON.stringify(currentCart));
    } catch (error) {
      console.error('Error adding to cart in localStorage:', error);
    }
  },

  updateCartItemQuantity: (productId: number, quantity: number): void => {
    try {
      const currentCart = localStorageService.getCartItems();
      const itemIndex = currentCart.findIndex(item => item.productId === productId);

      if (itemIndex > -1) {
        if (quantity <= 0) {
          // Remove item if quantity is 0 or less
          currentCart.splice(itemIndex, 1);
        } else {
          currentCart[itemIndex].quantity = quantity;
        }
        localStorage.setItem(CART_STORAGE_KEY, JSON.stringify(currentCart));
      }
    } catch (error) {
      console.error('Error updating cart item in localStorage:', error);
    }
  },

  removeFromCart: (productId: number): void => {
    try {
      const currentCart = localStorageService.getCartItems();
      const filteredCart = currentCart.filter(item => item.productId !== productId);
      localStorage.setItem(CART_STORAGE_KEY, JSON.stringify(filteredCart));
    } catch (error) {
      console.error('Error removing from cart in localStorage:', error);
    }
  },

  clearCart: (): void => {
    try {
      localStorage.removeItem(CART_STORAGE_KEY);
    } catch (error) {
      console.error('Error clearing cart from localStorage:', error);
    }
  },

  getCartItemCount: (): number => {
    const items = localStorageService.getCartItems();
    return items.reduce((total, item) => total + item.quantity, 0);
  },

  getCartTotal: (): number => {
    const items = localStorageService.getCartItems();
    return items.reduce((total, item) => {
      if (item.product?.price) {
        return total + (item.product.price * item.quantity);
      }
      return total;
    }, 0);
  },

  // Auth token operations
  setAuthToken: (token: string): void => {
    localStorage.setItem('authToken', token);
  },

  getAuthToken: (): string | null => {
    return localStorage.getItem('authToken');
  },

  removeAuthToken: (): void => {
    localStorage.removeItem('authToken');
  },

  // User operations
  setUser: (user: any): void => {
    localStorage.setItem('user', JSON.stringify(user));
  },

  getUser: <T>(): T | null => {
    try {
      const userData = localStorage.getItem('user');
      return userData ? JSON.parse(userData) : null;
    } catch (error) {
      console.error('Error reading user from localStorage:', error);
      return null;
    }
  },

  removeUser: (): void => {
    localStorage.removeItem('user');
  },

  // Clear all app data
  clearAll: (): void => {
    localStorageService.removeAuthToken();
    localStorageService.removeUser();
    localStorageService.clearCart();
  },
};