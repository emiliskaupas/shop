import React from 'react';
import {
  Box,
  Container,
  Typography,
  Link,
  IconButton,
  Divider,
  useTheme,
} from '@mui/material';
import { Link as RouterLink } from 'react-router-dom';
import {
  Email as EmailIcon,
  Phone as PhoneIcon,
  LocationOn as LocationIcon,
} from '@mui/icons-material';

export const Footer: React.FC = () => {
  const theme = useTheme();
  const currentYear = new Date().getFullYear();

  return (
    <Box
      component="footer"
      sx={{
        backgroundColor: theme.palette.mode === 'light' ? 'grey.100' : 'grey.900',
        mt: 'auto',
        py: 3,
      }}
    >
      <Container maxWidth="lg">
        <Box
          sx={{
            display: 'flex',
            flexWrap: 'wrap',
            gap: 4,
            justifyContent: 'space-between',
          }}
        >
          {/* Company Info */}
          <Box sx={{ flex: '1 1 250px', minWidth: '250px' }}>
            <Typography variant="h6" color="text.primary" gutterBottom>
              Shop App
            </Typography>
            <Typography variant="body2" color="text.secondary" paragraph>
              Your one-stop destination for quality products. We offer a wide range of items 
              with excellent customer service and competitive prices.
            </Typography>
          </Box>

          {/* Quick Links */}
          <Box sx={{ flex: '1 1 200px', minWidth: '200px' }}>
            <Typography variant="h6" color="text.primary" gutterBottom>
              Quick Links
            </Typography>
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1 }}>
              <Link component={RouterLink} to="/products" color="text.secondary" underline="hover">
                Home
              </Link>
              <Link component={RouterLink} to="/cart" color="text.secondary" underline="hover">
                Shopping Cart
              </Link>
              <Link component={RouterLink} to="/profile" color="text.secondary" underline="hover">
                My Account
              </Link>
            </Box>
          </Box>

          {/* About */}
          <Box sx={{ flex: '1 1 200px', minWidth: '200px' }}>
            <Typography variant="h6" color="text.primary" gutterBottom>
              About
            </Typography>
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1 }}>
              <Typography variant="body2" color="text.secondary">
                Quality products
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Fast shipping
              </Typography>
              <Typography variant="body2" color="text.secondary">
                Customer support
              </Typography>
            </Box>
          </Box>

          {/* Contact Info */}
          <Box sx={{ flex: '1 1 250px', minWidth: '250px' }}>
            <Typography variant="h6" color="text.primary" gutterBottom>
              Contact Info
            </Typography>
            <Box sx={{ display: 'flex', flexDirection: 'column', gap: 1.5 }}>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                <LocationIcon color="primary" fontSize="small" />
                <Typography variant="body2" color="text.secondary">
                  123 Shop Street, Commerce City, CC 12345
                </Typography>
              </Box>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                <PhoneIcon color="primary" fontSize="small" />
                <Typography variant="body2" color="text.secondary">
                  +1 (555) 123-4567
                </Typography>
              </Box>
              <Box sx={{ display: 'flex', alignItems: 'center', gap: 1 }}>
                <EmailIcon color="primary" fontSize="small" />
                <Typography variant="body2" color="text.secondary">
                  support@shopapp.com
                </Typography>
              </Box>
            </Box>
          </Box>
        </Box>

        <Divider sx={{ my: 3 }} />

        {/* Bottom Section */}
        <Box
          sx={{
            display: 'flex',
            justifyContent: 'center',
            alignItems: 'center',
            flexDirection: { xs: 'column', sm: 'row' },
            gap: 2,
          }}
        >
          <Typography variant="body2" color="text.secondary">
            © {currentYear} Shop App. All rights reserved.
          </Typography>
        </Box>
      </Container>
    </Box>
  );
};