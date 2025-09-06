import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card, CardHeader, CardBody, Button, Badge, Form, FormInput, Alert } from "shards-react";
import PageTitle from "../../components/common/PageTitle";
import Pagination from "../../components/common/Pagination";
import userAPI from "../../api/user/userApi";
import roleAPI from "../../api/auth/roleApi";

const UserManagement = () => {
  // State management
  const [users, setUsers] = useState([]);
  const [roles, setRoles] = useState([]);
  const [positions, setPositions] = useState([]);
  const [departments, setDepartments] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  
  // View states
  const [currentView, setCurrentView] = useState('list'); // 'list', 'add', 'edit'
  const [editingUser, setEditingUser] = useState(null);
  const [formLoading, setFormLoading] = useState(false);
  
  // Pagination states
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(10);
  const [totalItems, setTotalItems] = useState(0);
  const [totalPages, setTotalPages] = useState(0);
  
  // Search state
  const [searchQuery, setSearchQuery] = useState("");
  const [searchTimeout, setSearchTimeout] = useState(null);
  
  // Form state
  const [formData, setFormData] = useState({
    username: "",
    email: "",
    firstName: "",
    lastName: "",
    phoneNumber: "",
    positionId: "",
    departmentId: "",
    roleId: "",
    isActive: true
  });

  // Load users
  useEffect(() => {
    if (currentView === 'list') {
      loadUsers();
    }
    loadRoles();
    loadPositions();
    loadDepartments();
  }, [currentPage, itemsPerPage, searchQuery, currentView]);

  const loadUsers = async () => {
    try {
      setLoading(true);
      const result = await userAPI.getUsers(currentPage, itemsPerPage, searchQuery);
      
      if (result.success) {
        setUsers(result.data);
        setTotalItems(result.pagination.totalItems);
        setTotalPages(result.pagination.totalPages);
        setError("");
      } else {
        setError(result.message || "Failed to load users");
        setUsers([]);
      }
    } catch (err) {
      setError("An error occurred while loading users");
      setUsers([]);
    } finally {
      setLoading(false);
    }
  };

  const loadRoles = async () => {
    try {
      const result = await roleAPI.getRoles();
      if (result.success) {
        setRoles(result.data);
      }
    } catch (err) {
      console.error('Failed to load roles:', err);
    }
  };

  const loadPositions = async () => {
    try {
      // Mock positions data
      const mockPositions = [
        { id: 1, name: "CEO" },
        { id: 2, name: "CTO" },
        { id: 3, name: "Manager" },
        { id: 4, name: "Senior Developer" },
        { id: 5, name: "Developer" },
        { id: 6, name: "Junior Developer" }
      ];
      setPositions(mockPositions);
    } catch (err) {
      console.error('Failed to load positions:', err);
    }
  };

  const loadDepartments = async () => {
    try {
      // Mock departments data
      const mockDepartments = [
        { id: 1, name: "Information Technology", code: "IT" },
        { id: 2, name: "Human Resources", code: "HR" },
        { id: 3, name: "Finance", code: "FIN" },
        { id: 4, name: "Sales", code: "SALES" },
        { id: 5, name: "Marketing", code: "MKT" }
      ];
      setDepartments(mockDepartments);
    } catch (err) {
      console.error('Failed to load departments:', err);
    }
  };

  // Handle search with debounce
  const handleSearch = (query) => {
    if (searchTimeout) {
      clearTimeout(searchTimeout);
    }
    
    const timeout = setTimeout(() => {
      setSearchQuery(query);
      setCurrentPage(1); // Reset to first page on search
    }, 500);
    
    setSearchTimeout(timeout);
  };

  // Pagination handlers
  const handlePageChange = (page) => {
    setCurrentPage(page);
  };

  const handleItemsPerPageChange = (newItemsPerPage) => {
    setItemsPerPage(newItemsPerPage);
    setCurrentPage(1); // Reset to first page
  };

  // View handlers
  const handleAddUser = () => {
    setEditingUser(null);
    setFormData({
      username: "",
      email: "",
      firstName: "",
      lastName: "",
      phoneNumber: "",
      positionId: "",
      departmentId: "",
      roleId: "",
      isActive: true
    });
    setCurrentView('add');
  };

  const handleEditUser = (user) => {
    setEditingUser(user);
    setFormData({
      username: user.username || "",
      email: user.email || "",
      firstName: user.firstName || "",
      lastName: user.lastName || "",
      phoneNumber: user.phoneNumber || "",
      positionId: user.positionId || "",
      departmentId: user.departmentId || "",
      roleId: user.roleId || "",
      isActive: user.isActive !== false
    });
    setCurrentView('edit');
  };

  const handleCancel = () => {
    setCurrentView('list');
    setEditingUser(null);
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
      let result;
      if (editingUser) {
        result = await userAPI.updateUser(editingUser.id, formData);
      } else {
        result = await userAPI.createUser(formData);
      }

      if (result.success) {
        setSuccess(result.message);
        setTimeout(() => {
          setCurrentView('list');
          setSuccess("");
        }, 2000);
      } else {
        setError(result.message);
      }
    } catch (err) {
      setError("An error occurred while saving user");
    } finally {
      setFormLoading(false);
    }
  };

  const handleDeleteUser = async (userId, username) => {
    if (window.confirm(`Are you sure you want to delete user "${username}"?`)) {
      try {
        const result = await userAPI.deleteUser(userId);
        
        if (result.success) {
          setSuccess(result.message);
          
          // If current page becomes empty, go to previous page
          if (users.length === 1 && currentPage > 1) {
            setCurrentPage(currentPage - 1);
          } else {
            loadUsers();
          }
        } else {
          setError(result.message);
        }
      } catch (err) {
        setError("An error occurred while deleting user");
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
          title="User Management" 
          subtitle="Manage system users and their permissions" 
          className="text-sm-left mb-3" 
        />
      </Row>

      {/* Search and Actions */}
      <Row className="mb-4">
        <Col md="6">
          <div className="d-flex align-items-center">
            <div className="input-group">
              <div className="input-group-prepend">
                <span className="input-group-text">
                  <i className="fas fa-search"></i>
                </span>
              </div>
              <FormInput
                placeholder="Search users..."
                onChange={(e) => handleSearch(e.target.value)}
              />
            </div>
          </div>
        </Col>
        <Col md="6" className="text-right">
          <Button theme="primary" onClick={handleAddUser}>
            <i className="fas fa-plus mr-2"></i>
            Add New User
          </Button>
        </Col>
      </Row>

      {/* Users Table */}
      <Row>
        <Col>
          <Card small className="mb-4">
            <CardHeader className="border-bottom">
              <h6 className="m-0">
                Users ({totalItems} total)
                {searchQuery && <small className="text-muted ml-2">- filtered by "{searchQuery}"</small>}
              </h6>
            </CardHeader>
            <CardBody className="p-0">
              {loading ? (
                <div className="text-center p-4">
                  <i className="fas fa-spinner fa-spin mr-2"></i>
                  Loading users...
                </div>
              ) : (
                <>
                  <div className="table-responsive">
                    <table className="table mb-0">
                      <thead className="bg-light">
                        <tr>
                          <th className="border-0">#</th>
                          <th className="border-0">Name</th>
                          <th className="border-0">Username</th>
                          <th className="border-0">Email</th>
                          <th className="border-0">Position</th>
                          <th className="border-0">Status</th>
                          <th className="border-0">Actions</th>
                        </tr>
                      </thead>
                      <tbody>
                        {users.map((user, index) => (
                          <tr key={user.id}>
                            <td>{(currentPage - 1) * itemsPerPage + index + 1}</td>
                            <td>
                              <div>
                                <strong>{`${user.firstName || ''} ${user.lastName || ''}`.trim() || 'N/A'}</strong>
                                {user.department && <div><small className="text-muted">{user.department}</small></div>}
                              </div>
                            </td>
                            <td>{user.username}</td>
                            <td>{user.email}</td>
                            <td>{user.position || 'N/A'}</td>
                            <td>
                              <Badge theme={user.isActive !== false ? "success" : "secondary"}>
                                {user.isActive !== false ? "Active" : "Inactive"}
                              </Badge>
                            </td>
                            <td>
                              <Button 
                                theme="info" 
                                size="sm" 
                                className="mr-2"
                                onClick={() => handleEditUser(user)}
                              >
                                <i className="fas fa-edit"></i>
                              </Button>
                              <Button 
                                theme="danger" 
                                size="sm"
                                onClick={() => handleDeleteUser(user.id, user.username)}
                              >
                                <i className="fas fa-trash"></i>
                              </Button>
                            </td>
                          </tr>
                        ))}
                        {users.length === 0 && (
                          <tr>
                            <td colSpan="7" className="text-center py-4">
                              <i className="fas fa-users fa-2x text-muted mb-2"></i>
                              <div>No users found</div>
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
              <span className="text-uppercase page-subtitle">User Management</span>
              <h3 className="page-title mb-0">
                <i className={`fas ${editingUser ? 'fa-edit' : 'fa-plus'} mr-2`}></i>
                {editingUser ? "Edit User" : "Add New User"}
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
              <h6 className="m-0">User Information</h6>
            </CardHeader>
            <CardBody>
              <Form onSubmit={handleSubmit}>
                <Row>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="firstName">First Name *</label>
                      <FormInput
                        id="firstName"
                        name="firstName"
                        value={formData.firstName}
                        onChange={handleChange}
                        required
                      />
                    </div>
                  </Col>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="lastName">Last Name *</label>
                      <FormInput
                        id="lastName"
                        name="lastName"
                        value={formData.lastName}
                        onChange={handleChange}
                        required
                      />
                    </div>
                  </Col>
                </Row>

                <Row>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="username">Username *</label>
                      <FormInput
                        id="username"
                        name="username"
                        value={formData.username}
                        onChange={handleChange}
                        required
                      />
                    </div>
                  </Col>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="email">Email *</label>
                      <FormInput
                        id="email"
                        name="email"
                        type="email"
                        value={formData.email}
                        onChange={handleChange}
                        required
                      />
                    </div>
                  </Col>
                </Row>

                <Row>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="positionId">Position</label>
                      <select
                        className="form-control"
                        id="positionId"
                        name="positionId"
                        value={formData.positionId}
                        onChange={handleChange}
                      >
                        <option value="">Select Position</option>
                        {positions.map(position => (
                          <option key={position.id} value={position.id}>
                            {position.name}
                          </option>
                        ))}
                      </select>
                    </div>
                  </Col>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="departmentId">Department</label>
                      <select
                        className="form-control"
                        id="departmentId"
                        name="departmentId"
                        value={formData.departmentId}
                        onChange={handleChange}
                      >
                        <option value="">Select Department</option>
                        {departments.map(department => (
                          <option key={department.id} value={department.id}>
                            {department.name} ({department.code})
                          </option>
                        ))}
                      </select>
                    </div>
                  </Col>
                </Row>

                <Row>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="roleId">Role</label>
                      <select
                        className="form-control"
                        id="roleId"
                        name="roleId"
                        value={formData.roleId}
                        onChange={handleChange}
                      >
                        <option value="">Select Role</option>
                        {roles.map(role => (
                          <option key={role.id} value={role.id}>
                            {role.name}
                          </option>
                        ))}
                      </select>
                    </div>
                  </Col>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="phoneNumber">Phone Number</label>
                      <FormInput
                        id="phoneNumber"
                        name="phoneNumber"
                        value={formData.phoneNumber}
                        onChange={handleChange}
                      />
                    </div>
                  </Col>
                </Row>

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
                      Active User
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
                        {editingUser ? "Updating..." : "Creating..."}
                      </>
                    ) : (
                      <>
                        <i className={`fas ${editingUser ? 'fa-save' : 'fa-plus'} mr-2`}></i>
                        {editingUser ? "Update User" : "Create User"}
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
              <h6 className="m-0">User Roles</h6>
            </CardHeader>
            <CardBody>
              {roles.map(role => (
                <div key={role.id} className="mb-2">
                  <Badge theme="primary" className="mr-2">{role.name}</Badge>
                  <small>{role.description}</small>
                </div>
              ))}
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

export default UserManagement;
