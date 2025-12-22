# Notification & Alert Management - Backend Specification

## 1. Introduction

### 1.1 Purpose
This document specifies the backend requirements for the Notification & Alert Management feature of the Event Management Platform. The system manages system notifications, alerts, and user notification preferences using .NET 8, Azure services, and Azure AI capabilities.

### 1.2 Scope
The backend system provides:
- Real-time notification delivery and management
- Alert generation and escalation
- User notification preferences management
- Multi-channel notification delivery (email, SMS, push, in-app)
- AI-powered intelligent notification prioritization and summarization
- Emergency broadcast capabilities
- Notification analytics and reporting

### 1.3 Technology Stack
- **Framework**: .NET 8 (ASP.NET Core)
- **Cloud Platform**: Microsoft Azure
- **Database**: Azure SQL Database / Azure Cosmos DB (for high-scale scenarios)
- **Message Queue**: Azure Service Bus
- **Real-time Communication**: Azure SignalR Service
- **AI Services**: Azure OpenAI Service, Azure Cognitive Services
- **Storage**: Azure Blob Storage
- **Notifications**: Azure Notification Hubs, Azure Communication Services
- **Caching**: Azure Redis Cache
- **Identity**: Azure AD B2C / Entra ID

## 2. Domain Model

### 2.1 Core Entities

#### 2.1.1 Notification
```csharp
public class Notification
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public NotificationType Type { get; set; }
    public NotificationPriority Priority { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public string Category { get; set; }
    public Dictionary<string, object> Metadata { get; set; }
    public List<NotificationAction> Actions { get; set; }
    public NotificationStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ViewedAt { get; set; }
    public DateTime? DismissedAt { get; set; }
    public DateTime? ActionTakenAt { get; set; }
    public DateTime? ExpiresAt { get; set; }
    public string SourceEntityType { get; set; }
    public Guid? SourceEntityId { get; set; }
    public string Channel { get; set; }
    public bool IsRead { get; set; }
    public bool IsDismissed { get; set; }
}

public enum NotificationType
{
    Info,
    Warning,
    Alert,
    Critical,
    Emergency,
    Reminder,
    Action
}

public enum NotificationPriority
{
    Low,
    Normal,
    High,
    Urgent,
    Critical
}

public enum NotificationStatus
{
    Created,
    Queued,
    Sent,
    Delivered,
    Viewed,
    Dismissed,
    ActionTaken,
    Failed,
    Expired
}
```

#### 2.1.2 Alert
```csharp
public class Alert
{
    public Guid Id { get; set; }
    public AlertType Type { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public AlertSeverity Severity { get; set; }
    public string Source { get; set; }
    public Guid? SourceEntityId { get; set; }
    public Dictionary<string, object> Context { get; set; }
    public AlertStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
    public Guid? AcknowledgedBy { get; set; }
    public DateTime? ResolvedAt { get; set; }
    public Guid? ResolvedBy { get; set; }
    public DateTime? EscalatedAt { get; set; }
    public int EscalationLevel { get; set; }
    public List<Guid> AssignedUserIds { get; set; }
    public List<string> Tags { get; set; }
}

public enum AlertType
{
    Overbooking,
    LowInventory,
    MaintenanceDue,
    PaymentDue,
    StaffNoShow,
    DeliveryDelay,
    SystemIssue,
    SecurityIncident
}

public enum AlertSeverity
{
    Low,
    Medium,
    High,
    Critical,
    Emergency
}

public enum AlertStatus
{
    Active,
    Acknowledged,
    InProgress,
    Resolved,
    Escalated,
    Closed
}
```

#### 2.1.3 NotificationPreferences
```csharp
public class NotificationPreferences
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Dictionary<string, ChannelPreference> CategoryPreferences { get; set; }
    public bool EnableEmailNotifications { get; set; }
    public bool EnableSmsNotifications { get; set; }
    public bool EnablePushNotifications { get; set; }
    public bool EnableInAppNotifications { get; set; }
    public QuietHoursSettings QuietHours { get; set; }
    public DigestSettings DigestSettings { get; set; }
    public NotificationFrequency MaxFrequency { get; set; }
    public List<string> MutedCategories { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class ChannelPreference
{
    public bool Email { get; set; }
    public bool Sms { get; set; }
    public bool Push { get; set; }
    public bool InApp { get; set; }
    public NotificationPriority MinimumPriority { get; set; }
}

public class QuietHoursSettings
{
    public bool Enabled { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string TimeZone { get; set; }
    public List<DayOfWeek> Days { get; set; }
    public bool AllowUrgent { get; set; }
}

public class DigestSettings
{
    public bool Enabled { get; set; }
    public DigestFrequency Frequency { get; set; }
    public TimeSpan PreferredTime { get; set; }
    public List<string> IncludedCategories { get; set; }
}

public enum NotificationFrequency
{
    RealTime,
    Hourly,
    Daily,
    Weekly
}

public enum DigestFrequency
{
    Daily,
    Weekly,
    BiWeekly
}
```

