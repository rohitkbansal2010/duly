import { Context, ServerlessCallback } from '@twilio-labs/serverless-runtime-types/types';
import { withApp } from '../../lib/middleware';
import { AppContext } from '../../lib/appContext';
import { getOptOutType, IncomingSmsData, IncomingSmsDataForAdvancedOptOut } from '../../lib/messagesOptOut';
import { getCompiledTemplate } from '../../lib/templates';

type HandlerEvent = IncomingSmsData | IncomingSmsDataForAdvancedOptOut;

export const handler = withApp(async (
  context: Context<AppContext>,
  event: HandlerEvent,
  callback: ServerlessCallback,
) => {
  const optOut = getOptOutType(event, (context.SMS_MESSAGES_ADVANCED_OPTOUT || 'false').toLowerCase() === 'true');

  const syncData = await context.syncMap.getOrCreate(event.From);
  const data = syncData.getData();

  if (optOut && !data) {
    context.logger.log(`${optOut} from unknown phone number: ${event.From}`);
    return callback(null);
  }

  if (optOut && data) {
    switch (optOut) {
      case 'STOP':
        syncData.optout = true;
        await context.syncMap.save(syncData);
        await context.executeCallback(
          { correlationToken: data.correlationToken, status: 'stop', meta: event.Body },
          data.callbackUrl,
        );
        return callback(null);
      case 'START':
        if (syncData.optout) {
          syncData.optout = false;
          await context.syncMap.save(syncData);
          await context.executeCallback(
            { correlationToken: data.correlationToken, status: 'start', meta: event.Body },
            data.callbackUrl,
          );
          return callback(null);
        }
        // we will proceed this message as unhandled reply
        break;
      default:
        // HELP
        return callback(null);
    }
  }

  if (data) {
    await context.executeCallback(
      { correlationToken: data.correlationToken, status: 'reply', meta: event.Body },
      data.callbackUrl,
    );
  } else {
    context.logger.log(`REPLY from unknown phone number: ${event.From}`);
  }

  const twiml = new Twilio.twiml.MessagingResponse();
  twiml.message(getCompiledTemplate('default', 'unhandled')());
  return callback(null, twiml);
});
