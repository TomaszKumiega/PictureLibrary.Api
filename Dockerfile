FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

ARG CERTIFICATE_PATH

ENV ASPNETCORE_URLS="https://+;http://+"
ENV ASPNETCORE_HTTPS_PORT=8081
ENV ASPNETCORE_Kestrel__Certificates__Default__Path=$CERTIFICATE_PATH
ENV ASPNETCORE_ENVIRONMENT=Production

RUN --mount=type=secret,id=password \
    export CERT_PASSWORD=$(cat /run/secrets/password) && \
    echo "ASPNETCORE_Kestrel__Certificates__Default__Password=$CERT_PASSWORD" >> /etc/environment

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