#### 2.1.4 NotificationTemplate
```csharp
public class NotificationTemplate
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Category { get; set; }
    public NotificationType Type { get; set; }
    public Dictionary<string, ChannelTemplate> ChannelTemplates { get; set; }
    public List<string> RequiredVariables { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class ChannelTemplate
{
    public string Subject { get; set; }
    public string BodyTemplate { get; set; }
    public string HtmlBodyTemplate { get; set; }
    public TemplateFormat Format { get; set; }
}

public enum TemplateFormat
{
    Plain,
    Html,
    Markdown,
    Liquid,
    Razor
}
```

### 2.2 Domain Events

#### 2.2.1 Alert Events
```csharp
public record OverbookingAlertSent(
    Guid AlertId,
    Guid EventId,
    int CurrentCapacity,
    int MaxCapacity,
    DateTime Timestamp
) : IDomainEvent;

public record LowInventoryAlertSent(
    Guid AlertId,
    Guid ItemId,
    string ItemName,
    int CurrentStock,
    int ThresholdLevel,
    DateTime Timestamp
) : IDomainEvent;

public record MaintenanceDueAlertSent(
    Guid AlertId,
    Guid AssetId,
    string AssetName,
    DateTime DueDate,
    MaintenancePriority Priority,
    DateTime Timestamp
) : IDomainEvent;

public record PaymentDueAlertSent(
    Guid AlertId,
    Guid PaymentId,
    decimal Amount,
    DateTime DueDate,
    int DaysOverdue,
    DateTime Timestamp
) : IDomainEvent;

public record StaffNoShowAlertSent(
    Guid AlertId,
    Guid StaffId,
    string StaffName,
    Guid ShiftId,
    DateTime ShiftTime,
    DateTime Timestamp
) : IDomainEvent;

public record EventReminderSent(
    Guid NotificationId,
    Guid EventId,
    Guid UserId,
    DateTime EventDateTime,
    int HoursBefore,
    DateTime Timestamp
) : IDomainEvent;

public record DeliveryDelayAlertSent(
    Guid AlertId,
    Guid DeliveryId,
    DateTime OriginalETA,
    DateTime RevisedETA,
    string Reason,
    DateTime Timestamp
) : IDomainEvent;
```

#### 2.2.2 Notification Events
```csharp
public record UserNotificationCreated(
    Guid NotificationId,
    Guid UserId,
    NotificationType Type,
    NotificationPriority Priority,
    string Category,
    DateTime Timestamp
) : IDomainEvent;

public record UserNotificationViewed(
    Guid NotificationId,
    Guid UserId,
    DateTime ViewedAt
) : IDomainEvent;

public record UserNotificationDismissed(
    Guid NotificationId,
    Guid UserId,
    DateTime DismissedAt
) : IDomainEvent;

public record UserNotificationActionTaken(
    Guid NotificationId,
    Guid UserId,
    string ActionType,
    Dictionary<string, object> ActionData,
    DateTime Timestamp
) : IDomainEvent;

public record NotificationPreferencesUpdated(
    Guid UserId,
    NotificationPreferences OldPreferences,
    NotificationPreferences NewPreferences,
    DateTime Timestamp
) : IDomainEvent;
```

#### 2.2.3 Escalation & Emergency Events
```csharp
public record IssueEscalated(
    Guid IssueId,
    string IssueType,
    int FromLevel,
    int ToLevel,
    List<Guid> EscalatedToUserIds,
    string Reason,
    DateTime Timestamp
) : IDomainEvent;

public record UrgentAlertSent(
    Guid AlertId,
    AlertType Type,
    AlertSeverity Severity,
    List<Guid> TargetUserIds,
    DateTime Timestamp
) : IDomainEvent;

public record EmergencyNotificationBroadcast(
    Guid BroadcastId,
    string Title,
    string Message,
    List<string> Channels,
    List<Guid> TargetUserIds,
    DateTime Timestamp
) : IDomainEvent;
```

## 3. API Endpoints

### 3.1 Notification Management

#### 3.1.1 Get User Notifications
```
GET /api/v1/notifications
Query Parameters:
  - userId: Guid (required)
  - status: NotificationStatus (optional)
  - priority: NotificationPriority (optional)
  - category: string (optional)
  - isRead: bool (optional)
  - fromDate: DateTime (optional)
  - toDate: DateTime (optional)
  - pageNumber: int (default: 1)
  - pageSize: int (default: 20)

Response: 200 OK
{
  "items": [NotificationDto],
  "totalCount": int,
  "pageNumber": int,
  "pageSize": int,
  "hasNextPage": bool
}
```

