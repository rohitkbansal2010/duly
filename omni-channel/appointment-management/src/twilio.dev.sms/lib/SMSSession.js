const { sendSMS } = require('./twilio');

module.exports = class SMSSession {
  constructor(from, to) {
    this.from = from;
    this.to = to;
  }

  async send(body) {
    if (!this.from || !this.to || !body) {
      throw new Error('There is no enough data to send message');
    }
    return sendSMS({ from: this.from, to: this.to, body });
  }

  setTo(phoneNumber) {
    this.to = phoneNumber;
  }

  setFrom(phoneNumber) {
    this.from = phoneNumber;
  }

  getDescription() {
    return `SMS Thread with: ${this.to || '... waiting for incoming SMS'}`;
  }
};
