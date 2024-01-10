FROM mcr.microsoft.com/dotnet/aspnet:5.0
EXPOSE 80
EXPOSE 443

RUN apt-get update 
# Install last version of apt-utils 
RUN apt-get install -y apt-utils
# Install last version of libgdiplus 
RUN apt-get install -y libgdiplus

# Switch to TLSv1
RUN sed -i 's/MinProtocol = TLSv1.2/MinProtocol = TLSv1/' /etc/ssl/openssl.cnf \
&& sed -i 's/CipherString = DEFAULT@SECLEVEL=2/CipherString = DEFAULT@SECLEVEL=1/' /etc/ssl/openssl.cnf

WORKDIR /app
COPY . .
RUN find . -type f -iname \*.pdb -delete

# HEALTHCHECK --interval=5s --timeout=10s --retries=3 CMD curl -f http://localhost/health || exit 1

# CMD ["dotnet", "Duly.Clinic.Api.dll"]
CMD ["dotnet", "Duly.Clinic.Api.dll", "vaultConfigurationKey=ApplicationSettings:KeyVaultHostUrl"]