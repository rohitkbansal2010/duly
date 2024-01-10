const readline = require('readline');
const dateFormat = require('dateformat');

module.exports = class Terminal {
  constructor() {
    this.closeHandler = undefined;
    this.smsHandler = undefined;
  }

  initialize() {
    this.rl = readline.createInterface({
      input: process.stdin,
      output: process.stdout,
    });
    this.rl.on('close', () => {
      console.log('\nSMS Session finished');
      this.closeHandler && this.closeHandler();
    });
    this.rl.on('line', async (line) => this.processInputLine(line));
  }

  async processInputLine(line) {
    const matches = line.match(/^\/to:\s?(\+\d{11})$/);
    if (matches) {
      this.smsHandler.setTo(matches[1]);
      this.info(`\n${this.smsHandler.getDescription()}\n`);
      this.prompt();
      return;
    }
    this.rl.pause();
    try {
      const { date, to, body } = await this.smsHandler.send(line);
      console.log(`\x1b[32m${dateFormat(date, 'HH:MM:ss')} >> ${to}:\x1b[0m ${body} \n`);
    } catch (error) {
      console.log(error);
    }
    this.prompt();
  }

  prompt() {
    if (this.smsHandler.to) {
      this.rl.setPrompt('SMS: ');
      this.rl.prompt();
    } else {
      this.rl.resume();
    }
  }

  incomingSMS({ date, body, from }) {
    readline.clearLine(process.stdout, 0);
    readline.moveCursor(process.stdout, -1000, 0);
    console.log(`\x1b[93m${dateFormat(date, 'HH:MM:ss')} << ${from}:\x1b[0m ${body}\n`);
    if (this.smsHandler.to !== from) {
      this.smsHandler.setTo(from);
      this.info(`\n${this.smsHandler.getDescription()}\n`);
    }
    this.prompt();
  }

  incomingLog(data) {
    readline.clearLine(process.stdout, 0);
    readline.moveCursor(process.stdout, -1000, 0);
    console.log('----> LOG');
    console.log(data);
    this.prompt();
  }

  // eslint-disable-next-line class-methods-use-this
  info(msg) {
    console.log(msg);
  }
};
