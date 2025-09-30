import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import {
  Box,
  Card,
  CardContent,
  Typography,
  Button,
  IconButton,
  Divider,
  Alert,
  CircularProgress,
  Avatar,
  Chip,
  Stack,
  Paper,
  TextField,
} from '@mui/material';
import {
  Add as AddIcon,
  Remove as RemoveIcon,
  Delete as DeleteIcon,
  ShoppingCart as ShoppingCartIcon,
} from '@mui/icons-material';
import { useCart } from '../../hooks/useCart';
import { useAuth } from '../../hooks/useAuth';
import { CartItemDto, LocalCartItem } from '../../types/api';
import { ProductType } from '../../types/enums';

const isCartItemDto = (item: CartItemDto | LocalCartItem): item is CartItemDto => {
  return 'userId' in item;
};

const getProduct = (item: CartItemDto | LocalCartItem) => {
  return item.product;
};

const getProductTypeLabel = (type: ProductType): string => {
  return ProductType[type];
};

// Custom Quantity Input Component
interface QuantityInputProps {
  value: number;
  onQuantityChange: (newQuantity: number) => void;
  min?: number;
  max?: number;
}

const QuantityInput: React.FC<QuantityInputProps> = ({ 
  value, 
  onQuantityChange, 
  min = 1, 
  max = 999 
}) => {
  const [inputValue, setInputValue] = useState<string>(value.toString());
  const [isEditing, setIsEditing] = useState(false);

  const handleInputChange = (event: React.ChangeEvent<HTMLInputElement>) => {
    const newValue = event.target.value;
    // Allow empty string and numbers only
    if (newValue === '' || /^\d+$/.test(newValue)) {
      setInputValue(newValue);
    }
  };

  const handleInputBlur = () => {
    setIsEditing(false);
    const numValue = parseInt(inputValue) || min;
    const clampedValue = Math.max(min, Math.min(max, numValue));
    setInputValue(clampedValue.toString());
    if (clampedValue !== value) {
      onQuantityChange(clampedValue);
    }
  };

  const handleInputKeyDown = (event: React.KeyboardEvent<HTMLInputElement>) => {
    if (event.key === 'Enter') {
      handleInputBlur();
    } else if (event.key === 'Escape') {
      setInputValue(value.toString());
      setIsEditing(false);
    }
  };

  const handleIncrement = () => {
    const newValue = Math.min(max, value + 1);
    if (newValue !== value) {
      onQuantityChange(newValue);
    }
  };

  const handleDecrement = () => {
    const newValue = Math.max(min, value - 1);
    if (newValue !== value) {
      onQuantityChange(newValue);
    }
  };

  // Update input value when prop value changes
  React.useEffect(() => {
    if (!isEditing) {
      setInputValue(value.toString());
    }
  }, [value, isEditing]);

  return (
    <Box display="flex" alignItems="center" gap={1}>
      <IconButton
        size="small"
        onClick={handleDecrement}
        disabled={value <= min}
        sx={{
          border: 1,
          borderColor: 'divider',
          '&:hover': {
            backgroundColor: 'action.hover',
          }
        }}
      >
        <RemoveIcon />
      </IconButton>
      
      <TextField
        value={inputValue}
        onChange={handleInputChange}
        onBlur={handleInputBlur}
        onFocus={() => setIsEditing(true)}
        onKeyDown={handleInputKeyDown}
        size="small"
        inputProps={{
          style: { 
            textAlign: 'center', 
            padding: '8px 4px',
            fontSize: '0.875rem',
            fontWeight: 'medium'
          },
          min: min,
          max: max,
        }}
        sx={{
          width: 60,
          '& .MuiOutlinedInput-root': {
            '& fieldset': {
              borderColor: 'divider',
            },
            '&:hover fieldset': {
              borderColor: 'primary.main',
            },
            '&.Mui-focused fieldset': {
              borderColor: 'primary.main',
            },
          },
          '& .MuiInputBase-input': {
            '&::-webkit-outer-spin-button, &::-webkit-inner-spin-button': {
              display: 'none',
            },
            '&[type=number]': {
              MozAppearance: 'textfield',
            },
          },
        }}
      />
      
      <IconButton
        size="small"
        onClick={handleIncrement}
        disabled={value >= max}
        sx={{
          border: 1,
          borderColor: 'divider',
          '&:hover': {
            backgroundColor: 'action.hover',
          }
        }}
      >
        <AddIcon />
      </IconButton>
    </Box>
  );
};

