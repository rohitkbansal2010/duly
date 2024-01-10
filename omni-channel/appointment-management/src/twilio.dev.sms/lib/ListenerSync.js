const EventEmitter = require('events');
const { Client } = require('twilio-sync');
const readline = require('readline');
const { sms2event } = require('./listenerUtils');
const {
  createSyncToken, prepareSyncList, getClient, getServerlessApiClient,
} = require('./twilio');

const SYNC_SMS_LIST_NAME = 'sms';
const SYNC_LOG_LIST_NAME = 'logs';
const SYNC_LIST_ITEM_TTL = 3600; // in seconds

const FUNCTIONS_FOLDER = 'sync-listener-handler';
const FUNCTIONS_SMS_HANDLER = '/sms';
const FUNCTIONS_LOG_HANDLER = '/log';

module.exports = class ListenerSync extends EventEmitter {
  constructor(syncServiceName, functionsServiceName) {
    super({});
    if (!syncServiceName || !functionsServiceName) {
      throw new Error('Missing parameters');
    }
    this.ready = false;
    this.syncServiceName = syncServiceName;
    this.functionsServiceName = functionsServiceName;
  }

  async initializeFunctions(syncServiceSid) {
    const client = getServerlessApiClient();
    let index = 0;
    client.on('status-update', (evt) => {
      index += 1;
      readline.clearLine(process.stdout, -1);
      readline.cursorTo(process.stdout, 0);
      process.stdout.write(`... ${evt.message} (${index})`);
    });
    const { domain } = await client.deployLocalProject({
      cwd: __dirname,
      env: {
        SYNC_SERVICE_SID: syncServiceSid,
        SYNC_LOG_LIST_NAME,
        SYNC_SMS_LIST_NAME,
        SYNC_LIST_ITEM_TTL,
      },
      pkgJson: { name: this.functionsServiceName, dependencies: {} },
      serviceName: this.functionsServiceName,
      functionsEnv: 'dev',
      functionsFolderName: FUNCTIONS_FOLDER,
      noAssets: true,
      force: true,
    });
    if (index > 0) {
      readline.clearLine(process.stdout, -1);
      readline.cursorTo(process.stdout, 0);
    }

    this.serviceDomain = `https://${domain}`;
    this.smsEndpoint = `https://${domain}${FUNCTIONS_SMS_HANDLER}`;
    this.logEndpoint = `https://${domain}${FUNCTIONS_LOG_HANDLER}`;
  }

  async initializeSync() {
    const client = getClient();
    const services = await client.sync.services.list();
    let srv = services.find((item) => item.friendlyName === this.syncServiceName);
    if (!srv) {
      srv = await client.sync.services.create({ friendlyName: this.syncServiceName });
    }
    await prepareSyncList(srv, SYNC_SMS_LIST_NAME);
    await prepareSyncList(srv, SYNC_LOG_LIST_NAME);
    return srv;
  }

  async initialize() {
    if (this.ready) {
      throw new Error('Already initialized');
    }
    // PREPARE SYNC
    const sync = await this.initializeSync();
    // DEPLOY FUNCTIONS
    await this.initializeFunctions(sync.sid);
    // SYNC LISTS
    const syncClient = new Client(createSyncToken(sync.sid));
    const syncListForSMS = await syncClient.list(SYNC_SMS_LIST_NAME);
    syncListForSMS.on('itemAdded', ({ item }) => {
      this.emit('sms', sms2event(item.data));
    });
    const syncListForLogs = await syncClient.list(SYNC_LOG_LIST_NAME);
    syncListForLogs.on('itemAdded', ({ item }) => {
      this.emit('log', { date: new Date(), data: item.data });
    });
    // ready
    this.ready = true;
  }

  getDescription() {
    return `SYNC Handler: ${this.serviceDomain}
  SMS Endpoint: ${this.smsEndpoint}
  LOG Endpoint: ${this.logEndpoint}`;
  }

  getPhoneNumberWebhook() {
    return this.smsEndpoint;
  }
};
