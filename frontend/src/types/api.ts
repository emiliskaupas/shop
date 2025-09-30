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

export interface LoginResponseDto {
  user: UserDto;
  token: string;
  expiresAt: string;
}

// Product DTOs
export interface ProductDto {
  id: number;
  name: string;
  shortDescription: string;
  price: number;
  imageUrl?: string;
  productType: ProductType;
  createdByUserId: number;
  createdByUserName: string;
  createdByEmail: string;
  createdAt: string;
  modifiedAt?: string;
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

// API Response Types
export interface ApiSuccessResponse<T> {
  data: T;
  success: true;
}

export interface ApiErrorResponse {
  error: string;
  success: false;
}

export type ApiResponse<T> = ApiSuccessResponse<T> | ApiErrorResponse;

// Local Cart Item (for localStorage)
export interface LocalCartItem {
  productId: number;
  quantity: number;
  product?: ProductDto;
}