FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["AspNetCoreQuickAdmin/AspNetCoreQuickAdmin.csproj", "AspNetCoreQuickAdmin/"]
RUN dotnet restore "AspNetCoreQuickAdmin/AspNetCoreQuickAdmin.csproj"
COPY . .
WORKDIR "/src/AspNetCoreQuickAdmin"
RUN dotnet build "AspNetCoreQuickAdmin.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AspNetCoreQuickAdmin.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AspNetCoreQuickAdmin.dll"]