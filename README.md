# TaskTrackerSample

Тестовое приложение состоящее из одного сервиса **Asp.NetCore WebAPI**.

Содержание:
- TaskTracker.API проект сервиса (контроллеры, реализация CQRS)
- TaskTracker.Domain проект домена приложения (модели)
- TaskTracker.Infrastructure проект инфраструктуры (репозитории, контексты базы данных)
- TaskTracker.API.Tests проект интеграционных тестов

Для запуска локально:
- нужен SQLExpress localdb
- миграции применяются автоматически из **Program.cs**
- доступ к WepAPi `http://localhost:5000/swagger/index.html`

Так же есть возможность запускать через **docker-compose**, доступ к WebApi: `http://localhost:8080/swagger/index.html`