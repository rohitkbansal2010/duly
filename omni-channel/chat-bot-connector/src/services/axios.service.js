const dotenv = require('dotenv').config();
const Axios = require('axios');

const { DefaultAzureCredential } = require("@azure/identity");
const { CertificateClient } = require("@azure/keyvault-certificates");
const { SecretClient } = require("@azure/keyvault-secrets");
const https = require('https');
const { default: axios } = require('axios');

let certificateSecret, axiosInstance;

const initialize = async () => {
  const credential = new DefaultAzureCredential();
  const vaultUrl = process.env.KEY_VAULT_URL;
  const certificateName = process.env.CERTIFICATE_NAME
  const secretClient = new SecretClient(vaultUrl, credential);
  certificateSecret = await secretClient.getSecret(certificateName);

  if (!certificateSecret) {
    return console.log('Error, failed to retrieve certificate from Azure vault!');
  }

  axiosInstance = Axios.create({
    baseURL: process.env.EPIC_BASEURL,
    timeout: 60000,
    headers: {
      'Authorization': `${process.env.EPIC_TOKEN_TYPE} ${process.env.EPIC_TOKEN_2}`,
    },
    httpsAgent: new https.Agent({
      pfx: Buffer.from(certificateSecret.value, 'base64'),
      requestCert: true,
      rejectUnauthorized: false,
    }),
  });
};

initialize();


const axiosService = {
  post: async (url, params) => {
    if (!certificateSecret) {
      return console.log('Error, failed to retrieve certificate from Azure vault!');
    }

    const headers = {
      'Epic-Client-ID': url.includes('GETPATIENTREFERRALS') ? process.env.EPIC_CLIENT_ID_2 : process.env.EPIC_CLIENT_ID,
    };

    const response = await axiosInstance.post(url, params, { headers });
    return response;
  }
};

module.exports = { axiosService };