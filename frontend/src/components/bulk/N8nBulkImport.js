import React, { useState } from "react";
import { Card, CardHeader, CardBody, Button, FormInput, Alert } from "shards-react";
import API_CONFIG from '../../config/apiConfig';

const N8nBulkImport = ({ type, onComplete }) => {
  const [file, setFile] = useState(null);
  const [importing, setImporting] = useState(false);
  const [results, setResults] = useState(null);
  const [error, setError] = useState("");

  const N8N_WEBHOOKS = {
    products: API_CONFIG.getN8nWebhook('bulk-import-products'),
    customers: API_CONFIG.getN8nWebhook('bulk-import-customers')
  };

  const handleFileChange = (e) => {
    const selectedFile = e.target.files[0];
    if (selectedFile && selectedFile.type === 'text/csv') {
      setFile(selectedFile);
      setError("");
    } else {
      setError("Please select a valid CSV file");
      setFile(null);
    }
  };

  const handleImport = async () => {
    if (!file) {
      setError("Please select a file");
      return;
    }

    setImporting(true);
    setError("");
    setResults(null);

    try {
      const csvData = await file.text();
      const token = localStorage.getItem('accessToken');
      const userId = localStorage.getItem('userId');

      const response = await fetch(N8N_WEBHOOKS[type], {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'Authorization': `Bearer ${token}`
        },
        body: JSON.stringify({
          csvData,
          userId,
          type
        })
      });

      if (!response.ok) {
        throw new Error(`HTTP error! status: ${response.status}`);
      }

      const result = await response.json();
      setResults(result);
      
      if (onComplete) {
        onComplete(result);
      }

    } catch (error) {
      setError("Import failed: " + error.message);
    } finally {
      setImporting(false);
    }
  };

  const downloadTemplate = () => {
    let csvContent;
    let filename;

    if (type === 'products') {
      csvContent = "name,code,description,price,stock,minStock,isActive\n";
      csvContent += "Sample Product,PROD001,High-quality product,99.99,50,10,true\n";
      csvContent += "Another Product,PROD002,Premium quality,149.99,25,5,true";
      filename = "product-import-template.csv";
    } else if (type === 'customers') {
      csvContent = "name,email,phone,address,customerType,isActive\n";
      csvContent += "John Doe,john@example.com,+1234567890,123 Main St,Regular,true\n";
      csvContent += "Jane Smith,jane@example.com,+1234567891,456 Oak Ave,VIP,true";
      filename = "customer-import-template.csv";
    }

    const blob = new Blob([csvContent], { type: 'text/csv' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = filename;
    a.click();
    URL.revokeObjectURL(url);
  };

  return (
    <Card small>
      <CardHeader>
        <h6 className="m-0">
          <i className="fas fa-robot mr-2"></i>
          n8n Bulk Import - {type === 'products' ? 'Products' : 'Customers'}
        </h6>
      </CardHeader>
      <CardBody>
        {error && <Alert theme="danger" className="mb-3">{error}</Alert>}
        
        {results && (
          <Alert theme={results.failed === 0 ? "success" : "warning"} className="mb-3">
            <div className="d-flex justify-content-between">
              <div>
                <strong>Import Results:</strong><br/>
                ✅ Success: {results.success}<br/>
                ❌ Failed: {results.failed}
              </div>
              <div>
                <i className="fas fa-robot fa-2x text-info"></i>
              </div>
            </div>
            
            {results.errors && results.errors.length > 0 && (
              <details className="mt-2">
                <summary className="cursor-pointer">View Errors ({results.errors.length})</summary>
                <ul className="mb-0 mt-2 small">
                  {results.errors.slice(0, 5).map((error, index) => (
                    <li key={index}>{error}</li>
                  ))}
                  {results.errors.length > 5 && (
                    <li>... and {results.errors.length - 5} more errors</li>
                  )}
                </ul>
              </details>
            )}

            {results.successItems && results.successItems.length > 0 && (
              <details className="mt-2">
                <summary className="cursor-pointer">View Success ({results.successItems.length})</summary>
                <ul className="mb-0 mt-2 small">
                  {results.successItems.slice(0, 5).map((item, index) => (
                    <li key={index}>{item.name} ({item.code})</li>
                  ))}
                  {results.successItems.length > 5 && (
                    <li>... and {results.successItems.length - 5} more items</li>
                  )}
                </ul>
              </details>
            )}
          </Alert>
        )}

        <div className="mb-3">
          <Button theme="info" size="sm" onClick={downloadTemplate} className="mb-2">
            <i className="fas fa-download mr-2"></i>
            Download CSV Template
          </Button>
          <div className="small text-muted">
            <i className="fas fa-info-circle mr-1"></i>
            Powered by n8n automation - handles validation, error recovery, and notifications
          </div>
        </div>

        <div className="mb-3">
          <label>Select CSV File</label>
          <FormInput
            type="file"
            accept=".csv"
            onChange={handleFileChange}
            disabled={importing}
          />
          <small className="text-muted">
            CSV headers: {type === 'products' 
              ? 'name, code, description, price, stock, minStock, isActive'
              : 'name, email, phone, address, customerType, isActive'
            }
          </small>
        </div>

        <div className="d-flex justify-content-between align-items-center">
          <Button 
            theme="primary" 
            onClick={handleImport}
            disabled={!file || importing}
          >
            {importing ? (
              <>
                <i className="fas fa-robot fa-spin mr-2"></i>
                Processing with n8n...
              </>
            ) : (
              <>
                <i className="fas fa-robot mr-2"></i>
                Import via n8n
              </>
            )}
          </Button>
          
          <div className="text-muted small">
            <i className="fas fa-shield-alt mr-1"></i>
            Auto error handling & notifications
          </div>
        </div>

        {importing && (
          <div className="mt-3 p-3 bg-light rounded">
            <div className="d-flex align-items-center">
              <i className="fas fa-robot fa-spin mr-2 text-primary"></i>
              <div>
                <strong>n8n is processing your import...</strong>
                <div className="small text-muted">
                  • Validating data format<br/>
                  • Creating records in batches<br/>
                  • Handling errors automatically<br/>
                  • Sending notifications on completion
                </div>
              </div>
            </div>
          </div>
        )}
      </CardBody>
    </Card>
  );
};

export default N8nBulkImport;