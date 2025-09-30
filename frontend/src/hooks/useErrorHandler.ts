import { useCallback } from 'react';
import { useError } from '../contexts/ErrorContext';
import { useToast } from '../contexts/ToastContext';

export const useErrorHandler = () => {
  const { handleError, handleNetworkError, handleValidationError, handleAuthError } = useError();
  const { showError, showSuccess, showWarning, showInfo } = useToast();

  // Generic async operation wrapper
  const withErrorHandling = useCallback(
    async <T>(
      operation: () => Promise<T>,
      options?: {
        context?: string;
        successMessage?: string;
        showSuccessToast?: boolean;
        onSuccess?: (result: T) => void;
        onError?: (error: Error) => void;
      }
    ): Promise<T | null> => {
      try {
        const result = await operation();
        
        if (options?.successMessage && options?.showSuccessToast !== false) {
          showSuccess(options.successMessage);
        }
        
        if (options?.onSuccess) {
          options.onSuccess(result);
        }
        
        return result;
      } catch (error) {
        const errorInstance = error instanceof Error ? error : new Error(String(error));
        
        // Handle different types of errors
        if (errorInstance.message.includes('network') || errorInstance.message.includes('fetch')) {
          handleNetworkError(errorInstance, options?.context);
        } else if (errorInstance.message.includes('401')) {
          handleAuthError('Please log in to continue', options?.context);
        } else if (errorInstance.message.includes('403')) {
          handleError(errorInstance, options?.context);
        } else if (errorInstance.message.includes('400') || errorInstance.message.includes('validation')) {
          handleValidationError(errorInstance.message, options?.context);
        } else {
          handleError(errorInstance, options?.context);
        }
        
        if (options?.onError) {
          options.onError(errorInstance);
        }
        
        return null;
      }
    },
    [handleError, handleNetworkError, handleValidationError, handleAuthError, showSuccess]
  );

  // Specific helpers for common operations
  const handleAsyncOperation = useCallback(
    <T>(
      operation: () => Promise<T>,
      successMessage?: string,
      context?: string
    ) => {
      return withErrorHandling(operation, {
        successMessage,
        context,
        showSuccessToast: !!successMessage,
      });
    },
    [withErrorHandling]
  );

  const handleFormSubmit = useCallback(
    <T>(
      operation: () => Promise<T>,
      successMessage: string = 'Operation completed successfully',
      context?: string
    ) => {
      return withErrorHandling(operation, {
        successMessage,
        context,
        showSuccessToast: true,
      });
    },
    [withErrorHandling]
  );

  const handleApiCall = useCallback(
    <T>(
      operation: () => Promise<T>,
      context?: string
    ) => {
      return withErrorHandling(operation, {
        context,
        showSuccessToast: false,
      });
    },
    [withErrorHandling]
  );

  // Direct toast helpers
  const toast = {
    success: showSuccess,
    error: showError,
    warning: showWarning,
    info: showInfo,
  };

  return {
    withErrorHandling,
    handleAsyncOperation,
    handleFormSubmit,
    handleApiCall,
    toast,
    // Direct error handlers
    handleError,
    handleNetworkError,
    handleValidationError,
    handleAuthError,
  };
};

export default useErrorHandler;