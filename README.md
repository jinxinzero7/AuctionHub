# AuctionHub - Backend API для системы онлайн-аукционов

[![Статус](https://img.shields.io/badge/🔰-активная_разработка-yellowgreen)]()
[![Общий прогресс](https://img.shields.io/badge/общий_прогресс-25%25-yellow)]()

AuctionHub — это backend-система для онлайн-аукционов на ASP.NET Core, обеспечивающая работу со ставками в реальном времени через SignalR. Решает проблему конкурентных обновлений при одновременных ставках и автоматически завершает лоты по расписанию с помощью фоновых служб.

Система построена с использованием чистой архитектуры и готовится к переходу на CQRS. Включает полноценную JWT-аутентификацию, а также оптимистичную блокировку для предотвращения "гонки ставок".

## Прогресс разработки

[![Этап 0](https://img.shields.io/badge/0.Настройка_проекта-100%25-brightgreen)]()
[![Этап 1](https://img.shields.io/badge/1.CRUD_и_аутентификация-60%25-yellowgreen)]()
[![Этап 2](https://img.shields.io/badge/2.Ставки_и_конкурентность-0%25-lightgrey)]()
[![Этап 3](https://img.shields.io/badge/3.SignalR-0%25-lightgrey)]()
[![Этап 4](https://img.shields.io/badge/4.Фоновые_задачи-0%25-lightgrey)]()
[![Этап 5](https://img.shields.io/badge/5.Доработки-0%25-lightgrey)]()

## Технологии

[![.NET](https://img.shields.io/badge/.NET%209-512BD4?logo=dotnet)]()
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?logo=dotnet)]()
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?logo=postgresql)]()
[![Entity Framework](https://img.shields.io/badge/EF%20Core-512BD4?logo=dotnet)]()
[![SignalR](https://img.shields.io/badge/SignalR-FF6F00?logo=dotnet)]()
[![JWT](https://img.shields.io/badge/JWT%20Auth-black?logo=jsonwebtokens)]()
[![Background Services](https://img.shields.io/badge/Background%20Services-0052CC?logo=dotnet)]()

## Текущие задачи

- Завершение CRUD контроллера для ставок
- Внедрение паттерна Result для унификации возвращаемых значений
- Реализация DTO и маппинга между слоями приложения
- Добавление валидации с использованием FluentValidation
- Рефакторинг архитектуры на CQRS
- Реализация конкурентного контроля для ставок

## Основные эндпоинты

**Аутентификация**
- `POST /api/auth/register` - регистрация пользователя
- `POST /api/auth/login` - вход в систему (JWT)
- `GET /api/auth/me` - профиль текущего пользователя

**Лоты**
- `GET /api/lots` - список лотов с пагинацией
- `GET /api/lots/{id}` - детальная информация о лоте
- `POST /api/lots` - создание нового лота
- `PUT /api/lots/{id}` - обновление лота
- `DELETE /api/lots/{id}` - удаление лота

**Ставки**
- `GET /api/lots/{id}/bids` - история ставок по лоту
- `POST /api/lots/{id}/bids` - размещение новой ставки

## Запуск проекта

```bash
# Клонирование репозитория
git clone https://github.com/jinxinzero7/AuctionHub.git
cd AuctionHub

# Настройка базы данных - убедитесь, что PostgreSQL запущен
# Отредактируйте строку подключения в appsettings.Development.json

# Применение миграций Entity Framework
dotnet ef database update

# Запуск приложения
dotnet run
```

После запуска API будет доступно на `https://localhost:5432`. Для тестирования API используется Postman.

---

*Учебный проект, разрабатываемый на современном стеке .NET*
