import React from "react";
import { Container, Row, Col, Card, CardHeader, CardBody } from "shards-react";
import PageTitle from "../../components/common/PageTitle";
import CompanySettings from "../../components/settings/CompanySettings";

const BusinessDashboard = () => {
  return (
    <Container fluid className="main-content-container px-4">
      <Row noGutters className="page-header py-4">
        <PageTitle 
          title="Business Dashboard" 
          subtitle="Overview" 
          className="text-sm-left mb-3" 
        />
      </Row>

      <Row>
        <Col lg="8" className="mb-4">
          <Card small>
            <CardHeader className="border-bottom">
              <h6 className="m-0">Welcome to BizStack!</h6>
            </CardHeader>
            <CardBody>
              <p>Your business management platform is ready.</p>
              <div className="mt-3">
                <h6>Quick Stats:</h6>
                <ul>
                  <li>Backend Services: 10/10 Online</li>
                  <li>GraphQL API: Ready</li>
                  <li>n8n Workflows: Available</li>
                  <li>Multi-tenant: Enabled</li>
                </ul>
              </div>
            </CardBody>
          </Card>
        </Col>
        
        <Col lg="4" className="mb-4">
          <CompanySettings />
        </Col>
      </Row>
    </Container>
  );
};

export default BusinessDashboard;