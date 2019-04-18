FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Rabbitproxy/Rabbitproxy.csproj", "Rabbitproxy/"]
RUN dotnet restore "Rabbitproxy/Rabbitproxy.csproj"
COPY . .
WORKDIR "/src/Rabbitproxy"
RUN dotnet build "Rabbitproxy.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Rabbitproxy.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
RUN ls
ENTRYPOINT ["dotnet", "Rabbitproxy.dll"]