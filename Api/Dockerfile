FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["BookshelfApi.csproj", "/src"]
RUN dotnet restore "BookshelfApi.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "BookshelfApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "BookshelfApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "BookshelfApi.dll"]