import React, { useEffect, useCallback, useRef, useState } from 'react';
import { Link } from 'react-router-dom';
import {
  Box,
  Card,
  CardMedia,
  CardContent,
  Typography,
  Button,
  CircularProgress,
  Alert,
  Avatar,
  Chip,
  IconButton,
  CardActions,
  Stack,
  Paper,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Slider,
  FormControlLabel,
  Checkbox,
  Divider,
  TextField,
} from '@mui/material';
import {
  Add as AddIcon,
  ShoppingCart as CartIcon,
  Inventory as InventoryIcon,
  FilterList as FilterIcon,
} from '@mui/icons-material';
import { useProducts, useCart, useAuth } from '../../hooks';
import { useError } from '../../contexts/ErrorContext';
import { useToast } from '../../contexts/ToastContext';
import { LoadingSpinner, ProductListSkeleton } from '../common/LoadingStates';
import { ErrorState, EmptyState } from '../common/ErrorStates';
import { ProductDto, UserDto } from '../../types/api';
import { ProductType } from '../../types/enums';

const getProductTypeLabel = (type: ProductType): string => {
  return ProductType[type];
};

const getAllProductTypes = (): ProductType[] => {
  return Object.values(ProductType).filter(value => typeof value === 'number') as ProductType[];
};

interface FilterState {
  productTypes: ProductType[];
  priceRange: [number, number];
  minPrice: number;
  maxPrice: number;
}

interface FilterSidebarProps {
  filters: FilterState;
  onFiltersChange: (filters: FilterState) => void;
}

const FilterSidebar: React.FC<FilterSidebarProps> = ({ filters, onFiltersChange }) => {
  const handleProductTypeChange = (productType: ProductType, checked: boolean) => {
    const newProductTypes = checked
      ? [...filters.productTypes, productType]
      : filters.productTypes.filter(type => type !== productType);
    
    onFiltersChange({
      ...filters,
      productTypes: newProductTypes,
    });
  };

  const handlePriceRangeChange = (event: Event, newValue: number | number[]) => {
    const value = newValue as [number, number];
    onFiltersChange({
      ...filters,
      priceRange: value,
    });
  };

  const handleMinPriceChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const minPrice = Math.max(0, Math.min(parseFloat(event.target.value) || 0, filters.maxPrice - 1));
    onFiltersChange({
      ...filters,
      priceRange: [minPrice, filters.priceRange[1]],
    });
  };

  const handleMaxPriceChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const maxPrice = Math.max(filters.minPrice + 1, parseFloat(event.target.value) || filters.maxPrice);
    onFiltersChange({
      ...filters,
      priceRange: [filters.priceRange[0], maxPrice],
    });
  };

  const clearFilters = () => {
    onFiltersChange({
      ...filters,
      productTypes: [],
      priceRange: [filters.minPrice, filters.maxPrice],
    });
  };

  return (
    <Paper sx={{ p: 3, height: 'fit-content', minWidth: 280 }}>
      <Box display="flex" alignItems="center" mb={2}>
        <FilterIcon sx={{ mr: 1 }} />
        <Typography variant="h6">Filters</Typography>
        <Button 
          size="small" 
          onClick={clearFilters}
          sx={{ ml: 'auto' }}
        >
          Clear All
        </Button>
      </Box>
      
      <Divider sx={{ mb: 2 }} />
      
      {/* Product Type Filter */}
      <Typography variant="subtitle1" gutterBottom>
        Product Type
      </Typography>
      <Box sx={{ mb: 3 }}>
        {getAllProductTypes().map(productType => (
          <FormControlLabel
            key={productType}
            control={
              <Checkbox
                checked={filters.productTypes.includes(productType)}
                onChange={(e) => handleProductTypeChange(productType, e.target.checked)}
                size="small"
              />
            }
            label={getProductTypeLabel(productType)}
            sx={{ display: 'block', mb: 0.5 }}
          />
        ))}
      </Box>
      
      <Divider sx={{ mb: 2 }} />
      
      {/* Price Range Filter */}
      <Typography variant="subtitle1" gutterBottom>
        Price Range
      </Typography>
      <Box sx={{ px: 1, mb: 2 }}>
        <Slider
          value={filters.priceRange}
          onChange={handlePriceRangeChange}
          valueLabelDisplay="auto"
          valueLabelFormat={(value) => `$${value}`}
          min={filters.minPrice}
          max={filters.maxPrice}
          step={10}
        />
      </Box>
      
      <Box display="flex" gap={1} mb={2}>
        <TextField
          label="Min Price"
          type="number"
          value={filters.priceRange[0]}
          onChange={handleMinPriceChange}
          size="small"
          inputProps={{ min: filters.minPrice, max: filters.maxPrice - 1, step: 1 }}
          InputProps={{
            startAdornment: <Typography variant="body2">$</Typography>,
          }}
        />
        <TextField
          label="Max Price"
          type="number"
          value={filters.priceRange[1]}
          onChange={handleMaxPriceChange}
          size="small"
          inputProps={{ min: filters.minPrice + 1, max: filters.maxPrice, step: 1 }}
          InputProps={{
            startAdornment: <Typography variant="body2">$</Typography>,
          }}
        />
      </Box>
    </Paper>
  );
};

