import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card, CardHeader, CardBody, Form, FormInput, Button, FormCheckbox } from "shards-react";
import CompanyStorage from "../../utils/companyStorage";

// Import required styles
import "bootstrap/dist/css/bootstrap.min.css";
import "../../shards-dashboard/styles/shards-dashboards.1.1.0.min.css";
import "./CompanySelector.css";

const CompanySelector = () => {
  const [companyCode, setCompanyCode] = useState("");
  const [rememberCompany, setRememberCompany] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    // Check if company code is remembered and auto-redirect
    const rememberedCompany = CompanyStorage.getRememberedCompany();
    if (rememberedCompany) {
      window.location.href = `/${rememberedCompany}/login`;
      return;
    }
    
    // Set initial state
    setRememberCompany(CompanyStorage.isRememberEnabled());
    setLoading(false);
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    
    if (!companyCode.trim()) return;

    const code = companyCode.trim().toLowerCase();
    setLoading(true);
    
    // Simple validation using static list
    const validCompanies = ['demo', 'test', 'bizstack', 'company1', 'company2', 'blitz'];
    
    if (!validCompanies.includes(code)) {
      setError(`Company code "${code}" not found. Valid codes: ${validCompanies.join(', ')}`);
      setLoading(false);
      return;
    }

    // Save company code if remember is checked
    if (rememberCompany) {
      CompanyStorage.setRememberedCompany(code);
    } else {
      CompanyStorage.forgetCompany();
    }

    // Redirect to company login
    window.location.href = `/${code}/login`;
  };

  const handleForgetCompany = () => {
    CompanyStorage.forgetCompany();
    setCompanyCode('');
    setRememberCompany(false);
    setError('');
  };

  if (loading) {
    return (
      <div className="min-vh-100 d-flex align-items-center justify-content-center bg-light">
        <div className="text-center">
          <div className="spinner-border text-primary" role="status">
            <span className="sr-only">Loading...</span>
          </div>
          <p className="mt-2 text-muted">Checking saved settings...</p>
        </div>
      </div>
    );
  }

  return (
    <div className="auth-wrapper">
      <Container>
        <Row className="justify-content-center">
          <Col lg="5" md="7">
            <Card className="auth-card">
              <CardHeader className="text-center">
                <img
                  src="/images/bizstack-logo.png"
                  alt="BizStack"
                  className="auth-logo"
                  onError={(e) => e.target.style.display = 'none'}
                />
                <h1 className="company-title">BizStack</h1>
                <p className="company-subtitle">Enter your company code to continue</p>
              </CardHeader>
              
              <CardBody>
                <Form onSubmit={handleSubmit}>
                  <div className="mb-4">
                    <FormInput
                      placeholder="Company Code"
                      value={companyCode}
                      onChange={(e) => setCompanyCode(e.target.value)}
                      required
                      autoFocus
                    />
                  </div>

                  <div className="remember-checkbox">
                    <FormCheckbox
                      checked={rememberCompany}
                      onChange={(e) => setRememberCompany(e.target.checked)}
                    >
                      Remember this company code
                    </FormCheckbox>
                  </div>

                  <Button 
                    type="submit" 
                    theme="primary" 
                    block
                    disabled={!companyCode.trim() || loading}
                  >
                    {loading ? (
                      <>
                        <span className="spinner-border spinner-border-sm mr-2" role="status" aria-hidden="true"></span>
                        Validating...
                      </>
                    ) : (
                      'Continue'
                    )}
                  </Button>
                </Form>

                {error && (
                  <div className="alert alert-danger mt-3" role="alert">
                    {error}
                  </div>
                )}

                <div className="valid-companies">
                  <div className="text-muted text-center">
                    <small>Valid company codes:</small>
                    <div className="mt-2">
                      <code className="mr-2">demo</code>
                      <code className="mr-2">test</code>
                      <code className="mr-2">bizstack</code>
                      <code>blitz</code>
                    </div>
                  </div>
                  
                  {CompanyStorage.getRememberedCompany() && (
                    <div className="saved-company text-center">
                      <small className="text-muted d-block mb-2">
                        Saved company: <strong>{CompanyStorage.getRememberedCompany()}</strong>
                      </small>
                      <Button 
                        theme="light" 
                        size="sm"
                        onClick={handleForgetCompany}
                      >
                        <i className="fas fa-times mr-1"></i>
                        Forget Saved Company
                      </Button>
                    </div>
                  )}
                </div>
              </CardBody>
            </Card>
          </Col>
        </Row>
      </Container>
    </div>
  );
};

export default CompanySelector;