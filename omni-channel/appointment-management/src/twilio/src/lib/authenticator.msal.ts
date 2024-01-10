import { ConfidentialClientApplication, Configuration } from '@azure/msal-node';
import { ServiceContext } from 'twilio/lib/rest/sync/v1/service';
import Authenticator from './authenticator';
import Logger from './logger';

const TIME_MARGIN = 5 * 60 * 1000;

export class MSALAuthenticatorWithSyncCache implements Authenticator {
  client: ConfidentialClientApplication;

  clientId: string;

  authority: string | undefined;

  scopes: string[];

  sync: ServiceContext;

  syncDocUniqueName: string;

  logger: Logger | undefined = undefined;

  constructor(
    config: Configuration,
    scopes: string[],
    sync: ServiceContext,
    syncDocUniqueName: string,
    logger?: Logger,
  ) {
    this.clientId = config.auth.clientId;
    this.authority = config.auth.authority;
    this.client = new ConfidentialClientApplication(config);
    this.scopes = scopes;
    this.sync = sync;
    this.syncDocUniqueName = syncDocUniqueName;
    this.logger = logger;
  }

  async acquireToken() {
    const syncDocument = await this.fetchSyncDocument();
    const { data } = syncDocument;

    const expirationLimit = new Date(Date.now() - TIME_MARGIN);
    if (data.expiresOn && new Date(data.expiresOn) > expirationLimit) {
      const { scopes, authority, clientId } = data;
      let isSame = this.authority && this.authority === authority;
      isSame = isSame && clientId === this.clientId;
      isSame = isSame && this.scopes.reduce((acc, item) => acc && scopes.includes(item), true);
      if (isSame) {
        this.logger && this.logger.log('MSAL - cached token');
        return data.accessToken;
      }
    }

    this.logger && this.logger.log('MSAL - acquire token');
    const response = await this.client.acquireTokenByClientCredential({ scopes: this.scopes });
    if (!response) {
      throw new Error('Empty token response');
    }
    await syncDocument.update({ data: { ...response, clientId: this.clientId } });
    const { accessToken } = response;
    return accessToken;
  }

  async fetchSyncDocument() {
    const uniqueName = this.syncDocUniqueName;
    try {
      return await this.sync.documents(uniqueName).fetch();
    } catch (error) {
      if (error?.code === 20404) {
        return this.sync.documents.create({ uniqueName });
      }
      throw error;
    }
  }
}