interface ProductCardProps {
  product: ProductDto;
  onAddToCart: (product: ProductDto) => void;
  isAddingToCart?: boolean;
  currentUser?: UserDto | null;
}

const ProductCard: React.FC<ProductCardProps> = ({ 
  product, 
  onAddToCart, 
  isAddingToCart = false,
  currentUser 
}) => {
  return (
    <Card sx={{ 
      height: 533, // Fixed height (increased by 1/3)
      width: '100%', // Fixed width
      display: 'flex', 
      flexDirection: 'column',
      maxWidth: 373, // Maximum width to prevent cards from getting too wide (increased by 1/3)
    }}>
      <CardMedia sx={{ 
        position: 'relative', 
        height: 267, // Fixed height instead of responsive padding (increased by 1/3)
        width: '100%',
      }}>
        {product.imageUrl ? (
          <img
            src={product.imageUrl}
            alt={product.name}
            style={{
              position: 'absolute',
              top: 0,
              left: 0,
              width: '100%',
              height: '100%',
              objectFit: 'cover',
            }}
          />
        ) : (
          <Box
            sx={{
              position: 'absolute',
              top: 0,
              left: 0,
              width: '100%',
              height: '100%',
              display: 'flex',
              alignItems: 'center',
              justifyContent: 'center',
              bgcolor: 'grey.200',
            }}
          >
            <Avatar sx={{ width: 80, height: 80 }}>
              {product.name.charAt(0)}
            </Avatar>
          </Box>
        )}
      </CardMedia>
      
      <CardContent sx={{ flexGrow: 1 }}>
        <Typography variant="h6" gutterBottom noWrap>
          {product.name}
        </Typography>
        
        <Typography variant="body2" color="text.secondary" gutterBottom sx={{ 
          display: '-webkit-box',
          WebkitLineClamp: 2,
          WebkitBoxOrient: 'vertical',
          overflow: 'hidden',
        }}>
          {product.shortDescription}
        </Typography>
        
        <Box display="flex" justifyContent="space-between" alignItems="center" mb={1}>
          <Chip 
            label={getProductTypeLabel(product.productType)} 
            size="small" 
            color="primary"
            variant="outlined"
          />
          <Typography variant="h6" color="primary">
            ${product.price.toFixed(2)}
          </Typography>
        </Box>
        
        <Typography variant="caption" color="text.secondary">
          By {product.createdByUserName}
        </Typography>
      </CardContent>
      
      <CardActions>
        {currentUser && currentUser.id === product.createdByUserId ? (
          <Button
            variant="contained"
            fullWidth
            sx={{
              bgcolor: 'success.main',
              color: 'success.contrastText',
              '&:hover': {
                bgcolor: 'success.dark',
              },
              cursor: 'default'
            }}
            disabled
          >
            Your Product
          </Button>
        ) : (
          <Button
            variant="contained"
            fullWidth
            startIcon={isAddingToCart ? <CircularProgress size={16} /> : <CartIcon />}
            onClick={() => onAddToCart(product)}
            disabled={isAddingToCart}
          >
            {isAddingToCart ? 'Adding...' : 'Add to Cart'}
          </Button>
        )}
      </CardActions>
    </Card>
  );
};

