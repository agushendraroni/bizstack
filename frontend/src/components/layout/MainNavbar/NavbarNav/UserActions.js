import React from "react";
import { Link } from "react-router-dom";
import {
  Dropdown,
  DropdownToggle,
  DropdownMenu,
  DropdownItem,
  Collapse,
  NavItem,
  NavLink
} from "shards-react";

export default class UserActions extends React.Component {
  constructor(props) {
    super(props);

    this.state = {
      visible: false
    };

    this.toggleUserActions = this.toggleUserActions.bind(this);
    this.handleLogout = this.handleLogout.bind(this);
  }

  toggleUserActions() {
    this.setState({
      visible: !this.state.visible
    });
  }

  handleLogout() {
    // Clear authentication
    localStorage.removeItem('authToken');
    localStorage.removeItem('user');
    
    // Redirect to login
    window.location.href = '/login';
  }

  render() {
    // Get user from localStorage or use default BLITZ admin
    const user = JSON.parse(localStorage.getItem('user') || '{}');
    const username = user.username || 'admin';
    const displayName = user.firstName ? `${user.firstName} ${user.lastName}` : 'System Administrator';

    return (
      <NavItem tag={Dropdown} caret toggle={this.toggleUserActions}>
        <DropdownToggle caret tag={NavLink} className="text-nowrap px-3">
          <img
            className="user-avatar rounded-circle mr-2"
            src={require("./../../../../images/avatars/0.jpg")}
            alt="User Avatar"
          />{" "}
          <span className="d-none d-md-inline-block">{displayName}</span>
        </DropdownToggle>
        <Collapse tag={DropdownMenu} right small open={this.state.visible}>
          <DropdownItem tag={Link} to="/profile">
            <i className="fas fa-user mr-2"></i> My Profile
          </DropdownItem>
          <DropdownItem tag={Link} to="/profile">
            <i className="fas fa-cog mr-2"></i> Account Settings
          </DropdownItem>
          <DropdownItem tag={Link} to="/profile">
            <i className="fas fa-key mr-2"></i> Change Password
          </DropdownItem>
          <DropdownItem divider />
          <DropdownItem onClick={this.handleLogout} className="text-danger" style={{cursor: 'pointer'}}>
            <i className="fas fa-sign-out-alt text-danger mr-2"></i> Logout
          </DropdownItem>
        </Collapse>
      </NavItem>
    );
  }
}
