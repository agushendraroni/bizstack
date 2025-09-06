import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card, CardHeader, CardBody, Button, Badge, Form, FormInput, Alert } from "shards-react";
import PageTitle from "../../components/common/PageTitle";

const MenuTreeManagement = () => {
  // State management
  const [menuItems, setMenuItems] = useState([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  
  // View states
  const [currentView, setCurrentView] = useState('list'); // 'list', 'add', 'edit'
  const [editingItem, setEditingItem] = useState(null);
  const [formLoading, setFormLoading] = useState(false);
  
  // Form state
  const [formData, setFormData] = useState({
    title: "",
    to: "",
    htmlBefore: "",
    htmlAfter: "",
    level: 1,
    parentId: "",
    order: 1,
    isActive: true,
    isExpandable: false
  });

  // Default menu data
  const defaultMenuData = [
    {
      id: 1,
      title: "Business Dashboard",
      to: "/dashboard",
      htmlBefore: '<i class="fas fa-tachometer-alt"></i>',
      htmlAfter: "",
      level: 1,
      parentId: null,
      order: 1,
      isActive: true,
      isExpandable: false
    },
    {
      id: 2,
      title: "Organization",
      to: "#",
      htmlBefore: '<i class="fas fa-building"></i>',
      htmlAfter: "",
      level: 1,
      parentId: null,
      order: 2,
      isActive: true,
      isExpandable: true
    },
    {
      id: 3,
      title: "User Management",
      to: "/users",
      htmlBefore: '<i class="fas fa-users"></i>',
      htmlAfter: "",
      level: 2,
      parentId: 2,
      order: 1,
      isActive: true,
      isExpandable: false
    },
    {
      id: 4,
      title: "Role Management",
      to: "/roles",
      htmlBefore: '<i class="fas fa-shield-alt"></i>',
      htmlAfter: "",
      level: 2,
      parentId: 2,
      order: 2,
      isActive: true,
      isExpandable: false
    },
    {
      id: 5,
      title: "Companies",
      to: "/organizations",
      htmlBefore: '<i class="fas fa-building"></i>',
      htmlAfter: "",
      level: 2,
      parentId: 2,
      order: 3,
      isActive: true,
      isExpandable: false
    },
    {
      id: 6,
      title: "Departments",
      to: "/departments",
      htmlBefore: '<i class="fas fa-sitemap"></i>',
      htmlAfter: "",
      level: 2,
      parentId: 2,
      order: 4,
      isActive: true,
      isExpandable: false
    },
    {
      id: 7,
      title: "Positions",
      to: "/positions",
      htmlBefore: '<i class="fas fa-user-tie"></i>',
      htmlAfter: "",
      level: 2,
      parentId: 2,
      order: 5,
      isActive: true,
      isExpandable: false
    },
    {
      id: 8,
      title: "System Settings",
      to: "#",
      htmlBefore: '<i class="fas fa-cogs"></i>',
      htmlAfter: "",
      level: 1,
      parentId: null,
      order: 3,
      isActive: true,
      isExpandable: true
    },
    {
      id: 9,
      title: "Menu Tree",
      to: "/menu-tree",
      htmlBefore: '<i class="fas fa-sitemap"></i>',
      htmlAfter: "",
      level: 2,
      parentId: 8,
      order: 1,
      isActive: true,
      isExpandable: false
    }
  ];

  // Load menu items
  useEffect(() => {
    if (currentView === 'list') {
      loadMenuItems();
    }
  }, [currentView]);

  const loadMenuItems = async () => {
    setLoading(true);
    
    try {
      // Mock API call - use default data
      await new Promise(resolve => setTimeout(resolve, 500));
      setMenuItems(defaultMenuData);
      setError("");
    } catch (err) {
      setError("Failed to load menu items");
      setMenuItems([]);
    } finally {
      setLoading(false);
    }
  };

  // View handlers
  const handleAddItem = () => {
    setEditingItem(null);
    setFormData({
      title: "",
      to: "",
      htmlBefore: "",
      htmlAfter: "",
      level: 1,
      parentId: "",
      order: 1,
      isActive: true,
      isExpandable: false
    });
    setCurrentView('add');
  };

  const handleEditItem = (item) => {
    setEditingItem(item);
    setFormData({
      title: item.title || "",
      to: item.to || "",
      htmlBefore: item.htmlBefore || "",
      htmlAfter: item.htmlAfter || "",
      level: item.level || 1,
      parentId: item.parentId || "",
      order: item.order || 1,
      isActive: item.isActive !== false,
      isExpandable: item.isExpandable || false
    });
    setCurrentView('edit');
  };

  const handleCancel = () => {
    setCurrentView('list');
    setEditingItem(null);
    setError("");
    setSuccess("");
  };

  // Form handlers
  const handleChange = (e) => {
    const { name, value, type, checked } = e.target;
    setFormData({
      ...formData,
      [name]: type === 'checkbox' ? checked : (name === 'level' || name === 'order' ? parseInt(value) : value)
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
      
      if (editingItem) {
        setSuccess("Menu item updated successfully");
      } else {
        setSuccess("Menu item created successfully");
      }
      
      setTimeout(() => {
        setCurrentView('list');
        setSuccess("");
      }, 2000);
    } catch (err) {
      setError("An error occurred while saving menu item");
    } finally {
      setFormLoading(false);
    }
  };

  const handleDeleteItem = async (itemId, itemTitle) => {
    if (window.confirm(`Are you sure you want to delete menu item "${itemTitle}"?`)) {
      try {
        // Mock API call
        await new Promise(resolve => setTimeout(resolve, 500));
        setSuccess("Menu item deleted successfully");
        loadMenuItems();
      } catch (err) {
        setError("An error occurred while deleting menu item");
      }
    }
  };

  // Helper functions
  const getParentItems = () => {
    return menuItems.filter(item => item.level === 1 && item.isExpandable);
  };

  const getChildItems = (parentId) => {
    return menuItems.filter(item => item.parentId === parentId);
  };

  const renderMenuTree = () => {
    const rootItems = menuItems.filter(item => !item.parentId);
    
    return rootItems.map(item => (
      <div key={item.id} className="mb-2">
        <div className="d-flex align-items-center p-2 border rounded">
          <div 
            className="flex-grow-1 d-flex align-items-center"
            dangerouslySetInnerHTML={{ __html: item.htmlBefore }}
          />
          <span className="mx-2 font-weight-bold">{item.title}</span>
          <Badge theme={item.isActive ? "success" : "secondary"} className="mr-2">
            {item.isActive ? "Active" : "Inactive"}
          </Badge>
          <Badge theme="info" className="mr-2">
            Level {item.level}
          </Badge>
          <Button 
            theme="info" 
            size="sm" 
            className="mr-1"
            onClick={() => handleEditItem(item)}
          >
            <i className="fas fa-edit"></i>
          </Button>
          <Button 
            theme="danger" 
            size="sm"
            onClick={() => handleDeleteItem(item.id, item.title)}
          >
            <i className="fas fa-trash"></i>
          </Button>
        </div>
        
        {/* Child items */}
        {item.isExpandable && (
          <div className="ml-4 mt-2">
            {getChildItems(item.id).map(child => (
              <div key={child.id} className="d-flex align-items-center p-2 border rounded mb-1">
                <div 
                  className="flex-grow-1 d-flex align-items-center"
                  dangerouslySetInnerHTML={{ __html: child.htmlBefore }}
                />
                <span className="mx-2">{child.title}</span>
                <Badge theme={child.isActive ? "success" : "secondary"} className="mr-2">
                  {child.isActive ? "Active" : "Inactive"}
                </Badge>
                <Badge theme="warning" className="mr-2">
                  Level {child.level}
                </Badge>
                <Button 
                  theme="info" 
                  size="sm" 
                  className="mr-1"
                  onClick={() => handleEditItem(child)}
                >
                  <i className="fas fa-edit"></i>
                </Button>
                <Button 
                  theme="danger" 
                  size="sm"
                  onClick={() => handleDeleteItem(child.id, child.title)}
                >
                  <i className="fas fa-trash"></i>
                </Button>
              </div>
            ))}
          </div>
        )}
      </div>
    ));
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
        <Col>
          <div className="d-flex justify-content-between align-items-center">
            <div>
              <span className="text-uppercase page-subtitle">System Settings</span>
              <h3 className="page-title mb-0">Menu Tree Management</h3>
              <p className="page-subtitle mb-0">Manage system navigation menu structure</p>
            </div>
            <Button theme="primary" onClick={handleAddItem}>
              <i className="fas fa-plus mr-2"></i>
              Add Menu Item
            </Button>
          </div>
        </Col>
      </Row>

      {/* Menu Tree */}
      <Row>
        <Col>
          <Card small className="mb-4">
            <CardHeader className="border-bottom">
              <h6 className="m-0">Current Menu Structure</h6>
            </CardHeader>
            <CardBody>
              {loading ? (
                <div className="text-center p-4">
                  <i className="fas fa-spinner fa-spin mr-2"></i>
                  Loading menu items...
                </div>
              ) : (
                <div className="menu-tree">
                  {renderMenuTree()}
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
              <span className="text-uppercase page-subtitle">System Settings</span>
              <h3 className="page-title mb-0">
                <i className={`fas ${editingItem ? 'fa-edit' : 'fa-plus'} mr-2`}></i>
                {editingItem ? "Edit Menu Item" : "Add Menu Item"}
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
              <h6 className="m-0">Menu Item Information</h6>
            </CardHeader>
            <CardBody>
              <Form onSubmit={handleSubmit}>
                <Row>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="title">Menu Title *</label>
                      <FormInput
                        id="title"
                        name="title"
                        value={formData.title}
                        onChange={handleChange}
                        required
                        placeholder="e.g., User Management"
                      />
                    </div>
                  </Col>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="to">Route/URL *</label>
                      <FormInput
                        id="to"
                        name="to"
                        value={formData.to}
                        onChange={handleChange}
                        required
                        placeholder="e.g., /users or #"
                      />
                    </div>
                  </Col>
                </Row>

                <Row>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="htmlBefore">Icon HTML</label>
                      <FormInput
                        id="htmlBefore"
                        name="htmlBefore"
                        value={formData.htmlBefore}
                        onChange={handleChange}
                        placeholder='e.g., <i class="fas fa-users"></i>'
                      />
                    </div>
                  </Col>
                  <Col md="6">
                    <div className="mb-3">
                      <label htmlFor="htmlAfter">After HTML</label>
                      <FormInput
                        id="htmlAfter"
                        name="htmlAfter"
                        value={formData.htmlAfter}
                        onChange={handleChange}
                        placeholder="Optional HTML after title"
                      />
                    </div>
                  </Col>
                </Row>

                <Row>
                  <Col md="4">
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
                        <option value={1}>Level 1 (Root)</option>
                        <option value={2}>Level 2 (Sub-menu)</option>
                        <option value={3}>Level 3 (Sub-sub-menu)</option>
                      </select>
                    </div>
                  </Col>
                  <Col md="4">
                    <div className="mb-3">
                      <label htmlFor="parentId">Parent Menu</label>
                      <select
                        className="form-control"
                        id="parentId"
                        name="parentId"
                        value={formData.parentId}
                        onChange={handleChange}
                        disabled={formData.level === 1}
                      >
                        <option value="">No Parent</option>
                        {getParentItems().map(parent => (
                          <option key={parent.id} value={parent.id}>
                            {parent.title}
                          </option>
                        ))}
                      </select>
                    </div>
                  </Col>
                  <Col md="4">
                    <div className="mb-3">
                      <label htmlFor="order">Order *</label>
                      <FormInput
                        id="order"
                        name="order"
                        type="number"
                        value={formData.order}
                        onChange={handleChange}
                        required
                        min="1"
                      />
                    </div>
                  </Col>
                </Row>

                <Row>
                  <Col md="6">
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
                          Active Menu Item
                        </label>
                      </div>
                    </div>
                  </Col>
                  <Col md="6">
                    <div className="mb-3">
                      <div className="custom-control custom-checkbox">
                        <input
                          type="checkbox"
                          className="custom-control-input"
                          id="isExpandable"
                          name="isExpandable"
                          checked={formData.isExpandable}
                          onChange={handleChange}
                        />
                        <label className="custom-control-label" htmlFor="isExpandable">
                          Expandable Menu (Has Sub-items)
                        </label>
                      </div>
                    </div>
                  </Col>
                </Row>

                <div className="d-flex justify-content-end">
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
                        {editingItem ? "Updating..." : "Creating..."}
                      </>
                    ) : (
                      <>
                        <i className={`fas ${editingItem ? 'fa-save' : 'fa-plus'} mr-2`}></i>
                        {editingItem ? "Update Menu Item" : "Create Menu Item"}
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
              <h6 className="m-0">Icon Examples</h6>
            </CardHeader>
            <CardBody>
              <div className="mb-2">
                <code>&lt;i class="fas fa-users"&gt;&lt;/i&gt;</code>
                <span className="ml-2"><i className="fas fa-users"></i> Users</span>
              </div>
              <div className="mb-2">
                <code>&lt;i class="fas fa-building"&gt;&lt;/i&gt;</code>
                <span className="ml-2"><i className="fas fa-building"></i> Building</span>
              </div>
              <div className="mb-2">
                <code>&lt;i class="fas fa-cogs"&gt;&lt;/i&gt;</code>
                <span className="ml-2"><i className="fas fa-cogs"></i> Settings</span>
              </div>
              <div className="mb-2">
                <code>&lt;i class="fas fa-shield-alt"&gt;&lt;/i&gt;</code>
                <span className="ml-2"><i className="fas fa-shield-alt"></i> Security</span>
              </div>
              <div className="mb-2">
                <code>&lt;i class="fas fa-user-tie"&gt;&lt;/i&gt;</code>
                <span className="ml-2"><i className="fas fa-user-tie"></i> Position</span>
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

export default MenuTreeManagement;
