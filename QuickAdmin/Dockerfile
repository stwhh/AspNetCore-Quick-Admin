FROM mcr.microsoft.com/dotnet/core/aspnet:3.1.5-bionic AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src

COPY *.sln .
COPY ["QuickAdmin/*.csproj", "QuickAdmin/"]
COPY ["QuickAdmin.Services/*.csproj", "QuickAdmin.Services/"]
COPY ["QuickAdmin.DAO/*.csproj", "QuickAdmin.DAO/"]
COPY ["QuickAdmin.Model/*.csproj", "QuickAdmin.Model/"]
COPY ["QuickAdmin.Common/*.csproj", "QuickAdmin.Common/"]
RUN dotnet restore

COPY . .
WORKDIR "/src/QuickAdmin"
RUN dotnet build "QuickAdmin.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "QuickAdmin.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "QuickAdmin.dll"]
