FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
EXPOSE 80

FROM Mannsminds/net5-npm:15.0 AS build
WORKDIR /src
COPY ["MannsBlog/MannsBlog.csproj", "MannsBlog/"]
COPY ["MannsBlog.Data/MannsBlog.Data.csproj", "MannsBlog.Data/"]
RUN dotnet restore "MannsBlog/MannsBlog.csproj"
COPY . .
WORKDIR "/src/MannsBlog"
RUN npm ci
RUN npm run purge

RUN dotnet build "MannsBlog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MannsBlog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MannsBlog.dll"]