FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

COPY OrderService/*.csproj OrderService/
COPY Application/*.csproj Application/
COPY Infrasctructure/*.csproj Infrasctructure/
COPY Domain/*.csproj Domain/

RUN dotnet restore OrderService/OrderService.csproj

COPY . ./

COPY OrderService/. OrderService/
COPY Application/. Application/
COPY Infrasctructure/. Infrasctructure/
COPY Domain/. Domain/

WORKDIR /app/OrderService
RUN dotnet publish -c Release -o /out

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

COPY --from=build /out .

ENTRYPOINT ["dotnet", "OrderService.dll"]