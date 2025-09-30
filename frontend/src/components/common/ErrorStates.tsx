import React from 'react';
import {
  Box,
  Typography,
  Button,
  Card,
  CardContent,
  CardActions,
  Alert,
  Container,
} from '@mui/material';
import {
  Error as ErrorIcon,
  Warning as WarningIcon,
  Refresh as RefreshIcon,
  Home as HomeIcon,
  WifiOff as OfflineIcon,
  CloudOff as ServerErrorIcon,
} from '@mui/icons-material';
import { useNavigate } from 'react-router-dom';

interface ErrorStateProps {
  title?: string;
  message?: string;
  onRetry?: () => void;
  showHomeButton?: boolean;
  variant?: 'error' | 'warning' | 'offline' | 'server-error' | 'not-found';
}

export const ErrorState: React.FC<ErrorStateProps> = ({
  title,
  message,
  onRetry,
  showHomeButton = true,
  variant = 'error',
}) => {
  const navigate = useNavigate();

  const getVariantConfig = () => {
    switch (variant) {
      case 'offline':
        return {
          icon: <OfflineIcon sx={{ fontSize: 64 }} color="warning" />,
          title: title || 'You\'re offline',
          message: message || 'Please check your internet connection and try again.',
          color: 'warning' as const,
        };
      case 'server-error':
        return {
          icon: <ServerErrorIcon sx={{ fontSize: 64 }} color="error" />,
          title: title || 'Server Error',
          message: message || 'Our servers are experiencing issues. Please try again later.',
          color: 'error' as const,
        };
      case 'not-found':
        return {
          icon: <WarningIcon sx={{ fontSize: 64 }} color="warning" />,
          title: title || 'Not Found',
          message: message || 'The content you\'re looking for could not be found.',
          color: 'warning' as const,
        };
      case 'warning':
        return {
          icon: <WarningIcon sx={{ fontSize: 64 }} color="warning" />,
          title: title || 'Something\'s not right',
          message: message || 'There was an issue processing your request.',
          color: 'warning' as const,
        };
      default:
        return {
          icon: <ErrorIcon sx={{ fontSize: 64 }} color="error" />,
          title: title || 'Something went wrong',
          message: message || 'An unexpected error occurred. Please try again.',
          color: 'error' as const,
        };
    }
  };

  const config = getVariantConfig();

  const handleGoHome = () => {
    navigate('/');
  };

  return (
    <Container maxWidth="sm" sx={{ py: 4 }}>
      <Card elevation={2}>
        <CardContent sx={{ textAlign: 'center', py: 4 }}>
          <Box sx={{ mb: 3 }}>
            {config.icon}
          </Box>
          
          <Typography variant="h5" gutterBottom color={config.color}>
            {config.title}
          </Typography>
          
          <Typography variant="body1" color="text.secondary" paragraph>
            {config.message}
          </Typography>
        </CardContent>

        <CardActions sx={{ justifyContent: 'center', pb: 3, gap: 1 }}>
          {onRetry && (
            <Button
              variant="contained"
              startIcon={<RefreshIcon />}
              onClick={onRetry}
              color={config.color}
            >
              Try Again
            </Button>
          )}
          {showHomeButton && (
            <Button
              variant="outlined"
              startIcon={<HomeIcon />}
              onClick={handleGoHome}
            >
              Go Home
            </Button>
          )}
        </CardActions>
      </Card>
    </Container>
  );
};

interface InlineErrorProps {
  message: string;
  onRetry?: () => void;
  severity?: 'error' | 'warning' | 'info';
}

export const InlineError: React.FC<InlineErrorProps> = ({
  message,
  onRetry,
  severity = 'error',
}) => {
  return (
    <Alert 
      severity={severity} 
      sx={{ my: 2 }}
      action={
        onRetry ? (
          <Button 
            color="inherit" 
            size="small" 
            onClick={onRetry}
            startIcon={<RefreshIcon />}
          >
            Retry
          </Button>
        ) : undefined
      }
    >
      {message}
    </Alert>
  );
};

interface EmptyStateProps {
  title?: string;
  message?: string;
  actionText?: string;
  onAction?: () => void;
  icon?: React.ReactNode;
}

export const EmptyState: React.FC<EmptyStateProps> = ({
  title = 'No items found',
  message = 'There are no items to display at the moment.',
  actionText,
  onAction,
  icon,
}) => {
  return (
    <Box
      display="flex"
      flexDirection="column"
      alignItems="center"
      justifyContent="center"
      py={6}
      textAlign="center"
    >
      {icon && (
        <Box sx={{ mb: 3, opacity: 0.5 }}>
          {icon}
        </Box>
      )}
      
      <Typography variant="h6" gutterBottom color="text.secondary">
        {title}
      </Typography>
      
      <Typography variant="body2" color="text.secondary" paragraph>
        {message}
      </Typography>

      {actionText && onAction && (
        <Button variant="contained" onClick={onAction} sx={{ mt: 1 }}>
          {actionText}
        </Button>
      )}
    </Box>
  );
};