FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["Reads/Reads.csproj", "Reads/"]
RUN dotnet restore "Reads/Reads.csproj"
COPY . .
WORKDIR "/src/Reads"
RUN dotnet build "Reads.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Reads.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Reads.dll"]