ARG DOTNET_VERSION=7.0
FROM mcr.microsoft.com/dotnet/sdk:${DOTNET_VERSION} AS build

COPY src/ /app/
RUN dotnet publish /app/AdminNET/AdminNET.csproj -c Release -o /public

FROM mcr.microsoft.com/dotnet/aspnet:${DOTNET_VERSION}
WORKDIR /public
COPY --from=build /public .

ENTRYPOINT ["/usr/bin/dotnet", "/public/AdminNET.dll"]
