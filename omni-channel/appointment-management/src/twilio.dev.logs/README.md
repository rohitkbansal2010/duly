# Application configuration in .env file

Project configuration (PORT) should be specified in `.env` file (example: `.env.example`)

# Twilio Functions configuration .env.runtime files 

You should prepare `.env.runtime` files with credentials for every Twilio Account.
For example:
 - .env.dev
 - .env.prod
 - .env.qa

Looks at the example `.env.runtime.example`


# Install dependencies and build project
  
```
npm run install && npm run build
```

# Run

```
npm run start
```
