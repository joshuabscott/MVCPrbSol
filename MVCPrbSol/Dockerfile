#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MVCPrbSol/MVCPrbSol.csproj", "MVCPrbSol/"]
RUN dotnet restore "MVCPrbSol/MVCPrbSol.csproj"
COPY . .
WORKDIR "/src/MVCPrbSol"
RUN dotnet build "MVCPrbSol.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MVCPrbSol.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*PORT dotnet MVCPrbSol.dll
#ENTRYPOINT ["dotnet", "MVCPrbSol.dll"]