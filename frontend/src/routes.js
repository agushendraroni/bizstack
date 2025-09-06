import React from "react";
import { Redirect } from "react-router-dom";

// Layout Types
import { DefaultLayout } from "./layouts";

// Import User Management
import UserManagement from "./pages/users/UserManagement";
import RoleManagement from "./pages/roles/RoleManagement";
import PositionManagement from "./pages/organizations/PositionManagement";
import DepartmentManagement from "./pages/organizations/DepartmentManagement";
import UserProfile from "./pages/profile/UserProfile";
import MenuTreeManagement from "./pages/settings/MenuTreeManagement";
import CompanyManagement from "./pages/settings/CompanyManagement";
import CompanySetup from "./pages/setup/CompanySetup";
import BlogOverview from "./views/BlogOverview";
import UserProfileLite from "./views/UserProfileLite";
import AddNewPost from "./views/AddNewPost";
import Errors from "./views/Errors";
import ComponentsOverview from "./views/ComponentsOverview";
import Login from "./views/Login";
import Tables from "./views/Tables";
import BlogPosts from "./views/BlogPosts";

// Simple Login Component
const SimpleLogin = () => (
  <div className="container mt-5">
    <div className="row justify-content-center">
      <div className="col-md-6">
        <div className="card">
          <div className="card-header">
            <h3>BizStack Login</h3>
          </div>
          <div className="card-body">
            <form>
              <div className="mb-3">
                <label>Username</label>
                <input type="text" className="form-control" defaultValue="admin" />
              </div>
              <div className="mb-3">
                <label>Password</label>
                <input type="password" className="form-control" defaultValue="password123" />
              </div>
              <button type="button" className="btn btn-primary" onClick={() => {
                localStorage.setItem('authToken', 'test-token');
                localStorage.setItem('user', JSON.stringify({username: 'admin'}));
                window.location.href = '/dashboard';
              }}>
                Login
              </button>
            </form>
          </div>
        </div>
      </div>
    </div>
  </div>
);

// Simple User Management
const SimpleUserManagement = () => (
  <div className="container-fluid main-content-container px-4">
    <div className="row no-gutters page-header py-4">
      <div className="col-12 col-sm-4 text-center text-sm-left mb-0">
        <span className="text-uppercase page-subtitle">Management</span>
        <h3 className="page-title">User Management</h3>
      </div>
    </div>
    <div className="row">
      <div className="col-12">
        <div className="card card-small mb-4">
          <div className="card-header border-bottom">
            <h6 className="m-0">System Users</h6>
          </div>
          <div className="card-body p-0">
            <table className="table mb-0">
              <thead className="bg-light">
                <tr>
                  <th>Name</th>
                  <th>Email</th>
                  <th>Role</th>
                  <th>Status</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>Admin User</td>
                  <td>admin@bizstack.com</td>
                  <td><span className="badge badge-primary">Administrator</span></td>
                  <td><span className="badge badge-success">Active</span></td>
                </tr>
                <tr>
                  <td>Demo User</td>
                  <td>demo@bizstack.com</td>
                  <td><span className="badge badge-secondary">User</span></td>
                  <td><span className="badge badge-success">Active</span></td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>
);

