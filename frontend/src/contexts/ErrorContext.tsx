import React, { createContext, useContext, useState, ReactNode, useCallback } from 'react';
import { useToast } from './ToastContext';

export interface AppError {
  id: string;
  message: string;
  type: 'network' | 'validation' | 'authentication' | 'authorization' | 'server' | 'unknown';
  timestamp: Date;
  context?: string;
  originalError?: Error;
}

interface ErrorContextType {
  errors: AppError[];
  addError: (error: Partial<AppError>) => void;
  removeError: (id: string) => void;
  clearErrors: () => void;
  handleError: (error: Error | string, context?: string) => void;
  handleNetworkError: (error: Error, context?: string) => void;
  handleValidationError: (message: string, context?: string) => void;
  handleAuthError: (message: string, context?: string) => void;
}

const ErrorContext = createContext<ErrorContextType | undefined>(undefined);

interface ErrorProviderProps {
  children: ReactNode;
}

export const ErrorProvider: React.FC<ErrorProviderProps> = ({ children }) => {
  const [errors, setErrors] = useState<AppError[]>([]);
  const { showError, showWarning } = useToast();

  const addError = useCallback((errorData: Partial<AppError>) => {
    const error: AppError = {
      id: Date.now().toString() + Math.random().toString(36).substr(2, 9),
      message: errorData.message || 'An unexpected error occurred',
      type: errorData.type || 'unknown',
      timestamp: new Date(),
      context: errorData.context,
      originalError: errorData.originalError,
    };

    setErrors(prev => [error, ...prev.slice(0, 9)]); // Keep last 10 errors
    return error.id;
  }, []);

  const removeError = useCallback((id: string) => {
    setErrors(prev => prev.filter(error => error.id !== id));
  }, []);

  const clearErrors = useCallback(() => {
    setErrors([]);
  }, []);

  const getErrorMessage = (error: Error | string, type: AppError['type']): string => {
    if (typeof error === 'string') return error;

    switch (type) {
      case 'network':
        if (error.message.includes('fetch')) {
          return 'Unable to connect to the server. Please check your internet connection.';
        }
        if (error.message.includes('timeout')) {
          return 'Request timed out. Please try again.';
        }
        return 'Network error occurred. Please try again.';
      
      case 'authentication':
        return 'Please log in to continue.';
      
      case 'authorization':
        return 'You don\'t have permission to perform this action.';
      
      case 'validation':
        return error.message || 'Please check your input and try again.';
      
      case 'server':
        if (error.message.includes('500')) {
          return 'Server error. Please try again later.';
        }
        if (error.message.includes('404')) {
          return 'The requested resource was not found.';
        }
        return 'Server error occurred. Please try again later.';
      
      default:
        return error.message || 'An unexpected error occurred.';
    }
  };

  const handleError = useCallback((error: Error | string, context?: string) => {
    const message = typeof error === 'string' ? error : error.message;
    const errorType: AppError['type'] = 'unknown';
    
    // Try to determine error type from message
    let type: AppError['type'] = 'unknown';
    if (typeof error !== 'string') {
      if (error.message.includes('fetch') || error.message.includes('network')) {
        type = 'network';
      } else if (error.message.includes('401')) {
        type = 'authentication';
      } else if (error.message.includes('403')) {
        type = 'authorization';
      } else if (error.message.includes('400') || error.message.includes('validation')) {
        type = 'validation';
      } else if (error.message.includes('500') || error.message.includes('server')) {
        type = 'server';
      }
    }

    const friendlyMessage = getErrorMessage(error, type);
    
    addError({
      message: friendlyMessage,
      type,
      context,
      originalError: typeof error === 'string' ? undefined : error,
    });

    // Show toast notification
    if (type === 'validation' || type === 'authorization') {
      showWarning(friendlyMessage);
    } else {
      showError(friendlyMessage);
    }
  }, [addError, showError, showWarning]);

  const handleNetworkError = useCallback((error: Error, context?: string) => {
    const message = getErrorMessage(error, 'network');
    
    addError({
      message,
      type: 'network',
      context,
      originalError: error,
    });

    showError(message);
  }, [addError, showError]);

  const handleValidationError = useCallback((message: string, context?: string) => {
    addError({
      message,
      type: 'validation',
      context,
    });

    showWarning(message);
  }, [addError, showWarning]);

  const handleAuthError = useCallback((message: string, context?: string) => {
    const friendlyMessage = message || 'Authentication required. Please log in.';
    
    addError({
      message: friendlyMessage,
      type: 'authentication',
      context,
    });

    showWarning(friendlyMessage);
  }, [addError, showWarning]);

  const contextValue: ErrorContextType = {
    errors,
    addError,
    removeError,
    clearErrors,
    handleError,
    handleNetworkError,
    handleValidationError,
    handleAuthError,
  };

  return (
    <ErrorContext.Provider value={contextValue}>
      {children}
    </ErrorContext.Provider>
  );
};

export const useError = (): ErrorContextType => {
  const context = useContext(ErrorContext);
  if (context === undefined) {
    throw new Error('useError must be used within an ErrorProvider');
  }
  return context;
};