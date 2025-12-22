# Reporting & Analytics - Backend Specification

## Document Information
| Field | Value |
|-------|-------|
| Feature | Reporting & Analytics |
| Version | 1.0 |
| Date | 2025-12-22 |
| Status | Final |

---

## 1. Overview

### 1.1 Purpose
The Reporting & Analytics module provides comprehensive business intelligence capabilities, including automated report generation, customizable dashboards, and statistical analysis for the EventManagementPlatform system.

### 1.2 Scope
This specification covers all backend requirements for report generation, dashboard management, statistical calculations, data export, and business intelligence features.

### 1.3 Technology Stack
- **Framework**: .NET 8.0
- **Database**: SQL Server Express (via Entity Framework Core)
- **Cloud**: Azure App Service, Azure SQL Database
- **AI Integration**: Azure AI Services for predictive analytics and insights
- **Messaging**: MediatR for CQRS pattern implementation
- **Export Libraries**: iTextSharp (PDF), EPPlus (Excel)
- **Charting**: Chart.js data preparation

---

## 2. Domain Model

### 2.1 Aggregate: Report
The Report aggregate manages report definitions and generated report instances.

#### 2.1.1 Entity Properties
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| ReportId | Guid | Yes | Unique identifier |
| ReportName | string | Yes | Report name (max 200 chars) |
| ReportType | ReportType | Yes | Type of report |
| Description | string | No | Report description (max 1000 chars) |
| Parameters | string | No | JSON parameters for report generation |
| ScheduleConfig | ScheduleConfig | No | Schedule configuration |
| IsScheduled | bool | Yes | Whether report is scheduled |
| CreatedBy | Guid | Yes | User who created the report |
| CreatedAt | DateTime | Yes | Creation timestamp |
| ModifiedAt | DateTime | No | Last modification timestamp |
| LastGeneratedAt | DateTime | No | Last generation timestamp |

#### 2.1.2 ReportType Enumeration
```csharp
public enum ReportType
{
    DailySummary = 0,
    WeeklySummary = 1,
    MonthlySummary = 2,
    YearlySummary = 3,
    CustomReport = 4,
    EventStatistics = 5,
    RevenueStatistics = 6,
    StaffStatistics = 7,
    EquipmentUtilization = 8,
    CustomerStatistics = 9,
    VenueStatistics = 10
}
```

### 2.2 Entity: ReportInstance
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| ReportInstanceId | Guid | Yes | Unique identifier |
| ReportId | Guid | Yes | Parent report reference |
| GeneratedAt | DateTime | Yes | Generation timestamp |
| GeneratedBy | Guid | No | User who generated (null for scheduled) |
| StartDate | DateTime | Yes | Report period start |
| EndDate | DateTime | Yes | Report period end |
| Status | ReportStatus | Yes | Generation status |
| OutputFormat | OutputFormat | Yes | Output format (PDF, Excel) |
| FileUrl | string | No | URL to generated file |
| FileSize | long | No | File size in bytes |
| ErrorMessage | string | No | Error message if failed |

#### 2.2.1 ReportStatus Enumeration
```csharp
public enum ReportStatus
{
    Queued = 0,
    Generating = 1,
    Completed = 2,
    Failed = 3,
    Exported = 4,
    Emailed = 5
}
```

#### 2.2.2 OutputFormat Enumeration
```csharp
public enum OutputFormat
{
    PDF = 0,
    Excel = 1,
    CSV = 2,
    JSON = 3
}
```

### 2.3 Aggregate: Dashboard
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| DashboardId | Guid | Yes | Unique identifier |
| Name | string | Yes | Dashboard name |
| Description | string | No | Dashboard description |
| Layout | string | Yes | JSON layout configuration |
| IsDefault | bool | Yes | Whether this is default dashboard |
| OwnerId | Guid | Yes | Dashboard owner |
| IsPublic | bool | Yes | Whether dashboard is public |
| CreatedAt | DateTime | Yes | Creation timestamp |
| ModifiedAt | DateTime | No | Last modification timestamp |
| LastViewedAt | DateTime | No | Last viewed timestamp |

