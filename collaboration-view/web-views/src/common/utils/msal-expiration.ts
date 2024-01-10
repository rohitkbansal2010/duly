import { PublicClientApplication, RedirectRequest } from '@azure/msal-browser';

import { ACCOUNT } from '@constants';
import { cleanAccessData, getLocalStorageItem } from '@utils';

let expirationTimeoutId: ReturnType<typeof setTimeout>;

export function setExpirationTimeout (
  instance: PublicClientApplication,
  request: RedirectRequest,
  expiresOnString: string
) {
  if (expirationTimeoutId) {
    clearTimeout(expirationTimeoutId);
  }

  const TOKEN_SPARETIME_COEFFICIENT = 0.75;
  const tokenLifeTime = TOKEN_SPARETIME_COEFFICIENT * (+(new Date(expiresOnString)) - Date.now());
  if (tokenLifeTime <= 0) {
    cleanAccessData();
    instance.loginRedirect(request)
      .catch((error) => {
        console.error(error);
        location.reload();
      });
    return;
  }
  expirationTimeoutId = setTimeout(() => {
    instance
      .acquireTokenSilent({
        scopes: request.scopes,
        account: JSON.parse(getLocalStorageItem(ACCOUNT) as string),
        forceRefresh: true,
      })
      .catch(() =>
        instance.acquireTokenRedirect(request))
      .catch((error) => {
        console.error(error);
        location.reload();
      });
  }, tokenLifeTime);
}
