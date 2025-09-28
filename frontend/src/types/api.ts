import { UserRole, ProductType, NotificationType } from './enums';

// Auth DTOs
export interface LoginDto {
  email: string;
  password: string;
}

export interface RegisterDto {
  username: string;
  email: string;
  password: string;
}

export interface UserDto {
  id: number;
  username: string;
  email: string;
  role: UserRole;
}

// Product DTOs
export interface ProductDto {
  id: number;
  name: string;
  shortDescription: string;
  price: number;
  imageUrl?: string;
  productType: ProductType;
}

export interface CreateProductDto {
  name: string;
  shortDescription: string;
  price: number;
  imageUrl?: string;
  productType: ProductType;
}

export interface UpdateProductDto {
  name: string;
  shortDescription: string;
  price: number;
  imageUrl?: string;
  productType: ProductType;
}

// Cart DTOs
export interface CartItemDto {
  id: number;
  userId: number;
  productId: number;
  product?: ProductDto;
  quantity: number;
}

export interface AddToCartRequestDto {
  productId: number;
  quantity: number;
}

export interface UpdateQuantityRequestDto {
  quantity: number;
}

// Pagination DTOs
export interface PaginationRequest {
  page: number;
  pageSize: number;
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasNextPage: boolean;
  hasPreviousPage: boolean;
  startIndex: number;
  endIndex: number;
}

// Notification DTOs
export interface Notification {
  message: string;
  type: NotificationType;
  createdAt: string;
  title?: string;
  metadata: Record<string, any>;
}

export interface NotificationStatus {
  hasNotifications: boolean;
  totalCount: number;
  latestNotification?: Notification;
  lastUpdated?: string;
}

// API Response DTOs
export interface ApiResponse<T> {
  data?: T;
  error?: string;
  isSuccess: boolean;
  errorMessage?: string;
}

export interface ErrorResponse {
  error: string;
}