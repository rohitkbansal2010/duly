import { ServerlessCallback } from '@twilio-labs/serverless-runtime-types/types';
import Joi from 'joi';
import { TriggerEvent, triggerSMSResponse, validateTriggerEvent } from '../lib/trigger';
import { sendSMS } from '../lib/messages';
import { withApp } from '../lib/middleware';
import { getCompiledTemplate } from '../lib/templates';

const STAGE = 'acknowledgment';

type HandlerEvent = TriggerEvent<{
  name: string
}>

const handlerEventSchema = Joi.object({
  providerName: Joi.string().required(), // 'Dr. Mary Codo',
  confirmationPageUrl: Joi.string().required(), // 'https://dev-confirmation.link',
  appointmentDateTime: Joi.string().isoDate().required(), // '2022-03-21T13:45:30',
  streetName: Joi.string().required(), // '1133 South Blvd',
  city: Joi.string().required(), // 'Oak Park',
  state: Joi.string().required(), // 'IL',
  zipCode: Joi.string().required(), // '60302',

});

export const handler = withApp(async (
  context,
  dirtyEvent: HandlerEvent,
  callback: ServerlessCallback,
) => {
  context.logger.triggerEvent(dirtyEvent);

  // EVENT VALIDATION
  const event = validateTriggerEvent(dirtyEvent, context.TRIGGER_AUTH_SECRET, handlerEventSchema);
  const {
    to, correlationToken, parameters, callbackUrl,
  } = event;

  // SMS
  const body = getCompiledTemplate('confirmation', 'default')(parameters);
  const sms = await sendSMS(to, body, context, { correlationToken, callbackUrl });

  // SYNC MAP
  await context.addSyncMapData(STAGE, event);

  // RESPONSE
  return triggerSMSResponse(callback, sms, context.executionId, correlationToken);
});
