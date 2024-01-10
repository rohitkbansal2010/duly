import { Context, ServerlessCallback } from '@twilio-labs/serverless-runtime-types/types';
import { AppContext, withAppContext } from '../lib/appContext';
import { verifyJWT } from '../lib/auth';

type HandlerEvent = {
  token: string,
}

export const handler = withAppContext(async (
  context: Context<AppContext>,
  event: HandlerEvent,
  callback: ServerlessCallback,
) => {
  try {
    const { token } = event;

    if (!token) {
      const response = new Twilio.Response();
      response.appendHeader('Content-Type', 'application/json; charset=UTF-8');
      response.setStatusCode(422);
      response.setBody({
        message: 'Missing token',
        executionId: context.executionId,
      });
      return callback(null, response);
    }

    const decodedAuthToken = verifyJWT(token, context.TRIGGER_AUTH_SECRET);

    return callback(null, {
      now: Math.floor(Date.now() / 1000),
      token: decodedAuthToken,
    });
  } catch (error) {
    error.executionId = context.executionId;
    const response = new Twilio.Response();
    response.appendHeader('Content-Type', 'application/json; charset=UTF-8');
    response.setBody(JSON.stringify(error, ['executionId', 'message', 'name', 'code', 'status']));
    return callback(null, response);
  }
});
