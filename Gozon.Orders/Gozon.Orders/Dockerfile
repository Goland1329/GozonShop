FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Gozon.Orders.csproj", "./"]
RUN dotnet restore "Gozon.Orders.csproj"
COPY . .
RUN dotnet build "Gozon.Orders.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gozon.Orders.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gozon.Orders.dll"]