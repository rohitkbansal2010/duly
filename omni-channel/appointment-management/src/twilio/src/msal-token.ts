/* eslint-disable no-console */

import path from 'path';
import fs from 'fs';
import twilio from 'twilio';
import dotenv from 'dotenv';
import { Context } from '@twilio-labs/serverless-runtime-types/types';
import { AppContext, createMSALAuthenticator } from './lib/appContext';

const ENV_FILENAME = '.env.local';

const envPath = path.join(__dirname, '..', ENV_FILENAME);
if (!fs.existsSync(envPath)) {
  console.log(`\x1b[31mMissing file ${ENV_FILENAME}\x1b[0m`);
  process.exit();
}

dotenv.config({ path: envPath });

const context = {
  CALLBACK_CLIENT_ID: process.env.CALLBACK_CLIENT_ID,
  CALLBACK_CLIENT_SECRET: process.env.CALLBACK_CLIENT_SECRET,
  CALLBACK_AUTHORITY: process.env.CALLBACK_AUTHORITY,
  CALLBACK_SCOPE: process.env.CALLBACK_SCOPE,
  SYNC_SERVICE_SID: process.env.SYNC_SERVICE_SID,
  getTwilioClient: () => twilio(
    process.env.ACCOUNT_SID,
    process.env.AUTH_TOKEN,
    { accountSid: process.env.TWILIO_ACCOUNT_SID },
  ),
} as unknown as Context<AppContext>;

const missingVariables: string[] = [];
Object.entries(context).forEach(([key, value]) => {
  value || missingVariables.push(key);
});
if (missingVariables.length) {
  console.log(`\x1b[31mMissing env variables: ${missingVariables.join(', ')}\x1b[0m`);
  process.exit();
}

createMSALAuthenticator(context).acquireToken()
  .then((token) => {
    console.log('\x1b[33mToken:\x1b[0m');
    console.log(`${token}\n`);
  })
  .catch((error) => {
    console.log('\x1b[31mError\x1b[0m');
    console.log(error);
  });
