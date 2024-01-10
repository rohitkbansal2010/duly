const twilio = require('twilio');
const { TwilioServerlessApiClient } = require('@twilio-labs/serverless-api');

const { AccessToken } = twilio.jwt;
const { SyncGrant } = AccessToken;

function getCredentialsFromEnv() {
  const accountSid = process.env.TWILIO_ACCOUNT_SID;
  if (!accountSid) {
    throw new Error('Empty Twilio AccountSid');
  }
  const apiKey = process.env.TWILIO_API_KEY;
  const apiSecret = process.env.TWILIO_API_SECRET;
  if (!apiKey || !apiSecret) {
    throw new Error('Empty Twilio API Key data');
  }
  return { apiKey, apiSecret, accountSid };
}

let twilioClient = null;

function getClient() {
  if (!twilioClient) {
    const accountSid = process.env.TWILIO_ACCOUNT_SID;
    if (!accountSid) {
      throw new Error('Empty Twilio AccountSid');
    }
    const apiKey = process.env.TWILIO_API_KEY;
    const apiSecret = process.env.TWILIO_API_SECRET;
    if (apiKey && apiSecret) {
      twilioClient = twilio(apiKey, apiSecret, { accountSid });
    } else {
      const authToken = process.env.TWILIO_AUTH_TOKEN;
      if (authToken) {
        twilioClient = twilio(accountSid, authToken);
      } else {
        throw new Error('Not enough parameters for initializing Twilio Client');
      }
    }
  }
  return twilioClient;
}

async function preparePhoneNumber(sid, phoneNumber, webhook) {
  const client = getClient();
  let phone = await client.incomingPhoneNumbers(sid).fetch();
  if (phone.phoneNumber !== phoneNumber) {
    throw new Error('Wrong configuration: PhoneNumber SID do not corresponds to actual phone number');
  }

  if (phone.smsUrl !== webhook) {
    console.log('Updating phoneNumber webhook');
    phone = await client.incomingPhoneNumbers(sid).update({
      smsUrl: webhook,
    });
  }
  return phone;
}

async function sendSMS(options) {
  const { from, to, body } = options;
  if (!from || !to) {
    throw new Error(`Can not send SMS, phone number is missing (from: ${from}, to: ${to})`);
  }
  const data = await getClient().messages.create({ body, from, to });
  return {
    date: new Date(), to, from, body, data,
  };
}

const SYNC_IDENTITY = `dev.sms.${Math.random().toString(36).slice(2, 10).toUpperCase()}`;

function createSyncToken(syncServiceSid) {
  const { accountSid, apiKey, apiSecret } = getCredentialsFromEnv();
  const syncGrant = new SyncGrant({ serviceSid: syncServiceSid });
  const token = new AccessToken(accountSid, apiKey, apiSecret);
  token.addGrant(syncGrant);
  token.identity = SYNC_IDENTITY;
  return token.toJwt();
}

async function prepareSyncList(syncSrv, listUniqueName) {
  const client = getClient();
  try {
    return await client.sync.services(syncSrv.sid).syncLists(listUniqueName).fetch();
  } catch (error) {
    if (error.code === 20404) {
      return client.sync.services(syncSrv.sid).syncLists.create({ uniqueName: listUniqueName });
    }
    throw error;
  }
}

function getServerlessApiClient() {
  const { apiKey, apiSecret } = getCredentialsFromEnv();
  return new TwilioServerlessApiClient({
    username: apiKey,
    password: apiSecret,
  });
}

module.exports = {
  getClient,
  getServerlessApiClient,
  sendSMS,
  preparePhoneNumber,
  createSyncToken,
  prepareSyncList,
};
