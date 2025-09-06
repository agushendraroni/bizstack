import React, { useState } from "react";
import { Container, Row, Col, Card, CardHeader, CardBody, Button, Form, FormInput, Alert } from "shards-react";

const CompanySetup = () => {
  // State management
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [step, setStep] = useState(1); // 1: Company, 2: Admin User, 3: Complete
  
  // Company form state
  const [companyData, setCompanyData] = useState({
    code: "",
    name: "",
    description: "",
    address: "",
    phone: "",
    email: "",
    website: ""
  });

  // Admin user form state
  const [adminData, setAdminData] = useState({
    firstName: "",
    lastName: "",
    username: "",
    email: "",
    password: "",
    confirmPassword: ""
  });

  // Form handlers
  const handleCompanyChange = (e) => {
    const { name, value } = e.target;
    setCompanyData({
      ...companyData,
      [name]: name === 'code' ? value.toUpperCase() : value
    });
  };

  const handleAdminChange = (e) => {
    const { name, value } = e.target;
    setAdminData({
      ...adminData,
      [name]: value
    });
  };

  // Step handlers
  const handleCompanySubmit = (e) => {
    e.preventDefault();
    setError("");
    
    // Validation
    if (!companyData.code || !companyData.name || !companyData.email) {
      setError("Please fill in all required fields");
      return;
    }

    if (companyData.code.length < 3) {
      setError("Company code must be at least 3 characters");
      return;
    }

    setStep(2);
  };

  const handleAdminSubmit = (e) => {
    e.preventDefault();
    setError("");
    
    // Validation
    if (!adminData.firstName || !adminData.lastName || !adminData.username || !adminData.email || !adminData.password) {
      setError("Please fill in all required fields");
      return;
    }

    if (adminData.password !== adminData.confirmPassword) {
      setError("Passwords do not match");
      return;
    }

    if (adminData.password.length < 6) {
      setError("Password must be at least 6 characters");
      return;
    }

    handleFinalSubmit();
  };

  const handleFinalSubmit = async () => {
    setLoading(true);
    setError("");

    try {
      // Mock API calls to create company and admin user
      await new Promise(resolve => setTimeout(resolve, 2000));
      
      setSuccess("Company and admin user created successfully!");
      setStep(3);
    } catch (err) {
      setError("An error occurred during setup");
    } finally {
      setLoading(false);
    }
  };

  const handleBackToCompany = () => {
    setStep(1);
    setError("");
  };

  const handleStartOver = () => {
    setStep(1);
    setCompanyData({
      code: "",
      name: "",
      description: "",
      address: "",
      phone: "",
      email: "",
      website: ""
    });
    setAdminData({
      firstName: "",
      lastName: "",
      username: "",
      email: "",
      password: "",
      confirmPassword: ""
    });
    setError("");
    setSuccess("");
  };

  // Render Company Setup Step
  const renderCompanyStep = () => (
    <Card small className="mb-4">
      <CardHeader className="border-bottom">
        <h6 className="m-0">
          <i className="fas fa-building mr-2"></i>
          Step 1: Company Information
        </h6>
      </CardHeader>
      <CardBody>
        <Form onSubmit={handleCompanySubmit}>
          <Row>
            <Col md="6">
              <div className="mb-3">
                <label htmlFor="code">Company Code * <small className="text-muted">(Multi-tenant ID)</small></label>
                <FormInput
                  id="code"
                  name="code"
                  value={companyData.code}
                  onChange={handleCompanyChange}
                  required
                  placeholder="e.g., PTABC"
                  style={{ textTransform: 'uppercase' }}
                />
                <small className="text-muted">Users will login via: {companyData.code || 'CODE'}/login</small>
              </div>
            </Col>
            <Col md="6">
              <div className="mb-3">
                <label htmlFor="name">Company Name *</label>
                <FormInput
                  id="name"
                  name="name"
                  value={companyData.name}
                  onChange={handleCompanyChange}
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
              value={companyData.description}
              onChange={handleCompanyChange}
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
              value={companyData.address}
              onChange={handleCompanyChange}
              placeholder="Company address"
            />
          </div>

          <Row>
            <Col md="4">
              <div className="mb-3">
                <label htmlFor="phone">Phone</label>
                <FormInput
                  id="phone"
                  name="phone"
                  value={companyData.phone}
                  onChange={handleCompanyChange}
                  placeholder="e.g., +62-21-12345678"
                />
              </div>
            </Col>
            <Col md="4">
              <div className="mb-3">
                <label htmlFor="email">Email *</label>
                <FormInput
                  id="email"
                  name="email"
                  type="email"
                  value={companyData.email}
                  onChange={handleCompanyChange}
                  required
                  placeholder="e.g., info@company.com"
                />
              </div>
            </Col>
            <Col md="4">
              <div className="mb-3">
                <label htmlFor="website">Website</label>
                <FormInput
                  id="website"
                  name="website"
                  value={companyData.website}
                  onChange={handleCompanyChange}
                  placeholder="e.g., https://www.company.com"
                />
              </div>
            </Col>
          </Row>

          <div className="d-flex justify-content-end">
            <Button type="submit" theme="primary">
              <i className="fas fa-arrow-right mr-2"></i>
              Next: Create Admin User
            </Button>
          </div>
        </Form>
      </CardBody>
    </Card>
  );

  // Render Admin User Setup Step
  const renderAdminStep = () => (
    <Card small className="mb-4">
      <CardHeader className="border-bottom">
        <h6 className="m-0">
          <i className="fas fa-user-shield mr-2"></i>
          Step 2: Admin User
        </h6>
      </CardHeader>
      <CardBody>
        <div className="alert alert-info mb-3">
          <i className="fas fa-info-circle mr-2"></i>
          Creating admin user for company: <strong>{companyData.name}</strong> ({companyData.code})
        </div>

        <Form onSubmit={handleAdminSubmit}>
          <Row>
            <Col md="6">
              <div className="mb-3">
                <label htmlFor="firstName">First Name *</label>
                <FormInput
                  id="firstName"
                  name="firstName"
                  value={adminData.firstName}
                  onChange={handleAdminChange}
                  required
                  placeholder="e.g., John"
                />
              </div>
            </Col>
            <Col md="6">
              <div className="mb-3">
                <label htmlFor="lastName">Last Name *</label>
                <FormInput
                  id="lastName"
                  name="lastName"
                  value={adminData.lastName}
                  onChange={handleAdminChange}
                  required
                  placeholder="e.g., Doe"
                />
              </div>
            </Col>
          </Row>

          <Row>
            <Col md="6">
              <div className="mb-3">
                <label htmlFor="username">Username *</label>
                <FormInput
                  id="username"
                  name="username"
                  value={adminData.username}
                  onChange={handleAdminChange}
                  required
                  placeholder="e.g., admin"
                />
              </div>
            </Col>
            <Col md="6">
              <div className="mb-3">
                <label htmlFor="email">Email *</label>
                <FormInput
                  id="email"
                  name="email"
                  type="email"
                  value={adminData.email}
                  onChange={handleAdminChange}
                  required
                  placeholder="e.g., admin@company.com"
                />
              </div>
            </Col>
          </Row>

          <Row>
            <Col md="6">
              <div className="mb-3">
                <label htmlFor="password">Password *</label>
                <FormInput
                  id="password"
                  name="password"
                  type="password"
                  value={adminData.password}
                  onChange={handleAdminChange}
                  required
                  placeholder="Minimum 6 characters"
                />
              </div>
            </Col>
            <Col md="6">
              <div className="mb-3">
                <label htmlFor="confirmPassword">Confirm Password *</label>
                <FormInput
                  id="confirmPassword"
                  name="confirmPassword"
                  type="password"
                  value={adminData.confirmPassword}
                  onChange={handleAdminChange}
                  required
                  placeholder="Repeat password"
                />
              </div>
            </Col>
          </Row>

          <div className="d-flex justify-content-between">
            <Button type="button" theme="secondary" onClick={handleBackToCompany}>
              <i className="fas fa-arrow-left mr-2"></i>
              Back to Company
            </Button>
            <Button type="submit" theme="primary" disabled={loading}>
              {loading ? (
                <>
                  <i className="fas fa-spinner fa-spin mr-2"></i>
                  Creating...
                </>
              ) : (
                <>
                  <i className="fas fa-check mr-2"></i>
                  Create Company & Admin
                </>
              )}
            </Button>
          </div>
        </Form>
      </CardBody>
    </Card>
  );

  // Render Complete Step
  const renderCompleteStep = () => (
    <Card small className="mb-4">
      <CardHeader className="border-bottom bg-success text-white">
        <h6 className="m-0">
          <i className="fas fa-check-circle mr-2"></i>
          Setup Complete!
        </h6>
      </CardHeader>
      <CardBody className="text-center">
        <div className="py-4">
          <i className="fas fa-check-circle fa-4x text-success mb-3"></i>
          <h4>Company Setup Successful!</h4>
          <p className="text-muted mb-4">Your multi-tenant company has been created successfully.</p>
          
          <div className="row">
            <div className="col-md-6">
              <div className="card bg-light">
                <div className="card-body">
                  <h6><i className="fas fa-building mr-2"></i>Company Details</h6>
                  <p><strong>Name:</strong> {companyData.name}</p>
                  <p><strong>Code:</strong> {companyData.code}</p>
                  <p><strong>Email:</strong> {companyData.email}</p>
                </div>
              </div>
            </div>
            <div className="col-md-6">
              <div className="card bg-light">
                <div className="card-body">
                  <h6><i className="fas fa-user-shield mr-2"></i>Admin User</h6>
                  <p><strong>Name:</strong> {adminData.firstName} {adminData.lastName}</p>
                  <p><strong>Username:</strong> {adminData.username}</p>
                  <p><strong>Email:</strong> {adminData.email}</p>
                </div>
              </div>
            </div>
          </div>

          <div className="alert alert-success mt-4">
            <h6><i className="fas fa-link mr-2"></i>Multi-Tenant Access</h6>
            <p className="mb-0">
              Users can now login via: <strong><code>{companyData.code}/login</code></strong>
            </p>
          </div>

          <div className="mt-4">
            <Button theme="primary" className="mr-2" onClick={() => window.location.href = `/${companyData.code}/login`}>
              <i className="fas fa-sign-in-alt mr-2"></i>
              Go to Company Login
            </Button>
            <Button theme="secondary" onClick={handleStartOver}>
              <i className="fas fa-plus mr-2"></i>
              Setup Another Company
            </Button>
          </div>
        </div>
      </CardBody>
    </Card>
  );

  return (
    <Container fluid className="main-content-container px-4">
      {/* Page Header */}
      <Row noGutters className="page-header py-4">
        <Col>
          <div className="text-center">
            <h2 className="page-title">
              <i className="fas fa-rocket mr-2"></i>
              Company Setup Wizard
            </h2>
            <p className="page-subtitle mb-0">Multi-Tenant Company Registration Backdoor</p>
          </div>
        </Col>
      </Row>

      {/* Progress Indicator */}
      <Row className="mb-4">
        <Col>
          <div className="d-flex justify-content-center">
            <div className="d-flex align-items-center">
              <div className={`rounded-circle d-flex align-items-center justify-content-center mr-3 ${step >= 1 ? 'bg-primary text-white' : 'bg-light'}`} style={{width: '40px', height: '40px'}}>
                {step > 1 ? <i className="fas fa-check"></i> : '1'}
              </div>
              <span className={`mr-3 ${step >= 1 ? 'text-primary font-weight-bold' : 'text-muted'}`}>Company</span>
              
              <div className={`border-top mr-3 ${step >= 2 ? 'border-primary' : 'border-light'}`} style={{width: '50px'}}></div>
              
              <div className={`rounded-circle d-flex align-items-center justify-content-center mr-3 ${step >= 2 ? 'bg-primary text-white' : 'bg-light'}`} style={{width: '40px', height: '40px'}}>
                {step > 2 ? <i className="fas fa-check"></i> : '2'}
              </div>
              <span className={`mr-3 ${step >= 2 ? 'text-primary font-weight-bold' : 'text-muted'}`}>Admin User</span>
              
              <div className={`border-top mr-3 ${step >= 3 ? 'border-primary' : 'border-light'}`} style={{width: '50px'}}></div>
              
              <div className={`rounded-circle d-flex align-items-center justify-content-center mr-3 ${step >= 3 ? 'bg-success text-white' : 'bg-light'}`} style={{width: '40px', height: '40px'}}>
                {step >= 3 ? <i className="fas fa-check"></i> : '3'}
              </div>
              <span className={step >= 3 ? 'text-success font-weight-bold' : 'text-muted'}>Complete</span>
            </div>
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

      {/* Form Steps */}
      <Row>
        <Col lg="8" className="mx-auto">
          {step === 1 && renderCompanyStep()}
          {step === 2 && renderAdminStep()}
          {step === 3 && renderCompleteStep()}
        </Col>
      </Row>
    </Container>
  );
};

export default CompanySetup;
