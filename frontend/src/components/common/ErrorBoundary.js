import React from 'react';
import { Alert, Button } from 'shards-react';

class ErrorBoundary extends React.Component {
  constructor(props) {
    super(props);
    this.state = { hasError: false, error: null };
  }

  static getDerivedStateFromError(error) {
    return { hasError: true, error };
  }

  componentDidCatch(error, errorInfo) {
    console.error('Error caught by boundary:', error, errorInfo);
  }

  render() {
    if (this.state.hasError) {
      return (
        <div className="error-boundary p-4">
          <Alert theme="danger">
            <h4>Something went wrong</h4>
            <p>An unexpected error occurred. Please try refreshing the page.</p>
            <Button 
              theme="danger" 
              outline 
              onClick={() => this.setState({ hasError: false, error: null })}
            >
              Try Again
            </Button>
          </Alert>
        </div>
      );
    }
    return this.props.children;
  }
}

export default ErrorBoundary;