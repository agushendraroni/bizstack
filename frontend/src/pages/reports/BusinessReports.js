import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card, CardHeader, CardBody, Button, FormInput } from "shards-react";
import { dashboardApi, productApi, customerApi, orderApi } from "../../api";

const BusinessReports = () => {
  const [reportData, setReportData] = useState({
    salesSummary: { totalSales: 0, totalOrders: 0, avgOrderValue: 0 },
    topProducts: [],
    customerStats: { total: 0, vip: 0, corporate: 0 },
    lowStockItems: [],
    recentActivity: []
  });
  const [dateRange, setDateRange] = useState({
    startDate: new Date(Date.now() - 30 * 24 * 60 * 60 * 1000).toISOString().split('T')[0],
    endDate: new Date().toISOString().split('T')[0]
  });
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    loadReportData();
  }, [dateRange]);

  const loadReportData = async () => {
    try {
      setLoading(true);
      const [orders, products, customers] = await Promise.all([
        orderApi.getAllOrders(),
        productApi.getAllProducts(),
        customerApi.getAllCustomers()
      ]);

      // Calculate sales summary
      const orderData = orders.success ? orders.data : [];
      const totalSales = orderData.reduce((sum, order) => sum + (parseFloat(order.totalAmount || order.total || 0)), 0);
      const avgOrderValue = orderData.length > 0 ? totalSales / orderData.length : 0;

      // Top products (mock calculation)
      const productData = products.success ? products.data : [];
      const topProducts = productData.slice(0, 5).map(p => ({
        ...p,
        soldQuantity: Math.floor(Math.random() * 50) + 10,
        revenue: parseFloat(p.price || 0) * (Math.floor(Math.random() * 50) + 10)
      }));

      // Customer statistics
      const customerData = customers.success ? customers.data : [];
      const customerStats = {
        total: customerData.length,
        vip: customerData.filter(c => c.customerType === 'VIP').length,
        corporate: customerData.filter(c => c.customerType === 'Corporate').length
      };

      // Low stock items
      const lowStockItems = productData.filter(p => p.stock <= (p.minStock || 5));

      setReportData({
        salesSummary: {
          totalSales: totalSales,
          totalOrders: orderData.length,
          avgOrderValue: avgOrderValue
        },
        topProducts,
        customerStats,
        lowStockItems,
        recentActivity: orderData.slice(0, 10)
      });
    } catch (error) {
      console.error('Error loading report data:', error);
    } finally {
      setLoading(false);
    }
  };

  const exportReport = () => {
    const reportContent = `
BizStack Business Report
Generated: ${new Date().toLocaleString()}
Period: ${dateRange.startDate} to ${dateRange.endDate}

SALES SUMMARY
=============
Total Sales: $${reportData.salesSummary.totalSales.toFixed(2)}
Total Orders: ${reportData.salesSummary.totalOrders}
Average Order Value: $${reportData.salesSummary.avgOrderValue.toFixed(2)}

TOP PRODUCTS
============
${reportData.topProducts.map(p => `${p.name}: ${p.soldQuantity} sold, $${p.revenue.toFixed(2)} revenue`).join('\n')}

CUSTOMER STATISTICS
==================
Total Customers: ${reportData.customerStats.total}
VIP Customers: ${reportData.customerStats.vip}
Corporate Customers: ${reportData.customerStats.corporate}

LOW STOCK ALERTS
===============
${reportData.lowStockItems.map(p => `${p.name}: ${p.stock} remaining (min: ${p.minStock || 5})`).join('\n')}
    `;

    const blob = new Blob([reportContent], { type: 'text/plain' });
    const url = URL.createObjectURL(blob);
    const a = document.createElement('a');
    a.href = url;
    a.download = `bizstack-report-${new Date().toISOString().split('T')[0]}.txt`;
    a.click();
    URL.revokeObjectURL(url);
  };

  return (
    <Container fluid className="main-content-container px-4">
      <Row noGutters className="page-header py-4">
        <Col>
          <h3>Business Reports</h3>
        </Col>
        <Col className="text-right">
          <Button theme="success" onClick={exportReport} className="mr-2">
            <i className="fas fa-download mr-2"></i>Export Report
          </Button>
          <Button theme="primary" onClick={loadReportData}>
            <i className="fas fa-sync mr-2"></i>Refresh
          </Button>
        </Col>
      </Row>

      {/* Date Range Filter */}
      <Row className="mb-4">
        <Col md="3">
          <label>Start Date</label>
          <FormInput
            type="date"
            value={dateRange.startDate}
            onChange={(e) => setDateRange({...dateRange, startDate: e.target.value})}
          />
        </Col>
        <Col md="3">
          <label>End Date</label>
          <FormInput
            type="date"
            value={dateRange.endDate}
            onChange={(e) => setDateRange({...dateRange, endDate: e.target.value})}
          />
        </Col>
      </Row>

      {loading ? (
        <div className="text-center p-4">
          <i className="fas fa-spinner fa-spin fa-2x"></i>
          <div>Loading reports...</div>
        </div>
      ) : (
        <>
          {/* Sales Summary */}
          <Row className="mb-4">
            <Col md="4">
              <Card small>
                <CardBody className="text-center">
                  <h2 className="text-primary">${reportData.salesSummary.totalSales.toFixed(2)}</h2>
                  <p className="mb-0">Total Sales</p>
                </CardBody>
              </Card>
            </Col>
            <Col md="4">
              <Card small>
                <CardBody className="text-center">
                  <h2 className="text-info">{reportData.salesSummary.totalOrders}</h2>
                  <p className="mb-0">Total Orders</p>
                </CardBody>
              </Card>
            </Col>
            <Col md="4">
              <Card small>
                <CardBody className="text-center">
                  <h2 className="text-success">${reportData.salesSummary.avgOrderValue.toFixed(2)}</h2>
                  <p className="mb-0">Avg Order Value</p>
                </CardBody>
              </Card>
            </Col>
          </Row>

          <Row>
            {/* Top Products */}
            <Col lg="6" className="mb-4">
              <Card small>
                <CardHeader>
                  <h6 className="m-0">Top Products</h6>
                </CardHeader>
                <CardBody>
                  {reportData.topProducts.map(product => (
                    <div key={product.id} className="d-flex justify-content-between align-items-center py-2 border-bottom">
                      <div>
                        <strong>{product.name}</strong>
                        <div className="text-muted small">Sold: {product.soldQuantity}</div>
                      </div>
                      <div className="text-right">
                        <div className="font-weight-bold">${product.revenue.toFixed(2)}</div>
                        <div className="text-muted small">${product.price}</div>
                      </div>
                    </div>
                  ))}
                </CardBody>
              </Card>
            </Col>

            {/* Customer Stats */}
            <Col lg="6" className="mb-4">
              <Card small>
                <CardHeader>
                  <h6 className="m-0">Customer Statistics</h6>
                </CardHeader>
                <CardBody>
                  <div className="row text-center">
                    <div className="col-4">
                      <h4 className="text-primary">{reportData.customerStats.total}</h4>
                      <small>Total</small>
                    </div>
                    <div className="col-4">
                      <h4 className="text-warning">{reportData.customerStats.vip}</h4>
                      <small>VIP</small>
                    </div>
                    <div className="col-4">
                      <h4 className="text-info">{reportData.customerStats.corporate}</h4>
                      <small>Corporate</small>
                    </div>
                  </div>
                </CardBody>
              </Card>
            </Col>
          </Row>

          {/* Low Stock Alert */}
          {reportData.lowStockItems.length > 0 && (
            <Row>
              <Col lg="12" className="mb-4">
                <Card small>
                  <CardHeader className="bg-warning">
                    <h6 className="m-0 text-white">
                      <i className="fas fa-exclamation-triangle mr-2"></i>
                      Low Stock Items ({reportData.lowStockItems.length})
                    </h6>
                  </CardHeader>
                  <CardBody>
                    <div className="table-responsive">
                      <table className="table table-sm mb-0">
                        <thead>
                          <tr>
                            <th>Product</th>
                            <th>Current Stock</th>
                            <th>Min Stock</th>
                            <th>Status</th>
                          </tr>
                        </thead>
                        <tbody>
                          {reportData.lowStockItems.map(item => (
                            <tr key={item.id}>
                              <td>{item.name}</td>
                              <td className="text-danger">{item.stock}</td>
                              <td>{item.minStock || 5}</td>
                              <td>
                                <span className="badge badge-warning">Low Stock</span>
                              </td>
                            </tr>
                          ))}
                        </tbody>
                      </table>
                    </div>
                  </CardBody>
                </Card>
              </Col>
            </Row>
          )}
        </>
      )}
    </Container>
  );
};

export default BusinessReports;