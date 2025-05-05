FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GoodExpense.Notification.API/GoodExpense.Notification.API.csproj", "GoodExpense.Notification.API/"]
COPY ["GoodExpense.Notification.Domain/GoodExpense.Notification.Domain.csproj", "GoodExpense.Notification.Domain/"]
COPY ["GoodExpense.Common.Domain/GoodExpense.Common.Domain.csproj", "GoodExpense.Common.Domain/"]
COPY ["GoodExpense.Notification.Application/GoodExpense.Notification.Application.csproj", "GoodExpense.Notification.Application/"]
COPY ["GoodExpense.Common.Application/GoodExpense.Common.Application.csproj", "GoodExpense.Common.Application/"]
RUN dotnet restore "GoodExpense.Notification.API/GoodExpense.Notification.API.csproj"
COPY . .
WORKDIR "/src/GoodExpense.Notification.API"
RUN dotnet build "./GoodExpense.Notification.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GoodExpense.Notification.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoodExpense.Notification.API.dll"]
