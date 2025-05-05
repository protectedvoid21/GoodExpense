FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GoodExpense.API/GoodExpense.API.csproj", "GoodExpense.API/"]
COPY ["GoodExpense.Common.Application/GoodExpense.Common.Application.csproj", "GoodExpense.Common.Application/"]
COPY ["GoodExpense.Common.Domain/GoodExpense.Common.Domain.csproj", "GoodExpense.Common.Domain/"]
COPY ["GoodExpense.Domain/GoodExpense.Domain.csproj", "GoodExpense.Domain/"]
RUN dotnet restore "GoodExpense.API/GoodExpense.API.csproj"
COPY . .
WORKDIR "/src/GoodExpense.API"
RUN dotnet build "./GoodExpense.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GoodExpense.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoodExpense.API.dll"]