### 2.4 Entity: DashboardWidget
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| WidgetId | Guid | Yes | Unique identifier |
| DashboardId | Guid | Yes | Parent dashboard reference |
| WidgetType | WidgetType | Yes | Type of widget |
| Title | string | Yes | Widget title |
| Configuration | string | Yes | JSON widget configuration |
| Position | int | Yes | Position in dashboard |
| Size | WidgetSize | Yes | Widget size |
| DataSource | string | No | Data source query/endpoint |
| RefreshInterval | int | No | Auto-refresh interval (seconds) |

#### 2.4.1 WidgetType Enumeration
```csharp
public enum WidgetType
{
    LineChart = 0,
    BarChart = 1,
    PieChart = 2,
    StatCard = 3,
    Table = 4,
    Gauge = 5,
    Heatmap = 6,
    Timeline = 7
}
```

#### 2.4.2 WidgetSize Enumeration
```csharp
public enum WidgetSize
{
    Small = 0,      // 1x1
    Medium = 1,     // 2x1
    Large = 2,      // 2x2
    Wide = 3,       // 4x1
    ExtraLarge = 4  // 4x2
}
```

### 2.5 Entity: StatisticsSnapshot
| Property | Type | Required | Description |
|----------|------|----------|-------------|
| SnapshotId | Guid | Yes | Unique identifier |
| SnapshotType | StatisticsType | Yes | Type of statistics |
| SnapshotDate | DateTime | Yes | Snapshot timestamp |
| PeriodStart | DateTime | Yes | Period start date |
| PeriodEnd | DateTime | Yes | Period end date |
| Data | string | Yes | JSON statistics data |
| CalculatedAt | DateTime | Yes | Calculation timestamp |

#### 2.5.1 StatisticsType Enumeration
```csharp
public enum StatisticsType
{
    EventStatistics = 0,
    RevenueStatistics = 1,
    StaffStatistics = 2,
    EquipmentUtilization = 3,
    CustomerStatistics = 4,
    VenueStatistics = 5
}
```

---

## 3. Domain Events

### 3.1 Report Generation Events
| Event | Trigger | Payload |
|-------|---------|---------|
| DailySummaryGenerated | Daily summary report created | ReportInstanceId, ReportDate, TotalEvents |
| WeeklySummaryGenerated | Weekly summary report created | ReportInstanceId, WeekStart, WeekEnd, TotalEvents |
| MonthlySummaryGenerated | Monthly summary report created | ReportInstanceId, Month, Year, TotalEvents |
| YearlySummaryGenerated | Yearly summary report created | ReportInstanceId, Year, TotalEvents |
| CustomReportGenerated | Custom report created | ReportInstanceId, ReportName, Parameters |

### 3.2 Report Lifecycle Events
| Event | Trigger | Payload |
|-------|---------|---------|
| ReportScheduled | Report scheduled for generation | ReportId, ScheduleConfig, NextRunDate |
| ReportExportedToPDF | Report exported to PDF | ReportInstanceId, FileUrl, FileSize |
| ReportExportedToExcel | Report exported to Excel | ReportInstanceId, FileUrl, FileSize |
| ReportEmailedToRecipient | Report emailed | ReportInstanceId, RecipientEmail, SentAt |

### 3.3 Statistics Events
| Event | Trigger | Payload |
|-------|---------|---------|
| EventStatisticsCalculated | Event statistics computed | SnapshotId, TotalEvents, CompletedEvents, CancelledEvents |
| RevenueStatisticsCalculated | Revenue statistics computed | SnapshotId, TotalRevenue, AverageRevenue, TopCustomers |
| StaffStatisticsCalculated | Staff statistics computed | SnapshotId, TotalStaff, ActiveAssignments, Utilization |
| EquipmentUtilizationCalculated | Equipment usage computed | SnapshotId, TotalEquipment, UtilizationRate, TopEquipment |
| CustomerStatisticsCalculated | Customer statistics computed | SnapshotId, TotalCustomers, ActiveCustomers, Retention |
| VenueStatisticsCalculated | Venue statistics computed | SnapshotId, TotalVenues, BookingRate, TopVenues |

