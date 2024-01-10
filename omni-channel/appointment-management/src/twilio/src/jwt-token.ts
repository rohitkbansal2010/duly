/* eslint-disable no-console */

import dotenv from 'dotenv';
import * as path from 'path';
import * as fs from 'fs';
import { generateJWT } from './lib/auth';
import { getAppConfig } from './lib/appConfig';

const config = getAppConfig();

const ENV_FILENAME = '.env';

const envPath = path.join(__dirname, '..', ENV_FILENAME);
if (!fs.existsSync(envPath)) {
  console.log(`\x1b[31mMissing file ${ENV_FILENAME}\x1b[0m`);
  process.exit();
}

dotenv.config({ path: envPath });

const secret = process.env.TRIGGER_AUTH_SECRET;
if (!secret) {
  console.log('\x1b[31mMissing TRIGGER_AUTH_SECRET\x1b[0m');
  process.exit();
}

const token = generateJWT(secret, { perms: config.AUTH_REQUIRED_PERMISSION }, '31d');

console.log(`\x1b[33mToken:\x1b[0m\n${token}\n`);
