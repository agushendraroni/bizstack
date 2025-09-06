import React from "react";
import PropTypes from "prop-types";

const PermissionMatrix = ({ 
  modules, 
  permissions, 
  selectedPermissions, 
  onPermissionChange,
  readOnly = false 
}) => {
  
  const handlePermissionToggle = (moduleId, action) => {
    const permissionKey = `${moduleId}_${action}`;
    const newPermissions = { ...selectedPermissions };
    
    if (newPermissions[permissionKey]) {
      delete newPermissions[permissionKey];
    } else {
      newPermissions[permissionKey] = true;
    }
    
    onPermissionChange(newPermissions);
  };

  const handleColumnToggle = (action) => {
    const newPermissions = { ...selectedPermissions };
    const allSelected = modules.every(module => 
      selectedPermissions[`${module.id}_${action}`]
    );
    
    if (allSelected) {
      // Uncheck all in this column
      modules.forEach(module => {
        delete newPermissions[`${module.id}_${action}`];
      });
    } else {
      // Check all in this column
      modules.forEach(module => {
        newPermissions[`${module.id}_${action}`] = true;
      });
    }
    
    onPermissionChange(newPermissions);
  };

  const handleRowToggle = (moduleId) => {
    const newPermissions = { ...selectedPermissions };
    const actions = ['read', 'write', 'delete'];
    const allSelected = actions.every(action => 
      selectedPermissions[`${moduleId}_${action}`]
    );
    
    if (allSelected) {
      // Uncheck all in this row
      actions.forEach(action => {
        delete newPermissions[`${moduleId}_${action}`];
      });
    } else {
      // Check all in this row
      actions.forEach(action => {
        newPermissions[`${moduleId}_${action}`] = true;
      });
    }
    
    onPermissionChange(newPermissions);
  };

  const isPermissionSelected = (moduleId, action) => {
    return selectedPermissions[`${moduleId}_${action}`] || false;
  };

  const isColumnAllSelected = (action) => {
    return modules.every(module => isPermissionSelected(module.id, action));
  };

  const isRowAllSelected = (moduleId) => {
    const actions = ['read', 'write', 'delete'];
    return actions.every(action => isPermissionSelected(moduleId, action));
  };

  const getModulePermissionCount = (moduleId) => {
    const actions = ['read', 'write', 'delete'];
    return actions.filter(action => isPermissionSelected(moduleId, action)).length;
  };

  const getColumnCount = (action) => {
    return modules.filter(module => isPermissionSelected(module.id, action)).length;
  };

  return (
    <div className="permission-matrix">
      <div className="table-responsive">
        <table className="table table-bordered">
          <thead className="thead-light">
            <tr>
              <th style={{ width: '25%' }}>
                Module
                <div className="mt-1">
                  <small className="text-muted">Click module name to toggle row</small>
                </div>
              </th>
              <th className="text-center" style={{ width: '20%' }}>
                <div className="d-flex align-items-center justify-content-center">
                  <i className="fas fa-eye text-info mr-2"></i>
                  <span>Read</span>
                  {!readOnly && (
                    <div className="ml-2">
                      <div className="custom-control custom-checkbox">
                        <input
                          type="checkbox"
                          className="custom-control-input"
                          id="read_all"
                          checked={isColumnAllSelected('read')}
                          onChange={() => handleColumnToggle('read')}
                        />
                        <label className="custom-control-label" htmlFor="read_all">
                          <small>All</small>
                        </label>
                      </div>
                    </div>
                  )}
                </div>
                <div className="mt-1">
                  <small className="text-muted">{getColumnCount('read')}/{modules.length}</small>
                </div>
              </th>
              <th className="text-center" style={{ width: '20%' }}>
                <div className="d-flex align-items-center justify-content-center">
                  <i className="fas fa-edit text-warning mr-2"></i>
                  <span>Write</span>
                  {!readOnly && (
                    <div className="ml-2">
                      <div className="custom-control custom-checkbox">
                        <input
                          type="checkbox"
                          className="custom-control-input"
                          id="write_all"
                          checked={isColumnAllSelected('write')}
                          onChange={() => handleColumnToggle('write')}
                        />
                        <label className="custom-control-label" htmlFor="write_all">
                          <small>All</small>
                        </label>
                      </div>
                    </div>
                  )}
                </div>
                <div className="mt-1">
                  <small className="text-muted">{getColumnCount('write')}/{modules.length}</small>
                </div>
              </th>
              <th className="text-center" style={{ width: '20%' }}>
                <div className="d-flex align-items-center justify-content-center">
                  <i className="fas fa-trash text-danger mr-2"></i>
                  <span>Delete</span>
                  {!readOnly && (
                    <div className="ml-2">
                      <div className="custom-control custom-checkbox">
                        <input
                          type="checkbox"
                          className="custom-control-input"
                          id="delete_all"
                          checked={isColumnAllSelected('delete')}
                          onChange={() => handleColumnToggle('delete')}
                        />
                        <label className="custom-control-label" htmlFor="delete_all">
                          <small>All</small>
                        </label>
                      </div>
                    </div>
                  )}
                </div>
                <div className="mt-1">
                  <small className="text-muted">{getColumnCount('delete')}/{modules.length}</small>
                </div>
              </th>
              <th className="text-center" style={{ width: '15%' }}>
                Count
                <div className="mt-1">
                  <small className="text-muted">Per Module</small>
                </div>
              </th>
            </tr>
          </thead>
          <tbody>
            {modules.map(module => (
              <tr key={module.id}>
                <td>
                  <div 
                    className={`d-flex align-items-center ${!readOnly ? 'cursor-pointer' : ''}`}
                    onClick={!readOnly ? () => handleRowToggle(module.id) : undefined}
                    style={{ cursor: !readOnly ? 'pointer' : 'default' }}
                  >
                    <i className={`fas ${module.icon} mr-2`}></i>
                    <div className="flex-grow-1">
                      <strong>{module.name}</strong>
                      {module.description && (
                        <div><small className="text-muted">{module.description}</small></div>
                      )}
                    </div>
                    {!readOnly && (
                      <div className="ml-2">
                        <div className="custom-control custom-checkbox">
                          <input
                            type="checkbox"
                            className="custom-control-input"
                            id={`${module.id}_all`}
                            checked={isRowAllSelected(module.id)}
                            onChange={() => handleRowToggle(module.id)}
                          />
                          <label className="custom-control-label" htmlFor={`${module.id}_all`}>
                            <small>All</small>
                          </label>
                        </div>
                      </div>
                    )}
                  </div>
                </td>
                
                {/* Read Permission */}
                <td className="text-center">
                  <div className="custom-control custom-checkbox">
                    <input
                      type="checkbox"
                      className="custom-control-input"
                      id={`${module.id}_read`}
                      checked={isPermissionSelected(module.id, 'read')}
                      onChange={() => handlePermissionToggle(module.id, 'read')}
                      disabled={readOnly}
                    />
                    <label 
                      className="custom-control-label" 
                      htmlFor={`${module.id}_read`}
                    ></label>
                  </div>
                </td>
                
                {/* Write Permission */}
                <td className="text-center">
                  <div className="custom-control custom-checkbox">
                    <input
                      type="checkbox"
                      className="custom-control-input"
                      id={`${module.id}_write`}
                      checked={isPermissionSelected(module.id, 'write')}
                      onChange={() => handlePermissionToggle(module.id, 'write')}
                      disabled={readOnly}
                    />
                    <label 
                      className="custom-control-label" 
                      htmlFor={`${module.id}_write`}
                    ></label>
                  </div>
                </td>
                
                {/* Delete Permission */}
                <td className="text-center">
                  <div className="custom-control custom-checkbox">
                    <input
                      type="checkbox"
                      className="custom-control-input"
                      id={`${module.id}_delete`}
                      checked={isPermissionSelected(module.id, 'delete')}
                      onChange={() => handlePermissionToggle(module.id, 'delete')}
                      disabled={readOnly}
                    />
                    <label 
                      className="custom-control-label" 
                      htmlFor={`${module.id}_delete`}
                    ></label>
                  </div>
                </td>
                
                {/* Permission Count */}
                <td className="text-center">
                  <span className={`badge ${
                    getModulePermissionCount(module.id) === 3 ? 'badge-success' :
                    getModulePermissionCount(module.id) === 2 ? 'badge-warning' :
                    getModulePermissionCount(module.id) === 1 ? 'badge-info' :
                    'badge-secondary'
                  }`}>
                    {getModulePermissionCount(module.id)}/3
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
          <div className="col-md-3">
            <div className="text-center">
              <i className="fas fa-eye text-info fa-2x"></i>
              <div className="mt-2">
                <strong>Read Permissions</strong>
                <div className="text-muted">
                  {getColumnCount('read')} of {modules.length} modules
                </div>
              </div>
            </div>
          </div>
          <div className="col-md-3">
            <div className="text-center">
              <i className="fas fa-edit text-warning fa-2x"></i>
              <div className="mt-2">
                <strong>Write Permissions</strong>
                <div className="text-muted">
                  {getColumnCount('write')} of {modules.length} modules
                </div>
              </div>
            </div>
          </div>
          <div className="col-md-3">
            <div className="text-center">
              <i className="fas fa-trash text-danger fa-2x"></i>
              <div className="mt-2">
                <strong>Delete Permissions</strong>
                <div className="text-muted">
                  {getColumnCount('delete')} of {modules.length} modules
                </div>
              </div>
            </div>
          </div>
          <div className="col-md-3">
            <div className="text-center">
              <i className="fas fa-check-circle text-success fa-2x"></i>
              <div className="mt-2">
                <strong>Total Permissions</strong>
                <div className="text-muted">
                  {Object.keys(selectedPermissions).length} of {modules.length * 3} total
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
                  ['read', 'write', 'delete'].forEach(action => {
                    newPermissions[`${module.id}_${action}`] = true;
                  });
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
                modules.forEach(module => {
                  newPermissions[`${module.id}_read`] = true;
                });
                onPermissionChange(newPermissions);
              }}
            >
              <i className="fas fa-eye mr-1"></i>
              Read Only
            </button>
            <button 
              type="button"
              className="btn btn-sm btn-outline-warning mr-2 mb-2"
              onClick={() => {
                const newPermissions = {};
                modules.forEach(module => {
                  newPermissions[`${module.id}_read`] = true;
                  newPermissions[`${module.id}_write`] = true;
                });
                onPermissionChange(newPermissions);
              }}
            >
              <i className="fas fa-edit mr-1"></i>
              Read + Write
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
    icon: PropTypes.string
  })).isRequired,
  permissions: PropTypes.array,
  selectedPermissions: PropTypes.object.isRequired,
  onPermissionChange: PropTypes.func.isRequired,
  readOnly: PropTypes.bool
};

export default PermissionMatrix;
