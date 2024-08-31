FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app

ARG CERTIFICATE_PATH
COPY $CERTIFICATE_PATH /https/aspnetapp.pfx

WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ENV ASPNETCORE_URLS="https://+;http://+"
ENV ASPNETCORE_HTTPS_PORT=8081
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=/https/aspnetapp.pfx
ENV ASPNETCORE_Kestrel__Certificates__Default__Password=$CERTIFICATE_PASSWORD
ENV ASPNETCORE_ENVIRONMENT=Production

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY *.sln .
COPY . .
RUN dotnet restore

COPY . .
WORKDIR /src/PictureLibrary.Api
RUN dotnet build -c "$BUILD_CONFIGURATION" -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish ./PictureLibrary.Api.csproj -c "$BUILD_CONFIGURATION" -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PictureLibrary.Api.dll"]