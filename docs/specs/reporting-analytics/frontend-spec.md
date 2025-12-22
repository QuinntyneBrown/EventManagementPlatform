# Reporting & Analytics - Frontend Specification

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
This specification defines the frontend requirements for the Reporting & Analytics feature, providing users with comprehensive tools for generating reports, viewing dashboards, and analyzing business intelligence.

### 1.2 Technology Stack
- **Framework**: Angular 18+ (latest stable)
- **UI Library**: Angular Material (Material 3)
- **Charting**: Chart.js with ng2-charts
- **State Management**: RxJS (no NgRx)
- **Styling**: SCSS with BEM methodology
- **Testing**: Jest (unit), Playwright (e2e)

### 1.3 Design Principles
- Mobile-first responsive design
- Material 3 design guidelines
- Accessibility (WCAG 2.1 AA compliance)
- Default Angular Material theme colors only
- Data visualization best practices

---

## 2. Page Structure

### 2.1 Pages
```
src/EventManagementPlatform.WebApp/projects/EventManagementPlatform/src/app/
├── pages/
│   ├── reports/
│   │   ├── report-list/
│   │   │   ├── report-list.ts
│   │   │   ├── report-list.html
│   │   │   ├── report-list.scss
│   │   │   └── index.ts
│   │   ├── report-detail/
│   │   │   ├── report-detail.ts
│   │   │   ├── report-detail.html
│   │   │   ├── report-detail.scss
│   │   │   └── index.ts
│   │   ├── report-create/
│   │   │   ├── report-create.ts
│   │   │   ├── report-create.html
│   │   │   ├── report-create.scss
│   │   │   └── index.ts
│   │   ├── report-viewer/
│   │   │   ├── report-viewer.ts
│   │   │   ├── report-viewer.html
│   │   │   ├── report-viewer.scss
│   │   │   └── index.ts
│   │   └── index.ts
│   ├── dashboards/
│   │   ├── dashboard-list/
│   │   │   ├── dashboard-list.ts
│   │   │   ├── dashboard-list.html
│   │   │   ├── dashboard-list.scss
│   │   │   └── index.ts
│   │   ├── dashboard-view/
│   │   │   ├── dashboard-view.ts
│   │   │   ├── dashboard-view.html
│   │   │   ├── dashboard-view.scss
│   │   │   └── index.ts
│   │   ├── dashboard-builder/
│   │   │   ├── dashboard-builder.ts
│   │   │   ├── dashboard-builder.html
│   │   │   ├── dashboard-builder.scss
│   │   │   └── index.ts
│   │   └── index.ts
│   └── analytics/
│       ├── statistics-overview/
│       │   ├── statistics-overview.ts
│       │   ├── statistics-overview.html
│       │   ├── statistics-overview.scss
│       │   └── index.ts
│       └── index.ts
```