#### 3.1.2 Get Notification Details
```
GET /api/v1/notifications/{id}

Response: 200 OK
{
  "id": "guid",
  "userId": "guid",
  "type": "string",
  "priority": "string",
  "title": "string",
  "message": "string",
  "category": "string",
  "metadata": {},
  "actions": [],
  "status": "string",
  "createdAt": "datetime",
  "viewedAt": "datetime",
  "expiresAt": "datetime",
  "isRead": bool,
  "channel": "string"
}
```

#### 3.1.3 Mark Notification as Viewed
```
POST /api/v1/notifications/{id}/view

Response: 200 OK
{
  "success": true,
  "viewedAt": "datetime"
}
```

#### 3.1.4 Dismiss Notification
```
POST /api/v1/notifications/{id}/dismiss

Response: 200 OK
{
  "success": true,
  "dismissedAt": "datetime"
}
```

#### 3.1.5 Take Notification Action
```
POST /api/v1/notifications/{id}/action
Body:
{
  "actionType": "string",
  "actionData": {}
}

Response: 200 OK
{
  "success": true,
  "result": {}
}
```

#### 3.1.6 Mark All as Read
```
POST /api/v1/notifications/mark-all-read
Body:
{
  "userId": "guid",
  "category": "string" (optional)
}

Response: 200 OK
{
  "success": true,
  "updatedCount": int
}
```

#### 3.1.7 Delete Notification
```
DELETE /api/v1/notifications/{id}

Response: 204 No Content
```

#### 3.1.8 Get Notification Summary
```
GET /api/v1/notifications/summary
Query Parameters:
  - userId: Guid (required)

Response: 200 OK
{
  "totalUnread": int,
  "unreadByCategory": {},
  "unreadByPriority": {},
  "recentAlerts": int,
  "pendingActions": int
}
```

### 3.2 Alert Management

#### 3.2.1 Get Alerts
```
GET /api/v1/alerts
Query Parameters:
  - type: AlertType (optional)
  - severity: AlertSeverity (optional)
  - status: AlertStatus (optional)
  - assignedToUserId: Guid (optional)
  - fromDate: DateTime (optional)
  - toDate: DateTime (optional)
  - pageNumber: int (default: 1)
  - pageSize: int (default: 20)

Response: 200 OK
{
  "items": [AlertDto],
  "totalCount": int,
  "pageNumber": int,
  "pageSize": int
}
```

#### 3.2.2 Get Alert Details
```
GET /api/v1/alerts/{id}

Response: 200 OK
{
  "id": "guid",
  "type": "string",
  "title": "string",
  "description": "string",
  "severity": "string",
  "status": "string",
  "source": "string",
  "context": {},
  "createdAt": "datetime",
  "acknowledgedAt": "datetime",
  "assignedUserIds": []
}
```

#### 3.2.3 Acknowledge Alert
```
POST /api/v1/alerts/{id}/acknowledge

Response: 200 OK
{
  "success": true,
  "acknowledgedAt": "datetime",
  "acknowledgedBy": "guid"
}
```

#### 3.2.4 Resolve Alert
```
POST /api/v1/alerts/{id}/resolve
Body:
{
  "resolutionNotes": "string",
  "resolutionData": {}
}

Response: 200 OK
{
  "success": true,
  "resolvedAt": "datetime"
}
```

#### 3.2.5 Escalate Alert
```
POST /api/v1/alerts/{id}/escalate
Body:
{
  "reason": "string",
  "escalateTo": ["guid"],
  "notes": "string"
}

Response: 200 OK
{
  "success": true,
  "escalationLevel": int,
  "escalatedAt": "datetime"
}
```

#### 3.2.6 Assign Alert
```
POST /api/v1/alerts/{id}/assign
Body:
{
  "userIds": ["guid"]
}

Response: 200 OK
{
  "success": true,
  "assignedUserIds": []
}
```

### 3.3 Notification Preferences

#### 3.3.1 Get User Preferences
```
GET /api/v1/notification-preferences/{userId}

Response: 200 OK
{
  "userId": "guid",
  "categoryPreferences": {},
  "enableEmailNotifications": bool,
  "enableSmsNotifications": bool,
  "enablePushNotifications": bool,
  "enableInAppNotifications": bool,
  "quietHours": {},
  "digestSettings": {},
  "maxFrequency": "string",
  "mutedCategories": []
}
```

#### 3.3.2 Update User Preferences
```
PUT /api/v1/notification-preferences/{userId}
Body:
{
  "categoryPreferences": {},
  "enableEmailNotifications": bool,
  "enableSmsNotifications": bool,
  "enablePushNotifications": bool,
  "enableInAppNotifications": bool,
  "quietHours": {},
  "digestSettings": {},
  "maxFrequency": "string",
  "mutedCategories": []
}

Response: 200 OK
{
  "success": true,
  "preferences": {}
}
```

