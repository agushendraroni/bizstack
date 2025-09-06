import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card, CardHeader, CardBody } from "shards-react";
import PageTitle from "../../components/common/PageTitle";
import { useAuth } from "../../context/auth/AuthContext";
import userAPI from "../../api/user/userApi";
import organizationAPI from "../../api/organization/organizationApi";

const BusinessDashboard = () => {
  const { user } = useAuth();
  const [stats, setStats] = useState({
    totalUsers: 0,
    totalCompanies: 0,
    loading: true
  });

  useEffect(() => {
    loadDashboardData();
  }, []);

  const loadDashboardData = async () => {
    try {
      const [usersResult, companiesResult] = await Promise.all([
        userAPI.getUsers(),
        organizationAPI.getCompanies()
      ]);

      setStats({
        totalUsers: usersResult.success ? usersResult.data.length : 0,
        totalCompanies: companiesResult.success ? companiesResult.data.length : 0,
        loading: false
      });
    } catch (error) {
      console.error('Failed to load dashboard data:', error);
      setStats(prev => ({ ...prev, loading: false }));
    }
  };

  return (
    <Container fluid className="main-content-container px-4">
      {/* Page Header */}
      <Row noGutters className="page-header py-4">
        <PageTitle 
          title="Business Dashboard" 
          subtitle="Overview of your business operations" 
          className="text-sm-left mb-3" 
        />
      </Row>

      {/* Welcome Section */}
      <Row>
        <Col lg="12" className="mb-4">
          <Card small>
            <CardHeader className="border-bottom">
              <h6 className="m-0">Welcome back, {user?.firstName || 'User'}!</h6>
            </CardHeader>
            <CardBody>
              <p>Here's what's happening with your business today.</p>
            </CardBody>
          </Card>
        </Col>
      </Row>

      {/* Stats Cards */}
      <Row>
        <Col lg="3" md="6" sm="12" className="mb-4">
          <Card small className="stats-small">
            <CardBody className="p-0 d-flex">
              <div className="d-flex flex-column m-auto">
                <div className="stats-small__data text-center">
                  <span className="stats-small__label text-uppercase">Total Users</span>
                  <h6 className="stats-small__value count my-3">
                    {stats.loading ? "..." : stats.totalUsers}
                  </h6>
                </div>
              </div>
            </CardBody>
          </Card>
        </Col>

        <Col lg="3" md="6" sm="12" className="mb-4">
          <Card small className="stats-small">
            <CardBody className="p-0 d-flex">
              <div className="d-flex flex-column m-auto">
                <div className="stats-small__data text-center">
                  <span className="stats-small__label text-uppercase">Companies</span>
                  <h6 className="stats-small__value count my-3">
                    {stats.loading ? "..." : stats.totalCompanies}
                  </h6>
                </div>
              </div>
            </CardBody>
          </Card>
        </Col>

        <Col lg="3" md="6" sm="12" className="mb-4">
          <Card small className="stats-small">
            <CardBody className="p-0 d-flex">
              <div className="d-flex flex-column m-auto">
                <div className="stats-small__data text-center">
                  <span className="stats-small__label text-uppercase">Active Services</span>
                  <h6 className="stats-small__value count my-3">9</h6>
                </div>
              </div>
            </CardBody>
          </Card>
        </Col>

        <Col lg="3" md="6" sm="12" className="mb-4">
          <Card small className="stats-small">
            <CardBody className="p-0 d-flex">
              <div className="d-flex flex-column m-auto">
                <div className="stats-small__data text-center">
                  <span className="stats-small__label text-uppercase">System Status</span>
                  <h6 className="stats-small__value count my-3 text-success">Online</h6>
                </div>
              </div>
            </CardBody>
          </Card>
        </Col>
      </Row>

      {/* Quick Actions */}
      <Row>
        <Col lg="12" className="mb-4">
          <Card small>
            <CardHeader className="border-bottom">
              <h6 className="m-0">Quick Actions</h6>
            </CardHeader>
            <CardBody>
              <Row>
                <Col md="3" className="mb-2">
                  <a href="/users" className="btn btn-primary btn-sm btn-block">
                    Manage Users
                  </a>
                </Col>
                <Col md="3" className="mb-2">
                  <a href="/organizations" className="btn btn-secondary btn-sm btn-block">
                    Manage Organizations
                  </a>
                </Col>
                <Col md="3" className="mb-2">
                  <a href="/products" className="btn btn-success btn-sm btn-block">
                    Manage Products
                  </a>
                </Col>
                <Col md="3" className="mb-2">
                  <a href="/customers" className="btn btn-info btn-sm btn-block">
                    Manage Customers
                  </a>
                </Col>
              </Row>
            </CardBody>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default BusinessDashboard;
