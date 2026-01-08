# 🚂 TrainBooking Backend

Train Ticket Booking System (analogous to Ukrzaliznytsia) - Backend API built with .NET

## 📋 Project Overview

TrainBooking Backend is a RESTful API for an online train ticket booking system, developed on the .NET platform using Clean Architecture principles. The project provides functionality for searching routes, booking tickets, user management, and railway route administration.

## 🏗️ Architecture

The project is built following **Clean Architecture** principles and divided into four main layers:

```
TrainBooking_Backend/
├── TrainBooking.Domain/          # Business logic and entities
├── TrainBooking.Application/     # Use cases and business rules
├── TrainBooking.Infrastructure/  # Data access, external services
└── TrainBooking.WebApi/          # API Controllers, endpoints
```

### Project Layers

- **Domain** - System core with business logic, entities, value objects, and domain events
- **Application** - Services, DTOs, validation, interfaces for use cases
- **Infrastructure** - Repository implementations, DbContext, external APIs, email services
- **WebApi** - REST API controllers, middleware, Swagger configuration

## ✨ Key Features

- 🔍 **Route Search** - search for available trains by route and date
- 🎫 **Ticket Booking** - seat selection and booking processing
- 👤 **User Management** - registration, authentication, profile
- 🚆 **Train Management** - route and schedule administration
- 📧 **Email Notifications** - booking confirmations and notifications

## 🛠️ Technology Stack

- **.NET 8.0** - main framework
- **ASP.NET Core Web API** - for building RESTful API
- **Entity Framework Core** - ORM for database operations
- **SQL Server** - database management system
- **JWT Authentication** - authentication and authorization
- **AutoMapper** - object mapping
- **FluentValidation** - data validation
- **Swagger/OpenAPI** - API documentation

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/sql-server)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation

1. Clone the repository:
```bash
git clone https://github.com/MykhailivVolodymyr/TrainBooking_Backend.git
cd TrainBooking_Backend
```

2. Restore NuGet packages:
```bash
dotnet restore
```

3. Configure the database connection string in `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TrainBookingDb;Trusted_Connection=True;"
  }
}
```

4. Apply database migrations:
```bash
cd TrainBooking.WebApi
dotnet ef database update
```

5. Run the project:
```bash
dotnet run --project TrainBooking.WebApi
```


### Swagger UI

After launching the project, API documentation is available at:
```
https://localhost:5001/swagger
```

## 📁 Database Structure

### Main Tables

- **Users** - user information
- **Trains** - train data
- **Routes** - routes with stop points
- **Schedules** - trip schedules
- **Bookings** - ticket bookings
- **Tickets** - passenger tickets
- **Seats** - information about seats in carriages
- **Payments** - payments
