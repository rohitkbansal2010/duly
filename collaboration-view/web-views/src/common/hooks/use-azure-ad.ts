import { AuthenticationResult } from '@azure/msal-browser';
import { useMsal } from '@azure/msal-react';
import noop from 'lodash/noop';
import { useCallback } from 'react';
import { useSelector } from 'react-redux';

import { ACCOUNT } from '@constants';
import { CollaborationViewRoutes } from '@enums';
import { invalidateToken } from '@http/requests';
import { RootState } from '@redux/reducers';
import {
  msalRequest, cleanAccessData, getLocalStorageItem, msalUseSilentMode, msalSilentRequest
} from '@utils';

export const useAzureADAuth = () => {
  const { instance, inProgress } = useMsal();
  const isAuthenticated = useSelector(({ USER }: RootState) =>
    !!USER?.isAuthenticated);

  const loginAzureADRedirect = useCallback(() => {
    if (msalUseSilentMode()) {
      console.log('... loginAzureADRedirect SILENT'); // TODO: remove after tests
      instance.ssoSilent(msalSilentRequest).catch(console.error);
    } else {
      console.log('... loginAzureADRedirect NORMAL'); // TODO: remove after tests
      instance.loginRedirect(msalRequest).catch(console.error);
    }
  }, [ instance ]);

  const loginAzureADPopup = useCallback((callback: () => void = noop) => {
    let loginPromise: Promise<AuthenticationResult>;
    if (msalUseSilentMode()) {
      console.log('... loginAzureADPopup SILENT'); // TODO: remove after tests
      loginPromise = instance.ssoSilent(msalSilentRequest)
        .catch((error) => {
          // There is also can be not only InteractionRequiredAuthError but another type of error
          // after which is better to switch to normal flow
          console.error(error);
          console.log('... loginAzureADPopup BACK TO NORMAL'); // TODO: remove after tests
          return instance.loginPopup(msalSilentRequest);
        });
    } else {
      loginPromise = instance.loginPopup(msalSilentRequest);
    }

    loginPromise
      .then ((response) => {
        response && callback();
      })
      .catch(console.error);
  }, [ instance ]);

  const logoutAzureADRedirect = useCallback((redirectAfterLogout?: string) => {
    sessionStorage.removeItem('metric');
    invalidateToken()
      .then(() => {
        localStorage.setItem('fromAfterVisit', '0');
        const request = {
          account: JSON.parse(getLocalStorageItem(ACCOUNT) as string),
          postLogoutRedirectUri: redirectAfterLogout || CollaborationViewRoutes.home,
        };
        cleanAccessData();
        return instance.logoutRedirect(request);
      })
      .catch(console.error);
  }, [ instance ]);

  return {
    isAuthenticated,
    inProgress,
    loginAzureADPopup,
    loginAzureADRedirect,
    logoutAzureADRedirect,
  };
};
