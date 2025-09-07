import React, { useState, useEffect } from "react";
import { Card, CardHeader, CardBody, Button, FormCheckbox } from "shards-react";
import CompanyStorage from "../../utils/companyStorage";

const CompanySettings = () => {
  const [rememberEnabled, setRememberEnabled] = useState(false);
  const [currentCompany, setCurrentCompany] = useState("");
  const [savedCompany, setSavedCompany] = useState("");

  useEffect(() => {
    setRememberEnabled(CompanyStorage.isRememberEnabled());
    setCurrentCompany(CompanyStorage.getCurrentCompanyFromUrl());
    setSavedCompany(CompanyStorage.getRememberedCompany() || "");
  }, []);

  const handleToggleRemember = (enabled) => {
    if (enabled && currentCompany) {
      CompanyStorage.setRememberedCompany(currentCompany);
      setSavedCompany(currentCompany);
    } else {
      CompanyStorage.forgetCompany();
      setSavedCompany("");
    }
    setRememberEnabled(enabled);
  };

  const handleForgetCompany = () => {
    CompanyStorage.forgetCompany();
    setSavedCompany("");
    setRememberEnabled(false);
  };

  const handleSwitchCompany = () => {
    window.location.href = "/company";
  };

  return (
    <Card small>
      <CardHeader className="border-bottom">
        <h6 className="m-0">Company Settings</h6>
      </CardHeader>
      <CardBody>
        <div className="mb-3">
          <strong>Current Company:</strong> 
          <span className="ml-2 text-primary">{currentCompany || "Not set"}</span>
        </div>

        {savedCompany && (
          <div className="mb-3">
            <strong>Saved Company:</strong> 
            <span className="ml-2 text-success">{savedCompany}</span>
          </div>
        )}

        <div className="mb-3">
          <FormCheckbox
            checked={rememberEnabled}
            onChange={(e) => handleToggleRemember(e.target.checked)}
          >
            Remember this company code
          </FormCheckbox>
          <small className="text-muted d-block mt-1">
            When enabled, you'll be automatically redirected to this company's login page
          </small>
        </div>

        <div className="d-flex gap-2">
          <Button 
            theme="outline-primary" 
            size="sm"
            onClick={handleSwitchCompany}
          >
            Switch Company
          </Button>
          
          {savedCompany && (
            <Button 
              theme="outline-danger" 
              size="sm"
              onClick={handleForgetCompany}
            >
              Forget Saved Company
            </Button>
          )}
        </div>

        <div className="mt-3 p-2 bg-light rounded">
          <small className="text-muted">
            <strong>Available Companies:</strong><br/>
            {CompanyStorage.VALID_COMPANIES.join(", ")}
          </small>
        </div>
      </CardBody>
    </Card>
  );
};

export default CompanySettings;