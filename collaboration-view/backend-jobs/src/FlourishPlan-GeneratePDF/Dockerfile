#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS installer-env
#
## Build requires 3.1 SDK
#COPY --from=mcr.microsoft.com/dotnet/core/sdk:3.1 /usr/share/dotnet /usr/share/dotnet
#WORKDIR /app
#
#
#COPY . /src/dotnet-function-app
#RUN cd /src/dotnet-function-app && \
#    mkdir -p /home/site/wwwroot && \
#    dotnet publish ./FlourishPlan-GeneratePDF.csproj --output /home/site/wwwroot
#
### FROM mcr.microsoft.com/azure-functions/dotnet:4-appservice
#FROM mcr.microsoft.com/azure-functions/dotnet:3.0
#ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
#    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
#
#COPY --from=installer-env ["/home/site/wwwroot", "/home/site/wwwroot"]


FROM mcr.microsoft.com/dotnet/aspnet:5.0

EXPOSE 80

WORKDIR /app
COPY . .
RUN find . -type f -iname \*.pdb -delete
CMD ["dotnet", "FlourishPlan-GeneratePDF.dll", "vaultConfigurationKey=ApplicationSettings:KeyVaultHostUrl"]

#FROM mcr.microsoft.com/dotnet/sdk:5.0 AS installer-env
#
## Build requires 3.1 SDK
#COPY --from=mcr.microsoft.com/dotnet/core/sdk:3.1 /usr/share/dotnet /usr/share/dotnet
#WORKDIR /app
#
#COPY . .
#
#FROM mcr.microsoft.com/azure-functions/dotnet-isolated:3.0-dotnet-isolated5.0 AS image2
#ENV AzureWebJobsScriptRoot=/app \
#    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
#
#COPY --from=installer-env . .
