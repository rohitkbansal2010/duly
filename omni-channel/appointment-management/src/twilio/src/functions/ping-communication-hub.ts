import '@twilio-labs/serverless-runtime-types';
import { Context, ServerlessCallback } from '@twilio-labs/serverless-runtime-types/types';
import { AppContext } from '../lib/appContext';
import { EventWithHeaders, validateEventForJWT } from '../lib/auth';
import { getAppConfig } from '../lib/appConfig';
import { withApp } from '../lib/middleware';

const config = getAppConfig();

type HandlerEvent = EventWithHeaders & {
  correlationToken: string
  meta?: string
}

export const handler = withApp(async (
  context: Context<AppContext>,
  event: HandlerEvent,
  callback: ServerlessCallback,
) => {
  validateEventForJWT(event, context.TRIGGER_AUTH_SECRET, config.AUTH_REQUIRED_PERMISSION);

  const { correlationToken, meta } = event;

  if (!correlationToken) {
    const response = new Twilio.Response();
    response.appendHeader('Content-Type', 'application/json; charset=UTF-8');
    response.setStatusCode(422);
    response.setBody({
      message: 'Missing correlationToken',
      executionId: context.executionId,
    });
    return callback(null, response);
  }

  try {
    const response = await context.executeCallback({ status: 'ping', correlationToken, meta });
    const { data, status, statusText } = response || {};
    return callback(null, { success: true, response: { status, statusText, data } });
  } catch (error) {
    const { message, name } = error;
    return callback(null, { success: false, error: { name, message, ...error } });
  }
});
