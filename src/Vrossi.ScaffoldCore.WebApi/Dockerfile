FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 1443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Vrossi.ScaffoldCore.WebApi/Vrossi.ScaffoldCore.WebApi.csproj", "Vrossi.ScaffoldCore.WebApi/"]
COPY ["Vrossi.ScaffoldCore.Application/Vrossi.ScaffoldCore.Application.csproj", "Vrossi.ScaffoldCore.Application/"]
COPY ["Vrossi.ScaffoldCore.Core/Vrossi.ScaffoldCore.Core.csproj", "Vrossi.ScaffoldCore.Core/"]
COPY ["Vrossi.ScaffoldCore.Common/Vrossi.ScaffoldCore.Common.csproj", "Vrossi.ScaffoldCore.Common/"]
COPY ["Vrossi.ScaffoldCore.Infrastructure/Vrossi.ScaffoldCore.Infrastructure.csproj", "Vrossi.ScaffoldCore.Infrastructure/"]
RUN dotnet restore "Vrossi.ScaffoldCore.WebApi/Vrossi.ScaffoldCore.WebApi.csproj"
COPY . .
WORKDIR "/src/Vrossi.ScaffoldCore.WebApi"
RUN dotnet build "Vrossi.ScaffoldCore.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Vrossi.ScaffoldCore.WebApi.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vrossi.ScaffoldCore.WebApi.dll"]