#### 3.3.3 Update Channel Preferences
```
PATCH /api/v1/notification-preferences/{userId}/channels
Body:
{
  "category": "string",
  "email": bool,
  "sms": bool,
  "push": bool,
  "inApp": bool,
  "minimumPriority": "string"
}

Response: 200 OK
{
  "success": true
}
```

#### 3.3.4 Set Quiet Hours
```
POST /api/v1/notification-preferences/{userId}/quiet-hours
Body:
{
  "enabled": bool,
  "startTime": "time",
  "endTime": "time",
  "timeZone": "string",
  "days": [],
  "allowUrgent": bool
}

Response: 200 OK
{
  "success": true
}
```

### 3.4 Emergency Broadcasts

#### 3.4.1 Send Emergency Broadcast
```
POST /api/v1/emergency-broadcast
Body:
{
  "title": "string",
  "message": "string",
  "channels": ["string"],
  "targetUserIds": ["guid"],
  "targetGroups": ["string"],
  "priority": "string"
}

Response: 200 OK
{
  "broadcastId": "guid",
  "recipientCount": int,
  "estimatedDeliveryTime": "datetime"
}
```

#### 3.4.2 Get Broadcast Status
```
GET /api/v1/emergency-broadcast/{id}/status

Response: 200 OK
{
  "broadcastId": "guid",
  "status": "string",
  "sentCount": int,
  "deliveredCount": int,
  "failedCount": int,
  "viewedCount": int
}
```

## 4. Service Layer Architecture

### 4.1 Core Services

#### 4.1.1 NotificationService
```csharp
public interface INotificationService
{
    Task<NotificationDto> CreateNotificationAsync(CreateNotificationCommand command);
    Task<PagedResult<NotificationDto>> GetUserNotificationsAsync(GetUserNotificationsQuery query);
    Task<NotificationDto> GetNotificationByIdAsync(Guid id);
    Task MarkAsViewedAsync(Guid notificationId, Guid userId);
    Task DismissNotificationAsync(Guid notificationId, Guid userId);
    Task TakeActionAsync(Guid notificationId, string actionType, Dictionary<string, object> actionData);
    Task<int> MarkAllAsReadAsync(Guid userId, string category = null);
    Task DeleteNotificationAsync(Guid id);
    Task<NotificationSummaryDto> GetNotificationSummaryAsync(Guid userId);
}
```

#### 4.1.2 AlertService
```csharp
public interface IAlertService
{
    Task<AlertDto> CreateAlertAsync(CreateAlertCommand command);
    Task<PagedResult<AlertDto>> GetAlertsAsync(GetAlertsQuery query);
    Task<AlertDto> GetAlertByIdAsync(Guid id);
    Task AcknowledgeAlertAsync(Guid alertId, Guid userId);
    Task ResolveAlertAsync(Guid alertId, Guid userId, string notes, Dictionary<string, object> data);
    Task EscalateAlertAsync(Guid alertId, string reason, List<Guid> escalateTo);
    Task AssignAlertAsync(Guid alertId, List<Guid> userIds);
    Task CheckAndProcessAlertEscalationAsync();
}
```

#### 4.1.3 NotificationPreferencesService
```csharp
public interface INotificationPreferencesService
{
    Task<NotificationPreferencesDto> GetPreferencesAsync(Guid userId);
    Task<NotificationPreferencesDto> UpdatePreferencesAsync(Guid userId, UpdatePreferencesCommand command);
    Task UpdateChannelPreferencesAsync(Guid userId, string category, ChannelPreference preferences);
    Task SetQuietHoursAsync(Guid userId, QuietHoursSettings settings);
    Task<bool> ShouldSendNotificationAsync(Guid userId, NotificationType type, string category, NotificationPriority priority);
}
```

#### 4.1.4 NotificationDeliveryService
```csharp
public interface INotificationDeliveryService
{
    Task DeliverNotificationAsync(Notification notification, List<string> channels);
    Task SendEmailNotificationAsync(Guid userId, string subject, string body);
    Task SendSmsNotificationAsync(Guid userId, string message);
    Task SendPushNotificationAsync(Guid userId, string title, string body, Dictionary<string, string> data);
    Task SendInAppNotificationAsync(Guid userId, Notification notification);
    Task SendDigestEmailAsync(Guid userId, List<Notification> notifications);
}
```

#### 4.1.5 EmergencyBroadcastService
```csharp
public interface IEmergencyBroadcastService
{
    Task<BroadcastResultDto> SendEmergencyBroadcastAsync(EmergencyBroadcastCommand command);
    Task<BroadcastStatusDto> GetBroadcastStatusAsync(Guid broadcastId);
    Task<List<Guid>> ResolveTargetRecipientsAsync(List<Guid> userIds, List<string> groups);
}
```

