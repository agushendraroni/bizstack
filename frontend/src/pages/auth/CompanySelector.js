import React from "react";
import { Container, Row, Col, Card, CardHeader, CardBody, Form, FormInput, Button, FormCheckbox } from "shards-react";
import CompanyValidationService from "../../services/api/companyValidation";

// Import required styles
import "bootstrap/dist/css/bootstrap.min.css";
import "../../shards-dashboard/styles/shards-dashboards.1.1.0.min.css";

// Simple storage utility
const CompanyStorage = {
  getRememberedCompany: () => localStorage.getItem('rememberedCompanyCode'),
  setRememberedCompany: (code) => localStorage.setItem('rememberedCompanyCode', code),
  forgetCompany: () => localStorage.removeItem('rememberedCompanyCode'),
  isRememberEnabled: () => localStorage.getItem('rememberCompanyEnabled') === 'true',
  setCompanySettings: (code, data) => localStorage.setItem(`companySettings_${code}`, JSON.stringify(data))
};

class CompanySelector extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      companyCode: "",
      rememberCompany: false,
      loading: false,
      error: ""
    };
  }

  componentDidMount() {
    const rememberedCompany = CompanyStorage.getRememberedCompany();
    if (rememberedCompany) {
      window.location.href = `/${rememberedCompany}/login`;
      return;
    }
    
    this.setState({
      rememberCompany: CompanyStorage.isRememberEnabled()
    });
  }

  handleSubmit = (e) => {
    e.preventDefault();
    this.setState({ error: "" });
    
    const companyCode = this.state.companyCode;
    if (!companyCode.trim()) return;

    const code = companyCode.trim().toLowerCase();
    this.setState({ loading: true });
    
    CompanyValidationService.getCompanyDetails(code)
      .then((companyDetails) => {
        if (!companyDetails.success) {
          this.setState({
            error: companyDetails.error || `Company "${code}" not found`,
            loading: false
          });
          return;
        }

        if (this.state.rememberCompany) {
          CompanyStorage.setRememberedCompany(code);
          CompanyStorage.setCompanySettings(code, companyDetails.data);
        } else {
          CompanyStorage.forgetCompany();
        }

        window.location.href = `/${code}/login`;
      })
      .catch((error) => {
        console.error('Company validation error:', error);
        this.setState({
          error: 'Failed to validate company. Please try again.',
          loading: false
        });
      });
  }

  handleForgetCompany = () => {
    CompanyStorage.forgetCompany();
    this.setState({
      companyCode: '',
      rememberCompany: false,
      error: ''
    });
  }

  render() {
    return (
      <div className="min-vh-100 d-flex align-items-center bg-light">
        <Container>
          <Row className="justify-content-center">
            <Col lg="5" md="7">
              <Card className="shadow">
                <CardHeader className="bg-primary text-white text-center">
                  <div className="py-3">
                    <h4 className="mb-1">BizStack</h4>
                    <p className="mb-0 opacity-75">Enter your company code to continue</p>
                  </div>
                </CardHeader>
                
                <CardBody className="p-4">
                  <Form onSubmit={this.handleSubmit}>
                    <div className="mb-4">
                      <label htmlFor="companyCode">Company Code</label>
                      <FormInput
                        id="companyCode"
                        placeholder="Enter your company code"
                        value={this.state.companyCode}
                        onChange={(e) => this.setState({ companyCode: e.target.value })}
                        required
                        autoFocus
                      />
                    </div>

                    <div className="mb-4">
                      <FormCheckbox
                        checked={this.state.rememberCompany}
                        onChange={(e) => this.setState({ rememberCompany: e.target.checked })}
                      >
                        <i className="fas fa-bookmark mr-2"></i>
                        Remember this company code
                      </FormCheckbox>
                    </div>

                    <Button 
                      type="submit" 
                      theme="primary" 
                      size="lg"
                      block
                      disabled={!this.state.companyCode.trim() || this.state.loading}
                    >
                      {this.state.loading ? (
                        <>
                          <i className="fas fa-spinner fa-spin mr-2"></i>
                          Validating...
                        </>
                      ) : (
                        <>
                          <i className="fas fa-arrow-right mr-2"></i>
                          Continue
                        </>
                      )}
                    </Button>
                  </Form>

                  {this.state.error && (
                    <div className="alert alert-danger mt-3" role="alert">
                      <i className="fas fa-exclamation-triangle mr-2"></i>
                      {this.state.error}
                    </div>
                  )}

                  <div className="mt-4">
                    <div className="p-3 bg-light rounded">
                      <h6 className="mb-2">
                        <i className="fas fa-info-circle mr-2"></i>
                        Available Companies
                      </h6>
                      <div className="text-center">
                        <code className="mr-2">DEMO</code>
                        <code className="mr-2">TEST</code>
                        <code className="mr-2">BLITZ</code>
                      </div>
                    </div>
                  </div>
                </CardBody>
              </Card>

              {/* Footer */}
              <div className="text-center mt-4">
                <small className="text-muted">
                  <i className="fas fa-shield-alt mr-1"></i>
                  Secure multi-tenant access powered by BizStack
                </small>
              </div>
            </Col>
          </Row>
        </Container>
      </div>
    );
  }
}

export default CompanySelector;