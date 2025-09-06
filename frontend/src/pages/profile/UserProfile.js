import React, { useState, useEffect } from "react";
import { Container, Row, Col, Card, CardHeader, CardBody, Button, Form, FormInput, Alert, Nav, NavItem, NavLink, Badge } from "shards-react";
import PageTitle from "../../components/common/PageTitle";

const UserProfile = () => {
  // State management
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");
  
  // Tab state
  const [activeTab, setActiveTab] = useState('profile');
  
  // Profile form state
  const [profileData, setProfileData] = useState({
    firstName: "System",
    lastName: "Administrator",
    username: "admin",
    email: "admin@blitz.co.id",
    phoneNumber: "+62-21-5794-8001",
    position: "System Administrator",
    department: "Information Technology",
    company: "PT Blitz Technology Indonesia",
    companyCode: "BLITZ",
    bio: "System administrator for PT Blitz Technology Indonesia. Responsible for managing the BizStack platform and ensuring optimal system performance for all users."
  });

  // Password form state
  const [passwordData, setPasswordData] = useState({
    currentPassword: "",
    newPassword: "",
    confirmPassword: ""
  });

  // Preferences form state
  const [preferencesData, setPreferencesData] = useState({
    language: "en",
    timezone: "UTC",
    dateFormat: "DD/MM/YYYY",
    theme: "light",
    emailNotifications: true,
    pushNotifications: false,
    weeklyReports: true
  });

  // Form handlers
  const handleProfileChange = (e) => {
    const { name, value } = e.target;
    setProfileData({
      ...profileData,
      [name]: value
    });
  };

  const handlePasswordChange = (e) => {
    const { name, value } = e.target;
    setPasswordData({
      ...passwordData,
      [name]: value
    });
  };

  const handlePreferencesChange = (e) => {
    const { name, value, type, checked } = e.target;
    setPreferencesData({
      ...preferencesData,
      [name]: type === 'checkbox' ? checked : value
    });
  };

  // Submit handlers
  const handleProfileSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");
    setLoading(true);

    try {
      // Mock API call
      await new Promise(resolve => setTimeout(resolve, 1000));
      setSuccess("Profile updated successfully");
    } catch (err) {
      setError("Failed to update profile");
    } finally {
      setLoading(false);
    }
  };

  const handlePasswordSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");

    if (passwordData.newPassword !== passwordData.confirmPassword) {
      setError("New passwords do not match");
      return;
    }

    if (passwordData.newPassword.length < 6) {
      setError("Password must be at least 6 characters long");
      return;
    }

    setLoading(true);

    try {
      // Mock API call
      await new Promise(resolve => setTimeout(resolve, 1000));
      setSuccess("Password changed successfully");
      setPasswordData({
        currentPassword: "",
        newPassword: "",
        confirmPassword: ""
      });
    } catch (err) {
      setError("Failed to change password");
    } finally {
      setLoading(false);
    }
  };

  const handlePreferencesSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setSuccess("");
    setLoading(true);

    try {
      // Mock API call
      await new Promise(resolve => setTimeout(resolve, 1000));
      setSuccess("Preferences updated successfully");
    } catch (err) {
      setError("Failed to update preferences");
    } finally {
      setLoading(false);
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
          title="User Profile" 
          subtitle="Manage your account settings and preferences" 
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

      <Row>
        {/* Profile Card */}
        <Col lg="4">
          <Card small className="mb-4">
            <CardHeader className="border-bottom text-center">
              <div className="mb-3">
                <div className="avatar avatar-xl mx-auto mb-3">
                  <i className="fas fa-user-circle fa-5x text-primary"></i>
                </div>
                <h5 className="mb-0">{profileData.firstName} {profileData.lastName}</h5>
                <span className="text-muted">{profileData.position}</span>
              </div>
            </CardHeader>
            <CardBody className="pt-0">
              <div className="text-center">
                <div className="mb-2">
                  <i className="fas fa-envelope mr-2 text-muted"></i>
                  <small>{profileData.email}</small>
                </div>
                <div className="mb-2">
                  <i className="fas fa-phone mr-2 text-muted"></i>
                  <small>{profileData.phoneNumber}</small>
                </div>
                <div className="mb-2">
                  <i className="fas fa-building mr-2 text-muted"></i>
                  <small>{profileData.company}</small>
                </div>
                <div className="mb-2">
                  <i className="fas fa-sitemap mr-2 text-muted"></i>
                  <small>{profileData.department}</small>
                </div>
                <div className="mt-3">
                  <Badge theme="primary">{profileData.companyCode}</Badge>
                  <div><small className="text-muted">Tenant Code</small></div>
                </div>
              </div>
            </CardBody>
          </Card>
        </Col>

        {/* Settings Tabs */}
        <Col lg="8">
          <Card small className="mb-4">
            <CardHeader className="border-bottom">
              <Nav tabs>
                <NavItem>
                  <NavLink 
                    active={activeTab === 'profile'}
                    onClick={() => setActiveTab('profile')}
                    style={{ cursor: 'pointer' }}
                  >
                    <i className="fas fa-user mr-2"></i>
                    Profile
                  </NavLink>
                </NavItem>
                <NavItem>
                  <NavLink 
                    active={activeTab === 'password'}
                    onClick={() => setActiveTab('password')}
                    style={{ cursor: 'pointer' }}
                  >
                    <i className="fas fa-lock mr-2"></i>
                    Password
                  </NavLink>
                </NavItem>
                <NavItem>
                  <NavLink 
                    active={activeTab === 'preferences'}
                    onClick={() => setActiveTab('preferences')}
                    style={{ cursor: 'pointer' }}
                  >
                    <i className="fas fa-cog mr-2"></i>
                    Preferences
                  </NavLink>
                </NavItem>
              </Nav>
            </CardHeader>
            <CardBody>
              {/* Profile Tab */}
              {activeTab === 'profile' && (
                <Form onSubmit={handleProfileSubmit}>
                  <Row>
                    <Col md="6">
                      <div className="mb-3">
                        <label htmlFor="firstName">First Name *</label>
                        <FormInput
                          id="firstName"
                          name="firstName"
                          value={profileData.firstName}
                          onChange={handleProfileChange}
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
                          value={profileData.lastName}
                          onChange={handleProfileChange}
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
                          value={profileData.username}
                          onChange={handleProfileChange}
                          required
                          disabled
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
                          value={profileData.email}
                          onChange={handleProfileChange}
                          required
                        />
                      </div>
                    </Col>
                  </Row>

                  <Row>
                    <Col md="6">
                      <div className="mb-3">
                        <label htmlFor="phoneNumber">Phone Number</label>
                        <FormInput
                          id="phoneNumber"
                          name="phoneNumber"
                          value={profileData.phoneNumber}
                          onChange={handleProfileChange}
                        />
                      </div>
                    </Col>
                    <Col md="6">
                      <div className="mb-3">
                        <label htmlFor="position">Position</label>
                        <FormInput
                          id="position"
                          name="position"
                          value={profileData.position}
                          onChange={handleProfileChange}
                          disabled
                        />
                      </div>
                    </Col>
                  </Row>

                  <div className="mb-3">
                    <label htmlFor="bio">Bio</label>
                    <textarea
                      className="form-control"
                      id="bio"
                      name="bio"
                      rows="3"
                      value={profileData.bio}
                      onChange={handleProfileChange}
                      placeholder="Tell us about yourself..."
                    />
                  </div>

                  <Button type="submit" theme="primary" disabled={loading}>
                    {loading ? (
                      <>
                        <i className="fas fa-spinner fa-spin mr-2"></i>
                        Updating...
                      </>
                    ) : (
                      <>
                        <i className="fas fa-save mr-2"></i>
                        Update Profile
                      </>
                    )}
                  </Button>
                </Form>
              )}

              {/* Password Tab */}
              {activeTab === 'password' && (
                <Form onSubmit={handlePasswordSubmit}>
                  <div className="mb-3">
                    <label htmlFor="currentPassword">Current Password *</label>
                    <FormInput
                      id="currentPassword"
                      name="currentPassword"
                      type="password"
                      value={passwordData.currentPassword}
                      onChange={handlePasswordChange}
                      required
                    />
                  </div>

                  <div className="mb-3">
                    <label htmlFor="newPassword">New Password *</label>
                    <FormInput
                      id="newPassword"
                      name="newPassword"
                      type="password"
                      value={passwordData.newPassword}
                      onChange={handlePasswordChange}
                      required
                    />
                    <small className="text-muted">Password must be at least 6 characters long</small>
                  </div>

                  <div className="mb-3">
                    <label htmlFor="confirmPassword">Confirm New Password *</label>
                    <FormInput
                      id="confirmPassword"
                      name="confirmPassword"
                      type="password"
                      value={passwordData.confirmPassword}
                      onChange={handlePasswordChange}
                      required
                    />
                  </div>

                  <Button type="submit" theme="primary" disabled={loading}>
                    {loading ? (
                      <>
                        <i className="fas fa-spinner fa-spin mr-2"></i>
                        Changing...
                      </>
                    ) : (
                      <>
                        <i className="fas fa-key mr-2"></i>
                        Change Password
                      </>
                    )}
                  </Button>
                </Form>
              )}

              {/* Preferences Tab */}
              {activeTab === 'preferences' && (
                <Form onSubmit={handlePreferencesSubmit}>
                  <Row>
                    <Col md="6">
                      <div className="mb-3">
                        <label htmlFor="language">Language</label>
                        <select
                          className="form-control"
                          id="language"
                          name="language"
                          value={preferencesData.language}
                          onChange={handlePreferencesChange}
                        >
                          <option value="en">English</option>
                          <option value="id">Bahasa Indonesia</option>
                          <option value="es">Español</option>
                          <option value="fr">Français</option>
                        </select>
                      </div>
                    </Col>
                    <Col md="6">
                      <div className="mb-3">
                        <label htmlFor="timezone">Timezone</label>
                        <select
                          className="form-control"
                          id="timezone"
                          name="timezone"
                          value={preferencesData.timezone}
                          onChange={handlePreferencesChange}
                        >
                          <option value="UTC">UTC</option>
                          <option value="Asia/Jakarta">Asia/Jakarta</option>
                          <option value="America/New_York">America/New_York</option>
                          <option value="Europe/London">Europe/London</option>
                        </select>
                      </div>
                    </Col>
                  </Row>

                  <Row>
                    <Col md="6">
                      <div className="mb-3">
                        <label htmlFor="dateFormat">Date Format</label>
                        <select
                          className="form-control"
                          id="dateFormat"
                          name="dateFormat"
                          value={preferencesData.dateFormat}
                          onChange={handlePreferencesChange}
                        >
                          <option value="DD/MM/YYYY">DD/MM/YYYY</option>
                          <option value="MM/DD/YYYY">MM/DD/YYYY</option>
                          <option value="YYYY-MM-DD">YYYY-MM-DD</option>
                        </select>
                      </div>
                    </Col>
                    <Col md="6">
                      <div className="mb-3">
                        <label htmlFor="theme">Theme</label>
                        <select
                          className="form-control"
                          id="theme"
                          name="theme"
                          value={preferencesData.theme}
                          onChange={handlePreferencesChange}
                        >
                          <option value="light">Light</option>
                          <option value="dark">Dark</option>
                          <option value="auto">Auto</option>
                        </select>
                      </div>
                    </Col>
                  </Row>

                  <h6 className="mb-3">Notifications</h6>
                  
                  <div className="mb-3">
                    <div className="custom-control custom-checkbox">
                      <input
                        type="checkbox"
                        className="custom-control-input"
                        id="emailNotifications"
                        name="emailNotifications"
                        checked={preferencesData.emailNotifications}
                        onChange={handlePreferencesChange}
                      />
                      <label className="custom-control-label" htmlFor="emailNotifications">
                        Email Notifications
                      </label>
                    </div>
                  </div>

                  <div className="mb-3">
                    <div className="custom-control custom-checkbox">
                      <input
                        type="checkbox"
                        className="custom-control-input"
                        id="pushNotifications"
                        name="pushNotifications"
                        checked={preferencesData.pushNotifications}
                        onChange={handlePreferencesChange}
                      />
                      <label className="custom-control-label" htmlFor="pushNotifications">
                        Push Notifications
                      </label>
                    </div>
                  </div>

                  <div className="mb-3">
                    <div className="custom-control custom-checkbox">
                      <input
                        type="checkbox"
                        className="custom-control-input"
                        id="weeklyReports"
                        name="weeklyReports"
                        checked={preferencesData.weeklyReports}
                        onChange={handlePreferencesChange}
                      />
                      <label className="custom-control-label" htmlFor="weeklyReports">
                        Weekly Reports
                      </label>
                    </div>
                  </div>

                  <Button type="submit" theme="primary" disabled={loading}>
                    {loading ? (
                      <>
                        <i className="fas fa-spinner fa-spin mr-2"></i>
                        Saving...
                      </>
                    ) : (
                      <>
                        <i className="fas fa-save mr-2"></i>
                        Save Preferences
                      </>
                    )}
                  </Button>
                </Form>
              )}
            </CardBody>
          </Card>
        </Col>
      </Row>
    </Container>
  );
};

export default UserProfile;
