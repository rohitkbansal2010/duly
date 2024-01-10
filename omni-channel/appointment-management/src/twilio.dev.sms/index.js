require('dotenv').config();
const yargs = require('yargs');
const ListenerLocal = require('./lib/ListenerLocal');
const ListenerNgrok = require('./lib/ListenerNgrok');
const ListenerSync = require('./lib/ListenerSync');
const Terminal = require('./lib/Terminal');
const { preparePhoneNumber } = require('./lib/twilio');
const SMSSession = require('./lib/SMSSession');

// OPTIONS
const options = yargs
  .help()
  .option('type', {
    type: 'string',
    description: 'Type of Twilio webhooks listener',
    default: 'local',
    choices: ['local', 'ngrok', 'sync'],
  })
  .option('ngrokToken', {
    type: 'string',
    description: 'AuthToken for ngrok (in case of ngrok type)',
  })
  .option('port', {
    type: 'string',
    description: 'Port address (not applicable for sync type)',
  })
  .parse((process.argv.slice(2)));

// SMS SENDER
const sender = new SMSSession({ from: process.env.PHONE_NUMBER });

// TERMINAL
const terminal = new Terminal();
terminal.smsHandler = sender;
terminal.closeHandler = () => process.exit();

// LISTENER
let listener;
switch (options.type) {
  case 'local':
    listener = new ListenerLocal(options.port || process.env.PORT);
    break;
  case 'ngrok':
    listener = new ListenerNgrok(
      options.port || process.env.PORT,
      options.ngrokToken || process.env.NGROK_AUTHTOKEN,
    );
    break;
  case 'sync':
    listener = new ListenerSync(
      process.env.SYNC_SYNC_SERVICE_NAME,
      process.env.SYNC_FUNCTIONS_SERVICE_NAME,
    );
    break;
  default:
    throw new Error(`Listener for ${options.type}is not implemented`);
}
listener.on('sms', (data) => terminal.incomingSMS(data));
listener.on('log', (data) => terminal.incomingLog(data));
listener.on('request', (data) => terminal.incomingLog(data));

// START
(async () => {
  await listener.initialize();
  const { phoneNumber } = await preparePhoneNumber(
    process.env.PHONE_NUMBER_SID,
    process.env.PHONE_NUMBER,
    listener.getPhoneNumberWebhook(),
  );
  sender.setFrom(phoneNumber);

  terminal.initialize();
  terminal.info(`
${listener.getDescription()}\n
Phone Number: \x1b[32m${sender.from}\x1b[0m\n
${sender.getDescription()}
`);
  terminal.prompt();
})().catch((error) => {
  console.log(error);
  process.exit();
});
