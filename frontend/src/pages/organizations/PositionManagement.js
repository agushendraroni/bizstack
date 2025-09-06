import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card, CardHeader, CardBody, Button, Badge, Form, FormInput, Alert } from "shards-react";
import PageTitle from "../../components/common/PageTitle";
import Pagination from "../../components/common/Pagination";

const PositionManagement = () => {
  // State management
  const [positions, setPositions] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  
  // View states
  const [currentView, setCurrentView] = useState('list'); // 'list', 'add', 'edit'
  const [editingPosition, setEditingPosition] = useState(null);
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
    level: "",
    isActive: true
  });

  // Mock data for demo
  useEffect(() => {
    if (currentView === 'list') {
      loadPositions();
    }
  }, [currentPage, itemsPerPage, searchQuery, currentView]);

  const loadPositions = async () => {
    setLoading(true);
    
    // Mock data
    const mockPositions = [
      { id: 1, name: "CEO", description: "Chief Executive Officer", level: "Executive", isActive: true },
      { id: 2, name: "CTO", description: "Chief Technology Officer", level: "Executive", isActive: true },
      { id: 3, name: "Manager", description: "Department Manager", level: "Management", isActive: true },
      { id: 4, name: "Senior Developer", description: "Senior Software Developer", level: "Senior", isActive: true },
      { id: 5, name: "Developer", description: "Software Developer", level: "Staff", isActive: true },
      { id: 6, name: "Junior Developer", description: "Junior Software Developer", level: "Junior", isActive: true },
      { id: 7, name: "HR Manager", description: "Human Resources Manager", level: "Management", isActive: true },
      { id: 8, name: "Sales Manager", description: "Sales Department Manager", level: "Management", isActive: true },
      { id: 9, name: "Marketing Specialist", description: "Marketing Specialist", level: "Staff", isActive: true },
      { id: 10, name: "Accountant", description: "Financial Accountant", level: "Staff", isActive: true }
    ];

    // Filter by search
    let filteredPositions = mockPositions;
    if (searchQuery) {
      filteredPositions = mockPositions.filter(pos => 
        pos.name.toLowerCase().includes(searchQuery.toLowerCase()) ||
        pos.description.toLowerCase().includes(searchQuery.toLowerCase()) ||
        pos.level.toLowerCase().includes(searchQuery.toLowerCase())
      );
    }

    // Pagination
    const startIndex = (currentPage - 1) * itemsPerPage;
    const endIndex = startIndex + itemsPerPage;
    const paginatedPositions = filteredPositions.slice(startIndex, endIndex);

    setPositions(paginatedPositions);
    setTotalItems(filteredPositions.length);
    setTotalPages(Math.ceil(filteredPositions.length / itemsPerPage));
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
  const handleAddPosition = () => {
    setEditingPosition(null);
    setFormData({
      name: "",
      description: "",
      level: "",
      isActive: true
    });
    setCurrentView('add');
  };

  const handleEditPosition = (position) => {
    setEditingPosition(position);
    setFormData({
      name: position.name || "",
      description: position.description || "",
      level: position.level || "",
      isActive: position.isActive !== false
    });
    setCurrentView('edit');
  };

  const handleCancel = () => {
    setCurrentView('list');
    setEditingPosition(null);
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
      
      if (editingPosition) {
        setSuccess("Position updated successfully");
      } else {
        setSuccess("Position created successfully");
      }
      
      setTimeout(() => {
        setCurrentView('list');
        setSuccess("");
      }, 2000);
    } catch (err) {
      setError("An error occurred while saving position");
    } finally {
      setFormLoading(false);
    }
  };

  const handleDeletePosition = async (positionId, positionName) => {
    if (window.confirm(`Are you sure you want to delete position "${positionName}"?`)) {
      try {
        // Mock API call
        await new Promise(resolve => setTimeout(resolve, 500));
        setSuccess("Position deleted successfully");
        loadPositions();
      } catch (err) {
        setError("An error occurred while deleting position");
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
          title="Position Management" 
          subtitle="Manage organizational positions and job levels" 
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
              placeholder="Search positions..."
              onChange={(e) => handleSearch(e.target.value)}
            />
          </div>
        </Col>
        <Col md="6" className="text-right">
          <Button theme="primary" onClick={handleAddPosition}>
            <i className="fas fa-plus mr-2"></i>
            Add New Position
          </Button>
        </Col>
      </Row>

      {/* Positions Table */}
      <Row>
        <Col>
          <Card small className="mb-4">
            <CardHeader className="border-bottom">
              <h6 className="m-0">
                Positions ({totalItems} total)
                {searchQuery && <small className="text-muted ml-2">- filtered by "{searchQuery}"</small>}
              </h6>
            </CardHeader>
            <CardBody className="p-0">
              {loading ? (
                <div className="text-center p-4">
                  <i className="fas fa-spinner fa-spin mr-2"></i>
                  Loading positions...
                </div>
              ) : (
                <>
                  <div className="table-responsive">
                    <table className="table mb-0">
                      <thead className="bg-light">
                        <tr>
                          <th className="border-0">#</th>
                          <th className="border-0">Position Name</th>
                          <th className="border-0">Description</th>
                          <th className="border-0">Level</th>
                          <th className="border-0">Status</th>
                          <th className="border-0">Actions</th>
                        </tr>
                      </thead>
                      <tbody>
                        {positions.map((position, index) => (
                          <tr key={position.id}>
                            <td>{(currentPage - 1) * itemsPerPage + index + 1}</td>
                            <td>
                              <div>
                                <i className="fas fa-user-tie mr-2 text-primary"></i>
                                <strong>{position.name}</strong>
                              </div>
                            </td>
                            <td>{position.description}</td>
                            <td>
                              <Badge theme={
                                position.level === 'Executive' ? 'danger' :
                                position.level === 'Management' ? 'warning' :
                                position.level === 'Senior' ? 'info' :
                                'secondary'
                              }>
                                {position.level}
                              </Badge>
                            </td>
                            <td>
                              <Badge theme={position.isActive ? "success" : "secondary"}>
                                {position.isActive ? "Active" : "Inactive"}
                              </Badge>
                            </td>
                            <td>
                              <Button 
                                theme="info" 
                                size="sm" 
                                className="mr-2"
                                onClick={() => handleEditPosition(position)}
                              >
                                <i className="fas fa-edit"></i>
                              </Button>
                              <Button 
                                theme="danger" 
                                size="sm"
                                onClick={() => handleDeletePosition(position.id, position.name)}
                              >
                                <i className="fas fa-trash"></i>
                              </Button>
                            </td>
                          </tr>
                        ))}
                        {positions.length === 0 && (
                          <tr>
                            <td colSpan="6" className="text-center py-4">
                              <i className="fas fa-user-tie fa-2x text-muted mb-2"></i>
                              <div>No positions found</div>
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
              <span className="text-uppercase page-subtitle">Position Management</span>
              <h3 className="page-title mb-0">
                <i className={`fas ${editingPosition ? 'fa-edit' : 'fa-plus'} mr-2`}></i>
                {editingPosition ? "Edit Position" : "Add New Position"}
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
              <h6 className="m-0">Position Information</h6>
            </CardHeader>
            <CardBody>
              <Form onSubmit={handleSubmit}>
                <Row>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="name">Position Name *</label>
                      <FormInput
                        id="name"
                        name="name"
                        value={formData.name}
                        onChange={handleChange}
                        required
                        placeholder="e.g., Senior Developer"
                      />
                    </div>
                  </Col>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="level">Level *</label>
                      <select
                        className="form-control"
                        id="level"
                        name="level"
                        value={formData.level}
                        onChange={handleChange}
                        required
                      >
                        <option value="">Select Level</option>
                        <option value="Executive">Executive</option>
                        <option value="Management">Management</option>
                        <option value="Senior">Senior</option>
                        <option value="Staff">Staff</option>
                        <option value="Junior">Junior</option>
                      </select>
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
                    placeholder="Brief description of this position"
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
                      Active Position
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
                        {editingPosition ? "Updating..." : "Creating..."}
                      </>
                    ) : (
                      <>
                        <i className={`fas ${editingPosition ? 'fa-save' : 'fa-plus'} mr-2`}></i>
                        {editingPosition ? "Update Position" : "Create Position"}
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
              <h6 className="m-0">Position Levels</h6>
            </CardHeader>
            <CardBody>
              <div className="mb-2">
                <Badge theme="danger" className="mr-2">Executive</Badge>
                <small>CEO, CTO, VP</small>
              </div>
              <div className="mb-2">
                <Badge theme="warning" className="mr-2">Management</Badge>
                <small>Manager, Director</small>
              </div>
              <div className="mb-2">
                <Badge theme="info" className="mr-2">Senior</Badge>
                <small>Senior Staff, Lead</small>
              </div>
              <div className="mb-2">
                <Badge theme="secondary" className="mr-2">Staff</Badge>
                <small>Regular Staff</small>
              </div>
              <div className="mb-2">
                <Badge theme="light" className="mr-2">Junior</Badge>
                <small>Entry Level</small>
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

export default PositionManagement;
