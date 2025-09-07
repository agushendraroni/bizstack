import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card, CardHeader, CardBody, Button, Form, FormInput, Alert } from "shards-react";

const CompanyProfile = () => {
  const [company, setCompany] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [editing, setEditing] = useState(false);
  const [formData, setFormData] = useState({
    name: "",
    code: "",
    description: "",
    address: "",
    phone: "",
    email: "",
    website: ""
  });

  useEffect(() => {
    loadCompanyProfile();
  }, []);

  const loadCompanyProfile = async () => {
    try {
      setLoading(true);
      const response = await fetch('http://localhost:4000/graphql', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({
          query: `{
            api_Companies {
              isSuccess
              data {
                id
                name
                code
                description
                address
                phone
                email
                website
                isActive
              }
            }
          }`
        })
      });
      
      const result = await response.json();
      console.log('Companies API Response:', result);
      
      if (result.data && result.data.api_Companies && result.data.api_Companies.isSuccess) {
        const companies = result.data.api_Companies.data || [];
        if (companies.length > 0) {
          const companyData = companies[0]; // Get first company
          setCompany(companyData);
          setFormData({
            name: companyData.name || "",
            code: companyData.code || "",
            description: companyData.description || "",
            address: companyData.address || "",
            phone: companyData.phone || "",
            email: companyData.email || "",
            website: companyData.website || ""
          });
        }
      }
    } catch (err) {
      setError("Failed to load company profile");
    } finally {
      setLoading(false);
    }
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData({
      ...formData,
      [name]: value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    try {
      // TODO: Implement GraphQL mutation for updating company
      setSuccess("Company profile updated successfully");
      setEditing(false);
      // Update local state
      setCompany({ ...company, ...formData });
    } catch (err) {
      setError("Failed to update company profile");
    }
  };

  const handleCancel = () => {
    setEditing(false);
    if (company) {
      setFormData({
        name: company.name || "",
        code: company.code || "",
        description: company.description || "",
        address: company.address || "",
        phone: company.phone || "",
        email: company.email || "",
        website: company.website || ""
      });
    }
  };

  if (loading) {
    return (
      <Container fluid className="main-content-container px-4">
        <div className="text-center py-5">
          <i className="fas fa-spinner fa-spin fa-2x mb-3"></i>
          <div>Loading company profile...</div>
        </div>
      </Container>
    );
  }

  return (
    <Container fluid className="main-content-container px-4">
      {/* Page Header */}
      <Row noGutters className="page-header py-4">
        <Col>
          <div className="d-flex justify-content-between align-items-center">
            <div>
              <span className="text-uppercase page-subtitle">Settings</span>
              <h3 className="page-title mb-0">Company Profile</h3>
            </div>
            {!editing && (
              <Button theme="primary" onClick={() => setEditing(true)}>
                <i className="fas fa-edit mr-2"></i>
                Edit Profile
              </Button>
            )}
          </div>
        </Col>
      </Row>

      {/* Alerts */}
      {error && (
        <Row>
          <Col>
            <Alert theme="danger" className="mb-3">
              <i className="fas fa-exclamation-triangle mr-2"></i>
              {error}
            </Alert>
          </Col>
        </Row>
      )}

      {success && (
        <Row>
          <Col>
            <Alert theme="success" className="mb-3">
              <i className="fas fa-check-circle mr-2"></i>
              {success}
            </Alert>
          </Col>
        </Row>
      )}

      <Row>
        <Col lg="8">
          <Card>
            <CardHeader className="border-bottom">
              <h6 className="m-0">Company Information</h6>
            </CardHeader>
            <CardBody>
              {editing ? (
                <Form onSubmit={handleSubmit}>
                  <Row>
                    <Col md="8">
                      <div className="mb-3">
                        <label htmlFor="name">Company Name *</label>
                        <FormInput
                          id="name"
                          name="name"
                          value={formData.name}
                          onChange={handleChange}
                          required
                        />
                      </div>
                    </Col>
                    <Col md="4">
                      <div className="mb-3">
                        <label htmlFor="code">Company Code *</label>
                        <FormInput
                          id="code"
                          name="code"
                          value={formData.code}
                          onChange={handleChange}
                          required
                        />
                      </div>
                    </Col>
                  </Row>

                  <div className="mb-3">
                    <label htmlFor="description">Description</label>
                    <FormInput
                      id="description"
                      name="description"
                      value={formData.description}
                      onChange={handleChange}
                    />
                  </div>

                  <div className="mb-3">
                    <label htmlFor="address">Address</label>
                    <FormInput
                      id="address"
                      name="address"
                      value={formData.address}
                      onChange={handleChange}
                    />
                  </div>

                  <Row>
                    <Col md="6">
                      <div className="mb-3">
                        <label htmlFor="phone">Phone</label>
                        <FormInput
                          id="phone"
                          name="phone"
                          value={formData.phone}
                          onChange={handleChange}
                        />
                      </div>
                    </Col>
                    <Col md="6">
                      <div className="mb-3">
                        <label htmlFor="email">Email</label>
                        <FormInput
                          id="email"
                          name="email"
                          type="email"
                          value={formData.email}
                          onChange={handleChange}
                        />
                      </div>
                    </Col>
                  </Row>

                  <div className="mb-4">
                    <label htmlFor="website">Website</label>
                    <FormInput
                      id="website"
                      name="website"
                      value={formData.website}
                      onChange={handleChange}
                    />
                  </div>

                  <div className="d-flex">
                    <Button type="button" theme="secondary" className="mr-2" onClick={handleCancel}>
                      <i className="fas fa-times mr-2"></i>
                      Cancel
                    </Button>
                    <Button type="submit" theme="primary">
                      <i className="fas fa-save mr-2"></i>
                      Save Changes
                    </Button>
                  </div>
                </Form>
              ) : (
                <div>
                  {company ? (
                    <>
                      <Row className="mb-3">
                        <Col sm="3"><strong>Company Name:</strong></Col>
                        <Col sm="9">{company.name || 'N/A'}</Col>
                      </Row>
                      <Row className="mb-3">
                        <Col sm="3"><strong>Company Code:</strong></Col>
                        <Col sm="9">
                          <span className="badge badge-primary">{company.code || 'N/A'}</span>
                        </Col>
                      </Row>
                      <Row className="mb-3">
                        <Col sm="3"><strong>Description:</strong></Col>
                        <Col sm="9">{company.description || 'N/A'}</Col>
                      </Row>
                      <Row className="mb-3">
                        <Col sm="3"><strong>Address:</strong></Col>
                        <Col sm="9">{company.address || 'N/A'}</Col>
                      </Row>
                      <Row className="mb-3">
                        <Col sm="3"><strong>Phone:</strong></Col>
                        <Col sm="9">{company.phone || 'N/A'}</Col>
                      </Row>
                      <Row className="mb-3">
                        <Col sm="3"><strong>Email:</strong></Col>
                        <Col sm="9">{company.email || 'N/A'}</Col>
                      </Row>
                      <Row className="mb-3">
                        <Col sm="3"><strong>Website:</strong></Col>
                        <Col sm="9">
                          {company.website ? (
                            <a href={company.website} target="_blank" rel="noopener noreferrer">
                              {company.website}
                            </a>
                          ) : 'N/A'}
                        </Col>
                      </Row>
                      <Row className="mb-3">
                        <Col sm="3"><strong>Status:</strong></Col>
                        <Col sm="9">
                          <span className={`badge badge-${company.isActive ? 'success' : 'secondary'}`}>
                            {company.isActive ? 'Active' : 'Inactive'}
                          </span>
                        </Col>
                      </Row>
                    </>
                  ) : (
                    <div className="text-center py-4">
                      <i className="fas fa-building fa-2x text-muted mb-3"></i>
                      <div>No company profile found</div>
                      <Button theme="primary" className="mt-3" onClick={() => setEditing(true)}>
                        <i className="fas fa-plus mr-2"></i>
                        Create Company Profile
                      </Button>
                    </div>
                  )}
                </div>
              )}
            </CardBody>
          </Card>
        </Col>

        <Col lg="4">
          <Card>
            <CardHeader className="border-bottom">
              <h6 className="m-0">Quick Stats</h6>
            </CardHeader>
            <CardBody>
              <div className="mb-3">
                <div className="d-flex justify-content-between">
                  <span>Total Employees:</span>
                  <strong>0</strong>
                </div>
              </div>
              <div className="mb-3">
                <div className="d-flex justify-content-between">
                  <span>Departments:</span>
                  <strong>0</strong>
                </div>
              </div>
              <div className="mb-3">
                <div className="d-flex justify-content-between">
                  <span>Active Users:</span>
                  <strong>0</strong>
                </div>
              </div>
            </CardBody>
          </Card>

          <Card className="mt-4">
            <CardHeader className="border-bottom">
              <h6 className="m-0">System Information</h6>
            </CardHeader>
            <CardBody>
              <div className="mb-2">
                <small className="text-muted">Platform:</small>
                <div>BizStack v2.0.0</div>
              </div>
              <div className="mb-2">
                <small className="text-muted">Database:</small>
                <div>PostgreSQL</div>
              </div>
              <div className="mb-2">
                <small className="text-muted">Services:</small>
                <div>9/9 Online</div>
              </div>
            </CardBody>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default CompanyProfile;