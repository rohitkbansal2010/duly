require('dotenv').config({ path: '../.env' });

const path = require('path');
const cors = require('cors');
const express = require('express');
const twilio = require('twilio');
const opn = require('open');
const env = require('./utils/environments')('../');

const PORT = process.env.PORT || 4000;
const LOGS_PAGE_SIZE = 50;

const twilioClients = {};
const twilioFactory = (stage) => {
  if (!twilioClients[stage]) {
    const { TWILIO_API_KEY, TWILIO_API_SECRET, TWILIO_ACCOUNT_SID } = env.get(stage);
    twilioClients[stage] = twilio(
      TWILIO_API_KEY, TWILIO_API_SECRET, { accountSid: TWILIO_ACCOUNT_SID },
    );
  }
  return twilioClients[stage];
};

const app = express();

app.use(cors());
app.use(express.urlencoded({ extended: true }));
app.use(express.static(path.join(__dirname, '../frontend/build')));

const listener = app.listen(PORT, () => {
  const { port } = listener.address();
  console.log(`STARTED on port ${port}`); // eslint-disable-line no-console
  if (process.argv.includes('--browser')) {
    console.log('... opening browser'); // eslint-disable-line no-console
    opn(`http://localhost:${port}`).catch((error) => console.log(error)); // eslint-disable-line no-console
  }
});

app.get('/logs', async (req, res) => {
  const {
    func, environment, service, stage,
  } = req.query;
  try {
    const client = twilioFactory(stage);
    const data = await client.serverless.services(service).environments(environment).logs
      .page({
        functionSid: func,
        pageSize: LOGS_PAGE_SIZE,
      });
    return res.json(data);
  } catch (error) {
    return res.status(500).json({ message: error.message });
  }
});

app.get('/logs-page', async (req, res) => {
  const {
    link, environment, service, stage,
  } = req.query;
  const client = twilioFactory(stage);
  const data = await client.serverless.services(service).environments(environment).logs
    .getPage(link);
  return res.json(data);
});

app.get('/stages', async (req, res) => {
  const options = env.listTypes().map((value) => ({ sid: value, friendlyName: value }));
  return res.json(options);
});

app.get('/services', async (req, res) => {
  const { stage } = req.query;
  try {
    const client = twilioFactory(stage);
    const list = await client.serverless.services.list();
    return res.json(list);
  } catch (error) {
    return res.json({ error: error.message });
  }
});

app.get('/environments', async (req, res) => {
  const { stage, service } = req.query;
  try {
    const client = twilioFactory(stage);
    const list = await client.serverless.services(service).environments.list();
    return res.json(list);
  } catch (error) {
    return res.json({ error: error.message });
  }
});

app.get('/functions', async (req, res) => {
  const { stage, service } = req.query;
  try {
    const client = twilioFactory(stage);
    const list = await client.serverless.services(service).functions.list();
    return res.json(list);
  } catch (error) {
    return res.status(500).json({ message: error.message });
  }
});