### 2.2 Components
```
├── components/
│   ├── reports/
│   │   ├── report-card/
│   │   │   ├── report-card.ts
│   │   │   ├── report-card.html
│   │   │   ├── report-card.scss
│   │   │   └── index.ts
│   │   ├── report-form/
│   │   │   ├── report-form.ts
│   │   │   ├── report-form.html
│   │   │   ├── report-form.scss
│   │   │   └── index.ts
│   │   ├── report-schedule-dialog/
│   │   │   ├── report-schedule-dialog.ts
│   │   │   ├── report-schedule-dialog.html
│   │   │   ├── report-schedule-dialog.scss
│   │   │   └── index.ts
│   │   ├── report-instance-table/
│   │   │   ├── report-instance-table.ts
│   │   │   ├── report-instance-table.html
│   │   │   ├── report-instance-table.scss
│   │   │   └── index.ts
│   │   ├── report-export-menu/
│   │   │   ├── report-export-menu.ts
│   │   │   ├── report-export-menu.html
│   │   │   ├── report-export-menu.scss
│   │   │   └── index.ts
│   │   └── index.ts
│   ├── dashboards/
│   │   ├── dashboard-card/
│   │   │   ├── dashboard-card.ts
│   │   │   ├── dashboard-card.html
│   │   │   ├── dashboard-card.scss
│   │   │   └── index.ts
│   │   ├── dashboard-grid/
│   │   │   ├── dashboard-grid.ts
│   │   │   ├── dashboard-grid.html
│   │   │   ├── dashboard-grid.scss
│   │   │   └── index.ts
│   │   ├── widget-wrapper/
│   │   │   ├── widget-wrapper.ts
│   │   │   ├── widget-wrapper.html
│   │   │   ├── widget-wrapper.scss
│   │   │   └── index.ts
│   │   ├── widget-configurator/
│   │   │   ├── widget-configurator.ts
│   │   │   ├── widget-configurator.html
│   │   │   ├── widget-configurator.scss
│   │   │   └── index.ts
│   │   └── index.ts
│   └── widgets/
│       ├── line-chart-widget/
│       │   ├── line-chart-widget.ts
│       │   ├── line-chart-widget.html
│       │   ├── line-chart-widget.scss
│       │   └── index.ts
│       ├── bar-chart-widget/
│       │   ├── bar-chart-widget.ts
│       │   ├── bar-chart-widget.html
│       │   ├── bar-chart-widget.scss
│       │   └── index.ts
│       ├── pie-chart-widget/
│       │   ├── pie-chart-widget.ts
│       │   ├── pie-chart-widget.html
│       │   ├── pie-chart-widget.scss
│       │   └── index.ts
│       ├── stat-card-widget/
│       │   ├── stat-card-widget.ts
│       │   ├── stat-card-widget.html
│       │   ├── stat-card-widget.scss
│       │   └── index.ts
│       ├── table-widget/
│       │   ├── table-widget.ts
│       │   ├── table-widget.html
│       │   ├── table-widget.scss
│       │   └── index.ts
│       ├── gauge-widget/
│       │   ├── gauge-widget.ts
│       │   ├── gauge-widget.html
│       │   ├── gauge-widget.scss
│       │   └── index.ts
│       └── index.ts
```

---

## 3. User Interface Requirements

### 3.1 Report List Page

#### 3.1.1 Layout
- **Header**: Page title, create report button, view toggle
- **Filter Panel**: Report type, date range, status filters
- **Content**: Grid/list of report cards
- **Pagination**: Bottom pagination with page size selector

#### 3.1.2 Features
| Feature | Description |
|---------|-------------|
| Search | Real-time search by report name |
| Filter | Filter by type, creation date, schedule status |
| Sort | Sort by name, created date, last run |
| View Toggle | Card view / List view toggle |
| Quick Actions | Generate, Schedule, View instances |

#### 3.1.3 Responsive Behavior
| Breakpoint | Layout |
|------------|--------|
| < 600px | Single column, stacked cards |
| 600-960px | 2-column grid |
| > 960px | 3-column grid or table view |

### 3.2 Report Detail/Viewer Page

#### 3.2.1 Sections
| Section | Content |
|---------|---------|
| Header | Report name, type badge, action buttons |
| Parameters Card | Report configuration and parameters |
| Instances Tab | List of generated report instances |
| Preview Tab | Report data visualization |
| Schedule Tab | Schedule configuration (if applicable) |

#### 3.2.2 Actions
| Action | Condition | UI Element |
|--------|-----------|------------|
| Generate | Always | Primary button |
| Schedule | Not scheduled | Secondary button |
| Edit Schedule | Is scheduled | Secondary button |
| Export PDF | Instance exists | Menu item |
| Export Excel | Instance exists | Menu item |
| Email | Instance exists | Menu item |
| Download | Instance completed | Download button |

### 3.3 Dashboard View Page

#### 3.3.1 Layout
- **Header**: Dashboard name, refresh button, edit button
- **Grid Layout**: Responsive grid with draggable widgets
- **Widget Controls**: Resize, remove, configure per widget
- **Auto-refresh**: Optional auto-refresh with interval selector