### 3.4 Dashboard Events
| Event | Trigger | Payload |
|-------|---------|---------|
| DashboardViewed | Dashboard accessed | DashboardId, UserId, ViewedAt |
| DashboardRefreshed | Dashboard data refreshed | DashboardId, UserId, RefreshedAt |
| DashboardWidgetAdded | Widget added to dashboard | DashboardId, WidgetId, WidgetType |
| DashboardWidgetRemoved | Widget removed from dashboard | DashboardId, WidgetId |
| DashboardLayoutSaved | Dashboard layout updated | DashboardId, LayoutConfig, SavedBy |

---

## 4. API Endpoints

### 4.1 Report Management Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/reports | List all reports with pagination |
| GET | /api/reports/{reportId} | Get report by ID |
| POST | /api/reports | Create new report definition |
| PUT | /api/reports/{reportId} | Update report definition |
| DELETE | /api/reports/{reportId} | Delete report definition |
| POST | /api/reports/{reportId}/generate | Generate report instance |
| POST | /api/reports/{reportId}/schedule | Schedule report |
| DELETE | /api/reports/{reportId}/schedule | Remove schedule |

### 4.2 Report Instance Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/reports/{reportId}/instances | List report instances |
| GET | /api/reports/instances/{instanceId} | Get instance details |
| GET | /api/reports/instances/{instanceId}/download | Download report file |
| POST | /api/reports/instances/{instanceId}/export-pdf | Export to PDF |
| POST | /api/reports/instances/{instanceId}/export-excel | Export to Excel |
| POST | /api/reports/instances/{instanceId}/email | Email report |

### 4.3 Dashboard Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/dashboards | List all dashboards |
| GET | /api/dashboards/{dashboardId} | Get dashboard by ID |
| POST | /api/dashboards | Create new dashboard |
| PUT | /api/dashboards/{dashboardId} | Update dashboard |
| DELETE | /api/dashboards/{dashboardId} | Delete dashboard |
| POST | /api/dashboards/{dashboardId}/view | Record dashboard view |
| POST | /api/dashboards/{dashboardId}/refresh | Refresh dashboard data |
| POST | /api/dashboards/{dashboardId}/widgets | Add widget to dashboard |
| DELETE | /api/dashboards/{dashboardId}/widgets/{widgetId} | Remove widget |
| PUT | /api/dashboards/{dashboardId}/layout | Save dashboard layout |

### 4.4 Statistics Endpoints
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/statistics/events | Get event statistics |
| GET | /api/statistics/revenue | Get revenue statistics |
| GET | /api/statistics/staff | Get staff statistics |
| GET | /api/statistics/equipment | Get equipment utilization |
| GET | /api/statistics/customers | Get customer statistics |
| GET | /api/statistics/venues | Get venue statistics |
| POST | /api/statistics/calculate | Trigger statistics calculation |

---

## 5. Commands and Queries (CQRS)

