#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["API/System API/Duly.Education.System.Api/Duly.Education.System.Api.csproj", "API/System API/Duly.Education.System.Api/"]
COPY ["Common/Infrastructure/Infrastructure.csproj", "Common/Infrastructure/"]
COPY ["Common/Security/Security.csproj", "Common/Security/"]
COPY ["Common/Annotations/Annotations.csproj", "Common/Annotations/"]
COPY ["API/Common/Duly.Api.Common/Duly.Api.Common.csproj", "API/Common/Duly.Api.Common/"]
COPY ["Contracts/Contracts/Contracts.csproj", "Contracts/Contracts/"]
RUN dotnet restore "API/System API/Duly.Education.System.Api/Duly.Education.System.Api.csproj"
COPY . .
WORKDIR "/src/API/System API/Duly.Education.System.Api"
RUN dotnet build "Duly.Education.System.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Duly.Education.System.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Duly.Education.System.Api.dll"]