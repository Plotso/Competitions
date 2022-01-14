#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/Common/Competitions.Common/Competitions.Common.csproj", "src/Common/Competitions.Common/"]
COPY ["src/Data/Competitions.Data.Common/Competitions.Data.Common.csproj", "src/Data/Competitions.Data.Common/"]
COPY ["src/Data/Competitions.Data.Models/Competitions.Data.Models.csproj", "src/Data/Competitions.Data.Models/"]
COPY ["src/Data/Competitions.Data/Competitions.Data.csproj", "src/Data/Competitions.Data/"]
COPY ["src/Domain/Competitions.Domain.BL/Competitions.Domain.BL.csproj", "src/Domain/Competitions.Domain.BL/"]
COPY ["src/Domain/Competitions.Domain.BL/Competitions.Domain.Mapping.csproj", "src/Domain/Competitions.Domain.Mapping/"]
COPY ["src/Web/Competitions.Web.ViewModels/Competitions.Web.ViewModels.csproj", "src/Web/Competitions.Web.ViewModels/"]
COPY ["src/Web/Competitions.Web/Competitions.Web.csproj", "src/Web/Competitions.Web/"]
RUN dotnet restore "src/Web/Competitions.Web/Competitions.Web.csproj"
COPY . .
WORKDIR "/src/src/Web/Competitions.Web"
RUN dotnet build "Competitions.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Competitions.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Competitions.Web.dll"]