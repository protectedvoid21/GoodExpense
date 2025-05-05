FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GoodExpense.Invoicing.API/GoodExpense.Invoicing.API.csproj", "GoodExpense.Invoicing.API/"]
COPY ["GoodExpense.Common.Application/GoodExpense.Common.Application.csproj", "GoodExpense.Common.Application/"]
COPY ["GoodExpense.Common.Domain/GoodExpense.Common.Domain.csproj", "GoodExpense.Common.Domain/"]
COPY ["GoodExpense.Invoicing.Domain/GoodExpense.Invoicing.Domain.csproj", "GoodExpense.Invoicing.Domain/"]
COPY ["GoodExpense.Invoicing.Application/GoodExpense.Invoicing.Application.csproj", "GoodExpense.Invoicing.Application/"]
RUN dotnet restore "GoodExpense.Invoicing.API/GoodExpense.Invoicing.API.csproj"
COPY . .
WORKDIR "/src/GoodExpense.Invoicing.API"
RUN dotnet build "./GoodExpense.Invoicing.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GoodExpense.Invoicing.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoodExpense.Invoicing.API.dll"]
