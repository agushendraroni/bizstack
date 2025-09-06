import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card, CardHeader, CardBody, Button, Badge, Modal, ModalBody, ModalHeader, Form, FormInput, Alert } from "shards-react";
import PageTitle from "../../components/common/PageTitle";
import userAPI from "../../api/user/userApi";

const UserManagement = () => {
  const [users, setUsers] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  const [showModal, setShowModal] = useState(false);
  const [editingUser, setEditingUser] = useState(null);
  const [formData, setFormData] = useState({
    username: "",
    email: "",
    firstName: "",
    lastName: "",
    phoneNumber: "",
    position: "",
    department: ""
  });

  useEffect(() => {
    loadUsers();
  }, []);

  const loadUsers = async () => {
    try {
      setLoading(true);
      const result = await userAPI.getUsers();
      
      if (result.success) {
        setUsers(result.data);
        setError("");
      } else {
        setError(result.message || "Failed to load users");
      }
    } catch (err) {
      setError("An error occurred while loading users");
    } finally {
      setLoading(false);
    }
  };

  const handleAddUser = () => {
    setEditingUser(null);
    setFormData({
      username: "",
      email: "",
      firstName: "",
      lastName: "",
      phoneNumber: "",
      position: "",
      department: ""
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
      department: user.department || ""
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
      if (editingUser) {
        result = await userAPI.updateUser(editingUser.id, formData);
      } else {
        result = await userAPI.createUser(formData);
      }

      if (result.success) {
        setSuccess(result.message);
        setShowModal(false);
        loadUsers();
      } else {
        setError(result.message);
      }
    } catch (err) {
      setError("An error occurred while saving user");
    }
  };

  const handleDeleteUser = async (userId) => {
    if (window.confirm("Are you sure you want to delete this user?")) {
      try {
        const result = await userAPI.deleteUser(userId);
        
        if (result.success) {
          setSuccess(result.message);
          loadUsers();
        } else {
          setError(result.message);
        }
      } catch (err) {
        setError("An error occurred while deleting user");
      }
    }
  };

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

      {/* Users Table */}
      <Row>
        <Col>
          <Card small className="mb-4">
            <CardHeader className="border-bottom d-flex justify-content-between align-items-center">
              <h6 className="m-0">Users</h6>
              <Button theme="primary" size="sm" onClick={handleAddUser}>
                Add New User
              </Button>
            </CardHeader>
            <CardBody className="p-0 pb-3">
              {loading ? (
                <div className="text-center p-4">Loading users...</div>
              ) : (
                <table className="table table-responsive">
                  <thead>
                    <tr>
                      <th scope="col" className="border-0">#</th>
                      <th scope="col" className="border-0">Name</th>
                      <th scope="col" className="border-0">Username</th>
                      <th scope="col" className="border-0">Email</th>
                      <th scope="col" className="border-0">Position</th>
                      <th scope="col" className="border-0">Status</th>
                      <th scope="col" className="border-0">Actions</th>
                    </tr>
                  </thead>
                  <tbody>
                    {users.map((user, index) => (
                      <tr key={user.id}>
                        <td>{index + 1}</td>
                        <td>{`${user.firstName || ''} ${user.lastName || ''}`.trim() || 'N/A'}</td>
                        <td>{user.username}</td>
                        <td>{user.email}</td>
                        <td>{user.position || 'N/A'}</td>
                        <td>
                          <Badge theme={user.isActive ? "success" : "secondary"}>
                            {user.isActive ? "Active" : "Inactive"}
                          </Badge>
                        </td>
                        <td>
                          <Button 
                            theme="info" 
                            size="sm" 
                            className="mr-2"
                            onClick={() => handleEditUser(user)}
                          >
                            Edit
                          </Button>
                          <Button 
                            theme="danger" 
                            size="sm"
                            onClick={() => handleDeleteUser(user.id)}
                          >
                            Delete
                          </Button>
                        </td>
                      </tr>
                    ))}
                    {users.length === 0 && (
                      <tr>
                        <td colSpan="7" className="text-center">No users found</td>
                      </tr>
                    )}
                  </tbody>
                </table>
              )}
            </CardBody>
          </Card>
        </Col>
      </Row>

      {/* Add/Edit User Modal */}
      <Modal open={showModal} toggle={() => setShowModal(false)}>
        <ModalHeader>
          {editingUser ? "Edit User" : "Add New User"}
        </ModalHeader>
        <ModalBody>
          <Form onSubmit={handleSubmit}>
            <Row>
              <Col md="6">
                <div className="mb-3">
                  <label htmlFor="firstName">First Name</label>
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
                  <label htmlFor="lastName">Last Name</label>
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
                  <label htmlFor="username">Username</label>
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
                  <label htmlFor="email">Email</label>
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
                {editingUser ? "Update" : "Create"} User
              </Button>
            </div>
          </Form>
        </ModalBody>
      </Modal>
    </Container>
  );
};

export default UserManagement;
