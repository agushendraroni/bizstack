import React, { useState } from "react";
import { Container, Row, Col, Card, CardBody, Form, FormInput, Button, Alert } from "shards-react";
import { authApi } from "../../api";

const Login = () => {
  const [formData, setFormData] = useState({
    username: "",
    password: ""
  });
  const [localError, setLocalError] = useState("");
  const [success, setSuccess] = useState("");
  const [loading, setLoading] = useState(false);

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

    setLoading(true);
    try {
      const result = await authApi.login(formData);
      
      if (result.success) {
        setSuccess("Login successful! Redirecting...");
        setTimeout(() => {
          window.location.href = "/dashboard";
        }, 1000);
      } else {
        setLocalError(result.message || "Login failed");
      }
    } catch (err) {
      setLocalError("An error occurred during login");
    } finally {
      setLoading(false);
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

              {localError && (
                <Alert theme="danger" className="mb-3">
                  {localError}
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
                  Demo credentials: admin123 / admin123
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
