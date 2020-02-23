FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY ["Bookshelf.Core.csproj", "/src"]
RUN dotnet restore "Bookshelf.Core.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "Bookshelf.Core.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Bookshelf.Core.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookshelf.Core.dll"] 