#### 3.3.2 Widget Types
| Widget Type | Description | Data Source |
|-------------|-------------|-------------|
| Line Chart | Trend visualization | Time-series data |
| Bar Chart | Comparison visualization | Categorical data |
| Pie Chart | Proportion visualization | Distribution data |
| Stat Card | KPI display | Single metric |
| Table | Tabular data | Query results |
| Gauge | Progress/percentage | Percentage data |

#### 3.3.3 Interaction
- Drag and drop widgets to reorder
- Click widget to view details
- Hover for widget menu (edit, remove)
- Click refresh icon to reload widget data
- Click maximize to fullscreen view

### 3.4 Dashboard Builder Page

#### 3.4.1 Features
| Feature | Description |
|---------|-------------|
| Widget Library | Sidebar with available widget types |
| Drag & Drop | Drag widgets from library to canvas |
| Grid Layout | Snap-to-grid positioning |
| Widget Config | Configure each widget's data source |
| Layout Preview | Preview before saving |
| Save Layout | Save dashboard configuration |

### 3.5 Statistics Overview Page

#### 3.5.1 Sections
| Section | Content |
|---------|---------|
| Summary Cards | KPI cards for key metrics |
| Event Statistics | Charts and trends for events |
| Revenue Statistics | Financial analytics and trends |
| Staff Statistics | Staff utilization metrics |
| Equipment Statistics | Equipment usage analytics |
| Customer Statistics | Customer behavior insights |
| Venue Statistics | Venue booking analytics |

---

## 4. Services

### 4.1 ReportService
```typescript
@Injectable({ providedIn: 'root' })
export class ReportService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getReports(params: ReportQueryParams): Observable<PagedResult<ReportListDto>> { }

    getReportById(reportId: string): Observable<ReportDetailDto> { }

    createReport(report: CreateReportDto): Observable<ReportDetailDto> { }

    updateReport(reportId: string, report: UpdateReportDto): Observable<ReportDetailDto> { }

    deleteReport(reportId: string): Observable<void> { }

    generateReport(reportId: string, params: GenerateReportParams): Observable<ReportInstanceDto> { }

    scheduleReport(reportId: string, schedule: ScheduleConfig): Observable<void> { }

    removeSchedule(reportId: string): Observable<void> { }

    getReportInstances(reportId: string): Observable<ReportInstanceDto[]> { }

    downloadReport(instanceId: string): Observable<Blob> { }

    exportToPdf(instanceId: string): Observable<Blob> { }

    exportToExcel(instanceId: string): Observable<Blob> { }

    emailReport(instanceId: string, recipients: string[]): Observable<void> { }
}
```

### 4.2 DashboardService
```typescript
@Injectable({ providedIn: 'root' })
export class DashboardService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getDashboards(): Observable<DashboardListDto[]> { }

    getDashboardById(dashboardId: string): Observable<DashboardDetailDto> { }

    createDashboard(dashboard: CreateDashboardDto): Observable<DashboardDetailDto> { }

    updateDashboard(dashboardId: string, dashboard: UpdateDashboardDto): Observable<DashboardDetailDto> { }

    deleteDashboard(dashboardId: string): Observable<void> { }

    recordView(dashboardId: string): Observable<void> { }

    refreshDashboard(dashboardId: string): Observable<DashboardDataDto> { }

    addWidget(dashboardId: string, widget: AddWidgetDto): Observable<DashboardWidgetDto> { }

    removeWidget(dashboardId: string, widgetId: string): Observable<void> { }

    saveDashboardLayout(dashboardId: string, layout: LayoutConfig): Observable<void> { }

    getWidgetData(dashboardId: string, widgetId: string): Observable<any> { }
}
```

