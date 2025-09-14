import React, { useState, useEffect } from "react";
import { withRouter } from "react-router-dom";
import { Container, Row, Col, Card, CardHeader, CardBody, Button, Form, FormInput, Alert, ButtonGroup } from "shards-react";
import AuthAPI from "../api/auth/authApi";
import CompanyStorage from "../utils/companyStorage";
import "./Login.css";

const Login = ({ history, match }) => {
  const { companyCode } = match.params;
  
  // State management
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [companyInfo, setCompanyInfo] = useState(null);
  
  // Form state
  const [formData, setFormData] = useState({
    username: "",
    password: ""
  });

  // Load company info based on code
  useEffect(() => {
    if (companyCode) {
      loadCompanyInfo(companyCode);
    }
  }, [companyCode]);

  const loadCompanyInfo = async (code) => {
    try {
      // Normalize company code to uppercase for comparison
      const normalizedCode = code.toUpperCase();
      
      // Mock company lookup - in real app, call API with case-insensitive search
      const validCompanies = {
        'TEST': {
          code: 'TEST',
          name: 'PT Test Company',
          description: 'Test Company Description',
          logo: '/images/test-logo.png'
        },
        'DEMO': {
          code: 'DEMO',
          name: 'PT Demo Company',
          description: 'Demo Company Description',
          logo: '/images/demo-logo.png'
        },
        'BLITZ': {
          code: 'BLITZ',
          name: 'PT Blitz Technology Indonesia',
          description: 'Leading business technology solutions provider',
          logo: '/images/blitz-logo.png'
        }
      };

      const company = validCompanies[normalizedCode];
      if (company) {
        setCompanyInfo(company);
      } else {
        setError(`Company "${code}" not found`);
      }
    } catch (err) {
      setError("Failed to load company information");
    }
  };

  const handleBackToSelector = () => {
    history.push('/select-company');
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
    setLoading(true);

    try {
      const loginData = {
        ...formData,
        companyCode: companyCode
      };
      const result = await AuthAPI.login(loginData);
      
      if (result.success) {
        // Store company code, token and redirect to company dashboard
        localStorage.setItem('companyCode', companyCode.toUpperCase());
        if (result.data && result.data.accessToken) {
          localStorage.setItem('accessToken', result.data.accessToken);
        }
        history.push(`/${companyCode.toUpperCase()}/dashboard`);
      } else {
        setError(result.message || "Invalid username or password");
      }
    } catch (err) {
      setError("Login failed. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="min-vh-100 d-flex align-items-center bg-light">
      <Container>
        <Row className="justify-content-center">
          <Col lg="5" md="7">
            <Card className="shadow">
              <CardHeader className="bg-primary text-white text-center">
                <div className="py-3">
                  {companyInfo && (
                    <>
                      <h4 className="mb-1">{companyInfo.name}</h4>
                      <p className="mb-0 opacity-75">{companyInfo.description}</p>
                    </>
                  )}
                  {!companyInfo && companyCode && (
                    <h4 className="mb-0">Company Login</h4>
                  )}
                </div>
              </CardHeader>
              <CardBody className="p-4">
                {/* Company Code Display */}
                {companyCode && (
                  <div className="text-center mb-4">
                    <div className="badge badge-primary badge-lg">
                      <i className="fas fa-building mr-2"></i>
                      {companyInfo ? companyInfo.code : companyCode.toUpperCase()}
                    </div>
                    <div className="text-muted mt-2">
                      <small>
                        Multi-tenant login for {companyInfo ? companyInfo.code : companyCode.toUpperCase()}
                        {companyCode !== companyCode.toUpperCase() && (
                          <div className="mt-1">
                            <i className="fas fa-info-circle mr-1"></i>
                            Entered: <code>{companyCode}</code> → Normalized: <code>{companyCode.toUpperCase()}</code>
                          </div>
                        )}
                      </small>
                    </div>
                  </div>
                )}

                {/* Error Alert */}
                {error && (
                  <Alert theme="danger" className="mb-3">
                    <i className="fas fa-exclamation-triangle mr-2"></i>
                    {error}
                  </Alert>
                )}

                {/* Login Form */}
                {companyInfo && (
                  <Form onSubmit={handleSubmit}>
                    <div className="mb-3">
                      <label htmlFor="username">Username</label>
                      <FormInput
                        id="username"
                        name="username"
                        value={formData.username}
                        onChange={handleChange}
                        required
                        placeholder="Enter your username"
                        autoComplete="username"
                      />
                    </div>

                    <div className="mb-4">
                      <label htmlFor="password">Password</label>
                      <FormInput
                        id="password"
                        name="password"
                        type="password"
                        value={formData.password}
                        onChange={handleChange}
                        required
                        placeholder="Enter your password"
                        autoComplete="current-password"
                      />
                    </div>

                    <Button 
                      type="submit" 
                      theme="primary" 
                      size="lg" 
                      block
                      disabled={loading}
                    >
                      {loading ? (
                        <>
                          <i className="fas fa-spinner fa-spin mr-2"></i>
                          Signing in...
                        </>
                      ) : (
                        <>
                          <i className="fas fa-sign-in-alt mr-2"></i>
                          Sign In
                        </>
                      )}
                    </Button>
                  </Form>
                )}

                {/* Default Credentials & Back Button */}
                {companyInfo && (
                  <div className="mt-4">
                    <div className="p-3 bg-light rounded">
                      <h6 className="mb-2">
                        <i className="fas fa-info-circle mr-2"></i>
                        Default Credentials
                      </h6>
                      <div className="row">
                        <div className="col-6">
                          <small className="text-muted">Username:</small>
                          <div><code>admin</code></div>
                        </div>
                        <div className="col-6">
                          <small className="text-muted">Password:</small>
                          <div><code>admin123</code></div>
                        </div>
                      </div>
                    </div>

                    <div className="auth-actions mt-3">
                      <Button
                        theme="light"
                        className="back-button"
                        onClick={() => {
                          CompanyStorage.forgetCompany();
                          setCompanyInfo(null);
                          setError("");
                          setFormData({ username: "", password: "" });
                          history.push('/select-company');
                        }}
                      >
                        <i className="fas fa-arrow-left"></i>
                        Back to Company Selection
                      </Button>
                    </div>
                  </div>
                )}

                {/* Loading State */}
                {!companyInfo && companyCode && !error && (
                  <div className="text-center py-4">
                    <i className="fas fa-search fa-2x text-muted mb-3"></i>
                    <h5>Loading company information...</h5>
                    <p className="text-muted">Please wait while we verify the company code.</p>
                  </div>
                )}
              </CardBody>
            </Card>

            {/* Footer */}
            <div className="text-center mt-4">
              <small className="text-muted">
                <i className="fas fa-shield-alt mr-1"></i>
                Secure multi-tenant login powered by BizStack
              </small>
            </div>
          </Col>
        </Row>
      </Container>
    </div>
  );
};

export default withRouter(Login);
