import {
  Context, EnvironmentVariables, ServerlessCallback, ServerlessFunctionSignature,
} from '@twilio-labs/serverless-runtime-types/types';
import { JsonWebTokenError } from 'jsonwebtoken';
import { TriggerEventError } from './trigger';
import { AppContext, withAppContext } from './appContext';
import { AuthError, MissingAuthError } from './auth';
import Logger from './logger';

export function withErrorHandler<T extends EnvironmentVariables, U>(
  handler: ServerlessFunctionSignature<T, U>,
): ServerlessFunctionSignature<T, U> {
  return async (context: Context<T>, event: U, callback: ServerlessCallback) => {
    try {
      await handler(context, event, callback);
    } catch (error) {
      error.executionId = context.executionId;
      const response = new Twilio.Response();
      response.appendHeader('Content-Type', 'application/json; charset=UTF-8');
      response.setBody(JSON.stringify(error, ['executionId', 'message', 'name', 'code', 'status']));
      if (error instanceof MissingAuthError) {
        response.setStatusCode(401);
      } else if (error instanceof JsonWebTokenError || error instanceof AuthError) {
        response.setStatusCode(403);
      } else if (error instanceof TriggerEventError) {
        response.setStatusCode(422);
      } else {
        response.setStatusCode(500);
      }
      if (context.logger) {
        const logger = context.logger as any;
        logger instanceof Logger && logger.log(error.stack);
      }
      callback(null, response);
    }
  };
}

export function withApp<U>(handler: ServerlessFunctionSignature<Context<AppContext>, U>) {
  return withErrorHandler(
    withAppContext<U>(
      (context, event, callback) => handler(context, event, callback),
    ),
  );
}