### 4.3 StatisticsService
```typescript
@Injectable({ providedIn: 'root' })
export class StatisticsService {
    private readonly baseUrl = inject(API_BASE_URL);
    private readonly http = inject(HttpClient);

    getEventStatistics(params: StatisticsParams): Observable<EventStatisticsDto> { }

    getRevenueStatistics(params: StatisticsParams): Observable<RevenueStatisticsDto> { }

    getStaffStatistics(params: StatisticsParams): Observable<StaffStatisticsDto> { }

    getEquipmentStatistics(params: StatisticsParams): Observable<EquipmentStatisticsDto> { }

    getCustomerStatistics(params: StatisticsParams): Observable<CustomerStatisticsDto> { }

    getVenueStatistics(params: StatisticsParams): Observable<VenueStatisticsDto> { }

    calculateStatistics(type: StatisticsType): Observable<void> { }
}
```

### 4.4 ReportStateService
```typescript
@Injectable({ providedIn: 'root' })
export class ReportStateService {
    private readonly reportsSubject = new BehaviorSubject<ReportListDto[]>([]);
    private readonly selectedReportSubject = new BehaviorSubject<ReportDetailDto | null>(null);
    private readonly loadingSubject = new BehaviorSubject<boolean>(false);

    readonly reports$ = this.reportsSubject.asObservable();
    readonly selectedReport$ = this.selectedReportSubject.asObservable();
    readonly loading$ = this.loadingSubject.asObservable();

    loadReports(params: ReportQueryParams): void { }

    selectReport(reportId: string): void { }

    clearSelection(): void { }

    refreshReports(): void { }
}
```

### 4.5 DashboardStateService
```typescript
@Injectable({ providedIn: 'root' })
export class DashboardStateService {
    private readonly dashboardsSubject = new BehaviorSubject<DashboardListDto[]>([]);
    private readonly activeDashboardSubject = new BehaviorSubject<DashboardDetailDto | null>(null);
    private readonly widgetDataCache = new Map<string, any>();

    readonly dashboards$ = this.dashboardsSubject.asObservable();
    readonly activeDashboard$ = this.activeDashboardSubject.asObservable();

    loadDashboards(): void { }

    setActiveDashboard(dashboardId: string): void { }

    updateWidgetData(widgetId: string, data: any): void { }

    getWidgetData(widgetId: string): Observable<any> { }

    clearCache(): void { }
}
```

---

## 5. Models/Interfaces

### 5.1 DTOs
```typescript
export interface ReportListDto {
    reportId: string;
    reportName: string;
    reportType: ReportType;
    description?: string;
    isScheduled: boolean;
    lastGeneratedAt?: Date;
    createdAt: Date;
}

export interface ReportDetailDto {
    reportId: string;
    reportName: string;
    reportType: ReportType;
    description?: string;
    parameters?: string;
    isScheduled: boolean;
    scheduleConfig?: ScheduleConfig;
    createdAt: Date;
    lastGeneratedAt?: Date;
    recentInstances: ReportInstanceDto[];
}

export interface CreateReportDto {
    reportName: string;
    reportType: ReportType;
    description?: string;
    parameters?: string;
    isScheduled: boolean;
    scheduleConfig?: ScheduleConfig;
}

export interface ReportInstanceDto {
    reportInstanceId: string;
    reportId: string;
    generatedAt: Date;
    startDate: Date;
    endDate: Date;
    status: ReportStatus;
    outputFormat: OutputFormat;
    fileUrl?: string;
    fileSize?: number;
    errorMessage?: string;
}

export interface DashboardDetailDto {
    dashboardId: string;
    name: string;
    description?: string;
    layout: string;
    isDefault: boolean;
    isPublic: boolean;
    ownerId: string;
    ownerName: string;
    createdAt: Date;
    modifiedAt?: Date;
    widgets: DashboardWidgetDto[];
}

export interface DashboardWidgetDto {
    widgetId: string;
    widgetType: WidgetType;
    title: string;
    configuration: string;
    position: number;
    size: WidgetSize;
    dataSource?: string;
    refreshInterval?: number;
}

export interface EventStatisticsDto {
    totalEvents: number;
    completedEvents: number;
    cancelledEvents: number;
    pendingEvents: number;
    completionRate: number;
    cancellationRate: number;
    trend: TrendDataPoint[];
}

export interface RevenueStatisticsDto {
    totalRevenue: number;
    averageRevenuePerEvent: number;
    projectedRevenue: number;
    topCustomers: TopCustomer[];
    monthlyBreakdown: RevenueByMonth[];
}
```

