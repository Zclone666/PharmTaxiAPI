#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src

# установка окружения и пакетов Nuget
ARG ASPNETCORE_ENVIRONMENT
RUN set ASPNETCORE_ENVIRONMENT $ASPNETCORE_ENVIRONMENT
COPY ./nuget.config /root/.nuget/NuGet/NuGet.Config

COPY ["PharmTaxiAPI/PharmTaxiAPI.csproj", "PharmTaxiAPI/"]
RUN dotnet restore "PharmTaxiAPI/PharmTaxiAPI.csproj"
COPY . .
WORKDIR "/src/PharmTaxiAPI"
RUN dotnet build "PharmTaxiAPI.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "PharmTaxiAPI.csproj" -c Debug -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PharmTaxiAPI.dll"]
