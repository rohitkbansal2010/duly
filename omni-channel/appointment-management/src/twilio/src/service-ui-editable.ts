/* eslint-disable no-console */

import dotenv from 'dotenv';
import * as path from 'path';
import * as fs from 'fs';
import { ServiceInstance } from 'twilio/lib/rest/serverless/v1/service';
import { createTwilioClient } from './lib/twilio';

const ENV_FILENAME = '.env';

// .ENV FILE - FOR CREDENTIALS
const envPath = path.join(__dirname, '..', ENV_FILENAME);
if (!fs.existsSync(envPath)) {
  console.log(`\x1b[31mMissing file ${ENV_FILENAME}\x1b[0m\n`);
  process.exit();
}
dotenv.config({ path: envPath });

// PACKAGE.JSON - FOR SERVICE NAME
let packageJson: any;
try {
  packageJson = require('../package.json'); // eslint-disable-line global-require
} catch (error) {
  console.log('\x1b[31mMissing package.json file\x1b[0m\n');
  process.exit();
}
const { name: serviceName } = packageJson;
if (!serviceName) {
  console.log('\x1b[31mMissing name in package.json file\x1b[0m\n');
  process.exit();
}

// UPDATING SERVICE
(async () => {
  const client = createTwilioClient({
    TWILIO_ACCOUNT_SID: process.env.TWILIO_ACCOUNT_SID,
    ACCOUNT_SID: process.env.ACCOUNT_SID as string,
    AUTH_TOKEN: process.env.AUTH_TOKEN as string,
  });

  let service: ServiceInstance;
  try {
    service = await client.serverless.services(serviceName).fetch();
  } catch (error) {
    console.log(`\x1b[31mUnable to fetch Service with name "${serviceName}\x1b[0m\n`);
    return;
  }

  const { uiEditable } = service;
  console.log(`The ui-editing of the service \x1b[32m${serviceName}\x1b[0m was ${uiEditable ? 'enabled' : 'disabled'}`);
  console.log(`... changing ui-editable property to \x1b[33m${(!uiEditable).toString().toUpperCase()}\x1b[0m`);

  const result = await service.update({ uiEditable: !uiEditable });
  const value = result.uiEditable.toString().toUpperCase();
  console.log(`The ui-editable property has been set to \x1b[33m${value}\x1b[0m for \x1b[32m${result.friendlyName}\x1b[0m service\n`);
})().catch(() => {
  console.log('\x1b[31mSomething went wrong\x1b[0m');
});