// Simple Organization Management
const SimpleOrganizationManagement = () => (
  <div className="container-fluid main-content-container px-4">
    <div className="row no-gutters page-header py-4">
      <div className="col-12 col-sm-4 text-center text-sm-left mb-0">
        <span className="text-uppercase page-subtitle">Management</span>
        <h3 className="page-title">Organizations</h3>
      </div>
    </div>
    <div className="row">
      <div className="col-12">
        <div className="card card-small mb-4">
          <div className="card-header border-bottom">
            <h6 className="m-0">Companies</h6>
          </div>
          <div className="card-body p-0">
            <table className="table mb-0">
              <thead className="bg-light">
                <tr>
                  <th>Company Name</th>
                  <th>Industry</th>
                  <th>Employees</th>
                  <th>Status</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>BizStack Corp</td>
                  <td>Technology</td>
                  <td>50+</td>
                  <td><span className="badge badge-success">Active</span></td>
                </tr>
                <tr>
                  <td>Demo Company</td>
                  <td>Services</td>
                  <td>10-50</td>
                  <td><span className="badge badge-success">Active</span></td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>
    </div>
  </div>
);
const SimpleDashboard = () => (
  <div className="container-fluid main-content-container px-4">
    {/* Page Header */}
    <div className="row no-gutters page-header py-4">
      <div className="col-12 col-sm-4 text-center text-sm-left mb-0">
        <span className="text-uppercase page-subtitle">Dashboard</span>
        <h3 className="page-title">Business Overview</h3>
      </div>
    </div>

    {/* Stats Cards */}
    <div className="row">
      <div className="col-lg-3 col-md-6 col-sm-12 mb-4">
        <div className="stats-small stats-small--1 card card-small">
          <div className="card-body p-0 d-flex">
            <div className="d-flex flex-column m-auto">
              <div className="stats-small__data text-center">
                <span className="stats-small__label text-uppercase">Total Users</span>
                <h6 className="stats-small__value count my-3">0</h6>
              </div>
            </div>
          </div>
        </div>
      </div>
      
      <div className="col-lg-3 col-md-6 col-sm-12 mb-4">
        <div className="stats-small stats-small--1 card card-small">
          <div className="card-body p-0 d-flex">
            <div className="d-flex flex-column m-auto">
              <div className="stats-small__data text-center">
                <span className="stats-small__label text-uppercase">Companies</span>
                <h6 className="stats-small__value count my-3">0</h6>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="col-lg-3 col-md-6 col-sm-12 mb-4">
        <div className="stats-small stats-small--1 card card-small">
          <div className="card-body p-0 d-flex">
            <div className="d-flex flex-column m-auto">
              <div className="stats-small__data text-center">
                <span className="stats-small__label text-uppercase">Services</span>
                <h6 className="stats-small__value count my-3">9</h6>
              </div>
            </div>
          </div>
        </div>
      </div>

      <div className="col-lg-3 col-md-6 col-sm-12 mb-4">
        <div className="stats-small stats-small--1 card card-small">
          <div className="card-body p-0 d-flex">
            <div className="d-flex flex-column m-auto">
              <div className="stats-small__data text-center">
                <span className="stats-small__label text-uppercase">Status</span>
                <h6 className="stats-small__value count my-3 text-success">Online</h6>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>

    {/* Quick Actions */}
    <div className="row">
      <div className="col-lg-8 col-md-12 col-sm-12 mb-4">
        <div className="card card-small">
          <div className="card-header border-bottom">
            <h6 className="m-0">Quick Actions</h6>
          </div>
          <div className="card-body p-0">
            <table className="table mb-0">
              <thead className="bg-light">
                <tr>
                  <th scope="col" className="border-0">Module</th>
                  <th scope="col" className="border-0">Status</th>
                  <th scope="col" className="border-0">Action</th>
                </tr>
              </thead>
              <tbody>
                <tr>
                  <td>User Management</td>
                  <td><span className="badge badge-success">Active</span></td>
                  <td><a href="/user-profile-lite" className="btn btn-sm btn-primary">Manage</a></td>
                </tr>
                <tr>
                  <td>Blog Posts</td>
                  <td><span className="badge badge-success">Active</span></td>
                  <td><a href="/blog-posts" className="btn btn-sm btn-primary">Manage</a></td>
                </tr>
                <tr>
                  <td>Components</td>
                  <td><span className="badge badge-success">Active</span></td>
                  <td><a href="/components-overview" className="btn btn-sm btn-primary">View</a></td>
                </tr>
                <tr>
                  <td>Data Tables</td>
                  <td><span className="badge badge-success">Active</span></td>
                  <td><a href="/tables" className="btn btn-sm btn-primary">View</a></td>
                </tr>
              </tbody>
            </table>
          </div>
        </div>
      </div>

    {/* System Info & Logout */}
      <div className="col-lg-4 col-md-12 col-sm-12 mb-4">
        <div className="card card-small">
          <div className="card-header border-bottom">
            <h6 className="m-0">System Info</h6>
          </div>
          <div className="card-body">
            <ul className="list-group list-group-flush">
              <li className="list-group-item px-0">
                <strong>Backend Services:</strong> 9/9 Online
              </li>
              <li className="list-group-item px-0">
                <strong>Database:</strong> PostgreSQL
              </li>
              <li className="list-group-item px-0">
                <strong>Frontend:</strong> React + Shards
              </li>
              <li className="list-group-item px-0">
                <strong>Authentication:</strong> JWT
              </li>
            </ul>
            <div className="mt-3">
              <button 
                className="btn btn-danger btn-sm btn-block" 
                onClick={() => {
                  localStorage.clear();
                  window.location.href = '/login';
                }}
              >
                <i className="fas fa-sign-out-alt mr-2"></i>
                Logout
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
);

