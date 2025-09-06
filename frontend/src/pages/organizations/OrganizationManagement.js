import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card, CardHeader, CardBody, Button, Modal, ModalBody, ModalHeader, Form, FormInput, Alert } from "shards-react";
import PageTitle from "../../components/common/PageTitle";
import organizationAPI from "../../api/organization/organizationApi";

const OrganizationManagement = () => {
  const [companies, setCompanies] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [showModal, setShowModal] = useState(false);
  const [editingCompany, setEditingCompany] = useState(null);
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    address: "",
    phone: "",
    email: "",
    website: ""
  });

  useEffect(() => {
    loadCompanies();
  }, []);

  const loadCompanies = async () => {
    try {
      setLoading(true);
      const result = await organizationAPI.getCompanies();
      
      if (result.success) {
        setCompanies(result.data);
        setError("");
      } else {
        setError(result.message || "Failed to load companies");
      }
    } catch (err) {
      setError("An error occurred while loading companies");
    } finally {
      setLoading(false);
    }
  };

  const handleAddCompany = () => {
    setEditingCompany(null);
    setFormData({
      name: "",
      description: "",
      address: "",
      phone: "",
      email: "",
      website: ""
    });
    setShowModal(true);
  };

  const handleEditCompany = (company) => {
    setEditingCompany(company);
    setFormData({
      name: company.name || "",
      description: company.description || "",
      address: company.address || "",
      phone: company.phone || "",
      email: company.email || "",
      website: company.website || ""
    });
    setShowModal(true);
  };

  const handleChange = (e) => {
    setFormData({
      ...formData,
      [e.target.name]: e.target.value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    try {
      let result;
      if (editingCompany) {
        result = await organizationAPI.updateCompany(editingCompany.id, formData);
      } else {
        result = await organizationAPI.createCompany(formData);
      }

      if (result.success) {
        setSuccess(result.message);
        setShowModal(false);
        loadCompanies();
      } else {
        setError(result.message);
      }
    } catch (err) {
      setError("An error occurred while saving company");
    }
  };

  const handleDeleteCompany = async (companyId) => {
    if (window.confirm("Are you sure you want to delete this company?")) {
      try {
        const result = await organizationAPI.deleteCompany(companyId);
        
        if (result.success) {
          setSuccess(result.message);
          loadCompanies();
        } else {
          setError(result.message);
        }
      } catch (err) {
        setError("An error occurred while deleting company");
      }
    }
  };

  return (
    <Container fluid className="main-content-container px-4">
      {/* Page Header */}
      <Row noGutters className="page-header py-4">
        <PageTitle 
          title="Organization Management" 
          subtitle="Manage companies and organizational structure" 
          className="text-sm-left mb-3" 
        />
      </Row>

      {/* Alerts */}
      {error && (
        <Row>
          <Col>
            <Alert theme="danger" className="mb-3">
              {error}
            </Alert>
          </Col>
        </Row>
      )}

      {success && (
        <Row>
          <Col>
            <Alert theme="success" className="mb-3">
              {success}
            </Alert>
          </Col>
        </Row>
      )}

      {/* Companies Table */}
      <Row>
        <Col>
          <Card small className="mb-4">
            <CardHeader className="border-bottom d-flex justify-content-between align-items-center">
              <h6 className="m-0">Companies</h6>
              <Button theme="primary" size="sm" onClick={handleAddCompany}>
                Add New Company
              </Button>
            </CardHeader>
            <CardBody className="p-0 pb-3">
              {loading ? (
                <div className="text-center p-4">Loading companies...</div>
              ) : (
                <table className="table table-responsive">
                  <thead>
                    <tr>
                      <th scope="col" className="border-0">#</th>
                      <th scope="col" className="border-0">Company Name</th>
                      <th scope="col" className="border-0">Description</th>
                      <th scope="col" className="border-0">Email</th>
                      <th scope="col" className="border-0">Phone</th>
                      <th scope="col" className="border-0">Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {companies.map((company, index) => (
                      <tr key={company.id}>
                        <td>{index + 1}</td>
                        <td>{company.name}</td>
                        <td>{company.description || 'N/A'}</td>
                        <td>{company.email || 'N/A'}</td>
                        <td>{company.phone || 'N/A'}</td>
                        <td>
                          <Button 
                            theme="info" 
                            size="sm" 
                            className="mr-2"
                            onClick={() => handleEditCompany(company)}
                          >
                            Edit
                          </Button>
                          <Button 
                            theme="danger" 
                            size="sm"
                            onClick={() => handleDeleteCompany(company.id)}
                          >
                            Delete
                          </Button>
                        </td>
                      </tr>
                    ))}
                    {companies.length === 0 && (
                      <tr>
                        <td colSpan="6" className="text-center">No companies found</td>
                      </tr>
                    )}
                  </tbody>
                </table>
              )}
            </CardBody>
          </Card>
        </Col>
      </Row>

      {/* Add/Edit Company Modal */}
      <Modal open={showModal} toggle={() => setShowModal(false)}>
        <ModalHeader>
          {editingCompany ? "Edit Company" : "Add New Company"}
        </ModalHeader>
        <ModalBody>
          <Form onSubmit={handleSubmit}>
            <div className="mb-3">
              <label htmlFor="name">Company Name</label>
              <FormInput
                id="name"
                name="name"
                value={formData.name}
                onChange={handleChange}
                required
              />
            </div>

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
              <label htmlFor="email">Email</label>
              <FormInput
                id="email"
                name="email"
                type="email"
                value={formData.email}
                onChange={handleChange}
              />
            </div>

            <div className="mb-3">
              <label htmlFor="phone">Phone</label>
              <FormInput
                id="phone"
                name="phone"
                value={formData.phone}
                onChange={handleChange}
              />
            </div>

            <div className="mb-3">
              <label htmlFor="website">Website</label>
              <FormInput
                id="website"
                name="website"
                value={formData.website}
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

            <div className="d-flex justify-content-end">
              <Button 
                type="button" 
                theme="secondary" 
                className="mr-2"
                onClick={() => setShowModal(false)}
              >
                Cancel
              </Button>
              <Button type="submit" theme="primary">
                {editingCompany ? "Update" : "Create"} Company
              </Button>
            </div>
          </Form>
        </ModalBody>
      </Modal>
    </Container>
  );
};

export default OrganizationManagement;
