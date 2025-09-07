import React from 'react';

const LoadingSpinner = ({ size = 'md', text = 'Loading...' }) => {
  const sizeClass = {
    sm: 'spinner-border-sm',
    md: '',
    lg: 'spinner-border-lg'
  }[size];

  return (
    <div className="d-flex justify-content-center align-items-center p-4">
      <div className={`spinner-border text-primary ${sizeClass}`} role="status">
        <span className="sr-only">{text}</span>
      </div>
      {text && <span className="ml-2">{text}</span>}
    </div>
  );
};

export default LoadingSpinner;