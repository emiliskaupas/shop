import React, { useEffect, useState } from 'react';
import {
  Box,
  Typography,
  Button,
  Card,
  CardContent,
  CardMedia,
  CardActions,
  IconButton,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Alert,
  CircularProgress,
  Chip,
  Avatar,
  Stack,
} from '@mui/material';
import {
  Add as AddIcon,
  Edit as EditIcon,
  Delete as DeleteIcon,
  Person as PersonIcon,
} from '@mui/icons-material';
import { useAuthStore } from '../../stores/authStore';
import { useProductsStore } from '../../stores/productsStore';
import { ProductForm } from '../products/ProductForm';
import { ProductDto } from '../../types/api';
import { ProductType } from '../../types/enums';

const getProductTypeLabel = (type: ProductType): string => {
  return ProductType[type];
};

interface ProductCardProps {
  product: ProductDto;
  onEdit: (product: ProductDto) => void;
  onDelete: (product: ProductDto) => void;
  isDeleting?: boolean;
}

const ProductCard: React.FC<ProductCardProps> = ({ 
  product, 
  onEdit, 
  onDelete, 
  isDeleting = false 
}) => {
  return (
    <Card sx={{ height: '100%', display: 'flex', flexDirection: 'column' }}>
      <CardMedia sx={{ position: 'relative', paddingTop: '56.25%' }}>
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
          Created: {new Date(product.createdAt).toLocaleDateString()}
        </Typography>
      </CardContent>
      
      <CardActions>
        <Button
          size="small"
          startIcon={<EditIcon />}
          onClick={() => onEdit(product)}
        >
          Edit
        </Button>
        <Button
          size="small"
          color="error"
          startIcon={isDeleting ? <CircularProgress size={16} /> : <DeleteIcon />}
          onClick={() => onDelete(product)}
          disabled={isDeleting}
        >
          {isDeleting ? 'Deleting...' : 'Delete'}
        </Button>
      </CardActions>
    </Card>
  );
};

export const Profile: React.FC = () => {
  const { user } = useAuthStore();
  const {
    myProducts,
    isLoading,
    error,
    loadMyProducts,
    deleteProduct,
    clearError,
  } = useProductsStore();
  
  const [showProductForm, setShowProductForm] = useState(false);
  const [editingProduct, setEditingProduct] = useState<ProductDto | null>(null);
  const [deleteConfirm, setDeleteConfirm] = useState<ProductDto | null>(null);
  const [deletingId, setDeletingId] = useState<number | null>(null);

  useEffect(() => {
    if (user) {
      loadMyProducts();
    }
  }, [user, loadMyProducts]);

  const handleAddProduct = () => {
    setEditingProduct(null);
    setShowProductForm(true);
  };

  const handleEditProduct = (product: ProductDto) => {
    setEditingProduct(product);
    setShowProductForm(true);
  };

  const handleDeleteProduct = (product: ProductDto) => {
    setDeleteConfirm(product);
  };

  const confirmDelete = async () => {
    if (!deleteConfirm) return;
    
    setDeletingId(deleteConfirm.id);
    try {
      const success = await deleteProduct(deleteConfirm.id);
      if (success) {
        setDeleteConfirm(null);
      }
    } finally {
      setDeletingId(null);
    }
  };

  const handleProductFormSubmit = (product: ProductDto) => {
    setShowProductForm(false);
    setEditingProduct(null);
    loadMyProducts(); // Refresh the list
  };

  const handleProductFormCancel = () => {
    setShowProductForm(false);
    setEditingProduct(null);
  };

  if (!user) {
    return (
      <Box maxWidth="lg" mx="auto" p={2}>
        <Alert severity="warning">
          Please log in to view your profile.
        </Alert>
      </Box>
    );
  }

  return (
    <Box maxWidth="lg" mx="auto" p={2}>
      {/* User Info Section */}
      <Card sx={{ mb: 4 }}>
        <CardContent>
          <Box display="flex" alignItems="center" gap={2}>
            <Avatar sx={{ width: 64, height: 64, bgcolor: 'primary.main' }}>
              <PersonIcon sx={{ fontSize: 32 }} />
            </Avatar>
            <Box>
              <Typography variant="h4" gutterBottom>
                {user.username}
              </Typography>
              <Typography variant="body1" color="text.secondary" gutterBottom>
                {user.email}
              </Typography>
              <Chip
                label={user.role === 0 ? 'Customer' : 'Admin'}
                color="primary"
                variant="outlined"
              />
            </Box>
          </Box>
        </CardContent>
      </Card>

      {/* Products Section */}
      <Box display="flex" justifyContent="space-between" alignItems="center" mb={3}>
        <Typography variant="h5">
          My Products ({myProducts.length})
        </Typography>
        <Button
          variant="contained"
          startIcon={<AddIcon />}
          onClick={handleAddProduct}
        >
          Add Product
        </Button>
      </Box>
      
      {error && (
        <Alert severity="error" onClose={clearError} sx={{ mb: 2 }}>
          {error}
        </Alert>
      )}
      
      {isLoading ? (
        <Box display="flex" justifyContent="center" py={4}>
          <CircularProgress />
        </Box>
      ) : myProducts.length === 0 ? (
        <Card>
          <CardContent sx={{ textAlign: 'center', py: 6 }}>
            <Typography variant="h6" gutterBottom>
              No products yet
            </Typography>
            <Typography variant="body2" color="text.secondary" mb={3}>
              Start by adding your first product to the marketplace
            </Typography>
            <Button variant="contained" startIcon={<AddIcon />} onClick={handleAddProduct}>
              Add Your First Product
            </Button>
          </CardContent>
        </Card>
      ) : (
        <Box
          display="grid"
          gridTemplateColumns={{
            xs: '1fr',
            sm: 'repeat(2, 1fr)',
            md: 'repeat(3, 1fr)',
            lg: 'repeat(4, 1fr)',
          }}
          gap={3}
        >
          {myProducts.map((product) => (
            <ProductCard
              key={product.id}
              product={product}
              onEdit={handleEditProduct}
              onDelete={handleDeleteProduct}
              isDeleting={deletingId === product.id}
            />
          ))}
        </Box>
      )}
      
      {/* Product Form Dialog */}
      <Dialog
        open={showProductForm}
        onClose={handleProductFormCancel}
        maxWidth="md"
        fullWidth
      >
        <DialogContent>
          <ProductForm
            product={editingProduct || undefined}
            onSubmit={handleProductFormSubmit}
            onCancel={handleProductFormCancel}
          />
        </DialogContent>
      </Dialog>
      
      {/* Delete Confirmation Dialog */}
      <Dialog open={!!deleteConfirm} onClose={() => setDeleteConfirm(null)}>
        <DialogTitle>Confirm Delete</DialogTitle>
        <DialogContent>
          <Typography>
            Are you sure you want to delete "{deleteConfirm?.name}"? This action cannot be undone
            and will remove the product from all user carts.
          </Typography>
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setDeleteConfirm(null)}>Cancel</Button>
          <Button
            onClick={confirmDelete}
            color="error"
            disabled={!!deletingId}
            startIcon={deletingId ? <CircularProgress size={16} /> : null}
          >
            {deletingId ? 'Deleting...' : 'Delete'}
          </Button>
        </DialogActions>
      </Dialog>
    </Box>
  );
};