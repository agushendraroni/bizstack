import React from "react";
import { Redirect } from "react-router-dom";

// Layout Types
import { DefaultLayout } from "./layouts";

// Import Components
import UserManagement from "./pages/users/UserManagement";
import RoleManagement from "./pages/roles/RoleManagement";
import PositionManagement from "./pages/organizations/PositionManagement";
import DepartmentManagement from "./pages/organizations/DepartmentManagement";
import UserProfile from "./pages/profile/UserProfile";
import MenuTreeManagement from "./pages/settings/MenuTreeManagement";
import CompanyManagement from "./pages/settings/CompanyManagement";
import CompanySetup from "./pages/setup/CompanySetup";
import LoginView from "./views/Login";
import CompanySelector from "./pages/auth/CompanySelector";
import CompanyProfile from "./pages/profile/CompanyProfile";
// import BlogOverview from "./views/BlogOverview"; // Removed
import ProductManagement from "./pages/products/ProductManagement";
import CustomerManagement from "./pages/customers/CustomerManagement";
import OrderManagement from "./pages/orders/OrderManagement";
import BusinessDashboard from "./pages/dashboard/BusinessDashboard";

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

    {/* System Info & Logout */}
    <div className="row">
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
                  window.location.href = '/company';
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
  // Company selector
  {
    path: "/company",
    exact: true,
    layout: ({ children }) => <div>{children}</div>,
    component: CompanySelector
  },
  
  // Multi-tenant company routes
  {
    path: "/:companyCode/login",
    layout: ({ children }) => <div>{children}</div>,
    component: LoginView
  },
  {
    path: "/:companyCode/dashboard",
    layout: DefaultLayout,
    component: BusinessDashboard
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
    path: "/:companyCode/company-settings",
    layout: DefaultLayout,
    component: CompanyManagement
  },
  {
    path: "/:companyCode/menu-tree",
    layout: DefaultLayout,
    component: MenuTreeManagement
  },
  {
    path: "/:companyCode/company-profile",
    layout: DefaultLayout,
    component: CompanyProfile
  },
  {
    path: "/:companyCode/products",
    layout: DefaultLayout,
    component: ProductManagement
  },
  {
    path: "/:companyCode/customers",
    layout: DefaultLayout,
    component: CustomerManagement
  },
  {
    path: "/:companyCode/orders",
    layout: DefaultLayout,
    component: OrderManagement
  },
  
  // Global routes (if authenticated)
  {
    path: "/dashboard",
    layout: DefaultLayout,
    component: () => {
      const isAuth = localStorage.getItem('authToken');
      return isAuth ? <SimpleDashboard /> : <Redirect to="/company" />;
    }
  },
  
  // Root redirect
  {
    path: "/",
    exact: true,
    component: () => <Redirect to="/company" />
  },
  
  // Catch any other routes and redirect to company
  {
    path: "*",
    component: () => <Redirect to="/company" />
  }
];