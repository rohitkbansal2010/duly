
Please, use API Key instead of `AUTH_TOKEN` for Twilio Authorization

For local development you should create `.env` from the example `.env.example`


In order to deploy to the PROD from the local machine, you should copy `.env.prd` file to `.env.local.prd` and fill all the missing values 


# npm scripts

 - npm run token-for-trigger - creates token for twilio functions invocation (valid for 24h) 
 - npm run token-for-msal - creates token to authorize callbacks from Twilio to Communication Hub
 - npm run ui-editable - will toggle ui-editable flag for Functions Service
 - npm run dev - to start local server (will watch for changes in files)
 - npm run build
 - npm run deploy - will deploy with parameters from `.env` file
 - npm run build:deploy
 - ...
