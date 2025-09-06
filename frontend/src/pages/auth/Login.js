import React, { useState } from "react";
import { Container, Row, Col, Card, CardBody, Form, FormInput, Button, Alert } from "shards-react";
import { useAuth } from "../../context/auth/AuthContext";

const Login = () => {
  const { login, loading, error } = useAuth();
  const [formData, setFormData] = useState({
    username: "",
    password: ""
  });
  const [localError, setLocalError] = useState("");
  const [success, setSuccess] = useState("");

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
    setLocalError("");
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setLocalError("");
    setSuccess("");

    if (!formData.username || !formData.password) {
      setLocalError("Please fill in all fields");
      return;
    }

    try {
      const result = await login(formData);
      
      if (result.success) {
        setSuccess("Login successful! Redirecting...");
        // Redirect will be handled by route protection
        setTimeout(() => {
          window.location.href = "/dashboard";
        }, 1000);
      } else {
        setLocalError(result.message || "Login failed");
      }
    } catch (err) {
      setLocalError("An error occurred during login");
    }
  };

  return (
    <Container fluid className="main-content-container px-4 pb-4">
      <Row className="justify-content-center">
        <Col lg="5" md="8" sm="10">
          <Card className="mt-5">
            <CardBody className="p-4">
              <div className="text-center mb-4">
                <h3>BizStack Login</h3>
                <p className="text-muted">Sign in to your account</p>
              </div>

              {(error || localError) && (
                <Alert theme="danger" className="mb-3">
                  {error || localError}
                </Alert>
              )}

              {success && (
                <Alert theme="success" className="mb-3">
                  {success}
                </Alert>
              )}

              <Form onSubmit={handleSubmit}>
                <div className="mb-3">
                  <label htmlFor="username">Username</label>
                  <FormInput
                    id="username"
                    name="username"
                    type="text"
                    placeholder="Enter your username"
                    value={formData.username}
                    onChange={handleChange}
                    disabled={loading}
                  />
                </div>

                <div className="mb-3">
                  <label htmlFor="password">Password</label>
                  <FormInput
                    id="password"
                    name="password"
                    type="password"
                    placeholder="Enter your password"
                    value={formData.password}
                    onChange={handleChange}
                    disabled={loading}
                  />
                </div>

                <Button
                  type="submit"
                  theme="primary"
                  size="lg"
                  className="w-100"
                  disabled={loading}
                >
                  {loading ? "Signing in..." : "Sign In"}
                </Button>
              </Form>

              <div className="text-center mt-3">
                <small className="text-muted">
                  Demo credentials: admin / password123
                </small>
              </div>
            </CardBody>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default Login;