### 5.2 Enums
```typescript
export enum ReportType {
    DailySummary = 'DailySummary',
    WeeklySummary = 'WeeklySummary',
    MonthlySummary = 'MonthlySummary',
    YearlySummary = 'YearlySummary',
    CustomReport = 'CustomReport',
    EventStatistics = 'EventStatistics',
    RevenueStatistics = 'RevenueStatistics',
    StaffStatistics = 'StaffStatistics',
    EquipmentUtilization = 'EquipmentUtilization',
    CustomerStatistics = 'CustomerStatistics',
    VenueStatistics = 'VenueStatistics'
}

export enum ReportStatus {
    Queued = 'Queued',
    Generating = 'Generating',
    Completed = 'Completed',
    Failed = 'Failed',
    Exported = 'Exported',
    Emailed = 'Emailed'
}

export enum OutputFormat {
    PDF = 'PDF',
    Excel = 'Excel',
    CSV = 'CSV',
    JSON = 'JSON'
}

export enum WidgetType {
    LineChart = 'LineChart',
    BarChart = 'BarChart',
    PieChart = 'PieChart',
    StatCard = 'StatCard',
    Table = 'Table',
    Gauge = 'Gauge',
    Heatmap = 'Heatmap',
    Timeline = 'Timeline'
}

export enum WidgetSize {
    Small = 'Small',
    Medium = 'Medium',
    Large = 'Large',
    Wide = 'Wide',
    ExtraLarge = 'ExtraLarge'
}
```

---

## 6. Routing

### 6.1 Route Configuration
```typescript
export const reportingRoutes: Routes = [
    {
        path: 'reports',
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/reports/report-list').then(m => m.ReportList),
                title: 'Reports'
            },
            {
                path: 'create',
                loadComponent: () => import('./pages/reports/report-create').then(m => m.ReportCreate),
                title: 'Create Report',
                canActivate: [authGuard]
            },
            {
                path: ':reportId',
                loadComponent: () => import('./pages/reports/report-detail').then(m => m.ReportDetail),
                title: 'Report Details'
            },
            {
                path: ':reportId/viewer',
                loadComponent: () => import('./pages/reports/report-viewer').then(m => m.ReportViewer),
                title: 'View Report'
            }
        ]
    },
    {
        path: 'dashboards',
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/dashboards/dashboard-list').then(m => m.DashboardList),
                title: 'Dashboards'
            },
            {
                path: 'builder',
                loadComponent: () => import('./pages/dashboards/dashboard-builder').then(m => m.DashboardBuilder),
                title: 'Dashboard Builder',
                canActivate: [authGuard]
            },
            {
                path: ':dashboardId',
                loadComponent: () => import('./pages/dashboards/dashboard-view').then(m => m.DashboardView),
                title: 'Dashboard'
            }
        ]
    },
    {
        path: 'analytics',
        children: [
            {
                path: '',
                loadComponent: () => import('./pages/analytics/statistics-overview').then(m => m.StatisticsOverview),
                title: 'Analytics & Statistics'
            }
        ]
    }
];
```

---

## 7. Material Components Used

### 7.1 Component List
| Component | Usage |
|-----------|-------|
| mat-card | Report cards, widget containers |
| mat-table | Report instance table, data tables |
| mat-paginator | Pagination |
| mat-sort | Table sorting |
| mat-form-field | Form inputs |
| mat-input | Text inputs |
| mat-select | Dropdowns |
| mat-datepicker | Date range selection |
| mat-button | Action buttons |
| mat-icon | Icons throughout |
| mat-menu | Export menu, widget menu |
| mat-dialog | Schedule dialog, widget configurator |
| mat-snackbar | Notifications |
| mat-progress-spinner | Loading states |
| mat-progress-bar | Report generation progress |
| mat-chip | Report type badges, status chips |
| mat-tabs | Detail page sections |
| mat-expansion-panel | Filter panel, advanced options |
| mat-slide-toggle | Schedule toggle, auto-refresh |
| mat-button-toggle | View toggle (grid/list) |

