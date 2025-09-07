import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card, CardBody, Form, FormInput, Button, FormCheckbox } from "shards-react";
import CompanyStorage from "../../utils/companyStorage";

const CompanySelector = () => {
  const [companyCode, setCompanyCode] = useState("");
  const [rememberCompany, setRememberCompany] = useState(false);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    // Check if company code is remembered and auto-redirect
    const rememberedCompany = CompanyStorage.getRememberedCompany();
    if (rememberedCompany && CompanyStorage.isValidCompany(rememberedCompany)) {
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
    
    try {
      // Validate company code with backend
      const isValid = await CompanyStorage.isValidCompany(code);
      
      if (!isValid) {
        setError(`Company code "${code}" not found. Please check your company code.`);
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
    } catch (error) {
      setError('Unable to validate company code. Please try again.');
      setLoading(false);
    }
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
    <div className="min-vh-100 d-flex align-items-center bg-light">
      <Container>
        <Row className="justify-content-center">
          <Col lg="5" md="7">
            <Card className="shadow">
              <CardBody className="p-5 text-center">
                <div className="mb-4">
                  <h2 className="text-primary">BizStack</h2>
                  <p className="text-muted">Enter your company code to continue</p>
                </div>

                <Form onSubmit={handleSubmit}>
                  <div className="mb-4">
                    <FormInput
                      size="lg"
                      placeholder="Company Code"
                      value={companyCode}
                      onChange={(e) => setCompanyCode(e.target.value)}
                      required
                      className="text-center"
                      autoFocus
                    />
                  </div>

                  <div className="mb-4">
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
                    size="lg" 
                    block
                    disabled={!companyCode.trim() || loading}
                  >
                    {loading ? 'Validating...' : 'Continue'}
                  </Button>
                </Form>

                {error && (
                  <div className="alert alert-danger mt-3" role="alert">
                    {error}
                  </div>
                )}

                <div className="mt-4">
                  <div className="text-muted mb-2">
                    <small>Valid codes: {CompanyStorage.VALID_COMPANIES.slice(0, 5).join(', ')}, etc.</small>
                  </div>
                  
                  {CompanyStorage.getRememberedCompany() && (
                    <div className="mt-2">
                      <small className="text-info d-block mb-2">
                        Saved: {CompanyStorage.getRememberedCompany()}
                      </small>
                      <Button 
                        theme="outline-secondary" 
                        size="sm"
                        onClick={handleForgetCompany}
                      >
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