### 5.1 Commands
```
EventManagementPlatform.Api/Features/Reports/
├── CreateReport/
│   ├── CreateReportCommand.cs
│   ├── CreateReportCommandHandler.cs
│   └── CreateReportDto.cs
├── GenerateReport/
│   ├── GenerateReportCommand.cs
│   ├── GenerateReportCommandHandler.cs
│   └── GenerateReportDto.cs
├── ScheduleReport/
│   ├── ScheduleReportCommand.cs
│   └── ScheduleReportCommandHandler.cs
├── ExportReportToPdf/
│   ├── ExportReportToPdfCommand.cs
│   └── ExportReportToPdfCommandHandler.cs
├── ExportReportToExcel/
│   ├── ExportReportToExcelCommand.cs
│   └── ExportReportToExcelCommandHandler.cs
├── EmailReport/
│   ├── EmailReportCommand.cs
│   └── EmailReportCommandHandler.cs
└── CalculateStatistics/
    ├── CalculateStatisticsCommand.cs
    └── CalculateStatisticsCommandHandler.cs

EventManagementPlatform.Api/Features/Dashboards/
├── CreateDashboard/
│   ├── CreateDashboardCommand.cs
│   ├── CreateDashboardCommandHandler.cs
│   └── CreateDashboardDto.cs
├── UpdateDashboard/
│   ├── UpdateDashboardCommand.cs
│   └── UpdateDashboardCommandHandler.cs
├── AddWidget/
│   ├── AddWidgetCommand.cs
│   ├── AddWidgetCommandHandler.cs
│   └── AddWidgetDto.cs
├── RemoveWidget/
│   ├── RemoveWidgetCommand.cs
│   └── RemoveWidgetCommandHandler.cs
└── SaveDashboardLayout/
    ├── SaveDashboardLayoutCommand.cs
    └── SaveDashboardLayoutCommandHandler.cs
```

### 5.2 Queries
```
EventManagementPlatform.Api/Features/Reports/
├── GetReports/
│   ├── GetReportsQuery.cs
│   ├── GetReportsQueryHandler.cs
│   └── ReportListDto.cs
├── GetReportById/
│   ├── GetReportByIdQuery.cs
│   ├── GetReportByIdQueryHandler.cs
│   └── ReportDetailDto.cs
├── GetReportInstances/
│   ├── GetReportInstancesQuery.cs
│   ├── GetReportInstancesQueryHandler.cs
│   └── ReportInstanceDto.cs
└── DownloadReport/
    ├── DownloadReportQuery.cs
    └── DownloadReportQueryHandler.cs

EventManagementPlatform.Api/Features/Dashboards/
├── GetDashboards/
│   ├── GetDashboardsQuery.cs
│   ├── GetDashboardsQueryHandler.cs
│   └── DashboardListDto.cs
├── GetDashboardById/
│   ├── GetDashboardByIdQuery.cs
│   ├── GetDashboardByIdQueryHandler.cs
│   └── DashboardDetailDto.cs
└── GetDashboardData/
    ├── GetDashboardDataQuery.cs
    ├── GetDashboardDataQueryHandler.cs
    └── DashboardDataDto.cs

EventManagementPlatform.Api/Features/Statistics/
├── GetEventStatistics/
│   ├── GetEventStatisticsQuery.cs
│   ├── GetEventStatisticsQueryHandler.cs
│   └── EventStatisticsDto.cs
├── GetRevenueStatistics/
│   ├── GetRevenueStatisticsQuery.cs
│   ├── GetRevenueStatisticsQueryHandler.cs
│   └── RevenueStatisticsDto.cs
└── GetCustomerStatistics/
    ├── GetCustomerStatisticsQuery.cs
    ├── GetCustomerStatisticsQueryHandler.cs
    └── CustomerStatisticsDto.cs
```

---

## 6. Business Rules

### 6.1 Report Generation Rules
| Rule ID | Description |
|---------|-------------|
| RPT-001 | Report name must be unique per user |
| RPT-002 | Report period end date must be after start date |
| RPT-003 | Scheduled reports must have valid cron expression |
| RPT-004 | Custom reports must have at least one parameter |
| RPT-005 | Report generation cannot exceed 1 year date range |

### 6.2 Dashboard Rules
| Rule ID | Description |
|---------|-------------|
| DSH-001 | Dashboard name must be unique per user |
| DSH-002 | Dashboard can have maximum 20 widgets |
| DSH-003 | Widget position must be unique within dashboard |
| DSH-004 | Only dashboard owner can modify private dashboards |
| DSH-005 | Public dashboards are read-only for non-owners |

