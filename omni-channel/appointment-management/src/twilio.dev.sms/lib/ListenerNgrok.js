const ngrok = require('ngrok');
const LocalListener = require('./ListenerLocal');

module.exports = class ListenerNgrok extends LocalListener {
  constructor(port, authToken) {
    super(port);
    this.ngrokAuthToken = authToken;
  }

  async initialize() {
    await super.initialize();
    this.appEndpoint = await ngrok.connect({
      addr: this.appPort,
      authtoken: this.ngrokAuthToken,
    });
  }

  getTitle() {
    return `Local Listener with NGROK Tunnel: ${this.appEndpoint}`;
  }
};
