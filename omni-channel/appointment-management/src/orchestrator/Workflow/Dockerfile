FROM mcr.microsoft.com/dotnet/aspnet:5.0

EXPOSE 80

WORKDIR /app
COPY . .
RUN find . -type f -iname \*.pdb -delete

CMD ["dotnet", "Duly.OmniChannel.Orchestrator.Appointment.Workflow.dll", "vaultConfigurationKey=ApplicationSettings:KeyVaultHostUrl"]