import twilio, { Twilio } from 'twilio';

export const ENVIRONMENT_PRODUCTION = 'prod';

export function detectEnvironment(domain: string) {
  const matches = domain && domain.match(/^.*-\d{4}(-([^.]+))?\.twil.io$/i);
  return (matches && matches[2]) ? matches[2].toLowerCase() : ENVIRONMENT_PRODUCTION;
}

type EnvVariables = {
    TWILIO_ACCOUNT_SID?: string,
    ACCOUNT_SID: string
    AUTH_TOKEN: string
}

let client: Twilio;

export function createTwilioClient(env: EnvVariables): Twilio {
  const { TWILIO_ACCOUNT_SID, ACCOUNT_SID, AUTH_TOKEN } = env;
  if (!client) {
    if (!ACCOUNT_SID || !AUTH_TOKEN) {
      throw new Error('Empty Twilio credentials');
    }
    if (ACCOUNT_SID.startsWith('AC')) {
      client = twilio(ACCOUNT_SID, AUTH_TOKEN);
    } else {
      if (!TWILIO_ACCOUNT_SID) {
        throw new Error('Empty Twilio Account Sid');
      }
      client = twilio(ACCOUNT_SID, AUTH_TOKEN, { accountSid: TWILIO_ACCOUNT_SID });
    }
  }
  return client;
}
