FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["Directory.Build.props", "."]
COPY ["Directory.Packages.props", "."]
COPY ["webapi/cnt_gm_webapi/0 Utils/GM.WebApi.Utils/GM.WebApi.Utils.csproj", "webapi/cnt_gm_webapi/0 Utils/GM.WebApi.Utils/"]
COPY ["webapi/cnt_gm_webapi/1 Entities/GM.WebApi.Entities/GM.WebApi.Entities.csproj", "webapi/cnt_gm_webapi/1 Entities/GM.WebApi.Entities/"]
COPY ["webapi/cnt_gm_webapi/2 Infrastructure.Interfaces/GM.WebApi.Infrastructure.Interfaces/GM.WebApi.Infrastructure.Interfaces.csproj", "webapi/cnt_gm_webapi/2 Infrastructure.Interfaces/GM.WebApi.Infrastructure.Interfaces/"]
COPY ["webapi/cnt_gm_webapi/3 UseCases/GM.WebApi.UseCases/GM.WebApi.UseCases.csproj", "webapi/cnt_gm_webapi/3 UseCases/GM.WebApi.UseCases/"]
COPY ["webapi/cnt_gm_webapi/5 Infrastructure.Implementation/GM.WebApi.DataAccess.Postgres/GM.WebApi.DataAccess.Postgres.csproj", "webapi/cnt_gm_webapi/5 Infrastructure.Implementation/GM.WebApi.DataAccess.Postgres/"]
COPY ["webapi/cnt_gm_webapi/6 WebApp/GM.WebApi.WebApp.csproj", "webapi/cnt_gm_webapi/6 WebApp/"]
RUN dotnet restore "webapi/cnt_gm_webapi/6 WebApp/GM.WebApi.WebApp.csproj"

COPY . .
RUN dotnet publish "webapi/cnt_gm_webapi/6 WebApp/GM.WebApi.WebApp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "GM.WebApi.WebApp.dll"]
