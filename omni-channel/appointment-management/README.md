# Introduction

The Appointments Management solution is group of components responsible for processing, orchestration
and post-back handling of appointments management workflow. The solution group contains the following
projects:

- Worflow Orchestrator - a hosted service component with background task logic that implements the
  [Worker Service](https://docs.microsoft.com/en-us/dotnet/core/extensions/workers) interface;
- Postback Handler - an [isolated process functions](https://docs.microsoft.com/en-us/azure/azure-functions/dotnet-isolated-process-guide)
  responsible for tracking and analysis of postback events published by Communication Hub Notification
  Processor microservice;
- Database - contains the NGDP database additional artefacts required for Workflow Orchestrator and Postback handler.
- Twilio - a group of [Twilio Functions](https://www.twilio.com/docs/runtime/functions) for preparation
  and processing of SMS via Twilio platform.

Technology stack: .NET Core, Serilog, Dapper, Hangfire, Azure Functions, Azure Service Bus, Twilio Functions

# Build and Test

1. Install [.NET Core 5.0 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/5.0) and
   [Azure Functions Core Tools v.3](https://www.npmjs.com/package/azure-functions-core-tools);
2. Clone the project;
3. Adjust the connection strings and other configuration parameters in `local.settings.json`
   and `appsettings.*.json` files, depends on C# project type;
4. Open command prompt and go to ~\appointment-management\src\orchestrator\Workflow\ and run `dotnet run`;
5. Open command prompt and go to ~\appointment-management\src\orchestrator\Postback\ and run `func start`.

To build and deploy the database artefacts, open the project in Visual Studio, then open "Publish Database"
dialog, choose target connection and enter a database name. Push button "Load values" and "Publish".

For Twilio Functions components descriptions, look at separate instruction inside corresponding solution
sub-folders.

## Deploy

The following actions need to be performed during deployment of Postback component:

1. Provision Azure Key Vault to store service secret configuration parameters (database connection
   string, Communication Hub Ingestion API parameters and others);
2. Do not move into key vault "ServiceBusConnection" and "SourceQueue" configuration parameters,
   they must be setup only as environment variables;
3. Use `KeyVaultHostUrl` environment variable to pass the URL of Azure Key Vault;
4. Setup and configure Azure Service Bus binding in KEDA in case of AKS hosting model.
