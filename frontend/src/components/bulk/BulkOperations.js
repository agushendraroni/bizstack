import React, { useState } from "react";
import { Card, CardHeader, CardBody, Button, FormInput, Alert } from "shards-react";
import { productApi, customerApi } from "../../api";

const BulkOperations = ({ type, onComplete }) => {
  const [file, setFile] = useState(null);
  const [importing, setImporting] = useState(false);
  const [results, setResults] = useState(null);
  const [error, setError] = useState("");

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

  const parseCSV = (csvText) => {
    const lines = csvText.split('\n').filter(line => line.trim());
    const headers = lines[0].split(',').map(h => h.trim());
    
    return lines.slice(1).map(line => {
      const values = line.split(',').map(v => v.trim());
      const obj = {};
      headers.forEach((header, index) => {
        obj[header] = values[index] || '';
      });
      return obj;
    });
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
      const csvText = await file.text();
      const rawData = parseCSV(csvText);
      
      const results = { success: 0, failed: 0, errors: [] };
      
      for (let i = 0; i < rawData.length; i++) {
        const item = rawData[i];

        try {
          let result;
          if (type === 'products') {
            const productData = {
              name: item.name,
              code: item.code,
              description: item.description || '',
              price: parseFloat(item.price) || 0,
              stock: parseInt(item.stock) || 0,
              minStock: parseInt(item.minStock) || 5,
              isActive: item.isActive !== 'false'
            };
            result = await productApi.createProduct(productData);
          } else if (type === 'customers') {
            const customerData = {
              name: item.name,
              email: item.email,
              phone: item.phone || '',
              address: item.address || '',
              customerType: item.customerType || 'Regular',
              isActive: item.isActive !== 'false'
            };
            result = await customerApi.createCustomer(customerData);
          }

          if (result.success) {
            results.success++;
          } else {
            results.failed++;
            results.errors.push(`Row ${i + 2}: ${result.message}`);
          }
        } catch (error) {
          results.failed++;
          results.errors.push(`Row ${i + 2}: ${error.message}`);
        }
      }

      setResults(results);
      if (onComplete) onComplete(results);
      
    } catch (error) {
      setError("Error processing file: " + error.message);
    } finally {
      setImporting(false);
    }
  };

  const downloadTemplate = () => {
    let csvContent;
    let filename;

    if (type === 'products') {
      csvContent = "name,code,description,price,stock,minStock,isActive\n";
      csvContent += "Sample Product,PROD001,Sample description,99.99,50,10,true";
      filename = "product-template.csv";
    } else if (type === 'customers') {
      csvContent = "name,email,phone,address,customerType,isActive\n";
      csvContent += "John Doe,john@example.com,+1234567890,123 Main St,Regular,true";
      filename = "customer-template.csv";
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
        <h6 className="m-0">Bulk Import {type === 'products' ? 'Products' : 'Customers'}</h6>
      </CardHeader>
      <CardBody>
        {error && <Alert theme="danger">{error}</Alert>}
        
        {results && (
          <Alert theme={results.failed === 0 ? "success" : "warning"}>
            Success: {results.success}, Failed: {results.failed}
          </Alert>
        )}

        <Button theme="info" size="sm" onClick={downloadTemplate} className="mb-3">
          Download Template
        </Button>

        <FormInput
          type="file"
          accept=".csv"
          onChange={handleFileChange}
          disabled={importing}
          className="mb-3"
        />

        <Button 
          theme="primary" 
          onClick={handleImport}
          disabled={!file || importing}
        >
          {importing ? "Importing..." : "Import Data"}
        </Button>
      </CardBody>
    </Card>
  );
};

export default BulkOperations;