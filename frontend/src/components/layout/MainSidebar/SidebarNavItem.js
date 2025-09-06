import React, { useState } from "react";
import PropTypes from "prop-types";
import { NavLink as RouteNavLink } from "react-router-dom";
import { NavItem, NavLink, Collapse } from "shards-react";

const SidebarNavItem = ({ item }) => {
  const [isExpanded, setIsExpanded] = useState(false);

  const toggleExpanded = (e) => {
    if (item.items && item.items.length > 0) {
      e.preventDefault();
      setIsExpanded(!isExpanded);
    }
  };

  // If item has sub-items, render expandable menu
  if (item.items && item.items.length > 0) {
    return (
      <>
        <NavItem>
          <NavLink 
            href="#" 
            onClick={toggleExpanded}
            className={`d-flex align-items-center ${isExpanded ? 'active' : ''}`}
          >
            {item.htmlBefore && (
              <div
                className="d-inline-block item-icon-wrapper"
                dangerouslySetInnerHTML={{ __html: item.htmlBefore }}
              />
            )}
            {item.title && <span className="flex-grow-1">{item.title}</span>}
            <i className={`fas fa-angle-${isExpanded ? 'down' : 'right'} ml-auto`}></i>
          </NavLink>
        </NavItem>
        
        <Collapse open={isExpanded}>
          <div className="ml-3">
            {item.items.map((subItem, idx) => (
              <NavItem key={idx}>
                <NavLink tag={RouteNavLink} to={subItem.to} className="pl-4">
                  {subItem.htmlBefore && (
                    <div
                      className="d-inline-block item-icon-wrapper"
                      dangerouslySetInnerHTML={{ __html: subItem.htmlBefore }}
                    />
                  )}
                  {subItem.title && <span>{subItem.title}</span>}
                  {subItem.htmlAfter && (
                    <div
                      className="d-inline-block item-icon-wrapper"
                      dangerouslySetInnerHTML={{ __html: subItem.htmlAfter }}
                    />
                  )}
                </NavLink>
              </NavItem>
            ))}
          </div>
        </Collapse>
      </>
    );
  }

  // Regular menu item without sub-items
  return (
    <NavItem>
      <NavLink tag={RouteNavLink} to={item.to}>
        {item.htmlBefore && (
          <div
            className="d-inline-block item-icon-wrapper"
            dangerouslySetInnerHTML={{ __html: item.htmlBefore }}
          />
        )}
        {item.title && <span>{item.title}</span>}
        {item.htmlAfter && (
          <div
            className="d-inline-block item-icon-wrapper"
            dangerouslySetInnerHTML={{ __html: item.htmlAfter }}
          />
        )}
      </NavLink>
    </NavItem>
  );
};

SidebarNavItem.propTypes = {
  /**
   * The item object.
   */
  item: PropTypes.object
};

export default SidebarNavItem;
