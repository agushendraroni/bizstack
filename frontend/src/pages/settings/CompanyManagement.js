import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card, CardHeader, CardBody, Button, Badge, Form, FormInput, Alert } from "shards-react";
import PageTitle from "../../components/common/PageTitle";

const CompanyManagement = () => {
  // State management
  const [company, setCompany] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  
  // View states
  const [currentView, setCurrentView] = useState('view'); // 'view', 'edit'
  const [formLoading, setFormLoading] = useState(false);
  
  // Form state
  const [formData, setFormData] = useState({
    code: "",
    name: "",
    description: "",
    address: "",
    phone: "",
    email: "",
    website: "",
    npwp: "",
    nib: "",
    siup: ""
  });

  // Get current user's company
  useEffect(() => {
    loadCurrentCompany();
  }, []);

  const loadCurrentCompany = async () => {
    setLoading(true);
    
    try {
      // Default company BLITZ data
      const defaultCompany = {
        id: "1",
        code: "BLITZ",
        name: "PT Blitz Technology Indonesia",
        description: "Leading business technology solutions provider in Indonesia. Specializing in enterprise software development, digital transformation, and business process automation.",
        address: "Jl. HR Rasuna Said Kav. C-5, Kuningan, Jakarta Selatan 12940, Indonesia",
        phone: "+62-21-5794-8000",
        email: "info@blitz.co.id",
        website: "https://www.blitz.co.id",
        npwp: "01.234.567.8-901.000",
        nib: "1234567890123456",
        siup: "SIUP/BLITZ/2023/001",
        isActive: true,
        createdAt: "2023-01-01T00:00:00Z",
        adminUser: {
          firstName: "System",
          lastName: "Administrator",
          username: "admin",
          email: "admin@blitz.co.id",
          position: "System Administrator",
          department: "IT"
        }
      };
      
      setCompany(defaultCompany);
      setFormData({
        code: defaultCompany.code || "",
        name: defaultCompany.name || "",
        description: defaultCompany.description || "",
        address: defaultCompany.address || "",
        phone: defaultCompany.phone || "",
        email: defaultCompany.email || "",
        website: defaultCompany.website || "",
        npwp: defaultCompany.npwp || "",
        nib: defaultCompany.nib || "",
        siup: defaultCompany.siup || ""
      });
      setError("");
    } catch (err) {
      setError("Failed to load company information");
    } finally {
      setLoading(false);
    }
  };

  // View handlers
  const handleEdit = () => {
    setCurrentView('edit');
  };

  const handleCancel = () => {
    setCurrentView('view');
    // Reset form data
    if (company) {
      setFormData({
        code: company.code || "",
        name: company.name || "",
        description: company.description || "",
        address: company.address || "",
        phone: company.phone || "",
        email: company.email || "",
        website: company.website || "",
        npwp: company.npwp || "",
        nib: company.nib || "",
        siup: company.siup || ""
      });
    }
    setError("");
    setSuccess("");
  };

  // Form handlers
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
    setFormLoading(true);

    try {
      // Mock API call to update company
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      // Update local company data
      const updatedCompany = { ...company, ...formData };
      setCompany(updatedCompany);
      
      setSuccess("Company information updated successfully");
      setTimeout(() => {
        setCurrentView('view');
        setSuccess("");
      }, 2000);
    } catch (err) {
      setError("An error occurred while updating company information");
    } finally {
      setFormLoading(false);
    }
  };

  // Clear messages after 5 seconds
  useEffect(() => {
    if (error || success) {
      const timer = setTimeout(() => {
        setError("");
        setSuccess("");
      }, 5000);
      return () => clearTimeout(timer);
    }
  }, [error, success]);

  // Render View Mode
  const renderViewMode = () => (
    <>
      {/* Page Header */}
      <Row noGutters className="page-header py-4">
        <Col>
          <div className="d-flex justify-content-between align-items-center">
            <div>
              <span className="text-uppercase page-subtitle">System Settings</span>
              <h3 className="page-title mb-0">Company Information</h3>
              <p className="page-subtitle mb-0">View and manage your company details</p>
            </div>
            <Button theme="primary" onClick={handleEdit}>
              <i className="fas fa-edit mr-2"></i>
              Edit Company
            </Button>
          </div>
        </Col>
      </Row>

      {/* Company Information */}
      <Row>
        <Col lg="8">
          <Card small className="mb-4">
            <CardHeader className="border-bottom">
              <h6 className="m-0">Company Details</h6>
            </CardHeader>
            <CardBody>
              {loading ? (
                <div className="text-center p-4">
                  <i className="fas fa-spinner fa-spin mr-2"></i>
                  Loading company information...
                </div>
              ) : company ? (
                <Row>
                  <Col md="6">
                    <div className="mb-3">
                      <label className="font-weight-bold">Company Code</label>
                      <div className="p-2 bg-light rounded">
                        <Badge theme="primary" className="mr-2">{company.code}</Badge>
                        <small className="text-muted">Multi-tenant identifier</small>
                      </div>
                    </div>
                    <div className="mb-3">
                      <label className="font-weight-bold">Company Name</label>
                      <div className="p-2 bg-light rounded">{company.name}</div>
                    </div>
                    <div className="mb-3">
                      <label className="font-weight-bold">Email</label>
                      <div className="p-2 bg-light rounded">{company.email}</div>
                    </div>
                    <div className="mb-3">
                      <label className="font-weight-bold">Phone</label>
                      <div className="p-2 bg-light rounded">{company.phone}</div>
                    </div>
                    <div className="mb-3">
                      <label className="font-weight-bold">Website</label>
                      <div className="p-2 bg-light rounded">
                        {company.website ? (
                          <a href={company.website} target="_blank" rel="noopener noreferrer">
                            {company.website}
                          </a>
                        ) : (
                          <span className="text-muted">Not specified</span>
                        )}
                      </div>
                    </div>
                  </Col>
                  <Col md="6">
                    <div className="mb-3">
                      <label className="font-weight-bold">Description</label>
                      <div className="p-2 bg-light rounded">{company.description || "No description"}</div>
                    </div>
                    <div className="mb-3">
                      <label className="font-weight-bold">Address</label>
                      <div className="p-2 bg-light rounded">{company.address || "No address"}</div>
                    </div>
                    <div className="mb-3">
                      <label className="font-weight-bold">NPWP</label>
                      <div className="p-2 bg-light rounded">{company.npwp || "Not specified"}</div>
                    </div>
                    <div className="mb-3">
                      <label className="font-weight-bold">NIB</label>
                      <div className="p-2 bg-light rounded">{company.nib || "Not specified"}</div>
                    </div>
                    <div className="mb-3">
                      <label className="font-weight-bold">SIUP</label>
                      <div className="p-2 bg-light rounded">{company.siup || "Not specified"}</div>
                    </div>
                  </Col>
                </Row>
              ) : (
                <div className="text-center p-4">
                  <i className="fas fa-building fa-2x text-muted mb-2"></i>
                  <div>No company information found</div>
                </div>
              )}
            </CardBody>
          </Card>
        </Col>
        
        <Col lg="4">
          <Card small className="mb-4">
            <CardHeader className="border-bottom">
              <h6 className="m-0">Multi-Tenant Info</h6>
            </CardHeader>
            <CardBody>
              <div className="mb-3">
                <label className="font-weight-bold">Tenant Access URL</label>
                <div className="p-2 bg-primary text-white rounded">
                  <code>{company && company.code ? company.code : 'CODE'}/login</code>
                </div>
                <small className="text-muted">Users login via company code</small>
              </div>
              <div className="mb-3">
                <label className="font-weight-bold">Company Status</label>
                <div>
                  <Badge theme="success">Active Tenant</Badge>
                </div>
              </div>
              <div className="mb-3">
                <label className="font-weight-bold">Created Date</label>
                <div className="text-muted">
                  {company && company.createdAt ? new Date(company.createdAt).toLocaleDateString() : "Unknown"}
                </div>
              </div>
            </CardBody>
          </Card>

          {/* Admin User Info */}
          {company && company.adminUser && (
            <Card small className="mb-4">
              <CardHeader className="border-bottom">
                <h6 className="m-0">Default Admin User</h6>
              </CardHeader>
              <CardBody>
                <div className="mb-2">
                  <label className="font-weight-bold">Name</label>
                  <div>{company.adminUser.firstName} {company.adminUser.lastName}</div>
                </div>
                <div className="mb-2">
                  <label className="font-weight-bold">Username</label>
                  <div><code>{company.adminUser.username}</code></div>
                </div>
                <div className="mb-2">
                  <label className="font-weight-bold">Email</label>
                  <div>{company.adminUser.email}</div>
                </div>
                <div className="mb-2">
                  <label className="font-weight-bold">Position</label>
                  <div>{company.adminUser.position}</div>
                </div>
                <div className="alert alert-info mt-3">
                  <small>
                    <i className="fas fa-info-circle mr-1"></i>
                    Default admin user for {company.code} tenant
                  </small>
                </div>
              </CardBody>
            </Card>
          )}
        </Col>
      </Row>
    </>
  );

  // Render Edit Mode
  const renderEditMode = () => (
    <>
      {/* Page Header */}
      <Row noGutters className="page-header py-4">
        <Col>
          <div className="d-flex align-items-center">
            <Button 
              theme="light" 
              size="sm" 
              className="mr-3"
              onClick={handleCancel}
            >
              <i className="fas fa-arrow-left mr-2"></i>
              Back to View
            </Button>
            <div>
              <span className="text-uppercase page-subtitle">System Settings</span>
              <h3 className="page-title mb-0">
                <i className="fas fa-edit mr-2"></i>
                Edit Company Information
              </h3>
            </div>
          </div>
        </Col>
      </Row>

      {/* Edit Form */}
      <Row>
        <Col lg="8">
          <Card small className="mb-4">
            <CardHeader className="border-bottom">
              <h6 className="m-0">Company Details</h6>
            </CardHeader>
            <CardBody>
              <Form onSubmit={handleSubmit}>
                <Row>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="code">Company Code *</label>
                      <FormInput
                        id="code"
                        name="code"
                        value={formData.code}
                        onChange={handleChange}
                        required
                        disabled
                        placeholder="e.g., PTABC"
                      />
                      <small className="text-muted">Company code cannot be changed</small>
                    </div>
                  </Col>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="name">Company Name *</label>
                      <FormInput
                        id="name"
                        name="name"
                        value={formData.name}
                        onChange={handleChange}
                        required
                        placeholder="e.g., PT ABC Technology"
                      />
                    </div>
                  </Col>
                </Row>

                <div className="mb-3">
                  <label htmlFor="description">Description</label>
                  <textarea
                    className="form-control"
                    id="description"
                    name="description"
                    rows="3"
                    value={formData.description}
                    onChange={handleChange}
                    placeholder="Brief description of your company"
                  />
                </div>

                <div className="mb-3">
                  <label htmlFor="address">Address</label>
                  <textarea
                    className="form-control"
                    id="address"
                    name="address"
                    rows="2"
                    value={formData.address}
                    onChange={handleChange}
                    placeholder="Company address"
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
                        placeholder="e.g., +62-21-12345678"
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
                        placeholder="e.g., info@company.com"
                      />
                    </div>
                  </Col>
                </Row>

                <div className="mb-3">
                  <label htmlFor="website">Website</label>
                  <FormInput
                    id="website"
                    name="website"
                    value={formData.website}
                    onChange={handleChange}
                    placeholder="e.g., https://www.company.com"
                  />
                </div>

                <Row>
                  <Col md="4">
                    <div className="mb-3">
                      <label htmlFor="npwp">NPWP</label>
                      <FormInput
                        id="npwp"
                        name="npwp"
                        value={formData.npwp}
                        onChange={handleChange}
                        placeholder="e.g., 01.234.567.8-901.000"
                      />
                    </div>
                  </Col>
                  <Col md="4">
                    <div className="mb-3">
                      <label htmlFor="nib">NIB</label>
                      <FormInput
                        id="nib"
                        name="nib"
                        value={formData.nib}
                        onChange={handleChange}
                        placeholder="e.g., 1234567890123"
                      />
                    </div>
                  </Col>
                  <Col md="4">
                    <div className="mb-3">
                      <label htmlFor="siup">SIUP</label>
                      <FormInput
                        id="siup"
                        name="siup"
                        value={formData.siup}
                        onChange={handleChange}
                        placeholder="e.g., SIUP/123/2023"
                      />
                    </div>
                  </Col>
                </Row>

                <div className="d-flex justify-content-end">
                  <Button 
                    type="button" 
                    theme="secondary" 
                    className="mr-2"
                    onClick={handleCancel}
                    disabled={formLoading}
                  >
                    <i className="fas fa-times mr-2"></i>
                    Cancel
                  </Button>
                  <Button 
                    type="submit" 
                    theme="primary"
                    disabled={formLoading}
                  >
                    {formLoading ? (
                      <>
                        <i className="fas fa-spinner fa-spin mr-2"></i>
                        Updating...
                      </>
                    ) : (
                      <>
                        <i className="fas fa-save mr-2"></i>
                        Update Company
                      </>
                    )}
                  </Button>
                </div>
              </Form>
            </CardBody>
          </Card>
        </Col>
        
        <Col lg="4">
          <Card small className="mb-4">
            <CardHeader className="border-bottom">
              <h6 className="m-0">Important Notes</h6>
            </CardHeader>
            <CardBody>
              <div className="alert alert-warning">
                <i className="fas fa-exclamation-triangle mr-2"></i>
                <strong>Company Code</strong> cannot be changed as it's used for multi-tenant routing.
              </div>
              <div className="alert alert-info">
                <i className="fas fa-info-circle mr-2"></i>
                Users access your system via <code>{formData.code}/login</code>
              </div>
            </CardBody>
          </Card>
        </Col>
      </Row>
    </>
  );

  return (
    <Container fluid className="main-content-container px-4">
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

      {/* Render based on current view */}
      {currentView === 'view' ? renderViewMode() : renderEditMode()}
    </Container>
  );
};

export default CompanyManagement;
