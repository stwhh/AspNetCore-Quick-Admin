FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["QuickAdmin/QuickAdmin.csproj", "QuickAdmin/"]
RUN dotnet restore "QuickAdmin/QuickAdmin.csproj"
COPY . .
WORKDIR "/src/QuickAdmin"
RUN dotnet build "QuickAdmin.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "QuickAdmin.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "QuickAdmin.dll"]
