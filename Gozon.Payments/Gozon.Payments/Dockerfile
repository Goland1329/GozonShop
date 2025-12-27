FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Gozon.Payments.csproj", "./"]
RUN dotnet restore "Gozon.Payments.csproj"
COPY . .
RUN dotnet build "Gozon.Payments.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Gozon.Payments.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Gozon.Payments.dll"]