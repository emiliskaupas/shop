import React, { useState, useEffect } from 'react';
import {
  Box,
  Card,
  CardContent,
  TextField,
  Button,
  Typography,
  Alert,
  CircularProgress,
  MenuItem,
  FormControl,
  InputLabel,
  Select,
} from '@mui/material';
import { ProductDto, CreateProductDto, UpdateProductDto } from '../../types/api';
import { ProductType } from '../../types/enums';
import { useProductsStore } from '../../stores/productsStore';

interface ProductFormProps {
  product?: ProductDto;
  onSubmit?: (product: ProductDto) => void;
  onCancel?: () => void;
}

const productTypeOptions = Object.entries(ProductType)
  .filter(([key]) => isNaN(Number(key)))
  .map(([key, value]) => ({
    value: value as number,
    label: key,
  }));

export const ProductForm: React.FC<ProductFormProps> = ({
  product,
  onSubmit,
  onCancel,
}) => {
  const { createProduct, updateProduct, isLoading, error, clearError } = useProductsStore();
  
  const [formData, setFormData] = useState<CreateProductDto>({
    name: product?.name || '',
    shortDescription: product?.shortDescription || '',
    price: product?.price || 0,
    imageUrl: product?.imageUrl || '',
    productType: product?.productType ?? ProductType.Electronics,
  });
  
  const [validationErrors, setValidationErrors] = useState<Record<string, string>>({});

  useEffect(() => {
    if (product) {
      setFormData({
        name: product.name,
        shortDescription: product.shortDescription,
        price: product.price,
        imageUrl: product.imageUrl || '',
        productType: product.productType,
      });
    }
  }, [product]);

  const handleChange = (e: React.ChangeEvent<HTMLInputElement | HTMLTextAreaElement>) => {
    const { name, value } = e.target;
    
    let processedValue: any = value;
    if (name === 'price') {
      processedValue = parseFloat(value) || 0;
    } else if (name === 'productType') {
      processedValue = parseInt(value) as ProductType;
    }
    
    setFormData(prev => ({ ...prev, [name]: processedValue }));
    
    // Clear validation error when user starts typing
    if (validationErrors[name]) {
      setValidationErrors(prev => ({ ...prev, [name]: '' }));
    }
    
    // Clear general error
    if (error) {
      clearError();
    }
  };

  const validateForm = (): boolean => {
    const errors: Record<string, string> = {};
    
    if (!formData.name.trim()) {
      errors.name = 'Product name is required';
    } else if (formData.name.length < 3) {
      errors.name = 'Product name must be at least 3 characters long';
    }
    else if (formData.name.length > 100) {
      errors.name = 'Product name must be at most 100 characters long';
    }
    
    if (!formData.shortDescription.trim()) {
      errors.shortDescription = 'Description is required';
    } else if (formData.shortDescription.length < 10) {
      errors.shortDescription = 'Description must be at least 10 characters long';
    }
    else if (formData.shortDescription.length > 500) {
      errors.shortDescription = 'Description must be at most 500 characters long';
    }
    
    
    if (formData.price <= 0) {
      errors.price = 'Price must be greater than 0';
    } else if (formData.price > 999999) {
      errors.price = 'Price cannot exceed $999,999';
    }
    
    if (formData.imageUrl && !/^https?:\/\/.+\.(jpg|jpeg|png|gif|webp)$/i.test(formData.imageUrl)) {
      errors.imageUrl = 'Please enter a valid image URL (jpg, jpeg, png, gif, webp)';
    }
    
    setValidationErrors(errors);
    return Object.keys(errors).length === 0;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    
    if (!validateForm()) {
      return;
    }
    
    try {
      let result: ProductDto | null = null;
      
      if (product) {
        // Update existing product
        result = await updateProduct(product.id, formData as UpdateProductDto);
      } else {
        // Create new product
        result = await createProduct(formData);
      }
      
      if (result && onSubmit) {
        onSubmit(result);
      }
    } catch (err) {
      console.error('Failed to save product:', err);
    }
  };

  return (
    <Card>
      <CardContent>
        <Typography variant="h5" gutterBottom>
          {product ? 'Edit Product' : 'Add New Product'}
        </Typography>
        
        {error && (
          <Alert severity="error" onClose={clearError} sx={{ mb: 2 }}>
            {error}
          </Alert>
        )}
        
        <Box component="form" onSubmit={handleSubmit}>
          <TextField
            fullWidth
            label="Product Name"
            name="name"
            value={formData.name}
            onChange={handleChange}
            error={!!validationErrors.name}
            helperText={validationErrors.name}
            margin="normal"
            required
          />
          
          <TextField
            fullWidth
            label="Description"
            name="shortDescription"
            multiline
            rows={3}
            value={formData.shortDescription}
            onChange={handleChange}
            error={!!validationErrors.shortDescription}
            helperText={validationErrors.shortDescription}
            margin="normal"
            required
          />
          
          <TextField
            fullWidth
            label="Price"
            name="price"
            type="number"
            inputProps={{
              min: 0,
              step: 0.01,
            }}
            value={formData.price}
            onChange={handleChange}
            error={!!validationErrors.price}
            helperText={validationErrors.price}
            margin="normal"
            required
          />
          
          <FormControl fullWidth margin="normal">
            <InputLabel>Product Type</InputLabel>
            <Select
              name="productType"
              value={formData.productType}
              onChange={handleChange as any}
              label="Product Type"
            >
              {productTypeOptions.map((option) => (
                <MenuItem key={option.value} value={option.value}>
                  {option.label}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          
          <TextField
            fullWidth
            label="Image URL (optional)"
            name="imageUrl"
            value={formData.imageUrl}
            onChange={handleChange}
            error={!!validationErrors.imageUrl}
            helperText={validationErrors.imageUrl || 'Enter a direct link to an image (jpg, png, etc.)'}
            margin="normal"
          />
          
          <Box display="flex" gap={2} justifyContent="flex-end" mt={3}>
            {onCancel && (
              <Button variant="outlined" onClick={onCancel}>
                Cancel
              </Button>
            )}
            <Button
              type="submit"
              variant="contained"
              disabled={isLoading}
              startIcon={isLoading ? <CircularProgress size={16} /> : null}
            >
              {isLoading ? 'Saving...' : product ? 'Update Product' : 'Create Product'}
            </Button>
          </Box>
        </Box>
      </CardContent>
    </Card>
  );
};