#### 4.1.6 NotificationTemplateService
```csharp
public interface INotificationTemplateService
{
    Task<string> RenderTemplateAsync(string templateName, string channel, Dictionary<string, object> variables);
    Task<NotificationTemplate> GetTemplateAsync(string templateName);
    Task CreateOrUpdateTemplateAsync(NotificationTemplate template);
}
```

### 4.2 AI-Powered Services

#### 4.2.1 NotificationIntelligenceService
```csharp
public interface INotificationIntelligenceService
{
    Task<NotificationPriority> AnalyzeAndPrioritizeAsync(Notification notification);
    Task<string> SummarizeNotificationsAsync(List<Notification> notifications);
    Task<List<string>> GenerateSmartActionsAsync(Notification notification);
    Task<bool> DetectDuplicateNotificationAsync(Notification notification);
    Task<List<Notification>> ClusterSimilarNotificationsAsync(List<Notification> notifications);
}
```

#### 4.2.2 AlertPredictionService
```csharp
public interface IAlertPredictionService
{
    Task<List<PredictedAlert>> PredictPotentialAlertsAsync(string contextType, Guid contextId);
    Task<double> CalculateAlertRiskScoreAsync(AlertType type, Dictionary<string, object> context);
    Task<List<string>> SuggestPreventiveActionsAsync(AlertType type);
}
```

## 5. Azure Service Integration

### 5.1 Azure Service Bus
```csharp
public class NotificationMessageHandler
{
    private readonly ServiceBusClient _serviceBusClient;
    private readonly INotificationDeliveryService _deliveryService;

    public async Task ProcessNotificationMessageAsync(ServiceBusReceivedMessage message)
    {
        var notification = JsonSerializer.Deserialize<Notification>(message.Body);
        var channels = DetermineDeliveryChannels(notification);
        await _deliveryService.DeliverNotificationAsync(notification, channels);
    }
}

// Queue Names
public static class QueueNames
{
    public const string NotificationQueue = "notifications";
    public const string AlertQueue = "alerts";
    public const string EmergencyBroadcastQueue = "emergency-broadcasts";
    public const string DigestQueue = "notification-digests";
}
```

### 5.2 Azure SignalR Service
```csharp
public class NotificationHub : Hub
{
    public async Task SubscribeToNotifications(Guid userId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
    }

    public async Task UnsubscribeFromNotifications(Guid userId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"user-{userId}");
    }
}

public interface IRealtimeNotificationService
{
    Task SendNotificationToUserAsync(Guid userId, NotificationDto notification);
    Task BroadcastAlertAsync(AlertDto alert, List<Guid> userIds);
    Task UpdateNotificationStatusAsync(Guid notificationId, NotificationStatus status);
}
```

### 5.3 Azure OpenAI Service
```csharp
public class AzureOpenAINotificationService
{
    private readonly OpenAIClient _openAIClient;

    public async Task<string> GenerateNotificationSummaryAsync(List<Notification> notifications)
    {
        var prompt = BuildSummaryPrompt(notifications);
        var response = await _openAIClient.GetChatCompletionsAsync(
            new ChatCompletionsOptions
            {
                Messages = { new ChatMessage(ChatRole.User, prompt) },
                MaxTokens = 500,
                Temperature = 0.7f
            }
        );
        return response.Value.Choices[0].Message.Content;
    }

    public async Task<NotificationPriority> AnalyzeNotificationUrgencyAsync(Notification notification)
    {
        var prompt = $@"Analyze this notification and determine its urgency level:
            Type: {notification.Type}
            Category: {notification.Category}
            Title: {notification.Title}
            Message: {notification.Message}

            Respond with one of: Low, Normal, High, Urgent, Critical";

        var response = await _openAIClient.GetChatCompletionsAsync(
            new ChatCompletionsOptions
            {
                Messages = { new ChatMessage(ChatRole.User, prompt) },
                MaxTokens = 10
            }
        );

        return ParsePriority(response.Value.Choices[0].Message.Content);
    }
}
```

### 5.4 Azure Communication Services
```csharp
public class AzureCommunicationService
{
    private readonly EmailClient _emailClient;
    private readonly SmsClient _smsClient;

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        var emailContent = new EmailContent(subject)
        {
            PlainText = body,
            Html = body
        };

        var emailMessage = new EmailMessage(
            senderAddress: "notifications@eventplatform.com",
            recipientAddress: to,
            content: emailContent
        );

        await _emailClient.SendAsync(emailMessage);
    }

    public async Task SendSmsAsync(string phoneNumber, string message)
    {
        var smsMessage = new SmsSendOptions(
            from: "+1234567890",
            to: phoneNumber,
            message: message
        );

        await _smsClient.SendAsync(smsMessage);
    }
}
```