export const Cart: React.FC = () => {
  const { user } = useAuth();
  const {
    items,
    total,
    itemCount,
    isInitialLoading,
    error,
    loadCart,
    updateQuantity,
    removeFromCart,
    clearCart,
    clearError,
  } = useCart();

  useEffect(() => {
    loadCart(user?.id);
  }, [user, loadCart]);

  const handleQuantityChange = async (productId: number, newQuantity: number) => {
    await updateQuantity(productId, newQuantity);
  };

  const handleRemoveItem = async (productId: number) => {
    await removeFromCart(productId);
  };

  const handleClearCart = async () => {
    await clearCart();
  };

  if (isInitialLoading) {
    return (
      <Box display="flex" justifyContent="center" alignItems="center" minHeight="50vh">
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box maxWidth="xl" mx="auto" p={2}>
      <Typography variant="h4" gutterBottom>
        Shopping Cart
      </Typography>
      
      {error && (
        <Alert severity="error" onClose={clearError} sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}
      
      {items.length === 0 ? (
        <Card>
          <CardContent sx={{ textAlign: 'center', py: 6 }}>
            <ShoppingCartIcon sx={{ fontSize: 80, color: 'text.secondary', mb: 2 }} />
            <Typography variant="h6" gutterBottom>
              Your cart is empty
            </Typography>
            <Typography variant="body2" color="text.secondary" mb={3}>
              Add some products to your cart to get started
            </Typography>
            <Button variant="contained" component={Link} to="/products">
              Continue Shopping
            </Button>
          </CardContent>
        </Card>
      ) : (
        <Box 
          display="flex" 
          gap={3}
          sx={{
            flexDirection: { xs: 'column', md: 'row' },
            alignItems: { xs: 'stretch', md: 'flex-start' }
          }}
        >
          {/* Cart Items */}
          <Box sx={{ flex: { md: 2 }, width: '100%' }}>
            <Stack spacing={2}>
              {items.map((item) => {
                const product = getProduct(item);
                if (!product) return null;
                
                return (
                  <Card key={`${product.id}-${item.quantity}`}>
                    <CardContent>
                      <Box 
                        display="flex" 
                        alignItems="center" 
                        gap={2}
                        sx={{ 
                          flexDirection: { xs: 'column', sm: 'row' },
                          textAlign: { xs: 'center', sm: 'left' }
                        }}
                      >
                        <Avatar
                          src={product.imageUrl}
                          variant="rounded"
                          sx={{ width: 80, height: 80, alignSelf: { xs: 'center', sm: 'flex-start' } }}
                        >
                          {product.name.charAt(0)}
                        </Avatar>
                        
                        <Box flex={1}>
                          <Typography variant="h6" gutterBottom>
                            {product.name}
                          </Typography>
                          <Typography variant="body2" color="text.secondary" gutterBottom>
                            {product.shortDescription}
                          </Typography>
                          <Chip 
                            label={getProductTypeLabel(product.productType)} 
                            size="small" 
                            color="primary"
                            variant="outlined"
                          />
                        </Box>
                        
                          <Box display="flex" alignItems="center" gap={2} sx={{ flexDirection: { xs: 'column', sm: 'row' } }}>
                          <Typography variant="h6" sx={{ minWidth: 80, textAlign: 'center' }}>
                            ${product.price.toFixed(2)}
                          </Typography>
                          
                          <QuantityInput
                            value={item.quantity}
                            onQuantityChange={(newQuantity) => handleQuantityChange(product.id, newQuantity)}
                            min={1}
                            max={999}
                          />
                          
                          <IconButton
                            color="error"
                            onClick={() => handleRemoveItem(product.id)}
                            sx={{
                              border: 1,
                              borderColor: 'error.main',
                              '&:hover': {
                                backgroundColor: 'error.50',
                              }
                            }}
                          >
                            <DeleteIcon />
                          </IconButton>
                        </Box>
                      </Box>
                    </CardContent>
                  </Card>
                );
              })}
            </Stack>
          </Box>
          
          {/* Checkout Summary */}
          <Box sx={{ flex: { md: 1 }, width: { xs: '100%', md: '350px' } }}>
            <Paper 
              elevation={2} 
              sx={{ 
                position: { md: 'sticky' }, 
                top: 20,
                p: 3,
                borderRadius: 2,
                border: 1,
                borderColor: 'divider'
              }}
            >
              <Typography variant="h6" gutterBottom>
                Order Summary
              </Typography>
              
              <Box display="flex" justifyContent="space-between" alignItems="center" mb={1}>
                <Typography variant="body1">
                  Items ({itemCount}):
                </Typography>
                <Typography variant="body1">
                  ${total.toFixed(2)}
                </Typography>
              </Box>
              
              <Box display="flex" justifyContent="space-between" alignItems="center" mb={1}>
                <Typography variant="body1">
                  Shipping:
                </Typography>
                <Typography variant="body1">
                  FREE
                </Typography>
              </Box>
              
              <Divider sx={{ my: 2 }} />
              
              <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
                <Typography variant="h6">
                  Total:
                </Typography>
                <Typography variant="h6">
                  ${total.toFixed(2)}
                </Typography>
              </Box>
              
              <Stack spacing={2}>
                <Button 
                  variant="contained" 
                  size="large" 
                  fullWidth
                  sx={{ 
                    py: 1.5,
                    fontSize: '1rem',
                    fontWeight: 'bold'
                  }}
                >
                  Proceed to Checkout
                </Button>
                
                <Button 
                  variant="outlined" 
                  component={Link} 
                  to="/products" 
                  fullWidth
                  sx={{ py: 1.5 }}
                >
                  Continue Shopping
                </Button>
                
                <Button
                  variant="text"
                  color="error"
                  onClick={handleClearCart}
                  startIcon={<DeleteIcon />}
                  fullWidth
                  size="small"
                >
                  Clear Cart
                </Button>
              </Stack>
            </Paper>
          </Box>
        </Box>
      )}
    </Box>
  );
};