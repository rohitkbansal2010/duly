import {
  PublicClientApplication, Configuration, RedirectRequest,
  AuthenticationResult, EventMessage, EventType
} from '@azure/msal-browser';

import { ACCESS_TOKEN, ACCOUNT, EXPIRES_ON } from '@constants';
import { setIsAuthenticated } from '@redux/actions';
import { store } from '@redux/store';
import { getLocalStorageItem, setLocalStorageItem } from '@utils';

import { setExpirationTimeout } from './msal-expiration';

let USE_SILENT_MODE = false;

function abandonSilentMode() {
  console.log('MSAL back to NOT silent SSO'); // TODO: remove after tests
  USE_SILENT_MODE = false;
}

function resetSilentMode() {
  USE_SILENT_MODE = (window?.env?.SSO_SILENT_LOGIN_ACTIVE || '').toLowerCase() === 'true';
}

resetSilentMode();

export function msalUseSilentMode() {
  return USE_SILENT_MODE;
}

const msalConfig: Configuration = {
  auth: {
    clientId: window?.env?.CLIENT_ID || '',
    authority: window?.env?.AUTHORITY || '',
    redirectUri: window.location.origin,
    validateAuthority: false,
    knownAuthorities: [ '' ],
    navigateToLoginRequestUrl: true,
  },
  cache: {
    cacheLocation: 'localStorage',
    storeAuthStateInCookie: true,
  },
};

export const msalRequest: RedirectRequest = {
  scopes: [
    'openid', 'offline_access', 'email', 'profile', window?.env?.SCOPE || '',
  ],
};

export const msalSilentRequest: RedirectRequest = {
  ...msalRequest,
  redirectUri: window.location.origin + '/silent-refresh.html',
};

export const msalClient = new PublicClientApplication(msalConfig);

msalClient.addEventCallback((message: EventMessage) => {
  const { eventType, payload } = message;

  const successEvents = [
    EventType.ACQUIRE_TOKEN_SUCCESS,
    EventType.LOGIN_SUCCESS,
    EventType.SSO_SILENT_SUCCESS,
  ];
  if (successEvents.includes(eventType)) {
    const data = payload as AuthenticationResult;
    if (data && data.account) {
      resetSilentMode();
      const expiresOnFromResponse = String(data.expiresOn);
      setLocalStorageItem(ACCESS_TOKEN, data.accessToken);
      setLocalStorageItem(ACCOUNT, JSON.stringify(data.account));
      setLocalStorageItem(EXPIRES_ON, expiresOnFromResponse);
      setExpirationTimeout(msalClient, msalRequest, expiresOnFromResponse);
      store.dispatch(setIsAuthenticated(true));
    }
  }

  if (eventType === EventType.SSO_SILENT_FAILURE) {
    // There is also can be not only InteractionRequiredAuthError but another type of error
    // after which is better to switch to normal flow
    abandonSilentMode();
  }
});

window.addEventListener('storage', ({ key, newValue, oldValue }) =>{
  if (key === ACCESS_TOKEN && !newValue && oldValue) {
    // removing ACCESS_TOKEN from storage means that user logged out from another tab
    // user should be redirected to the auth page
    location.reload();
  }
});

export function checkMsalExpiration() {
  const expiresOnFromStorage = getLocalStorageItem(EXPIRES_ON);
  expiresOnFromStorage && setExpirationTimeout(msalClient, msalRequest, expiresOnFromStorage);
}
