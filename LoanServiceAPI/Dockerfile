# Этап 1: Базовый образ для запуска (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Этап 2: Сборка (SDK)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файлы проектов для восстановления зависимостей
# Это позволяет кешировать слои и ускорять последующие сборки
COPY ["LoanServiceAPI/LoanServiceAPI.csproj", "LoanServiceAPI/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Presentation/Presentation.csproj", "Presentation/"]

# Восстанавливаем зависимости
RUN dotnet restore "LoanServiceAPI/LoanServiceAPI.csproj"

# Копируем всё остальное и собираем
COPY . .
WORKDIR "/src/LoanServiceAPI"
RUN dotnet build "LoanServiceAPI.csproj" -c Release -o /app/build

# Этап 3: Публикация
FROM build AS publish
RUN dotnet publish "LoanServiceAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Этап 4: Финальный образ
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LoanServiceAPI.dll"]