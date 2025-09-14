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
      <div className="auth-wrapper">
        <Container>
          <Row className="justify-content-center">
            <Col lg="5" md="7">
              <Card className="auth-card">
                <CardHeader className="text-center">
                  <h1 className="company-title">BizStack</h1>
                  <p className="company-subtitle">Enter your company code to continue</p>
                </CardHeader>
                
                <CardBody>
                  <Form onSubmit={this.handleSubmit}>
                    <div className="mb-4">
                      <FormInput
                        placeholder="Company Code"
                        value={this.state.companyCode}
                        onChange={(e) => this.setState({ companyCode: e.target.value })}
                        required
                        autoFocus
                      />
                    </div>

                    <div className="remember-checkbox">
                      <FormCheckbox
                        checked={this.state.rememberCompany}
                        onChange={(e) => this.setState({ rememberCompany: e.target.checked })}
                      >
                        Remember this company code
                      </FormCheckbox>
                    </div>

                    <Button 
                      type="submit" 
                      theme="primary" 
                      block
                      disabled={!this.state.companyCode.trim() || this.state.loading}
                    >
                      {this.state.loading ? 'Validating...' : 'Continue'}
                    </Button>
                  </Form>

                  {this.state.error && (
                    <div className="alert alert-danger mt-3" role="alert">
                      {this.state.error}
                    </div>
                  )}

                  <div className="valid-companies">
                    <div className="text-muted text-center">
                      <small>Valid company codes:</small>
                      <div className="mt-2">
                        <code className="mr-2">demo</code>
                        <code className="mr-2">test</code>
                        <code className="mr-2">bizstack</code>
                      </div>
                    </div>
                  </div>
                </CardBody>
              </Card>
            </Col>
          </Row>
        </Container>
      </div>
    );
  }
}

export default CompanySelector;