export const ProductList: React.FC = () => {
  const { user } = useAuth();
  const { handleError, handleNetworkError } = useError();
  const { showSuccess, showError } = useToast();
  
  const {
    products,
    isLoading,
    error,
    hasMore,
    totalCount,
    loadProducts,
    loadMoreProducts,
    clearError,
  } = useProducts();
  
  const { addToCart } = useCart();
  const [addingToCartId, setAddingToCartId] = useState<number | null>(null);
  
  // Filter state
  const [filters, setFilters] = useState<FilterState>({
    productTypes: [],
    priceRange: [0, 5000],
    minPrice: 0,
    maxPrice: 5000,
  });
  
  // Update price range based on loaded products
  useEffect(() => {
    if (products.length > 0) {
      const prices = products.map(p => p.price);
      const minPrice = 0;
      const maxPrice = 5000;
      
      setFilters(prev => ({
        ...prev,
        minPrice,
        maxPrice,
        priceRange: prev.priceRange[0] === prev.minPrice && prev.priceRange[1] === prev.maxPrice
          ? [minPrice, maxPrice]
          : prev.priceRange,
      }));
    }
  }, [products]);
  
  // Filter products based on current filters
  const filteredProducts = products.filter(product => {
    // Filter by product type
    if (filters.productTypes.length > 0 && !filters.productTypes.includes(product.productType)) {
      return false;
    }
    
    // Filter by price range
    if (product.price < filters.priceRange[0] || product.price > filters.priceRange[1]) {
      return false;
    }
    
    return true;
  });
  
  const observerRef = useRef<IntersectionObserver | null>(null);
  const lastProductElementRef = useCallback((node: HTMLDivElement) => {
    if (isLoading) return;
    if (observerRef.current) observerRef.current.disconnect();
    
    observerRef.current = new IntersectionObserver(entries => {
      if (entries[0].isIntersecting && hasMore) {
        loadMoreProducts().catch(error => {
          handleNetworkError(error, 'Loading more products');
        });
      }
    });
    
    if (node) observerRef.current.observe(node);
  }, [isLoading, hasMore, loadMoreProducts, handleNetworkError]);

  const handleRetryLoad = useCallback(async () => {
    try {
      clearError();
      await loadProducts(1, 10);
    } catch (error) {
      handleNetworkError(error as Error, 'Loading products');
    }
  }, [clearError, loadProducts, handleNetworkError]);

  useEffect(() => {
    loadProducts(1, 10).catch(error => {
      handleNetworkError(error, 'Loading products');
    });
  }, [loadProducts, handleNetworkError]);

  const handleAddToCart = async (product: ProductDto) => {
    setAddingToCartId(product.id);
    try {
      const result = await addToCart(product.id, 1, product);
      if (result.success) {
        showSuccess(`Added ${product.name} to cart`);
      } else {
        handleError(result.error || 'Failed to add item to cart', 'Add to cart');
      }
    } catch (error) {
      handleError(error as Error, 'Add to cart');
    } finally {
      setAddingToCartId(null);
    }
  };

  // Show error state for critical errors
  if (error && products.length === 0) {
    return (
      <ErrorState
        variant="server-error"
        title="Unable to load products"
        message={error}
        onRetry={handleRetryLoad}
      />
    );
  }

  // Show loading skeleton for initial load
  if (isLoading && products.length === 0) {
    return (
      <Box maxWidth="xl" mx="auto" p={2}>
        <Typography variant="h4" gutterBottom mb={3}>
          Products
        </Typography>
        
        <Box display="flex" gap={3}>
          {/* Filter Sidebar Skeleton */}
          <Box sx={{ minWidth: 280, display: { xs: 'none', md: 'block' } }}>
            <Paper sx={{ p: 3, height: 400 }}>
              <Typography variant="h6" gutterBottom>Filters</Typography>
              <Box sx={{ opacity: 0.3 }}>
                <Typography variant="body2" mb={1}>Loading filters...</Typography>
              </Box>
            </Paper>
          </Box>
          
          {/* Main Content Skeleton */}
          <Box flex={1}>
            <ProductListSkeleton count={8} />
          </Box>
        </Box>
      </Box>
    );
  }

  return (
    <Box maxWidth="xl" mx="auto" p={2}>
      <Typography variant="h4" gutterBottom mb={3}>
        Products
      </Typography>
      
      <Box display="flex" gap={3}>
        {/* Filter Sidebar */}
        <Box sx={{ minWidth: 280, display: { xs: 'none', md: 'block' } }}>
          <FilterSidebar filters={filters} onFiltersChange={setFilters} />
        </Box>
        
        {/* Main Content */}
        <Box flex={1}>
          {filteredProducts.length === 0 && products.length > 0 ? (
            <EmptyState
              title="No products match your filters"
              message="Try adjusting your filters to see more products."
              icon={<FilterIcon sx={{ fontSize: 64 }} />}
            />
          ) : products.length === 0 ? (
            <EmptyState
              title="No products available"
              message="Be the first to add a product to our marketplace!"
              actionText={user ? "Add Product" : undefined}
              onAction={user ? () => window.location.href = '/profile' : undefined}
              icon={<InventoryIcon sx={{ fontSize: 64 }} />}
            />
          ) : (
            <>
              <Box display="flex" justifyContent="space-between" alignItems="center" mb={2}>
                <Typography variant="body1" color="text.secondary">
                  Showing {filteredProducts.length} of {totalCount} products
                </Typography>
              </Box>
              
              <Box
                display="grid"
                gridTemplateColumns={{
                  xs: 'repeat(auto-fit, minmax(333px, 1fr))',
                  sm: 'repeat(auto-fit, minmax(333px, 373px))',
                  md: 'repeat(auto-fit, minmax(333px, 373px))',
                  lg: 'repeat(auto-fit, minmax(333px, 373px))',
                }}
                gap={3}
                justifyContent="center"
                sx={{
                  '& > *': {
                    width: '100%',
                    maxWidth: 373,
                    justifySelf: 'center',
                  }
                }}
              >
                {filteredProducts.map((product, index) => (
                  <Box
                    key={product.id}
                    ref={index === products.length - 1 ? lastProductElementRef : null}
                  >
                    <ProductCard
                      product={product}
                      onAddToCart={handleAddToCart}
                      isAddingToCart={addingToCartId === product.id}
                      currentUser={user}
                    />
                  </Box>
                ))}
              </Box>
              
              {isLoading && (
                <Box display="flex" justifyContent="center" mt={4}>
                  <LoadingSpinner message="Loading more products..." />
                </Box>
              )}
              
              {!hasMore && products.length > 0 && (
                <Box textAlign="center" mt={4}>
                  <Typography variant="body2" color="text.secondary">
                    You've reached the end of our products!
                  </Typography>
                </Box>
              )}
            </>
          )}
        </Box>
      </Box>
    </Box>
  );
};