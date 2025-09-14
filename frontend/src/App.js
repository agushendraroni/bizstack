import React from "react";
import { BrowserRouter as Router, Route, Switch, Redirect } from "react-router-dom";
import routes from "./routes";
import withTracker from "./withTracker";
import ErrorBoundary from "./components/common/ErrorBoundary";
import CompanySelector from "./pages/auth/CompanySelector";
import LoginView from "./views/Login";

// Import CSS
import "bootstrap/dist/css/bootstrap.min.css";
import "./shards-dashboard/styles/shards-dashboards.1.1.0.min.css";
import "./assets/styles/app.css";

export default () => (
  <ErrorBoundary>
    <Router basename={process.env.REACT_APP_BASENAME || ""}>
      <Switch>
        {/* Default route redirects to company selector */}
        <Route exact path="/" render={() => <Redirect to="/select-company" />} />
        
        {/* Company selection route */}
        <Route path="/select-company" component={withTracker(CompanySelector)} />
        
        {/* Login route */}
        <Route path="/login" component={withTracker(LoginView)} />
        
        {/* Protected routes */}
        {routes.map((route, index) => {
          return (
            <Route
              key={index}
              path={route.path}
              exact={route.exact}
              component={withTracker(props => {
                return (
                  <route.layout {...props}>
                    <route.component {...props} />
                  </route.layout>
                );
              })}
            />
          );
        })}
      </Switch>
    </Router>
  </ErrorBoundary>
);
