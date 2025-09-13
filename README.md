# AuctionHub - Backend API для системы онлайн-аукционов

**AuctionHub** — это высокопроизводительный backend для системы онлайн-аукционов, построенный на ASP.NET Core Web API. Система предоставляет REST API для управления лотами, поддерживает работу со ставками в реальном времени с использованием SignalR, решает проблему конкурентных обновлений и выполняет фоновую обработку задач для завершения торгов.

## 🛠 Технологический стек

[![.NET](https://img.shields.io/badge/.NET%209-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![ASP.NET Core](https://img.shields.io/badge/ASP.NET%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/aspnet/core/)
[![PostgreSQL](https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white)](https://www.postgresql.org/)
[![Entity Framework](https://img.shields.io/badge/EF%20Core-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/ef/core/)
[![SignalR](https://img.shields.io/badge/SignalR-FF6F00?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/aspnet/core/signalr/)
[![JWT](https://img.shields.io/badge/JWT%20Auth-black?style=for-the-badge&logo=jsonwebtokens&logoColor=white)](https://jwt.io/)
[![Background Services](https://img.shields.io/badge/Background%20Services-0052CC?style=for-the-badge&logo=dotnet&logoColor=white)](https://learn.microsoft.com/dotnet/core/extensions/workers)

## 📊 Прогресс разработки

[![Статус](https://img.shields.io/badge/🔰-активная_разработка-yellowgreen?style=for-the-badge)](https://github.com/jinxinzero7/AuctionHub)
[![Общий прогресс](https://img.shields.io/badge/общий_прогресс-25%25-yellow?style=for-the-badge)](https://github.com/jinxinzero7/AuctionHub)

### Детализация по этапам
[![Этап 0](https://img.shields.io/badge/0.Настройка_проекта-100%25-brightgreen)](https://github.com/jinxinzero7/AuctionHub)
[![Этап 1](https://img.shields.io/badge/1.CRUD_и_аутентификация-60%25-yellowgreen)](https://github.com/jinxinzero7/AuctionHub)
[![Этап 2](https://img.shields.io/badge/2.Ставки_и_конкурентность-0%25-lightgrey)](https://github.com/jinxinzero7/AuctionHub)
[![Этап 3](https://img.shields.io/badge/3.SignalR-0%25-lightgrey)](https://github.com/jinxinzero7/AuctionHub)
[![Этап 4](https://img.shields.io/badge/4.Фоновые_задачи-0%25-lightgrey)](https://github.com/jinxinzero7/AuctionHub)
[![Этап 5](https://img.shields.io/badge/5.Доработки-0%25-lightgrey)](https://github.com/jinxinzero7/AuctionHub)

## 🚩 Реализуемые функции

- 🔑 **Аутентификация** [![100%](https://img.shields.io/badge/100%25-brightgreen)](https://github.com/jinxinzero7/AuctionHub)
- 🔄 **Управление лотами** [![10%](https://img.shields.io/badge/10%25-red)](https://github.com/jinxinzero7/AuctionHub)
- 💰 **Система ставок** [![0%](https://img.shields.io/badge/0%25-lightgrey)](https://github.com/jinxinzero7/AuctionHub)
- 🚀 **Real-time обновления** [![0%](https://img.shields.io/badge/0%25-lightgrey)](https://github.com/jinxinzero7/AuctionHub)
- ⏳ **Фоновые задачи** [![0%](https://img.shields.io/badge/0%25-lightgrey)](https://github.com/jinxinzero7/AuctionHub)

## 🎯 Ключевые особенности

- **RESTful API**: Полный CRUD для лотов и ставок.
- **Аутентификация JWT**: Безопасная регистрация и аутентификация пользователей.
- **Реальные ставки**: Мгновенные уведомления о новых ставках для всех участников аукциона через **SignalR**.
- **Конкурентный контроль**: Специально реализованный механизм для предотвращения "гонки ставок".
- **Фоновые службы**: Автоматическое определение победителей и завершение лотов по расписанию.
- **Пагинация и сортировка**: Для эффективной работы со списками лотов.

## 📋 Функциональность API

### Управление пользователями
- `POST /api/auth/register` - Регистрация нового пользователя
- `POST /api/auth/login` - Вход в систему (получение JWT-токена)
- `GET /api/auth/me` - Получение профиля текущего пользователя

### Управление лотами (требуют аутентификации)
- `GET /api/lots` - Получить список активных лотов (с пагинацией и сортировкой)
- `GET /api/lots/{id}` - Получить детальную информацию о лоте
- `POST /api/lots` - Создать новый лот
- `PUT /api/lots/{id}` - Обновить лот (только создатель, до первой ставки)
- `DELETE /api/lots/{id}` - Удалить лот (только создатель, если нет ставок)

### Система ставок
- `GET /api/lots/{id}/bids` - Получить историю ставок по лоту
- `POST /api/lots/{id}/bids` - Разместить новую ставку

### Real-time (SignalR)
- **Хаб**: `/lothub`
- **Событие**: `NewBidPosted` - Рассылка информации о новой ставке всем подписчикам лота.

## 🗄 Модели данных

Основные сущности системы:
- **User** (Пользователь): Учетные данные и списки созданных лотов и ставок.
- **Lot** (Лот): Информация о товаре на аукционе, включая текущую цену и ссылку на создателя и победителя.
- **Bid** (Ставка): Сумма, временная метка и ссылки на лот и пользователя.

## 🚦 Как запустить проект

1. **Клонируйте репозиторий**:
   ```bash
   git clone https://github.com/your-username/AuctionHub.git
   cd AuctionHub
   

2. **Настройте базу данных**:
   - Убедитесь, что у вас запущен PostgreSQL.
   - Отредактируйте строку подключения в `appsettings.Development.json`.

3. **Примените миграции EF Core**:
   ```bash
   dotnet ef database update
   ```

4. **Запустите приложение**:
   ```bash
   dotnet run
   ```

5. **Откройте Swagger**:
   - Перейдите по адресу `https://localhost:7000/swagger` (или другому порту, указанному в консоли).

---
Разработано в качестве учебного проекта по современному стеку .NET.
