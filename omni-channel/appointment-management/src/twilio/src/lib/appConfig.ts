import '@twilio-labs/serverless-runtime-types';
import path from 'path';

export type AppConfig = {
    SYNC_MAP_RECIPIENTS: string,

    SMS_STATUS_CALLBACK: string,

    AUTH_REQUIRED_ISSUER: string,

    AUTH_REQUIRED_AUDIENCE: string,

    AUTH_REQUIRED_PERMISSION: string,

    MSAL_SYNC_DOCUMENT_NAME: string,
}

let config: AppConfig;

export function getAppConfig(): AppConfig {
  if (!config) {
    let configPath: string;
    if (typeof Runtime === 'undefined') {
      configPath = path.join(__dirname, '..', 'assets', 'config.private');
    } else {
      ({ path: configPath } = Runtime.getAssets()['/config.js'] || {});
    }

    // eslint-disable-next-line global-require,import/no-dynamic-require
    const configFromAssets = configPath ? require(configPath) : undefined;
    if (!configFromAssets) {
      throw new Error('Missing config');
    }

    config = configFromAssets as AppConfig;
  }
  return config;
}