### 5.5 Azure Notification Hubs
```csharp
public class PushNotificationService
{
    private readonly NotificationHubClient _hubClient;

    public async Task SendPushNotificationAsync(Guid userId, string title, string message, Dictionary<string, string> data)
    {
        var notification = new Dictionary<string, string>
        {
            ["title"] = title,
            ["body"] = message,
            ["data"] = JsonSerializer.Serialize(data)
        };

        var tags = new List<string> { $"userId:{userId}" };
        await _hubClient.SendTemplateNotificationAsync(notification, tags);
    }

    public async Task RegisterDeviceAsync(Guid userId, string deviceToken, string platform)
    {
        var installation = new Installation
        {
            InstallationId = deviceToken,
            Platform = platform == "ios" ? NotificationPlatform.Apns : NotificationPlatform.Fcm,
            PushChannel = deviceToken,
            Tags = new List<string> { $"userId:{userId}" }
        };

        await _hubClient.CreateOrUpdateInstallationAsync(installation);
    }
}
```

## 6. Data Access Layer

### 6.1 Repository Interfaces
```csharp
public interface INotificationRepository
{
    Task<Notification> GetByIdAsync(Guid id);
    Task<PagedResult<Notification>> GetUserNotificationsAsync(Guid userId, NotificationFilter filter);
    Task<Notification> AddAsync(Notification notification);
    Task UpdateAsync(Notification notification);
    Task DeleteAsync(Guid id);
    Task<int> MarkAllAsReadAsync(Guid userId, string category = null);
    Task<NotificationSummary> GetSummaryAsync(Guid userId);
}

public interface IAlertRepository
{
    Task<Alert> GetByIdAsync(Guid id);
    Task<PagedResult<Alert>> GetAlertsAsync(AlertFilter filter);
    Task<Alert> AddAsync(Alert alert);
    Task UpdateAsync(Alert alert);
    Task<List<Alert>> GetUnacknowledgedAlertsAsync(DateTime olderThan);
    Task<List<Alert>> GetActiveAlertsByTypeAsync(AlertType type);
}

public interface INotificationPreferencesRepository
{
    Task<NotificationPreferences> GetByUserIdAsync(Guid userId);
    Task<NotificationPreferences> UpsertAsync(NotificationPreferences preferences);
}
```

### 6.2 Database Schema

```sql
-- Notifications Table
CREATE TABLE Notifications (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    Type NVARCHAR(50) NOT NULL,
    Priority NVARCHAR(50) NOT NULL,
    Title NVARCHAR(500) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    Category NVARCHAR(100),
    Metadata NVARCHAR(MAX), -- JSON
    Actions NVARCHAR(MAX), -- JSON
    Status NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ViewedAt DATETIME2,
    DismissedAt DATETIME2,
    ActionTakenAt DATETIME2,
    ExpiresAt DATETIME2,
    SourceEntityType NVARCHAR(100),
    SourceEntityId UNIQUEIDENTIFIER,
    Channel NVARCHAR(50),
    IsRead BIT NOT NULL DEFAULT 0,
    IsDismissed BIT NOT NULL DEFAULT 0,
    INDEX IX_Notifications_UserId (UserId),
    INDEX IX_Notifications_Status (Status),
    INDEX IX_Notifications_CreatedAt (CreatedAt),
    INDEX IX_Notifications_Priority (Priority)
);

-- Alerts Table
CREATE TABLE Alerts (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Type NVARCHAR(50) NOT NULL,
    Title NVARCHAR(500) NOT NULL,
    Description NVARCHAR(MAX),
    Severity NVARCHAR(50) NOT NULL,
    Source NVARCHAR(200),
    SourceEntityId UNIQUEIDENTIFIER,
    Context NVARCHAR(MAX), -- JSON
    Status NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    AcknowledgedAt DATETIME2,
    AcknowledgedBy UNIQUEIDENTIFIER,
    ResolvedAt DATETIME2,
    ResolvedBy UNIQUEIDENTIFIER,
    EscalatedAt DATETIME2,
    EscalationLevel INT NOT NULL DEFAULT 0,
    AssignedUserIds NVARCHAR(MAX), -- JSON array
    Tags NVARCHAR(MAX), -- JSON array
    INDEX IX_Alerts_Type (Type),
    INDEX IX_Alerts_Severity (Severity),
    INDEX IX_Alerts_Status (Status),
    INDEX IX_Alerts_CreatedAt (CreatedAt)
);

-- NotificationPreferences Table
CREATE TABLE NotificationPreferences (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL UNIQUE,
    CategoryPreferences NVARCHAR(MAX), -- JSON
    EnableEmailNotifications BIT NOT NULL DEFAULT 1,
    EnableSmsNotifications BIT NOT NULL DEFAULT 0,
    EnablePushNotifications BIT NOT NULL DEFAULT 1,
    EnableInAppNotifications BIT NOT NULL DEFAULT 1,
    QuietHours NVARCHAR(MAX), -- JSON
    DigestSettings NVARCHAR(MAX), -- JSON
    MaxFrequency NVARCHAR(50) NOT NULL DEFAULT 'RealTime',
    MutedCategories NVARCHAR(MAX), -- JSON array
    UpdatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    INDEX IX_NotificationPreferences_UserId (UserId)
);

-- NotificationTemplates Table
CREATE TABLE NotificationTemplates (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(200) NOT NULL UNIQUE,
    Category NVARCHAR(100),
    Type NVARCHAR(50) NOT NULL,
    ChannelTemplates NVARCHAR(MAX), -- JSON
    RequiredVariables NVARCHAR(MAX), -- JSON array
    IsActive BIT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    INDEX IX_NotificationTemplates_Name (Name),
    INDEX IX_NotificationTemplates_Category (Category)
);

-- EmergencyBroadcasts Table
CREATE TABLE EmergencyBroadcasts (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(500) NOT NULL,
    Message NVARCHAR(MAX) NOT NULL,
    Channels NVARCHAR(MAX), -- JSON array
    TargetUserIds NVARCHAR(MAX), -- JSON array
    TargetGroups NVARCHAR(MAX), -- JSON array
    Priority NVARCHAR(50) NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedBy UNIQUEIDENTIFIER NOT NULL,
    SentCount INT NOT NULL DEFAULT 0,
    DeliveredCount INT NOT NULL DEFAULT 0,
    FailedCount INT NOT NULL DEFAULT 0,
    ViewedCount INT NOT NULL DEFAULT 0,
    INDEX IX_EmergencyBroadcasts_CreatedAt (CreatedAt)
);
```

