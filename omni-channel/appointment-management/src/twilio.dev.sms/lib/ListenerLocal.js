const EventEmitter = require('events');
const express = require('express');
const bodyParser = require('body-parser');
const publicIp = require('public-ip');
const { sms2event } = require('./listenerUtils');

const PROTOCOL = 'http';
const EVENTS_ENDPOINTS = {
  request: '/',
  sms: '/sms',
  log: '/log',
};

module.exports = class ListenerLocal extends EventEmitter {
  constructor(port = 1337) {
    super({});

    this.ready = false;
    this.appPort = port;
    this.appEndpoint = undefined;
    this.appServer = undefined;

    this.app = express();
    this.app.use(bodyParser.urlencoded({ extended: true }));
    this.app.use(bodyParser.json());
    this.app.use(express.urlencoded({ extended: true }));

    this.app.all(EVENTS_ENDPOINTS.sms, (req, res) => {
      this.emit('sms', sms2event(req.body));
      res.writeHead(200, { 'Content-Type': 'text/xml' });
      res.end('<Response></Response>');
    });

    this.app.all(EVENTS_ENDPOINTS.log, (req, res) => {
      const { body: data, query } = req;
      this.emit('log', { date: new Date(), data, query });
      res.send('');
    });

    this.app.all('/', (req, res) => {
      this.emit('request', req.body);
      res.send(`READY: ${(new Date()).toISOString()}`);
    });
  }

  async initialize() {
    if (this.ready) {
      throw new Error('Already initialized');
    }
    this.appServer = await this.app.listen(this.appPort);
    const ipAddress = await publicIp.v4();
    this.appEndpoint = `${PROTOCOL}://${ipAddress}:${this.appServer.address().port}`;
    this.ready = true;
  }

  // eslint-disable-next-line class-methods-use-this
  get endpoints() {
    return EVENTS_ENDPOINTS;
  }

  getTitle() {
    return `Local Listener: ${this.appEndpoint}`;
  }

  getDescription() {
    return `${this.getTitle()}
  SMS Endpoint: ${this.appEndpoint + this.endpoints.sms}
  LOG Endpoint: ${this.appEndpoint + this.endpoints.log}`;
  }

  getPhoneNumberWebhook() {
    return this.appEndpoint + this.endpoints.sms;
  }
};
