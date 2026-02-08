FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY BpmService/BpmService.csproj BpmService/
RUN dotnet restore BpmService/BpmService.csproj

COPY BpmService/ BpmService/
RUN dotnet publish BpmService/BpmService.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BpmService.dll"]
