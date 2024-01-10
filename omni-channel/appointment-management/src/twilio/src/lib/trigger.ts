import Joi from 'joi';
import { ServerlessCallback } from '@twilio-labs/serverless-runtime-types/types';
import { MessageInstance } from 'twilio/lib/rest/api/v2010/account/message';
import { validateEventForJWT } from './auth';
import { getAppConfig } from './appConfig';

const config = getAppConfig();

export class TriggerEventError extends Error {}

export type TriggerEvent<T> = {
    to: string, // phone number in international format
    correlationToken: string,
    parameters: T,
    callbackUrl?: string, // for dev environment, this value will substitute CALLBACK_URL
    request: {
        headers: Record<string, string>,
        cookies: Record<string, string>
    }
}

export function validateTriggerEvent<T>(
  event: TriggerEvent<T>,
  authSecret: string,
  parametersValidationSchema?: Joi.Schema<T>,
): TriggerEvent<T> {
  validateEventForJWT(event, authSecret, config.AUTH_REQUIRED_PERMISSION);

  const { to, correlationToken } = event;

  if (!correlationToken) {
    throw new TriggerEventError('correlationToken: parameter is missing or empty');
  }

  if (!to) {
    throw new TriggerEventError('to: parameter is missing or empty');
  }
  if (!/^\+(1[0-9]{10}|[2-9][0-9]{10,11})$/.test(to)) {
    throw new TriggerEventError(`to: ${to} - wrong recipient phone number`);
  }

  if (!parametersValidationSchema) {
    return event;
  }
  const { value, error } = parametersValidationSchema.validate(
    event.parameters || {},
    { stripUnknown: true },
  );
  if (error) {
    throw new TriggerEventError(`Parameters validation error: ${error.message}`);
  }
  return { ...event, parameters: value };
}

export function triggerSMSResponse(
  callback: ServerlessCallback,
  sms: MessageInstance,
  executionId: string,
  correlationToken?: string,
): void {
  callback(null, {
    executionId,
    correlationToken,
    time: new Date().toISOString(),
    smsTo: sms.to,
    smsSid: sms.sid,
    smsBody: sms.body,
    smsError: sms.errorCode,
  });
}
