import React, { createContext, useContext, useState, ReactNode } from 'react';
import { 
  Snackbar, 
  Alert, 
  AlertColor,
  Slide,
  SlideProps 
} from '@mui/material';

interface ToastMessage {
  id: string;
  message: string;
  type: AlertColor;
  duration?: number;
}

interface ToastContextType {
  showToast: (message: string, type?: AlertColor, duration?: number) => void;
  showError: (message: string, duration?: number) => void;
  showSuccess: (message: string, duration?: number) => void;
  showWarning: (message: string, duration?: number) => void;
  showInfo: (message: string, duration?: number) => void;
  hideToast: () => void;
}

const ToastContext = createContext<ToastContextType | undefined>(undefined);

function SlideTransition(props: SlideProps) {
  return <Slide {...props} direction="up" />;
}

interface ToastProviderProps {
  children: ReactNode;
}

export const ToastProvider: React.FC<ToastProviderProps> = ({ children }) => {
  const [toast, setToast] = useState<ToastMessage | null>(null);
  const [open, setOpen] = useState(false);

  const showToast = (
    message: string, 
    type: AlertColor = 'info', 
    duration: number = 6000
  ) => {
    const id = Date.now().toString();
    setToast({ id, message, type, duration });
    setOpen(true);
  };

  const showError = (message: string, duration: number = 8000) => {
    showToast(message, 'error', duration);
  };

  const showSuccess = (message: string, duration: number = 4000) => {
    showToast(message, 'success', duration);
  };

  const showWarning = (message: string, duration: number = 6000) => {
    showToast(message, 'warning', duration);
  };

  const showInfo = (message: string, duration: number = 6000) => {
    showToast(message, 'info', duration);
  };

  const hideToast = () => {
    setOpen(false);
  };

  const handleClose = (event?: React.SyntheticEvent | Event, reason?: string) => {
    if (reason === 'clickaway') {
      return;
    }
    hideToast();
  };

  const contextValue: ToastContextType = {
    showToast,
    showError,
    showSuccess,
    showWarning,
    showInfo,
    hideToast,
  };

  return (
    <ToastContext.Provider value={contextValue}>
      {children}
      <Snackbar
        open={open}
        autoHideDuration={toast?.duration}
        onClose={handleClose}
        anchorOrigin={{ vertical: 'bottom', horizontal: 'center' }}
        TransitionComponent={SlideTransition}
      >
        <Alert 
          onClose={handleClose} 
          severity={toast?.type} 
          sx={{ width: '100%', maxWidth: 400 }}
          variant="filled"
          elevation={6}
        >
          {toast?.message}
        </Alert>
      </Snackbar>
    </ToastContext.Provider>
  );
};

export const useToast = (): ToastContextType => {
  const context = useContext(ToastContext);
  if (context === undefined) {
    throw new Error('useToast must be used within a ToastProvider');
  }
  return context;
};