### 6.3 Statistics Calculation Rules
| Rule ID | Description |
|---------|-------------|
| STS-001 | Statistics snapshots are created daily at midnight |
| STS-002 | Historical snapshots are retained for 2 years |
| STS-003 | Real-time statistics are cached for 5 minutes |
| STS-004 | Statistics calculations run asynchronously |
| STS-005 | Failed calculations retry 3 times with exponential backoff |

### 6.4 Export Rules
| Rule ID | Description |
|---------|-------------|
| EXP-001 | PDF exports support maximum 1000 pages |
| EXP-002 | Excel exports support maximum 100,000 rows |
| EXP-003 | Exported files are retained for 30 days |
| EXP-004 | File size cannot exceed 50MB |
| EXP-005 | Export jobs timeout after 10 minutes |

---

## 7. Azure Integration

### 7.1 Azure AI Services
| Service | Purpose |
|---------|---------|
| Azure OpenAI | Generate natural language insights from data |
| Azure Cognitive Services | Anomaly detection in trends |
| Azure Machine Learning | Predictive analytics for forecasting |

### 7.2 Azure Infrastructure
| Service | Purpose |
|---------|---------|
| Azure App Service | Host API application |
| Azure SQL Database | Store reports and statistics |
| Azure Blob Storage | Store generated report files |
| Azure Service Bus | Queue report generation jobs |
| Azure Functions | Scheduled report generation |
| Azure Application Insights | Performance monitoring |
| Azure Cache for Redis | Cache statistics and dashboards |

---

## 8. Data Transfer Objects (DTOs)

### 8.1 CreateReportDto
```csharp
public record CreateReportDto(
    string ReportName,
    ReportType ReportType,
    string? Description,
    string? Parameters,
    bool IsScheduled,
    ScheduleConfig? ScheduleConfig
);
```

### 8.2 ReportDetailDto
```csharp
public record ReportDetailDto(
    Guid ReportId,
    string ReportName,
    string ReportType,
    string? Description,
    string? Parameters,
    bool IsScheduled,
    ScheduleConfig? ScheduleConfig,
    DateTime CreatedAt,
    DateTime? LastGeneratedAt,
    IEnumerable<ReportInstanceDto> RecentInstances
);
```

### 8.3 DashboardDetailDto
```csharp
public record DashboardDetailDto(
    Guid DashboardId,
    string Name,
    string? Description,
    string Layout,
    bool IsDefault,
    bool IsPublic,
    Guid OwnerId,
    string OwnerName,
    DateTime CreatedAt,
    DateTime? ModifiedAt,
    IEnumerable<DashboardWidgetDto> Widgets
);
```

### 8.4 EventStatisticsDto
```csharp
public record EventStatisticsDto(
    int TotalEvents,
    int CompletedEvents,
    int CancelledEvents,
    int PendingEvents,
    double CompletionRate,
    double CancellationRate,
    IEnumerable<TrendDataPoint> Trend
);
```

### 8.5 RevenueStatisticsDto
```csharp
public record RevenueStatisticsDto(
    decimal TotalRevenue,
    decimal AverageRevenuePerEvent,
    decimal ProjectedRevenue,
    IEnumerable<TopCustomer> TopCustomers,
    IEnumerable<RevenueByMonth> MonthlyBreakdown
);
```

---

## 9. Validation Requirements

### 9.1 Input Validation
| Field | Validation Rules |
|-------|-----------------|
| ReportName | Required, 1-200 characters, unique per user |
| ReportType | Required, valid enum value |
| StartDate | Required, valid date |
| EndDate | Required, must be after StartDate, max 1 year range |
| DashboardName | Required, 1-100 characters |
| WidgetType | Required, valid enum value |

