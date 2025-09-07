import React from "react";
import { BrowserRouter as Router, Route, Switch } from "react-router-dom";

import routes from "./routes";
import withTracker from "./withTracker";
import ErrorBoundary from "./components/common/ErrorBoundary";

import "bootstrap/dist/css/bootstrap.min.css";
import "./shards-dashboard/styles/shards-dashboards.1.1.0.min.css";

export default () => (
  <ErrorBoundary>
    <Router basename={process.env.REACT_APP_BASENAME || ""}>
      <Switch>
        {routes.map((route, index) => {
          return (
            <Route
              key={index}
              path={route.path}
              exact={route.exact}
              component={withTracker(props => {
                return (
                  <ErrorBoundary>
                    <route.layout {...props}>
                      <route.component {...props} />
                    </route.layout>
                  </ErrorBoundary>
                );
              })}
            />
          );
        })}
      </Switch>
    </Router>
  </ErrorBoundary>
);
