import React from "react";
import { Container, Row, Col, Card, CardHeader, CardBody } from "shards-react";

const UserManagement = () => {
  return (
    <Container fluid className="main-content-container px-4">
      <Row noGutters className="page-header py-4">
        <Col>
          <h3>User Management</h3>
        </Col>
      </Row>
      <Row>
        <Col>
          <Card>
            <CardHeader>
              <h6 className="m-0">Users</h6>
            </CardHeader>
            <CardBody>
              <p>User management functionality coming soon.</p>
            </CardBody>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default UserManagement;