export default [
  // Auth Routes
  {
    path: "/login",
    exact: true,
    layout: ({ children }) => <div>{children}</div>,
    component: SimpleLogin
  },
  // Business Routes
  {
    path: "/dashboard",
    layout: DefaultLayout,
    component: SimpleDashboard
  },
  {
    path: "/users",
    layout: DefaultLayout,
    component: UserManagement
  },
  {
    path: "/organizations",
    layout: DefaultLayout,
    component: SimpleOrganizationManagement
  },
  {
    path: "/positions",
    layout: DefaultLayout,
    component: PositionManagement
  },
  {
    path: "/departments",
    layout: DefaultLayout,
    component: DepartmentManagement
  },
  {
    path: "/profile",
    layout: DefaultLayout,
    component: UserProfile
  },
  {
    path: "/menu-tree",
    layout: DefaultLayout,
    component: MenuTreeManagement
  },
  {
    path: "/company",
    layout: DefaultLayout,
    component: CompanyManagement
  },
  {
    path: "/setup",
    layout: DefaultLayout,
    component: CompanySetup
  },
  
  // Multi-tenant company routes
  {
    path: "/:companyCode/login",
    layout: DefaultLayout,
    component: Login
  },
  {
    path: "/:companyCode/dashboard",
    layout: DefaultLayout,
    component: BlogOverview
  },
  {
    path: "/:companyCode/users",
    layout: DefaultLayout,
    component: UserManagement
  },
  {
    path: "/:companyCode/roles",
    layout: DefaultLayout,
    component: RoleManagement
  },
  {
    path: "/:companyCode/organizations",
    layout: DefaultLayout,
    component: SimpleOrganizationManagement
  },
  {
    path: "/:companyCode/positions",
    layout: DefaultLayout,
    component: PositionManagement
  },
  {
    path: "/:companyCode/departments",
    layout: DefaultLayout,
    component: DepartmentManagement
  },
  {
    path: "/:companyCode/profile",
    layout: DefaultLayout,
    component: UserProfile
  },
  {
    path: "/:companyCode/company",
    layout: DefaultLayout,
    component: CompanyManagement
  },
  {
    path: "/:companyCode/menu-tree",
    layout: DefaultLayout,
    component: MenuTreeManagement
  },
  {
    path: "/roles",
    layout: DefaultLayout,
    component: RoleManagement
  },
  
  // Root redirect
  {
    path: "/",
    exact: true,
    layout: DefaultLayout,
    component: () => {
      const isAuth = localStorage.getItem('authToken');
      return isAuth ? <Redirect to="/dashboard" /> : <Redirect to="/login" />;
    }
  },

  // Legacy routes
  {
    path: "/blog-overview",
    layout: DefaultLayout,
    component: BlogOverview
  },
  {
    path: "/user-profile-lite",
    layout: DefaultLayout,
    component: UserProfileLite
  },
  {
    path: "/add-new-post",
    layout: DefaultLayout,
    component: AddNewPost
  },
  {
    path: "/errors",
    layout: DefaultLayout,
    component: Errors
  },
  {
    path: "/components-overview",
    layout: DefaultLayout,
    component: ComponentsOverview
  },
  {
    path: "/tables",
    layout: DefaultLayout,
    component: Tables
  },
  {
    path: "/blog-posts",
    layout: DefaultLayout,
    component: BlogPosts
  }
];
