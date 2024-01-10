import { FC, useEffect } from 'react';
import { Route } from 'react-router';
import { RouteComponentProps, RouteProps } from 'react-router-dom';

import { useAzureADAuth } from '@hooks';

export type AuthRouteProps = RouteProps & {
  Component: FC<RouteComponentProps>;
  path: string;
  exact?: boolean;
}

export const AuthRoute = ({
  Component,
  path,
  children,
  exact = false,
}: AuthRouteProps) => {
  const { isAuthenticated, loginAzureADRedirect, inProgress } = useAzureADAuth();

  useEffect(() => {
    if (!isAuthenticated && inProgress === 'none') {
      loginAzureADRedirect();
    }
  }, [ isAuthenticated, inProgress, loginAzureADRedirect ]);

  if (!isAuthenticated) return null;

  return (
    <Route
      exact={exact}
      path={path}
      render={(props: RouteComponentProps) =>
        <Component {...props}>{children}</Component>}
    />
  );
};
