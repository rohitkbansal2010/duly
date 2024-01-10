import { Context, ServerlessCallback } from '@twilio-labs/serverless-runtime-types/types';
import Joi from 'joi';
import { AppContext } from '../lib/appContext';
import { TriggerEvent, triggerSMSResponse, validateTriggerEvent } from '../lib/trigger';
import { sendSMS } from '../lib/messages';
import { withApp } from '../lib/middleware';
import { getCompiledTemplate } from '../lib/templates';

const STAGE = 'invitation';

type HandlerEvent = TriggerEvent<{
  name: string
  link: string
}>

const invitationEventSchema = Joi.object({
  patientName: Joi.string().required(), // Anna
  micrositeServicesUrl: Joi.string().required(), // https://dev-invitation.link
});

export const handler = withApp(async (
  context: Context<AppContext>,
  dirtyEvent: HandlerEvent,
  callback: ServerlessCallback,
) => {
  context.logger.triggerEvent(dirtyEvent);

  // EVENT VALIDATION
  const event = validateTriggerEvent(dirtyEvent, context.TRIGGER_AUTH_SECRET, invitationEventSchema);
  const {
    to, correlationToken, parameters, callbackUrl,
  } = event;

  // SMS
  const body = getCompiledTemplate('invitation', 'default')(parameters);
  const sms = await sendSMS(to, body, context, { correlationToken, callbackUrl });

  // SYNC MAP
  await context.addSyncMapData(STAGE, event);

  // RESPONSE
  return triggerSMSResponse(callback, sms, context.executionId, correlationToken);
});