---

## 8. Chart.js Integration

### 8.1 Chart Configuration
```typescript
export interface ChartConfig {
    type: 'line' | 'bar' | 'pie' | 'doughnut' | 'radar' | 'polarArea';
    data: ChartData;
    options: ChartOptions;
}

export interface ChartData {
    labels: string[];
    datasets: ChartDataset[];
}

export interface ChartDataset {
    label: string;
    data: number[];
    backgroundColor?: string | string[];
    borderColor?: string | string[];
    borderWidth?: number;
}
```

### 8.2 Common Chart Types
| Chart Type | Use Case | Widget Type |
|------------|----------|-------------|
| Line Chart | Time-series trends | LineChart |
| Bar Chart | Comparisons | BarChart |
| Pie Chart | Proportions | PieChart |
| Doughnut | Percentages | PieChart |
| Gauge | KPI progress | Gauge |

---

## 9. Styling Guidelines

### 9.1 BEM Naming Convention
```scss
// Block
.report-card { }
.dashboard-grid { }
.widget-wrapper { }

// Element
.report-card__header { }
.report-card__title { }
.report-card__status { }
.dashboard-grid__item { }
.widget-wrapper__header { }
.widget-wrapper__content { }

// Modifier
.report-card--scheduled { }
.widget-wrapper--loading { }
.dashboard-grid--editing { }
```

### 9.2 Design Tokens
```scss
// Spacing tokens
$spacing-xs: 4px;
$spacing-sm: 8px;
$spacing-md: 16px;
$spacing-lg: 24px;
$spacing-xl: 32px;
$spacing-xxl: 48px;

// Grid layout
$grid-gap: 16px;
$grid-columns: 12;

// Use Angular Material theme colors only
```

### 9.3 Dashboard Grid Layout
```scss
.dashboard-grid {
    display: grid;
    grid-template-columns: repeat(12, 1fr);
    gap: $grid-gap;
    padding: $spacing-md;

    &__item {
        &--small { grid-column: span 3; grid-row: span 1; }
        &--medium { grid-column: span 6; grid-row: span 1; }
        &--large { grid-column: span 6; grid-row: span 2; }
        &--wide { grid-column: span 12; grid-row: span 1; }
        &--extra-large { grid-column: span 12; grid-row: span 2; }
    }
}
```

---

## 10. Error Handling

### 10.1 Error Display
| Error Type | Display Method |
|------------|----------------|
| Validation | Inline form errors |
| Generation Failed | Error dialog with retry |
| API 400 | Snackbar with details |
| API 404 | Redirect to not found page |
| API 401 | Redirect to login |
| API 403 | Display forbidden message |
| API 500 | Error dialog with support info |
| Network | Snackbar with retry action |
| File Download | Error dialog |

### 10.2 Loading States
| Operation | Loading Indicator |
|-----------|-------------------|
| Report Generation | Progress bar with percentage |
| Dashboard Load | Spinner overlay |
| Widget Refresh | Widget-level spinner |
| File Download | Button spinner |
| Statistics Calculation | Skeleton loaders |

---

## 11. Accessibility Requirements

### 11.1 WCAG 2.1 AA Compliance
| Requirement | Implementation |
|-------------|----------------|
| Keyboard Navigation | All interactive elements focusable |
| Screen Reader | ARIA labels on charts and widgets |
| Color Contrast | Material theme ensures compliance |
| Focus Indicators | Visible focus rings |
| Form Labels | Associated labels for all inputs |
| Chart Accessibility | Alt text and data tables for charts |

### 11.2 Chart Accessibility
```html
<canvas baseChart
    [datasets]="chartData"
    [labels]="chartLabels"
    [options]="chartOptions"
    [type]="chartType"
    aria-label="Event statistics trend chart"
    role="img">
</canvas>

<!-- Accessible data table alternative -->
<table class="visually-hidden" aria-label="Event statistics data">
    <caption>Monthly event statistics</caption>
    <thead>
        <tr>
            <th>Month</th>
            <th>Events</th>
        </tr>
    </thead>
    <tbody>
        <tr *ngFor="let item of chartData">
            <td>{{item.label}}</td>
            <td>{{item.value}}</td>
        </tr>
    </tbody>
</table>
```

