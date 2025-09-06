import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card, CardHeader, CardBody, Button, Badge, Modal, ModalBody, ModalHeader, Form, FormInput, Alert } from "shards-react";
import PageTitle from "../../components/common/PageTitle";
import Pagination from "../../components/common/Pagination";
import userAPI from "../../api/user/userApi";

const UserManagement = () => {
  // State management
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  
  // Modal states
  const [showModal, setShowModal] = useState(false);
  const [editingUser, setEditingUser] = useState(null);
  const [modalLoading, setModalLoading] = useState(false);
  
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
    position: "",
    department: "",
    isActive: true
  });

  // Load users
  useEffect(() => {
    loadUsers();
  }, [currentPage, itemsPerPage, searchQuery]);

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

  // Modal handlers
  const handleAddUser = () => {
    setEditingUser(null);
    setFormData({
      username: "",
      email: "",
      firstName: "",
      lastName: "",
      phoneNumber: "",
      position: "",
      department: "",
      isActive: true
    });
    setShowModal(true);
  };

  const handleEditUser = (user) => {
    setEditingUser(user);
    setFormData({
      username: user.username || "",
      email: user.email || "",
      firstName: user.firstName || "",
      lastName: user.lastName || "",
      phoneNumber: user.phoneNumber || "",
      position: user.position || "",
      department: user.department || "",
      isActive: user.isActive !== false
    });
    setShowModal(true);
  };

  const handleCloseModal = () => {
    setShowModal(false);
    setEditingUser(null);
    setModalLoading(false);
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
    setModalLoading(true);

    try {
      let result;
      if (editingUser) {
        result = await userAPI.updateUser(editingUser.id, formData);
      } else {
        result = await userAPI.createUser(formData);
      }

      if (result.success) {
        setSuccess(result.message);
        setShowModal(false);
        loadUsers(); // Reload current page
      } else {
        setError(result.message);
      }
    } catch (err) {
      setError("An error occurred while saving user");
    } finally {
      setModalLoading(false);
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

  return (
    <Container fluid className="main-content-container px-4">
      {/* Page Header */}
      <Row noGutters className="page-header py-4">
        <PageTitle 
          title="User Management" 
          subtitle="Manage system users and their permissions" 
          className="text-sm-left mb-3" 
        />
      </Row>

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

      {/* Add/Edit User Modal */}
      <Modal open={showModal} toggle={handleCloseModal} size="lg">
        <ModalHeader>
          <i className={`fas ${editingUser ? 'fa-edit' : 'fa-plus'} mr-2`}></i>
          {editingUser ? "Edit User" : "Add New User"}
        </ModalHeader>
        <ModalBody>
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
                  <label htmlFor="position">Position</label>
                  <FormInput
                    id="position"
                    name="position"
                    value={formData.position}
                    onChange={handleChange}
                  />
                </div>
              </Col>
              <Col md="6">
                <div className="mb-3">
                  <label htmlFor="department">Department</label>
                  <FormInput
                    id="department"
                    name="department"
                    value={formData.department}
                    onChange={handleChange}
                  />
                </div>
              </Col>
            </Row>

            <div className="mb-3">
              <label htmlFor="phoneNumber">Phone Number</label>
              <FormInput
                id="phoneNumber"
                name="phoneNumber"
                value={formData.phoneNumber}
                onChange={handleChange}
              />
            </div>

            <div className="mb-3">
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

            <div className="d-flex justify-content-end">
              <Button 
                type="button" 
                theme="secondary" 
                className="mr-2"
                onClick={handleCloseModal}
                disabled={modalLoading}
              >
                Cancel
              </Button>
              <Button 
                type="submit" 
                theme="primary"
                disabled={modalLoading}
              >
                {modalLoading ? (
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
        </ModalBody>
      </Modal>
    </Container>
  );
};

export default UserManagement;
