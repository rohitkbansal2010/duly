import qs from 'qs';
import { MessageInstance, MessageListInstanceCreateOptions } from 'twilio/lib/rest/api/v2010/account/message';
import { Context } from '@twilio-labs/serverless-runtime-types/types';
import LoggerPrivate from './logger';
import { AppContext } from './appContext';
import { getAppConfig } from './appConfig';

const config = getAppConfig();

export function createStatusCallback(domainName: string, params: any, logger?: LoggerPrivate) {
  const query = params ? qs.stringify(params) : '';
  if (/^localhost/.test(domainName)) {
    logger && logger.log(`SKIP sms statusCallback: ${JSON.stringify(params)}`);
    return undefined;
  }
  return `https://${domainName}/${config.SMS_STATUS_CALLBACK}?${query}`;
}

export async function sendSMS(
  recipient: string,
  body: string,
  context: Context<AppContext>,
  statusParams?: Record<'correlationToken' | 'callbackUrl' | string, string | undefined>,
): Promise<MessageInstance> {
  const options: MessageListInstanceCreateOptions = {
    body,
    from: context.SMS_MESSAGES_FROM,
    to: recipient,
    statusCallback: statusParams && createStatusCallback(
      context.DOMAIN_NAME,
      statusParams,
      context.logger,
    ),
  };
  return context.getTwilioClient().messages.create(options);
}