## 7. Background Jobs & Scheduled Tasks

### 7.1 Hangfire Jobs
```csharp
public class NotificationBackgroundJobs
{
    [AutomaticRetry(Attempts = 3)]
    public async Task ProcessDigestNotificationsAsync()
    {
        // Process daily/weekly digest notifications
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task CheckAlertEscalationsAsync()
    {
        // Check for alerts that need escalation
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task CleanupExpiredNotificationsAsync()
    {
        // Remove expired notifications
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task ProcessEventRemindersAsync()
    {
        // Send event reminders based on schedule
    }

    [AutomaticRetry(Attempts = 3)]
    public async Task AnalyzeNotificationPatternsAsync()
    {
        // AI-powered analysis of notification patterns
    }
}

// Job Registration
public static class BackgroundJobConfiguration
{
    public static void ConfigureJobs()
    {
        RecurringJob.AddOrUpdate<NotificationBackgroundJobs>(
            "process-digests",
            job => job.ProcessDigestNotificationsAsync(),
            "0 8 * * *"); // Daily at 8 AM

        RecurringJob.AddOrUpdate<NotificationBackgroundJobs>(
            "check-escalations",
            job => job.CheckAlertEscalationsAsync(),
            "*/15 * * * *"); // Every 15 minutes

        RecurringJob.AddOrUpdate<NotificationBackgroundJobs>(
            "cleanup-expired",
            job => job.CleanupExpiredNotificationsAsync(),
            "0 2 * * *"); // Daily at 2 AM
    }
}
```

## 8. Security & Authentication

### 8.1 Authorization Policies
```csharp
public static class NotificationPolicies
{
    public const string ViewNotifications = "ViewNotifications";
    public const string ManageOwnNotifications = "ManageOwnNotifications";
    public const string ManageAllNotifications = "ManageAllNotifications";
    public const string SendEmergencyBroadcasts = "SendEmergencyBroadcasts";
    public const string ManageAlerts = "ManageAlerts";
    public const string EscalateAlerts = "EscalateAlerts";
}

public class NotificationAuthorizationHandler : AuthorizationHandler<OperationAuthorizationRequirement, Notification>
{
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        OperationAuthorizationRequirement requirement,
        Notification resource)
    {
        var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (requirement.Name == "View" && resource.UserId.ToString() == userId)
        {
            context.Succeed(requirement);
        }

        if (context.User.IsInRole("Admin"))
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;
    }
}
```

## 9. Monitoring & Logging

### 9.1 Application Insights Integration
```csharp
public class NotificationTelemetry
{
    private readonly TelemetryClient _telemetryClient;

    public void TrackNotificationSent(Notification notification, string channel)
    {
        _telemetryClient.TrackEvent("NotificationSent", new Dictionary<string, string>
        {
            ["NotificationId"] = notification.Id.ToString(),
            ["Type"] = notification.Type.ToString(),
            ["Priority"] = notification.Priority.ToString(),
            ["Channel"] = channel,
            ["Category"] = notification.Category
        });
    }

    public void TrackAlertCreated(Alert alert)
    {
        _telemetryClient.TrackEvent("AlertCreated", new Dictionary<string, string>
        {
            ["AlertId"] = alert.Id.ToString(),
            ["Type"] = alert.Type.ToString(),
            ["Severity"] = alert.Severity.ToString()
        });
    }

    public void TrackNotificationDeliveryFailure(Guid notificationId, string channel, Exception ex)
    {
        _telemetryClient.TrackException(ex, new Dictionary<string, string>
        {
            ["NotificationId"] = notificationId.ToString(),
            ["Channel"] = channel
        });
    }
}
```

