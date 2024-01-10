import { StrictMode, useEffect } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { Route, Switch } from 'react-router';

import { BuildNumber } from '@components/build-number';
import { ACCESS_TOKEN, LOGOUT_IN_PROCESS } from '@constants';
import { useWindowSize } from '@hooks';
import { CvRootPage } from '@pages/collaboration-view/cv-root-page';
import { PageNotFound } from '@pages/page-not-found';
import { initiateAuthentication, setIsAuthenticated } from '@redux/actions';
import { RootState } from '@redux/reducers';
import { AppDispatch } from '@redux/store';
import { getLocalStorageItem, setLocalStorageItem, checkMsalExpiration } from '@utils';

export const App = () => {
  const dispatch: AppDispatch = useDispatch();

  const isAuthenticationInitiated = useSelector(
    ({ USER }: RootState) => 
      USER.isAuthenticationInitiated
  );

  useWindowSize();

  useEffect(() => {
    checkMsalExpiration();
  }, []);

  useEffect(() => {
    if (!isAuthenticationInitiated) {
      setLocalStorageItem(LOGOUT_IN_PROCESS, JSON.stringify(false));
      dispatch(initiateAuthentication(true));
      const accessToken_UNSAFE_TEMP = getLocalStorageItem(ACCESS_TOKEN);
      if (accessToken_UNSAFE_TEMP) {
        dispatch(setIsAuthenticated(true));
      }
    }
  }, [ isAuthenticationInitiated, dispatch ]);

  return (
    <StrictMode>
      {isAuthenticationInitiated && (
        <Switch>
          <Route component={CvRootPage} />
          <Route component={PageNotFound} />
        </Switch>
      )}
      <BuildNumber />
    </StrictMode>
  );
};
