import React, { Component, ErrorInfo, ReactNode } from 'react';
import { 
  Box, 
  Typography, 
  Button, 
  Alert, 
  Card, 
  CardContent, 
  CardActions,
  Divider,
  Container 
} from '@mui/material';
import { 
  Refresh as RefreshIcon, 
  Home as HomeIcon,
  Error as ErrorIcon 
} from '@mui/icons-material';

interface Props {
  children: ReactNode;
  fallback?: ReactNode;
  showErrorDetails?: boolean;
}

interface State {
  hasError: boolean;
  error?: Error;
  errorInfo?: ErrorInfo;
}

export class ErrorBoundary extends Component<Props, State> {
  public state: State = {
    hasError: false,
  };

  public static getDerivedStateFromError(error: Error): State {
    return { hasError: true, error };
  }

  public componentDidCatch(error: Error, errorInfo: ErrorInfo) {
    console.error('Uncaught error:', error, errorInfo);
    
    this.setState({
      error,
      errorInfo,
    });

    // Here you could log to an error reporting service
    // Example: logErrorToService(error, errorInfo);
  }

  private handleReload = () => {
    window.location.reload();
  };

  private handleReset = () => {
    this.setState({ hasError: false, error: undefined, errorInfo: undefined });
  };

  private handleGoHome = () => {
    window.location.href = '/';
  };

  public render() {
    if (this.state.hasError) {
      // Custom fallback UI provided
      if (this.props.fallback) {
        return this.props.fallback;
      }

      // Enhanced default fallback UI
      return (
        <Container maxWidth="md" sx={{ py: 4 }}>
          <Card elevation={3}>
            <CardContent sx={{ textAlign: 'center', py: 4 }}>
              <Box sx={{ mb: 3 }}>
                <ErrorIcon color="error" sx={{ fontSize: 64 }} />
              </Box>
              
              <Typography variant="h4" gutterBottom color="error">
                Oops! Something went wrong
              </Typography>
              
              <Typography variant="body1" color="text.secondary" paragraph>
                We're sorry for the inconvenience. The application encountered an unexpected error.
                Please try one of the options below.
              </Typography>

              {(this.props.showErrorDetails || process.env.NODE_ENV === 'development') && this.state.error && (
                <>
                  <Divider sx={{ my: 2 }} />
                  <Alert severity="error" sx={{ textAlign: 'left', mb: 2 }}>
                    <Typography variant="subtitle2" gutterBottom>
                      Error Details:
                    </Typography>
                    <Typography 
                      variant="body2" 
                      component="pre" 
                      sx={{ 
                        fontSize: '0.8rem', 
                        fontFamily: 'monospace',
                        whiteSpace: 'pre-wrap',
                        wordBreak: 'break-word'
                      }}
                    >
                      {this.state.error.toString()}
                    </Typography>
                    {this.state.errorInfo?.componentStack && (
                      <Typography 
                        variant="body2" 
                        component="pre" 
                        sx={{ 
                          fontSize: '0.7rem', 
                          mt: 1,
                          fontFamily: 'monospace',
                          whiteSpace: 'pre-wrap',
                          wordBreak: 'break-word'
                        }}
                      >
                        Component Stack:{this.state.errorInfo.componentStack}
                      </Typography>
                    )}
                  </Alert>
                </>
              )}
            </CardContent>

            <CardActions sx={{ justifyContent: 'center', pb: 3, gap: 1 }}>
              <Button
                variant="contained"
                startIcon={<RefreshIcon />}
                onClick={this.handleReset}
                color="primary"
              >
                Try Again
              </Button>
              <Button
                variant="outlined"
                startIcon={<RefreshIcon />}
                onClick={this.handleReload}
              >
                Reload Page
              </Button>
              <Button
                variant="outlined"
                startIcon={<HomeIcon />}
                onClick={this.handleGoHome}
              >
                Go Home
              </Button>
            </CardActions>
          </Card>
        </Container>
      );
    }

    return this.props.children;
  }
}