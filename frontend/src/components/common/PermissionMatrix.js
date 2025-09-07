import React from "react";
import PropTypes from "prop-types";

const PermissionMatrix = ({ 
  modules, 
  permissions, 
  selectedPermissions, 
  onPermissionChange,
  readOnly = false 
}) => {
  
  const handlePermissionToggle = (moduleId, permission) => {
    const permissionKey = permission || moduleId;
    const newPermissions = { ...selectedPermissions };
    
    if (newPermissions[permissionKey]) {
      delete newPermissions[permissionKey];
    } else {
      newPermissions[permissionKey] = true;
    }
    
    onPermissionChange(newPermissions);
  };

  const handleSelectAll = () => {
    const newPermissions = { ...selectedPermissions };
    const allSelected = modules.every(module => 
      selectedPermissions[module.permission || module.id]
    );
    
    if (allSelected) {
      // Uncheck all
      modules.forEach(module => {
        delete newPermissions[module.permission || module.id];
      });
    } else {
      // Check all
      modules.forEach(module => {
        newPermissions[module.permission || module.id] = true;
      });
    }
    
    onPermissionChange(newPermissions);
  };



  const isPermissionSelected = (module) => {
    return selectedPermissions[module.permission || module.id] || false;
  };

  const isAllSelected = () => {
    return modules.every(module => isPermissionSelected(module));
  };

  const getSelectedCount = () => {
    return modules.filter(module => isPermissionSelected(module)).length;
  };

  const getPermissionsByLevel = () => {
    const levels = {};
    modules.forEach(module => {
      const level = module.level || 1;
      if (!levels[level]) levels[level] = [];
      levels[level].push(module);
    });
    return levels;
  };

  return (
    <div className="permission-matrix">
      <div className="table-responsive">
        <table className="table table-bordered">
          <thead className="thead-light">
            <tr>
              <th style={{ width: '60%' }}>
                <div className="d-flex align-items-center justify-content-between">
                  <div>
                    Menu Permission
                    <div className="mt-1">
                      <small className="text-muted">Active menu items from system</small>
                    </div>
                  </div>
                  {!readOnly && (
                    <div className="custom-control custom-checkbox">
                      <input
                        type="checkbox"
                        className="custom-control-input"
                        id="select_all"
                        checked={isAllSelected()}
                        onChange={handleSelectAll}
                      />
                      <label className="custom-control-label" htmlFor="select_all">
                        <small>Select All</small>
                      </label>
                    </div>
                  )}
                </div>
              </th>
              <th className="text-center" style={{ width: '20%' }}>
                <div className="d-flex align-items-center justify-content-center">
                  <i className="fas fa-key text-primary mr-2"></i>
                  <span>Access</span>
                </div>
                <div className="mt-1">
                  <small className="text-muted">{getSelectedCount()}/{modules.length}</small>
                </div>
              </th>
              <th className="text-center" style={{ width: '20%' }}>
                Level
                <div className="mt-1">
                  <small className="text-muted">Menu Level</small>
                </div>
              </th>
            </tr>
          </thead>
          <tbody>
            {modules.map(module => (
              <tr key={module.id}>
                <td>
                  <div className="d-flex align-items-center">
                    <div style={{ marginLeft: `${(module.level - 1) * 20}px` }}>
                      <i className={`fas ${module.icon} mr-2 text-primary`}></i>
                    </div>
                    <div className="flex-grow-1">
                      <strong>{module.name}</strong>
                      {module.description && (
                        <div><small className="text-muted">{module.description}</small></div>
                      )}
                      {module.permission && (
                        <div><small className="text-info">Permission: {module.permission}</small></div>
                      )}
                      {module.route && (
                        <div><small className="text-secondary">Route: {module.route}</small></div>
                      )}
                    </div>
                  </div>
                </td>
                
                {/* Access Permission */}
                <td className="text-center">
                  <div className="custom-control custom-checkbox">
                    <input
                      type="checkbox"
                      className="custom-control-input"
                      id={`${module.id}_access`}
                      checked={isPermissionSelected(module)}
                      onChange={() => handlePermissionToggle(module.id, module.permission)}
                      disabled={readOnly}
                    />
                    <label 
                      className="custom-control-label" 
                      htmlFor={`${module.id}_access`}
                    ></label>
                  </div>
                </td>
                
                {/* Level */}
                <td className="text-center">
                  <span className={`badge ${
                    module.level === 1 ? 'badge-primary' :
                    module.level === 2 ? 'badge-info' :
                    module.level === 3 ? 'badge-warning' :
                    'badge-secondary'
                  }`}>
                    Level {module.level || 1}
                  </span>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      
      {/* Permission Summary */}
      <div className="mt-3">
        <div className="row">
          <div className="col-md-4">
            <div className="text-center">
              <i className="fas fa-key text-primary fa-2x"></i>
              <div className="mt-2">
                <strong>Selected Permissions</strong>
                <div className="text-muted">
                  {getSelectedCount()} of {modules.length} menus
                </div>
              </div>
            </div>
          </div>
          <div className="col-md-4">
            <div className="text-center">
              <i className="fas fa-sitemap text-info fa-2x"></i>
              <div className="mt-2">
                <strong>Menu Levels</strong>
                <div className="text-muted">
                  {Object.keys(getPermissionsByLevel()).length} levels available
                </div>
              </div>
            </div>
          </div>
          <div className="col-md-4">
            <div className="text-center">
              <i className="fas fa-percentage text-success fa-2x"></i>
              <div className="mt-2">
                <strong>Coverage</strong>
                <div className="text-muted">
                  {modules.length > 0 ? Math.round((getSelectedCount() / modules.length) * 100) : 0}% of available menus
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
      
      {/* Quick Actions */}
      {!readOnly && (
        <div className="mt-3 p-3 bg-light rounded">
          <h6 className="mb-2">
            <i className="fas fa-magic mr-2"></i>
            Quick Actions
          </h6>
          <div className="d-flex flex-wrap">
            <button 
              type="button"
              className="btn btn-sm btn-outline-success mr-2 mb-2"
              onClick={() => {
                const newPermissions = {};
                modules.forEach(module => {
                  newPermissions[module.permission || module.id] = true;
                });
                onPermissionChange(newPermissions);
              }}
            >
              <i className="fas fa-check-double mr-1"></i>
              Select All
            </button>
            <button 
              type="button"
              className="btn btn-sm btn-outline-secondary mr-2 mb-2"
              onClick={() => onPermissionChange({})}
            >
              <i className="fas fa-times mr-1"></i>
              Clear All
            </button>
            <button 
              type="button"
              className="btn btn-sm btn-outline-info mr-2 mb-2"
              onClick={() => {
                const newPermissions = {};
                modules.filter(m => m.level === 1).forEach(module => {
                  newPermissions[module.permission || module.id] = true;
                });
                onPermissionChange(newPermissions);
              }}
            >
              <i className="fas fa-layer-group mr-1"></i>
              Level 1 Only
            </button>
            <button 
              type="button"
              className="btn btn-sm btn-outline-warning mr-2 mb-2"
              onClick={() => {
                const newPermissions = {};
                modules.filter(m => m.level <= 2).forEach(module => {
                  newPermissions[module.permission || module.id] = true;
                });
                onPermissionChange(newPermissions);
              }}
            >
              <i className="fas fa-layers mr-1"></i>
              Level 1 & 2
            </button>
          </div>
        </div>
      )}
    </div>
  );
};

PermissionMatrix.propTypes = {
  modules: PropTypes.arrayOf(PropTypes.shape({
    id: PropTypes.string.isRequired,
    name: PropTypes.string.isRequired,
    description: PropTypes.string,
    icon: PropTypes.string,
    permission: PropTypes.string,
    route: PropTypes.string,
    level: PropTypes.number
  })).isRequired,
  permissions: PropTypes.array,
  selectedPermissions: PropTypes.object.isRequired,
  onPermissionChange: PropTypes.func.isRequired,
  readOnly: PropTypes.bool
};

export default PermissionMatrix;
