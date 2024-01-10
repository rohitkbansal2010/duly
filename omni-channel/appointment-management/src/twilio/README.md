# Twilio Authorization

Do not use (or share) Twilio `AUTH_TOKEN`. You should use API Keys instead.

You can create Api Key through Twilio Console (https://console.twilio.com/ -> Account -> API keys & tokens)
or with the help of `twilio-cli`



# Project Structure

The project is written with typescript.
Webpack is used to prepare deployment builds. 

- src - contains the code of the project
- src/functions - handlers which will be available in the Twilio
- src/assets - assets for Twilio (messages templates, etc.)
- build - contains builds for deployment



# Twilio configuration

We are currently using:
- Functions
- Messaging Services
- Sync Service (Sync Maps)


### Functions (Services)

Name of the Functions Service is `duly-communication-hub` (corresponds to the name in the package.json)

twilio-serverless uses the name from the package.json by defaults


### Messaging Service

- `duly-communication-hub` - for sending and receiving messages
- `duly-communication-hub-dev` - for DEV environment
- `duly-communication-hub-qat` - for QAT environment
- `duly-communication-hub-uat` - for UAT environment

SID of the Messaging Service should be indicated in the .env file as `SMS_MESSAGES_FROM`

Configuration:

- Corresponding phone numbers should be attached to the Messaging Service as a sender
- Integration -> Incoming Messages -> Send a webhook: provide the url for `sms/reply` handler (example:  https://duly-communication-hub-6963.twil.io/sms/reply)


### Messaging Service Opt-Out Management

Opt-out
```
You have successfully been unsubscribed. You will not receive any more messages from this number. Reply START to resubscribe.
```

Opt-in
```
You have successfully been re-subscribed to messages from this number. Reply HELP for help. Reply STOP to unsubscribe. Msg&Data Rates May Apply.
```

Help
```
Reply STOP to unsubscribe. Reply START to re-subscribe. Msg&Data Rates May Apply.
```


### Sync Service

- `duly-communication-hub` - for storing recipients data
- `duly-communication-hub-dev` - fof DEV environment
- `duly-communication-hub-qat` - fof QAT environment
- `duly-communication-hub-uat` - fof UAT environment

SID of the Sync Service should be indicated in the .env file as `SYNC_SERVICE_SID`

Configuration:

- Create Map with the name `recipients` (Name `recipients` should be the same as a value for `SYNC_MAP_RECIPIENTS` in the `src/config.ts` file)



# Communication Hub integration

### TRIGGER - REQUEST

To start communication we are expecting POST request from the Comm Hub to the Twilio endpoints (README-ENDPOINTS.md)

POST request with JSON body of the following structure:

```
{
    to: string, // phone number in international format
    correlationToken: string,
    parameters: {
        key: string
    },
    callbackUrl?: string, // for dev environment, this value will substitute CALLBACK_URL
}
```


### TRIGGER - AUTHORIZATION

Authorization is performed based on the jwt.
Secret is shared between Twilio and Communication Hub.
The value of the secret should be specified in the value of `TRIGGER_AUTH_SECRET` of the .env file.

We are testing jwt token for following claims:

- iss: Duly-ComHub ( `AUTH_REQUIRED_ISSUER` )
- aud: Twilio (`AUTH_REQUIRED_AUDIENCE`)
- perms: trigger (`AUTH_PERMISSION_TRIGGER`),


### TRIGGER - CONFIGURATION PARAMETERS

Parameters in `.env` file

```
TRIGGER_AUTH_SECRET= !!! sensitive information !!!
```

Parameters in `/assets/config.private.js`

```
AUTH_REQUIRED_ISSUER:
AUTH_REQUIRED_AUDIENCE:
AUTH_REQUIRED_PERMISSION:
```


### CALLBACK - REQUEST

Data from Twilio to Comm Hub are passed by the "callbacks".

POST request with JSON body of the following structure:

```
{ 
    correlationToken: <provided-by-the-Comm-Hub-during-initiating-of-the-communication>
    executionId: <twilio-execution-specific-id>
    status: string
    meta: string
    time: ISO DateTime
}
```


### CALLBACK - AUTHORIZATION

MSAL authorization


### CALLBACK - CONFIGURATION PARAMETERS

Parameters in `.env` file

```
CALLBACK_URL=
CALLBACK_CLIENT_ID=
CALLBACK_CLIENT_SECRET= !!! sensitive information !!!
CALLBACK_AUTHORITY=
CALLBACK_SCOPE=
```

Parameters in `/assets/config.private.js`

```
MSAL_SYNC_DOCUMENT_NAME:
```
