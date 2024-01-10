/** EXAMPLE WEBHOOK REQUEST {
    ToCountry: 'US',
    ToState: 'NC',
    SmsMessageSid: 'SM1b52dcaf4ce8afa222c094687dcdc96a',
    NumMedia: '0',
    ToCity: '',
    FromZip: '',
    SmsSid: 'SM1b52dcaf4ce8afa222c094687dcdc96a',
    FromState: 'FL',
    SmsStatus: 'received',
    FromCity: '',
    Body: 'Welcome SMS with Session Page Link',
    FromCountry: 'US',
    To: '+19108074102',
    ToZip: '',
    AddOns:'{"status":"successful","message":null,"code":null,"results":{}}',
    NumSegments: '1',
    MessageSid: 'SM1b52dcaf4ce8afa222c094687dcdc96a',
    AccountSid: 'AC294a161c0c8e559028c2d0bd89ec8350',
    From: '+19045605590',
    ApiVersion: '2010-04-01'
  } */
function sms2event(message) {
  const { To: to, From: from, Body: body } = message;
  return {
    date: new Date(), to, from, body,
  };
}

module.exports = {
  sms2event,
};