---

## 12. Testing Requirements

### 12.1 Unit Tests (Jest)
| Component/Service | Test Coverage |
|-------------------|---------------|
| ReportService | 100% |
| DashboardService | 100% |
| StatisticsService | 100% |
| ReportList | 90% |
| DashboardView | 90% |
| Widget components | 85% |
| All components | Minimum 80% |

### 12.2 E2E Tests (Playwright)
| Scenario | Priority |
|----------|----------|
| Generate report | High |
| Schedule report | High |
| Create dashboard | High |
| Add widget to dashboard | High |
| View statistics | Medium |
| Export to PDF | High |
| Export to Excel | High |
| Email report | Medium |
| Refresh dashboard | Medium |
| Filter reports | Medium |

### 12.3 Visual Regression Tests
- Dashboard layouts
- Chart rendering
- Widget responsiveness
- Mobile views

---

## 13. Performance Requirements

| Metric | Requirement |
|--------|-------------|
| First Contentful Paint | < 1.5s |
| Time to Interactive | < 3s |
| Dashboard Load | < 2s |
| Widget Render | < 500ms |
| Chart Animation | 60fps |
| Bundle Size | < 250KB (initial) |
| Report List Render | < 100ms for 50 items |

### 13.1 Optimization Strategies
- Lazy loading of routes
- Virtual scrolling for large tables
- OnPush change detection
- Chart data memoization
- Widget lazy loading
- Dashboard layout caching
- Pagination for large datasets

---

## 14. Real-time Updates

### 14.1 Auto-refresh Configuration
```typescript
export interface AutoRefreshConfig {
    enabled: boolean;
    interval: number; // seconds
    widgets: string[]; // widget IDs to refresh
}
```

### 14.2 Refresh Intervals
| Component | Default Interval | Configurable |
|-----------|-----------------|--------------|
| Dashboard | 60 seconds | Yes |
| Stat Cards | 30 seconds | Yes |
| Charts | 60 seconds | Yes |
| Tables | 120 seconds | Yes |
| Report Status | 5 seconds | No |

---

## 15. Internationalization

### 15.1 i18n Support
- Use Angular i18n for translations
- Date formatting via DatePipe with locale
- Number formatting via DecimalPipe
- Currency formatting for revenue statistics

### 15.2 Date/Time Handling
- Store all dates in UTC
- Display in user's local timezone
- Format according to user locale
- Support multiple date formats

---

## 16. Export Functionality

### 16.1 Export Options
| Format | Use Case | File Extension |
|--------|----------|----------------|
| PDF | Printable reports | .pdf |
| Excel | Data analysis | .xlsx |
| CSV | Data import | .csv |
| JSON | API integration | .json |

### 16.2 Download Handling
```typescript
downloadReport(instanceId: string, format: OutputFormat): void {
    this.reportService.downloadReport(instanceId).subscribe({
        next: (blob) => {
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = `report-${instanceId}.${format.toLowerCase()}`;
            link.click();
            window.URL.revokeObjectURL(url);
        },
        error: (error) => {
            this.showError('Download failed');
        }
    });
}
```

---

## 17. Appendices

### 17.1 Related Documents
- [Backend Specification](./backend-spec.md)
- [Use Case Diagram](./diagrams/use-case.drawio)
- [C4 Diagrams](./diagrams/c4-context.drawio)
- [Class Diagram](./plantuml/class-diagram.plantuml)
- [Sequence Diagrams](./plantuml/sequence-diagram.plantuml)

### 17.2 Wireframes
Wireframes are available in the design system documentation.

### 17.3 Revision History
| Version | Date | Author | Changes |
|---------|------|--------|---------|
| 1.0 | 2025-12-22 | System | Initial specification |
