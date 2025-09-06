import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card, CardHeader, CardBody, Button, Badge, Form, FormInput, Alert } from "shards-react";
import PageTitle from "../../components/common/PageTitle";
import Pagination from "../../components/common/Pagination";

const DepartmentManagement = () => {
  // State management
  const [departments, setDepartments] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  
  // View states
  const [currentView, setCurrentView] = useState('list'); // 'list', 'add', 'edit'
  const [editingDepartment, setEditingDepartment] = useState(null);
  const [formLoading, setFormLoading] = useState(false);
  
  // Pagination states
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(10);
  const [totalItems, setTotalItems] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  
  // Search state
  const [searchQuery, setSearchQuery] = useState("");
  
  // Form state
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    code: "",
    managerId: "",
    isActive: true
  });

  // Mock data for demo
  useEffect(() => {
    if (currentView === 'list') {
      loadDepartments();
    }
  }, [currentPage, itemsPerPage, searchQuery, currentView]);

  const loadDepartments = async () => {
    setLoading(true);
    
    // Mock data
    const mockDepartments = [
      { id: 1, name: "Information Technology", description: "IT Department", code: "IT", managerId: "John Doe", isActive: true },
      { id: 2, name: "Human Resources", description: "HR Department", code: "HR", managerId: "Jane Smith", isActive: true },
      { id: 3, name: "Finance", description: "Finance & Accounting", code: "FIN", managerId: "Mike Johnson", isActive: true },
      { id: 4, name: "Sales", description: "Sales Department", code: "SALES", managerId: "Sarah Wilson", isActive: true },
      { id: 5, name: "Marketing", description: "Marketing Department", code: "MKT", managerId: "David Brown", isActive: true },
      { id: 6, name: "Operations", description: "Operations Department", code: "OPS", managerId: "Lisa Davis", isActive: true },
      { id: 7, name: "Customer Service", description: "Customer Support", code: "CS", managerId: "Tom Anderson", isActive: true },
      { id: 8, name: "Research & Development", description: "R&D Department", code: "RND", managerId: "Emily Taylor", isActive: true },
      { id: 9, name: "Quality Assurance", description: "QA Department", code: "QA", managerId: "Robert Miller", isActive: true },
      { id: 10, name: "Legal", description: "Legal Department", code: "LEGAL", managerId: "Amanda White", isActive: true }
    ];

    // Filter by search
    let filteredDepartments = mockDepartments;
    if (searchQuery) {
      filteredDepartments = mockDepartments.filter(dept => 
        dept.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
        dept.description.toLowerCase().includes(searchQuery.toLowerCase()) ||
        dept.code.toLowerCase().includes(searchQuery.toLowerCase()) ||
        dept.managerId.toLowerCase().includes(searchQuery.toLowerCase())
      );
    }

    // Pagination
    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    const paginatedDepartments = filteredDepartments.slice(startIndex, endIndex);

    setDepartments(paginatedDepartments);
    setTotalItems(filteredDepartments.length);
    setTotalPages(Math.ceil(filteredDepartments.length / itemsPerPage));
    setLoading(false);
  };

  // Search handler
  const handleSearch = (query) => {
    setSearchQuery(query);
    setCurrentPage(1);
  };

  // Pagination handlers
  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const handleItemsPerPageChange = (newItemsPerPage) => {
    setItemsPerPage(newItemsPerPage);
    setCurrentPage(1);
  };

  // View handlers
  const handleAddDepartment = () => {
    setEditingDepartment(null);
    setFormData({
      name: "",
      description: "",
      code: "",
      managerId: "",
      isActive: true
    });
    setCurrentView('add');
  };

  const handleEditDepartment = (department) => {
    setEditingDepartment(department);
    setFormData({
      name: department.name || "",
      description: department.description || "",
      code: department.code || "",
      managerId: department.managerId || "",
      isActive: department.isActive !== false
    });
    setCurrentView('edit');
  };

  const handleCancel = () => {
    setCurrentView('list');
    setEditingDepartment(null);
    setError("");
    setSuccess("");
  };

  // Form handlers
  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? checked : value
    });
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");
    setFormLoading(true);

    try {
      // Mock API call
      await new Promise(resolve => setTimeout(resolve, 1000));
      
      if (editingDepartment) {
        setSuccess("Department updated successfully");
      } else {
        setSuccess("Department created successfully");
      }
      
      setTimeout(() => {
        setCurrentView('list');
        setSuccess("");
      }, 2000);
    } catch (err) {
      setError("An error occurred while saving department");
    } finally {
      setFormLoading(false);
    }
  };

  const handleDeleteDepartment = async (departmentId, departmentName) => {
    if (window.confirm(`Are you sure you want to delete department "${departmentName}"?`)) {
      try {
        // Mock API call
        await new Promise(resolve => setTimeout(resolve, 500));
        setSuccess("Department deleted successfully");
        loadDepartments();
      } catch (err) {
        setError("An error occurred while deleting department");
      }
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

  // Render List View
  const renderListView = () => (
    <>
      {/* Page Header */}
      <Row noGutters className="page-header py-4">
        <PageTitle 
          title="Department Management" 
          subtitle="Manage organizational departments and structure" 
          className="text-sm-left mb-3" 
        />
      </Row>

      {/* Search and Actions */}
      <Row className="mb-4">
        <Col md="6">
          <div className="input-group">
            <div className="input-group-prepend">
              <span className="input-group-text">
                <i className="fas fa-search"></i>
              </span>
            </div>
            <FormInput
              placeholder="Search departments..."
              onChange={(e) => handleSearch(e.target.value)}
            />
          </div>
        </Col>
        <Col md="6" className="text-right">
          <Button theme="primary" onClick={handleAddDepartment}>
            <i className="fas fa-plus mr-2"></i>
            Add New Department
          </Button>
        </Col>
      </Row>

      {/* Departments Table */}
      <Row>
        <Col>
          <Card small className="mb-4">
            <CardHeader className="border-bottom">
              <h6 className="m-0">
                Departments ({totalItems} total)
                {searchQuery && <small className="text-muted ml-2">- filtered by "{searchQuery}"</small>}
              </h6>
            </CardHeader>
            <CardBody className="p-0">
              {loading ? (
                <div className="text-center p-4">
                  <i className="fas fa-spinner fa-spin mr-2"></i>
                  Loading departments...
                </div>
              ) : (
                <>
                  <div className="table-responsive">
                    <table className="table mb-0">
                      <thead className="bg-light">
                        <tr>
                          <th className="border-0">#</th>
                          <th className="border-0">Department</th>
                          <th className="border-0">Code</th>
                          <th className="border-0">Description</th>
                          <th className="border-0">Manager</th>
                          <th className="border-0">Status</th>
                          <th className="border-0">Actions</th>
                        </tr>
                      </thead>
                      <tbody>
                        {departments.map((department, index) => (
                          <tr key={department.id}>
                            <td>{(currentPage - 1) * itemsPerPage + index + 1}</td>
                            <td>
                              <div>
                                <i className="fas fa-sitemap mr-2 text-primary"></i>
                                <strong>{department.name}</strong>
                              </div>
                            </td>
                            <td>
                              <Badge theme="info">{department.code}</Badge>
                            </td>
                            <td>{department.description}</td>
                            <td>{department.managerId}</td>
                            <td>
                              <Badge theme={department.isActive ? "success" : "secondary"}>
                                {department.isActive ? "Active" : "Inactive"}
                              </Badge>
                            </td>
                            <td>
                              <Button 
                                theme="info" 
                                size="sm" 
                                className="mr-2"
                                onClick={() => handleEditDepartment(department)}
                              >
                                <i className="fas fa-edit"></i>
                              </Button>
                              <Button 
                                theme="danger" 
                                size="sm"
                                onClick={() => handleDeleteDepartment(department.id, department.name)}
                              >
                                <i className="fas fa-trash"></i>
                              </Button>
                            </td>
                          </tr>
                        ))}
                        {departments.length === 0 && (
                          <tr>
                            <td colSpan="7" className="text-center py-4">
                              <i className="fas fa-sitemap fa-2x text-muted mb-2"></i>
                              <div>No departments found</div>
                              {searchQuery && (
                                <small className="text-muted">Try adjusting your search criteria</small>
                              )}
                            </td>
                          </tr>
                        )}
                      </tbody>
                    </table>
                  </div>
                  
                  {/* Pagination */}
                  {totalPages > 1 && (
                    <div className="card-footer">
                      <Pagination
                        currentPage={currentPage}
                        totalPages={totalPages}
                        totalItems={totalItems}
                        itemsPerPage={itemsPerPage}
                        onPageChange={handlePageChange}
                        onItemsPerPageChange={handleItemsPerPageChange}
                      />
                    </div>
                  )}
                </>
              )}
            </CardBody>
          </Card>
        </Col>
      </Row>
    </>
  );

  // Render Form View
  const renderFormView = () => (
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
              Back to List
            </Button>
            <div>
              <span className="text-uppercase page-subtitle">Department Management</span>
              <h3 className="page-title mb-0">
                <i className={`fas ${editingDepartment ? 'fa-edit' : 'fa-plus'} mr-2`}></i>
                {editingDepartment ? "Edit Department" : "Add New Department"}
              </h3>
            </div>
          </div>
        </Col>
      </Row>

      {/* Form */}
      <Row>
        <Col lg="8">
          <Card small className="mb-4">
            <CardHeader className="border-bottom">
              <h6 className="m-0">Department Information</h6>
            </CardHeader>
            <CardBody>
              <Form onSubmit={handleSubmit}>
                <Row>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="name">Department Name *</label>
                      <FormInput
                        id="name"
                        name="name"
                        value={formData.name}
                        onChange={handleChange}
                        required
                        placeholder="e.g., Information Technology"
                      />
                    </div>
                  </Col>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="code">Department Code *</label>
                      <FormInput
                        id="code"
                        name="code"
                        value={formData.code}
                        onChange={handleChange}
                        required
                        placeholder="e.g., IT"
                        style={{ textTransform: 'uppercase' }}
                      />
                    </div>
                  </Col>
                </Row>

                <div className="mb-3">
                  <label htmlFor="description">Description</label>
                  <FormInput
                    id="description"
                    name="description"
                    value={formData.description}
                    onChange={handleChange}
                    placeholder="Brief description of this department"
                  />
                </div>

                <div className="mb-3">
                  <label htmlFor="managerId">Department Manager</label>
                  <FormInput
                    id="managerId"
                    name="managerId"
                    value={formData.managerId}
                    onChange={handleChange}
                    placeholder="Manager name or ID"
                  />
                </div>

                <div className="mb-4">
                  <div className="custom-control custom-checkbox">
                    <input
                      type="checkbox"
                      className="custom-control-input"
                      id="isActive"
                      name="isActive"
                      checked={formData.isActive}
                      onChange={handleChange}
                    />
                    <label className="custom-control-label" htmlFor="isActive">
                      Active Department
                    </label>
                  </div>
                </div>

                <div className="d-flex">
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
                        {editingDepartment ? "Updating..." : "Creating..."}
                      </>
                    ) : (
                      <>
                        <i className={`fas ${editingDepartment ? 'fa-save' : 'fa-plus'} mr-2`}></i>
                        {editingDepartment ? "Update Department" : "Create Department"}
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
              <h6 className="m-0">Department Examples</h6>
            </CardHeader>
            <CardBody>
              <div className="mb-2">
                <Badge theme="info" className="mr-2">IT</Badge>
                <small>Information Technology</small>
              </div>
              <div className="mb-2">
                <Badge theme="warning" className="mr-2">HR</Badge>
                <small>Human Resources</small>
              </div>
              <div className="mb-2">
                <Badge theme="success" className="mr-2">FIN</Badge>
                <small>Finance & Accounting</small>
              </div>
              <div className="mb-2">
                <Badge theme="primary" className="mr-2">SALES</Badge>
                <small>Sales Department</small>
              </div>
              <div className="mb-2">
                <Badge theme="secondary" className="mr-2">MKT</Badge>
                <small>Marketing</small>
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
      {currentView === 'list' ? renderListView() : renderFormView()}
    </Container>
  );
};

export default DepartmentManagement;
