FROM mcr.microsoft.com/dotnet/aspnet:5.0
EXPOSE 80
EXPOSE 443

WORKDIR /app
COPY . .
RUN find . -type f -iname \*.pdb -delete

# HEALTHCHECK --interval=5s --timeout=10s --retries=3 CMD curl -f http://localhost/health || exit 1
# CMD ["dotnet", "Duly.CollaborationView.Resource.Api.dll"]
CMD ["dotnet", "Duly.CollaborationView.Resource.Api.dll", "vaultConfigurationKey=ApplicationSettings:KeyVaultHostUrl"]