## 10. Performance Optimization

### 10.1 Caching Strategy
```csharp
public class NotificationCachingService
{
    private readonly IDistributedCache _cache;

    public async Task<NotificationPreferences> GetCachedPreferencesAsync(Guid userId)
    {
        var cacheKey = $"preferences:{userId}";
        var cached = await _cache.GetStringAsync(cacheKey);

        if (cached != null)
        {
            return JsonSerializer.Deserialize<NotificationPreferences>(cached);
        }

        return null;
    }

    public async Task CachePreferencesAsync(Guid userId, NotificationPreferences preferences)
    {
        var cacheKey = $"preferences:{userId}";
        var serialized = JsonSerializer.Serialize(preferences);

        await _cache.SetStringAsync(cacheKey, serialized, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
        });
    }
}
```

## 11. Error Handling

### 11.1 Custom Exceptions
```csharp
public class NotificationNotFoundException : NotFoundException
{
    public NotificationNotFoundException(Guid notificationId)
        : base($"Notification with ID {notificationId} not found.") { }
}

public class NotificationDeliveryException : Exception
{
    public string Channel { get; }
    public Guid NotificationId { get; }

    public NotificationDeliveryException(Guid notificationId, string channel, string message, Exception innerException)
        : base(message, innerException)
    {
        NotificationId = notificationId;
        Channel = channel;
    }
}

public class AlertEscalationException : Exception
{
    public Guid AlertId { get; }

    public AlertEscalationException(Guid alertId, string message)
        : base(message)
    {
        AlertId = alertId;
    }
}
```

## 12. Configuration

### 12.1 Application Settings
```json
{
  "NotificationSettings": {
    "MaxNotificationsPerUser": 1000,
    "NotificationRetentionDays": 90,
    "DefaultPageSize": 20,
    "MaxPageSize": 100,
    "EnableRealTimeNotifications": true,
    "EnableDigestNotifications": true,
    "DigestSchedule": "0 8 * * *"
  },
  "AlertSettings": {
    "AutoEscalationEnabled": true,
    "EscalationTimeoutMinutes": 30,
    "MaxEscalationLevel": 3,
    "CriticalAlertRetentionDays": 365
  },
  "AzureServiceBus": {
    "ConnectionString": "...",
    "NotificationQueue": "notifications",
    "AlertQueue": "alerts"
  },
  "AzureSignalR": {
    "ConnectionString": "..."
  },
  "AzureOpenAI": {
    "Endpoint": "...",
    "ApiKey": "...",
    "DeploymentName": "gpt-4"
  },
  "AzureCommunicationServices": {
    "ConnectionString": "...",
    "EmailFrom": "notifications@eventplatform.com",
    "SmsFrom": "+1234567890"
  },
  "AzureNotificationHubs": {
    "ConnectionString": "...",
    "HubName": "event-platform-notifications"
  }
}
```

## 13. Testing Strategy

### 13.1 Unit Tests
- Test notification creation and delivery logic
- Test alert escalation rules
- Test preference validation
- Test template rendering

### 13.2 Integration Tests
- Test Azure Service Bus message processing
- Test SignalR real-time delivery
- Test email/SMS delivery via Azure Communication Services
- Test push notification delivery via Notification Hubs

### 13.3 End-to-End Tests
- Test complete notification flow from creation to delivery
- Test alert lifecycle (creation → acknowledgment → resolution)
- Test emergency broadcast delivery

## 14. Deployment Considerations

### 14.1 Azure Resources Required
- Azure SQL Database or Cosmos DB
- Azure Service Bus namespace
- Azure SignalR Service
- Azure OpenAI Service
- Azure Communication Services
- Azure Notification Hubs
- Azure Redis Cache
- Azure App Service or Container Apps
- Azure Application Insights

### 14.2 Scaling Strategy
- Use Azure Service Bus for asynchronous processing
- Implement connection multiplexing for SignalR
- Use Redis cache for frequently accessed data
- Implement rate limiting for notification delivery
- Use Azure Front Door for global distribution

## 15. Compliance & Data Privacy

### 15.1 GDPR Compliance
- Implement right to erasure for notifications
- Provide data export functionality
- Maintain audit logs for notification access
- Respect user consent for communication channels

### 15.2 Data Retention
- Automatically delete notifications after retention period
- Archive critical alerts for compliance
- Provide user control over notification history
