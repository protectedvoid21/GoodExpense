FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GoodExpense.Users.API/GoodExpense.Users.API.csproj", "GoodExpense.Users.API/"]
COPY ["GoodExpense.Users.Domain/GoodExpense.Users.Domain.csproj", "GoodExpense.Users.Domain/"]
COPY ["GoodExpense.Common.Domain/GoodExpense.Common.Domain.csproj", "GoodExpense.Common.Domain/"]
COPY ["GoodExpense.Users.Application/GoodExpense.Users.Application.csproj", "GoodExpense.Users.Application/"]
COPY ["GoodExpense.Common.Application/GoodExpense.Common.Application.csproj", "GoodExpense.Common.Application/"]
RUN dotnet restore "GoodExpense.Users.API/GoodExpense.Users.API.csproj"
COPY . .
WORKDIR "/src/GoodExpense.Users.API"
RUN dotnet build "./GoodExpense.Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GoodExpense.Users.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoodExpense.Users.API.dll"]
