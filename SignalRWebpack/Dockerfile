# Base stage for runtime environment
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

COPY ./bin/Publish /app
WORKDIR /app

ENTRYPOINT ["dotnet", "SignalRWebpack.dll"]
