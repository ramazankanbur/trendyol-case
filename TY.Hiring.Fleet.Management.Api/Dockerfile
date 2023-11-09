FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TY.Hiring.Fleet.Management.Api/TY.Hiring.Fleet.Management.Api.csproj", "TY.Hiring.Fleet.Management.Api/"]
COPY ["TY.Hiring.Fleet.Management.Service.Interface/TY.Hiring.Fleet.Management.Service.Interface.csproj", "TY.Hiring.Fleet.Management.Service.Interface/"]
COPY ["TY.Hiring.Fleet.Management.Model/TY.Hiring.Fleet.Management.Model.csproj", "TY.Hiring.Fleet.Management.Model/"]
COPY ["TY.Hiring.Fleet.Management.Data.ORM.EF/TY.Hiring.Fleet.Management.Data.ORM.EF.csproj", "TY.Hiring.Fleet.Management.Data.ORM.EF/"]
COPY ["TY.Hiring.Fleet.Management.AppConfig/TY.Hiring.Fleet.Management.AppConfig.csproj", "TY.Hiring.Fleet.Management.AppConfig/"]
COPY ["TY.Hiring.Fleet.Management.Mapper/TY.Hiring.Fleet.Management.Mapper.csproj", "TY.Hiring.Fleet.Management.Mapper/"]
COPY ["TY.Hiring.Fleet.Management.Service/TY.Hiring.Fleet.Management.Service.csproj", "TY.Hiring.Fleet.Management.Service/"]
RUN dotnet restore "TY.Hiring.Fleet.Management.Api/TY.Hiring.Fleet.Management.Api.csproj"
COPY . .
WORKDIR "/src/TY.Hiring.Fleet.Management.Api"
RUN dotnet build "TY.Hiring.Fleet.Management.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TY.Hiring.Fleet.Management.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TY.Hiring.Fleet.Management.Api.dll"]