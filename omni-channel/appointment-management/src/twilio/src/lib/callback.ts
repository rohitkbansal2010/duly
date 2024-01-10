import axios from 'axios';
import * as crypto from 'crypto';
import qs from 'qs';
import https from 'https';
import Authenticator from './authenticator';

export type CallbackPayload = {
    executionId: string,
    correlationToken: string,
    status: string,
    meta: string | Record<string, any> | undefined,
}

export async function executeCallback(
  url: string,
  payload: CallbackPayload,
  authenticator?: Authenticator,
) {
  const errors = [];
  payload.executionId || errors.push('ExecutionSid');
  payload.correlationToken || errors.push('CorrelationToken');
  payload.status || errors.push('Status');
  if (errors.length) {
    throw new Error(`Missing callback parameters: ${errors.join(', ')}`);
  }

  const headers = authenticator ? { Authorization: `Bearer ${await authenticator.acquireToken()}` } : undefined;

  // REQUEST
  return axios({
    url,
    method: 'POST',
    data: {
      ...payload,
      meta: (payload.meta && typeof payload.meta !== 'string') ? JSON.stringify(payload.meta) : payload.meta,
      time: (new Date()).toISOString(),
    },
    headers,
    // FIXME: remove after solving certificate issues
    httpsAgent: new https.Agent({
      rejectUnauthorized: false,
    }),
  });
}

// simulate Twilio request (with X-Twilio-Signature)
async function executeTwilioRequest(AUTH_TOKEN: string, url: string, data: any) {
  if (!AUTH_TOKEN || !url) {
    throw new Error('Invalid parameters for X-Twilio-Signature assembling');
  }

  // SIGNATURE
  const dataString = Object.keys(data || {})
    .filter((key) => typeof data[key] !== 'undefined')
    .sort()
    .reduce((acc, key) => acc + key + data[key], url);

  const signature = crypto.createHmac('sha1', AUTH_TOKEN)
    .update(Buffer.from(dataString, 'utf-8'))
    .digest('base64');

  // REQUEST
  return axios.post(url, qs.stringify(data), {
    headers: {
      'content-type': 'application/x-www-form-urlencoded',
      'X-Twilio-Signature': signature,
    },
  });
}
