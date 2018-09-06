FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 5884
EXPOSE 44320

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["simple.websocket.sample/simple.websocket.sample.csproj", "simple.websocket.sample/"]
RUN dotnet restore "simple.websocket.sample/simple.websocket.sample.csproj"
COPY . .
WORKDIR "/src/simple.websocket.sample"
RUN dotnet build "simple.websocket.sample.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "simple.websocket.sample.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "simple.websocket.sample.dll"]