### 9.2 FluentValidation Implementation
```csharp
public class GenerateReportCommandValidator : AbstractValidator<GenerateReportCommand>
{
    public GenerateReportCommandValidator()
    {
        RuleFor(x => x.ReportId)
            .NotEmpty();

        RuleFor(x => x.StartDate)
            .NotEmpty()
            .LessThan(x => x.EndDate)
            .WithMessage("Start date must be before end date");

        RuleFor(x => x.EndDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateTime.UtcNow)
            .WithMessage("End date cannot be in the future");

        RuleFor(x => x)
            .Must(x => (x.EndDate - x.StartDate).TotalDays <= 365)
            .WithMessage("Report period cannot exceed 1 year");

        RuleFor(x => x.OutputFormat)
            .IsInEnum();
    }
}
```

---

## 10. Security Requirements

### 10.1 Authentication
- All endpoints require JWT Bearer token authentication
- Tokens issued by Azure AD B2C or internal identity service

### 10.2 Authorization
| Role | Permissions |
|------|-------------|
| Customer | View own reports, View public dashboards |
| Staff | Generate reports, View all dashboards |
| Manager | All report operations, Create dashboards, View statistics |
| Admin | Full access including system reports |

### 10.3 Data Access Control
- Users can only view their own private reports
- Public dashboards are read-only for non-owners
- Statistics are filtered based on user permissions
- Exported files require authentication to download

---

## 11. Performance Requirements

| Metric | Requirement |
|--------|-------------|
| Report Generation | < 30s for 95% of reports |
| Dashboard Load | < 1s for initial load |
| Statistics Query | < 500ms for real-time stats |
| Export to PDF | < 10s for 100 pages |
| Export to Excel | < 15s for 10,000 rows |
| Concurrent Report Generation | Support 50 concurrent jobs |

---

## 12. Error Handling

### 12.1 Error Response Format
```json
{
    "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
    "title": "Bad Request",
    "status": 400,
    "detail": "Report period cannot exceed 1 year",
    "instance": "/api/reports/generate",
    "errors": {
        "DateRange": ["Report period cannot exceed 1 year"]
    }
}
```

### 12.2 Exception Types
| Exception | HTTP Status | Description |
|-----------|-------------|-------------|
| ValidationException | 400 | Input validation failed |
| NotFoundException | 404 | Report/Dashboard not found |
| ReportGenerationException | 500 | Report generation failed |
| ExportException | 500 | Export operation failed |
| UnauthorizedException | 401 | Authentication required |
| ForbiddenException | 403 | Insufficient permissions |

---

## 13. Background Jobs

### 13.1 Scheduled Jobs
| Job | Schedule | Purpose |
|-----|----------|---------|
| DailyReportGenerator | Daily at 1:00 AM | Generate daily summary reports |
| WeeklyReportGenerator | Monday at 2:00 AM | Generate weekly summary reports |
| MonthlyReportGenerator | 1st of month at 3:00 AM | Generate monthly summary reports |
| StatisticsCalculator | Every hour | Calculate and snapshot statistics |
| FileCleanup | Daily at 4:00 AM | Remove old exported files |

### 13.2 Queue Processing
| Queue | Purpose | Concurrency |
|-------|---------|-------------|
| report-generation | Process report generation requests | 10 workers |
| export-jobs | Process export to PDF/Excel | 5 workers |
| email-delivery | Send report emails | 3 workers |
| statistics-calculation | Calculate statistics snapshots | 5 workers |

---

## 14. Testing Requirements

### 14.1 Unit Tests
- Test all command handlers
- Test all query handlers
- Test report generation logic
- Test statistics calculations
- Test validation rules

### 14.2 Integration Tests
- Test API endpoints
- Test database operations
- Test Azure Blob Storage integration
- Test report export functionality

### 14.3 Test Coverage
- Minimum 80% code coverage
- 100% coverage for business rules
- 100% coverage for statistics calculations

---

## 15. Appendices

### 15.1 Related Documents
- [Frontend Specification](./frontend-spec.md)
- [Use Case Diagram](./diagrams/use-case.drawio)
- [C4 Diagrams](./diagrams/c4-context.drawio)
- [Class Diagram](./plantuml/class-diagram.plantuml)
- [Sequence Diagrams](./plantuml/sequence-diagram.plantuml)

### 15.2 Revision History
| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial specification |
