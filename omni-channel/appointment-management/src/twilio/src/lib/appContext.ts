import {
  Context, EnvironmentVariables, ServerlessCallback, ServerlessFunctionSignature,
} from '@twilio-labs/serverless-runtime-types/types';
import { v4 as uuidv4 } from 'uuid';
import { Twilio } from 'twilio';
import { Configuration } from '@azure/msal-node';
import { AxiosResponse } from 'axios';
import SyncMapDataAdapter from './syncMapDataAdapter';
import Logger from './logger';
import { CallbackPayload, executeCallback } from './callback';
import { TriggerEvent } from './trigger';
import SyncMapData from './syncMapData';
import { createTwilioClient, detectEnvironment, ENVIRONMENT_PRODUCTION } from './twilio';
import Authenticator from './authenticator';
import { MSALAuthenticatorWithSyncCache } from './authenticator.msal';
import { getAppConfig } from './appConfig';

const appConfig = getAppConfig();

type AppCallbackPayload = Omit<CallbackPayload, 'executionId'>

export type AppContext = {
    ACCOUNT_SID: string,
    AUTH_TOKEN: string,
    // trigger auth
    TRIGGER_AUTH_SECRET: string,
    // callback & callback auth
    CALLBACK_URL: string,
    CALLBACK_CLIENT_ID: string,
    CALLBACK_CLIENT_SECRET: string,
    CALLBACK_AUTHORITY: string,
    CALLBACK_SCOPE: string,
    // sms
    SMS_MESSAGES_FROM: string
    SMS_MESSAGES_ADVANCED_OPTOUT: string,
    // sync
    SYNC_SERVICE_SID: string
    // functions service
    SERVICE_SID: string // presents in the context but is not mentioned in Context
    // app
    executionId: string,
    readonly environment: string | undefined,
    readonly logger: Logger,
    readonly syncMap: SyncMapDataAdapter,
    addSyncMapData: (STAGE: string, event: TriggerEvent<any>) => Promise<SyncMapData>,
    getTemplates: (name: string) => { [key: string]: string }
    executeCallback: (
        payload: AppCallbackPayload,
        callback?: string
    ) => Promise<AxiosResponse | undefined>
} & EnvironmentVariables

export function createMSALAuthenticator(context: Context<AppContext>) {
  const configuration:Configuration = {
    auth: {
      clientId: context.CALLBACK_CLIENT_ID,
      clientSecret: context.CALLBACK_CLIENT_SECRET,
      authority: context.CALLBACK_AUTHORITY,
    },
  };
  return new MSALAuthenticatorWithSyncCache(
    configuration,
    [context.CALLBACK_SCOPE],
    context.getTwilioClient().sync.services(context.SYNC_SERVICE_SID),
    appConfig.MSAL_SYNC_DOCUMENT_NAME,
    context.logger,
  );
}

export function withAppContext<U>(
  handler: ServerlessFunctionSignature<Context<AppContext>, U>,
): ServerlessFunctionSignature<Context<AppContext>, U> {
  return (context: Context<AppContext>, event: U, callback: ServerlessCallback) => {
    context.executionId = uuidv4();

    Object.defineProperty(context, 'environment', {
      enumerable: true,
      value: detectEnvironment(context.DOMAIN_NAME),
    });

    let twilioClient: Twilio;
    context.getTwilioClient = ():Twilio => {
      twilioClient = twilioClient || createTwilioClient(context);
      return twilioClient;
    };

    let syncMap: SyncMapDataAdapter;
    Object.defineProperty(context, 'syncMap', {
      get: () => {
        syncMap = syncMap || new SyncMapDataAdapter(
          context.getTwilioClient().sync
            .services(context.SYNC_SERVICE_SID)
            .syncMaps(appConfig.SYNC_MAP_RECIPIENTS),
        );
        return syncMap;
      },
    });

    context.addSyncMapData = async (stage: string, triggerEvent: TriggerEvent<any>) => {
      const syncData = await context.syncMap.getOrCreate(triggerEvent.to);
      syncData.addData(
        stage,
        triggerEvent.correlationToken,
        triggerEvent.parameters,
        context.environment === ENVIRONMENT_PRODUCTION ? undefined : triggerEvent.callbackUrl,
      );
      return context.syncMap.save(syncData);
    };

    let logger: Logger;
    Object.defineProperty(context, 'logger', {
      get: () => {
        logger = logger || new Logger(context.executionId);
        return logger;
      },
    });

    let authenticator: Authenticator;
    context.executeCallback = async (payload, endpoint?: string) => {
      const url = context.environment === ENVIRONMENT_PRODUCTION
        ? context.CALLBACK_URL
        : endpoint || context.CALLBACK_URL;
      if (!url) {
        context.logger.warn(`Missing CALLBACK_URL. Payload: ${JSON.stringify(payload)}`);
        return undefined;
      }
      const data = { ...payload, executionId: context.executionId };
      try {
        if (url === context.CALLBACK_URL) {
          authenticator = authenticator || createMSALAuthenticator(context);
          return executeCallback(url, data, authenticator);
        }
        return await executeCallback(url, data);
      } catch (error) {
        context.logger.warn(`Callback ERROR (${error.name}) for ${url}: ${JSON.stringify(data)}`);
      }
    };

    return handler(context, event, callback);
  };
}
