FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY CustomerService/CustomerService.Core/CustomerService.Core.csproj CustomerService/CustomerService.Core/
COPY CustomerService/CustomerService.Domain/CustomerService.Domain.csproj CustomerService/CustomerService.Domain/
COPY CustomerService/CustomerService.Application/CustomerService.Application.csproj CustomerService/CustomerService.Application/
COPY CustomerService/CustomerService.Infrastructure/CustomerService.Infrastructure.csproj CustomerService/CustomerService.Infrastructure/
COPY CustomerService/CustomerService.Api/CustomerService.Api.csproj CustomerService/CustomerService.Api/
RUN dotnet restore CustomerService/CustomerService.Api/CustomerService.Api.csproj

COPY CustomerService/ CustomerService/
RUN dotnet publish CustomerService/CustomerService.Api/CustomerService.Api.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "CustomerService.Api.dll"]
