import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card, CardHeader, CardBody, Button, Badge, Form, FormInput, Alert } from "shards-react";
import PageTitle from "../../components/common/PageTitle";
import PermissionMatrix from "../../components/common/PermissionMatrix";
import roleAPI from "../../api/auth/roleApi";

const RoleManagement = () => {
  // State management
  const [roles, setRoles] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  
  // View states
  const [currentView, setCurrentView] = useState('list'); // 'list', 'add', 'edit'
  const [editingRole, setEditingRole] = useState(null);
  const [formLoading, setFormLoading] = useState(false);
  
  // Form state
  const [formData, setFormData] = useState({
    name: "",
    description: "",
    isActive: true
  });
  
  // Permission state
  const [selectedPermissions, setSelectedPermissions] = useState({});
  
  // System modules for permission matrix
  const [systemModules] = useState([
    {
      id: 'users',
      name: 'User Management',
      description: 'Manage system users and profiles',
      icon: 'fa-users'
    },
    {
      id: 'organizations',
      name: 'Organizations',
      description: 'Manage companies and organizational structure',
      icon: 'fa-building'
    },
    {
      id: 'products',
      name: 'Products',
      description: 'Manage product catalog and inventory',
      icon: 'fa-box'
    },
    {
      id: 'customers',
      name: 'Customers',
      description: 'Manage customer relationships and data',
      icon: 'fa-user-friends'
    },
    {
      id: 'transactions',
      name: 'Transactions',
      description: 'Manage sales and financial transactions',
      icon: 'fa-credit-card'
    },
    {
      id: 'reports',
      name: 'Reports',
      description: 'Access business reports and analytics',
      icon: 'fa-chart-bar'
    },
    {
      id: 'notifications',
      name: 'Notifications',
      description: 'Manage system notifications and alerts',
      icon: 'fa-bell'
    },
    {
      id: 'files',
      name: 'File Storage',
      description: 'Manage file uploads and storage',
      icon: 'fa-folder'
    },
    {
      id: 'roles',
      name: 'Role Management',
      description: 'Manage user roles and permissions',
      icon: 'fa-shield-alt'
    }
  ]);

  // Load roles
  useEffect(() => {
    if (currentView === 'list') {
      loadRoles();
    }
  }, [currentView]);

  const loadRoles = async () => {
    try {
      setLoading(true);
      const result = await roleAPI.getRoles();
      
      if (result.success) {
        setRoles(result.data);
        setError("");
      } else {
        setError(result.message || "Failed to load roles");
        setRoles([]);
      }
    } catch (err) {
      setError("An error occurred while loading roles");
      setRoles([]);
    } finally {
      setLoading(false);
    }
  };

  // View handlers
  const handleAddRole = () => {
    setEditingRole(null);
    setFormData({
      name: "",
      description: "",
      isActive: true
    });
    setSelectedPermissions({});
    setCurrentView('add');
  };

  const handleEditRole = (role) => {
    setEditingRole(role);
    setFormData({
      name: role.name || "",
      description: role.description || "",
      isActive: role.isActive !== false
    });
    
    // Convert role permissions to matrix format
    const permissions = {};
    if (role.permissions) {
      role.permissions.forEach(permission => {
        permissions[permission] = true;
      });
    }
    setSelectedPermissions(permissions);
    setCurrentView('edit');
  };

  const handleCancel = () => {
    setCurrentView('list');
    setEditingRole(null);
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

  const handlePermissionChange = (permissions) => {
    setSelectedPermissions(permissions);
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");
    setFormLoading(true);

    try {
      const roleData = {
        ...formData,
        permissions: Object.keys(selectedPermissions)
      };

      let result;
      if (editingRole) {
        result = await roleAPI.updateRole(editingRole.id, roleData);
      } else {
        result = await roleAPI.createRole(roleData);
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
      setError("An error occurred while saving role");
    } finally {
      setFormLoading(false);
    }
  };

  const handleDeleteRole = async (roleId, roleName) => {
    if (window.confirm(`Are you sure you want to delete role "${roleName}"?`)) {
      try {
        const result = await roleAPI.deleteRole(roleId);
        
        if (result.success) {
          setSuccess(result.message);
          loadRoles();
        } else {
          setError(result.message);
        }
      } catch (err) {
        setError("An error occurred while deleting role");
      }
    }
  };

  // Get permission count for role
  const getPermissionCount = (role) => {
    return role.permissions ? role.permissions.length : 0;
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
          title="Role Management" 
          subtitle="Manage user roles and permissions" 
          className="text-sm-left mb-3" 
        />
      </Row>

      {/* Actions */}
      <Row className="mb-4">
        <Col className="text-right">
          <Button theme="primary" onClick={handleAddRole}>
            <i className="fas fa-plus mr-2"></i>
            Add New Role
          </Button>
        </Col>
      </Row>

      {/* Roles Table */}
      <Row>
        <Col>
          <Card small className="mb-4">
            <CardHeader className="border-bottom">
              <h6 className="m-0">System Roles</h6>
            </CardHeader>
            <CardBody className="p-0">
              {loading ? (
                <div className="text-center p-4">
                  <i className="fas fa-spinner fa-spin mr-2"></i>
                  Loading roles...
                </div>
              ) : (
                <div className="table-responsive">
                  <table className="table mb-0">
                    <thead className="bg-light">
                      <tr>
                        <th className="border-0">Role Name</th>
                        <th className="border-0">Description</th>
                        <th className="border-0">Permissions</th>
                        <th className="border-0">Status</th>
                        <th className="border-0">Actions</th>
                      </tr>
                    </thead>
                    <tbody>
                      {roles.map((role) => (
                        <tr key={role.id}>
                          <td>
                            <div>
                              <i className="fas fa-shield-alt mr-2 text-primary"></i>
                              <strong>{role.name}</strong>
                            </div>
                          </td>
                          <td>{role.description || 'N/A'}</td>
                          <td>
                            <Badge theme="info">
                              {getPermissionCount(role)} permissions
                            </Badge>
                          </td>
                          <td>
                            <Badge theme={role.isActive !== false ? "success" : "secondary"}>
                              {role.isActive !== false ? "Active" : "Inactive"}
                            </Badge>
                          </td>
                          <td>
                            <Button 
                              theme="info" 
                              size="sm" 
                              className="mr-2"
                              onClick={() => handleEditRole(role)}
                            >
                              <i className="fas fa-edit"></i>
                            </Button>
                            <Button 
                              theme="danger" 
                              size="sm"
                              onClick={() => handleDeleteRole(role.id, role.name)}
                            >
                              <i className="fas fa-trash"></i>
                            </Button>
                          </td>
                        </tr>
                      ))}
                      {roles.length === 0 && (
                        <tr>
                          <td colSpan="5" className="text-center py-4">
                            <i className="fas fa-shield-alt fa-2x text-muted mb-2"></i>
                            <div>No roles found</div>
                            <small className="text-muted">Create your first role to get started</small>
                          </td>
                        </tr>
                      )}
                    </tbody>
                  </table>
                </div>
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
              <span className="text-uppercase page-subtitle">Role Management</span>
              <h3 className="page-title mb-0">
                <i className={`fas ${editingRole ? 'fa-edit' : 'fa-plus'} mr-2`}></i>
                {editingRole ? "Edit Role" : "Add New Role"}
              </h3>
            </div>
          </div>
        </Col>
      </Row>

      {/* Form */}
      <Row>
        <Col>
          <Card small className="mb-4">
            <CardHeader className="border-bottom">
              <h6 className="m-0">Role Information</h6>
            </CardHeader>
            <CardBody>
              <Form onSubmit={handleSubmit}>
                <Row>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="name">Role Name *</label>
                      <FormInput
                        id="name"
                        name="name"
                        value={formData.name}
                        onChange={handleChange}
                        required
                        placeholder="e.g., Manager, Employee, Admin"
                      />
                    </div>
                  </Col>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="description">Description</label>
                      <FormInput
                        id="description"
                        name="description"
                        value={formData.description}
                        onChange={handleChange}
                        placeholder="Brief description of this role"
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
                      Active Role
                    </label>
                  </div>
                </div>

                <hr />

                <h6 className="mb-3">
                  <i className="fas fa-key mr-2"></i>
                  Permissions
                </h6>
                
                <PermissionMatrix
                  modules={systemModules}
                  selectedPermissions={selectedPermissions}
                  onPermissionChange={handlePermissionChange}
                />

                <hr />

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
                        {editingRole ? "Updating..." : "Creating..."}
                      </>
                    ) : (
                      <>
                        <i className={`fas ${editingRole ? 'fa-save' : 'fa-plus'} mr-2`}></i>
                        {editingRole ? "Update Role" : "Create Role"}
                      </>
                    )}
                  </Button>
                </div>
              </Form>
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

export default RoleManagement;
