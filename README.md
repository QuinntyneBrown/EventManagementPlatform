# Event Management Platform

A comprehensive event management system built with .NET 9 and Angular 21, featuring a clean architecture approach with CQRS pattern implementation.

## Architecture

The solution follows Clean Architecture principles with clear separation of concerns:

- **API Layer** ([EventManagementPlatform.Api](src/EventManagementPlatform.Api)): REST API using ASP.NET Core with MediatR for CQRS pattern
- **Core Layer** ([EventManagementPlatform.Core](src/EventManagementPlatform.Core)): Domain models, interfaces, and business logic
- **Infrastructure Layer** ([EventManagementPlatform.Infrastructure](src/EventManagementPlatform.Infrastructure)): Data access with Entity Framework Core
- **Web Application** ([EventManagementPlatform.WebApp](src/EventManagementPlatform.WebApp)): Angular 21 frontend with Angular Material

## Features

### Domain Entities

- **Events**: Manage event creation, scheduling, and status tracking
  - Event types, dates, descriptions
  - Status workflow (Planned, InProgress, Completed, Cancelled)
  - Customer and venue associations

- **Customers**: Customer/client management
  - Individual and corporate customer types
  - Contact information and billing addresses
  - Status tracking (Active, Inactive, Blocked)

- **Venues**: Venue management and capacity tracking
  - Multiple venue types (Conference Center, Hotel, Theater, etc.)
  - Capacity management (max, seated, standing)
  - Contact information and location details

- **Staff**: Staff member management and assignment
  - Multiple roles (Manager, Coordinator, Technician, etc.)
  - Skill tracking and status management
  - Contact and rate information

- **Equipment**: Equipment inventory and tracking
  - Categories (Audio, Video, Lighting, Staging, etc.)
  - Condition and status tracking
  - Availability management

### Backend Features

- **CQRS Pattern**: Commands and queries separated using MediatR
- **Validation**: FluentValidation for request validation
- **Authentication**: JWT-based authentication with refresh tokens
- **Authorization**: Role-based access control
- **API Documentation**: Swagger/OpenAPI integration
- **Database**: Entity Framework Core with SQL Server

### Frontend Features

- **Angular 21**: Modern Angular with standalone components
- **Angular Material**: Comprehensive Material Design component library
- **Reactive Forms**: Type-safe form handling
- **HTTP Interceptors**: Authentication and error handling
- **Guards**: Route protection with auth and role guards
- **Services**: Feature-based service architecture
- **Shared Components**: Reusable UI components (page headers, status badges, loading spinners, etc.)
- **Storybook**: Component documentation and testing
- **Playwright**: End-to-end testing

## Technology Stack

### Backend

- .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- MediatR (CQRS)
- FluentValidation
- JWT Authentication

### Frontend

- Angular 21
- Angular Material 21
- RxJS 7.8
- TypeScript 5.9
- Storybook 10
- Playwright 1.57

## Project Structure

```
EventManagementPlatform/
├── src/
│   ├── EventManagementPlatform.Api/          # REST API
│   │   ├── Controllers/                      # API controllers
│   │   ├── Features/                         # CQRS features by domain
│   │   │   ├── Customers/
│   │   │   ├── Equipment/
│   │   │   ├── Events/
│   │   │   ├── Identity/
│   │   │   ├── Staff/
│   │   │   └── Venues/
│   │   └── ConfigureServices.cs
│   ├── EventManagementPlatform.Core/         # Domain layer
│   │   ├── Model/                            # Domain aggregates
│   │   │   ├── CustomerAggregate/
│   │   │   ├── EquipmentAggregate/
│   │   │   ├── EventAggregate/
│   │   │   ├── StaffAggregate/
│   │   │   ├── UserAggregate/
│   │   │   └── VenueAggregate/
│   │   └── Services/                         # Core services
│   ├── EventManagementPlatform.Infrastructure/  # Data access
│   │   ├── Configurations/                   # EF Core configurations
│   │   └── EventManagementPlatformDbContext.cs
│   └── EventManagementPlatform.WebApp/       # Angular frontend
│       └── projects/event-management-platform/
│           └── src/app/
│               ├── core/                     # Core services, guards, interceptors
│               ├── features/                 # Feature modules
│               │   ├── customers/
│               │   ├── equipment/
│               │   ├── events/
│               │   ├── staff/
│               │   └── venues/
│               └── shared/                   # Shared components
└── tests/
    ├── EventManagementPlatform.Api.Tests/
    ├── EventManagementPlatform.Core.Tests/
    └── EventManagementPlatform.Infrastructure.Tests/
```

## Getting Started

### Prerequisites

- .NET 9 SDK
- Node.js 18+ and npm
- SQL Server (LocalDB or full installation)

### Backend Setup

1. Navigate to the API project:
   ```bash
   cd src/EventManagementPlatform.Api
   ```

2. Update the database connection string in `appsettings.json`

3. Apply database migrations:
   ```bash
   dotnet ef database update
   ```

4. Run the API:
   ```bash
   dotnet run
   ```

The API will be available at `https://localhost:5001` (or the configured port) with Swagger UI at `/swagger`.

### Frontend Setup

1. Navigate to the web application:
   ```bash
   cd src/EventManagementPlatform.WebApp
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```

The application will be available at `http://localhost:4200`.

### Running Tests

**Backend tests:**
```bash
dotnet test
```

**Frontend tests:**
```bash
cd src/EventManagementPlatform.WebApp
npm test
```

**E2E tests:**
```bash
cd src/EventManagementPlatform.WebApp
npm run e2e
```

**Storybook:**
```bash
cd src/EventManagementPlatform.WebApp
npm run storybook
```

## API Endpoints

All endpoints are documented in Swagger UI. Key endpoint groups:

- `/api/customers` - Customer management
- `/api/events` - Event management
- `/api/venues` - Venue management
- `/api/staff` - Staff management
- `/api/equipment` - Equipment management
- `/api/identity` - Authentication (login, register, refresh token)

## Authentication

The application uses JWT (JSON Web Tokens) for authentication:

1. Register or login via `/api/identity/register` or `/api/identity/authenticate`
2. Include the JWT token in the `Authorization` header: `Bearer {token}`
3. Use refresh tokens to obtain new access tokens when expired

## Development

### CQRS Pattern

Each feature follows the CQRS pattern with:
- **Commands**: Create, Update, Delete operations
- **Queries**: Read operations (Get by ID, Get list)
- **Handlers**: Process commands and queries
- **Validators**: FluentValidation for command validation
- **DTOs**: Data transfer objects for API responses

Example structure for a feature:
```
Features/Customers/
├── CreateCustomer/
│   ├── CreateCustomerCommand.cs
│   ├── CreateCustomerCommandHandler.cs
│   ├── CreateCustomerCommandValidator.cs
│   └── CreateCustomerResponse.cs
├── GetCustomers/
├── GetCustomerById/
├── UpdateCustomer/
└── DeleteCustomer/
```

## License

Copyright (c) Quinntyne Brown. All Rights Reserved.
Licensed under the MIT License. See License.txt in the project root for license information.
