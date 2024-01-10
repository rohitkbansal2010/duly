import { Context, ServerlessCallback, ServerlessFunctionSignature } from '@twilio-labs/serverless-runtime-types/types';
import { withApp } from '../../lib/middleware';
import { AppContext } from '../../lib/appContext';

type OutgoingSmsStatus = 'sent' | 'delivered' | 'failed' | 'undelivered' | 'delivery_unknown'; // FIXME

// There is no appropriate type in Twilio libraries ???
export type SmsStatusData = {
    SmsSid: string,
    SmsStatus: OutgoingSmsStatus,
    ErrorCode?: string,
    AccountSid: string,
    To: string,
    From: string,
}

type HandlerEvent = SmsStatusData & {
    correlationToken?: string,
    callbackUrl?: string,
}

export const handler: ServerlessFunctionSignature<Context<AppContext>, HandlerEvent> = withApp(
  async (context: Context<AppContext>, event: HandlerEvent, callback: ServerlessCallback) => {
    const { correlationToken, SmsStatus, ErrorCode } = event;

    if (correlationToken) {
      const meta = ['failed', 'undelivered'].includes(SmsStatus)
        ? `Twilio Error Code: ${ErrorCode}`
        : undefined;
      await context.executeCallback(
        { correlationToken, status: SmsStatus, meta },
        event.callbackUrl,
      );
    }

    return callback(null);
  },
);
