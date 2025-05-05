FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GoodExpense.Expenses.API/GoodExpense.Expenses.API.csproj", "GoodExpense.Expenses.API/"]
COPY ["GoodExpense.Expenses.Domain/GoodExpense.Expenses.Domain.csproj", "GoodExpense.Expenses.Domain/"]
COPY ["GoodExpense.Common.Domain/GoodExpense.Common.Domain.csproj", "GoodExpense.Common.Domain/"]
COPY ["GoodExpense.Expenses.Application/GoodExpense.Expenses.Application.csproj", "GoodExpense.Expenses.Application/"]
COPY ["GoodExpense.Common.Application/GoodExpense.Common.Application.csproj", "GoodExpense.Common.Application/"]
RUN dotnet restore "GoodExpense.Expenses.API/GoodExpense.Expenses.API.csproj"
COPY . .
WORKDIR "/src/GoodExpense.Expenses.API"
RUN dotnet build "./GoodExpense.Expenses.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GoodExpense.Expenses.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GoodExpense.Expenses.API.dll"]
