# User & Access Management - Backend Specification

## 1. Introduction

### 1.1 Purpose
This Software Requirements Specification (SRS) document provides a comprehensive description of the backend implementation for the User & Access Management feature of the Event Management Platform. This document specifies functional and non-functional requirements for user account management, authentication, authorization, role-based access control (RBAC), and permissions management.

### 1.2 Scope
The User & Access Management backend provides:
- User account lifecycle management (creation, updates, deactivation, reactivation)
- Secure authentication and session management
- Role-Based Access Control (RBAC)
- Permission management and enforcement
- Password management (change, reset)
- Audit logging and security monitoring
- Integration with Azure services and Azure AI

### 1.3 Technology Stack
- **Framework**: .NET 8.0
- **Language**: C# 12
- **Cloud Platform**: Microsoft Azure
- **Database**: Azure SQL Database
- **Cache**: Azure Redis Cache
- **Identity**: Azure AD B2C / Azure Entra ID
- **AI Services**: Azure OpenAI Service, Azure AI Content Safety
- **Message Queue**: Azure Service Bus
- **Storage**: Azure Blob Storage
- **Monitoring**: Azure Application Insights
- **Security**: Azure Key Vault

### 1.4 Definitions and Acronyms
- **RBAC**: Role-Based Access Control
- **JWT**: JSON Web Token
- **MFA**: Multi-Factor Authentication
- **CQRS**: Command Query Responsibility Segregation
- **SRS**: Software Requirements Specification
- **API**: Application Programming Interface
- **SSO**: Single Sign-On

## 2. System Architecture

### 2.1 Architectural Pattern
- **Clean Architecture** with separation of concerns
- **CQRS Pattern** for command and query separation
- **Event-Driven Architecture** for domain events
- **Microservices** communication via Azure Service Bus
- **Repository Pattern** for data access abstraction
- **Unit of Work Pattern** for transaction management

### 2.2 Project Structure
```
UserManagement/
├── UserManagement.API/              # Web API Layer
│   ├── Controllers/
│   ├── Filters/
│   ├── Middleware/
│   └── Program.cs
├── UserManagement.Application/      # Application Layer
│   ├── Commands/
│   ├── Queries/
│   ├── DTOs/
│   ├── Validators/
│   ├── Interfaces/
│   └── Services/
├── UserManagement.Domain/           # Domain Layer
│   ├── Entities/
│   ├── ValueObjects/
│   ├── Events/
│   ├── Enums/
│   └── Exceptions/
├── UserManagement.Infrastructure/   # Infrastructure Layer
│   ├── Persistence/
│   ├── Identity/
│   ├── Messaging/
│   ├── Caching/
│   └── AI/
└── UserManagement.Tests/            # Test Projects
    ├── Unit/
    ├── Integration/
    └── E2E/
```

## 3. Domain Model

### 3.1 Core Entities

#### 3.1.1 User
```csharp
public class User : BaseEntity
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string PhoneNumber { get; private set; }
    public string ProfilePictureUrl { get; private set; }
    public UserStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }
    public DateTime? LastLoginAt { get; private set; }
    public DateTime? DeactivatedAt { get; private set; }
    public string DeactivationReason { get; private set; }

    // Navigation Properties
    public ICollection<UserRole> UserRoles { get; private set; }
    public ICollection<UserPermission> UserPermissions { get; private set; }
    public ICollection<UserSession> UserSessions { get; private set; }
    public ICollection<UserAuditLog> AuditLogs { get; private set; }
}
```

#### 3.1.2 Role
```csharp
public class Role : BaseEntity
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public bool IsSystemRole { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    // Navigation Properties
    public ICollection<UserRole> UserRoles { get; private set; }
    public ICollection<RolePermission> RolePermissions { get; private set; }
}
```

#### 3.1.3 Permission
```csharp
public class Permission : BaseEntity
{
    public Guid Id { get; private set; }
    public string Resource { get; private set; }
    public string Action { get; private set; }
    public string Description { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Navigation Properties
    public ICollection<RolePermission> RolePermissions { get; private set; }
    public ICollection<UserPermission> UserPermissions { get; private set; }
}
```

#### 3.1.4 UserRole (Junction Table)
```csharp
public class UserRole : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid RoleId { get; private set; }
    public DateTime AssignedAt { get; private set; }
    public Guid AssignedBy { get; private set; }

    // Navigation Properties
    public User User { get; private set; }
    public Role Role { get; private set; }
}
```

#### 3.1.5 UserPermission (Direct Permissions)
```csharp
public class UserPermission : BaseEntity
{
    public Guid UserId { get; private set; }
    public Guid PermissionId { get; private set; }
    public DateTime GrantedAt { get; private set; }
    public Guid GrantedBy { get; private set; }

    // Navigation Properties
    public User User { get; private set; }
    public Permission Permission { get; private set; }
}
```

#### 3.1.6 UserSession
```csharp
public class UserSession : BaseEntity
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string RefreshToken { get; private set; }
    public string DeviceInfo { get; private set; }
    public string IpAddress { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime ExpiresAt { get; private set; }
    public DateTime? RevokedAt { get; private set; }
    public bool IsActive { get; private set; }

    // Navigation Properties
    public User User { get; private set; }
}
```

#### 3.1.7 UserAuditLog
```csharp
public class UserAuditLog : BaseEntity
{
    public Guid Id { get; private set; }
    public Guid? UserId { get; private set; }
    public string EventType { get; private set; }
    public string Action { get; private set; }
    public string Details { get; private set; }
    public string IpAddress { get; private set; }
    public string UserAgent { get; private set; }
    public DateTime Timestamp { get; private set; }
    public bool IsSuccessful { get; private set; }
    public string FailureReason { get; private set; }

    // Navigation Properties
    public User User { get; private set; }
}
```

### 3.2 Enumerations

```csharp
public enum UserStatus
{
    Active = 1,
    Inactive = 2,
    Deactivated = 3,
    Suspended = 4,
    Locked = 5
}

public enum AuthenticationResult
{
    Success = 1,
    InvalidCredentials = 2,
    AccountLocked = 3,
    AccountDeactivated = 4,
    MfaRequired = 5,
    PasswordExpired = 6
}
```

### 3.3 Value Objects

```csharp
public class Email : ValueObject
{
    public string Value { get; private set; }

    private Email(string value)
    {
        Value = value;
    }

    public static Email Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email cannot be empty");

        if (!IsValidEmail(email))
            throw new ArgumentException("Invalid email format");

        return new Email(email.ToLowerInvariant());
    }
}

public class Password : ValueObject
{
    public string Hash { get; private set; }
    public string Salt { get; private set; }

    // Password strength validation and hashing logic
}
```

## 4. Domain Events

### 4.1 User Account Events

```csharp
public record UserAccountCreated : DomainEvent
{
    public Guid UserId { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
    public DateTime CreatedAt { get; init; }
}

public record UserAccountUpdated : DomainEvent
{
    public Guid UserId { get; init; }
    public Dictionary<string, object> Changes { get; init; }
    public DateTime UpdatedAt { get; init; }
}

public record UserAccountDeactivated : DomainEvent
{
    public Guid UserId { get; init; }
    public string Reason { get; init; }
    public DateTime DeactivatedAt { get; init; }
}

public record UserAccountReactivated : DomainEvent
{
    public Guid UserId { get; init; }
    public DateTime ReactivatedAt { get; init; }
}
```

### 4.2 Password Events

```csharp
public record UserPasswordChanged : DomainEvent
{
    public Guid UserId { get; init; }
    public DateTime ChangedAt { get; init; }
}

public record UserPasswordReset : DomainEvent
{
    public Guid UserId { get; init; }
    public string ResetMethod { get; init; }
    public DateTime ResetAt { get; init; }
}
```

### 4.3 Authentication Events

```csharp
public record UserLoginSuccessful : DomainEvent
{
    public Guid UserId { get; init; }
    public string IpAddress { get; init; }
    public string DeviceInfo { get; init; }
    public DateTime LoginAt { get; init; }
}

public record UserLoginFailed : DomainEvent
{
    public string Email { get; init; }
    public string Reason { get; init; }
    public string IpAddress { get; init; }
    public DateTime AttemptedAt { get; init; }
}

public record UserLoggedOut : DomainEvent
{
    public Guid UserId { get; init; }
    public Guid SessionId { get; init; }
    public DateTime LoggedOutAt { get; init; }
}

public record UserSessionExpired : DomainEvent
{
    public Guid UserId { get; init; }
    public Guid SessionId { get; init; }
    public DateTime ExpiredAt { get; init; }
}
```

### 4.4 Authorization Events

```csharp
public record UserRoleAssigned : DomainEvent
{
    public Guid UserId { get; init; }
    public Guid RoleId { get; init; }
    public string RoleName { get; init; }
    public Guid AssignedBy { get; init; }
    public DateTime AssignedAt { get; init; }
}

public record UserRoleRevoked : DomainEvent
{
    public Guid UserId { get; init; }
    public Guid RoleId { get; init; }
    public string RoleName { get; init; }
    public Guid RevokedBy { get; init; }
    public DateTime RevokedAt { get; init; }
}

public record UserPermissionGranted : DomainEvent
{
    public Guid UserId { get; init; }
    public Guid PermissionId { get; init; }
    public string Resource { get; init; }
    public string Action { get; init; }
    public Guid GrantedBy { get; init; }
    public DateTime GrantedAt { get; init; }
}

public record UserPermissionRevoked : DomainEvent
{
    public Guid UserId { get; init; }
    public Guid PermissionId { get; init; }
    public string Resource { get; init; }
    public string Action { get; init; }
    public Guid RevokedBy { get; init; }
    public DateTime RevokedAt { get; init; }
}

public record UnauthorizedAccessAttempted : DomainEvent
{
    public Guid? UserId { get; init; }
    public string Resource { get; init; }
    public string Action { get; init; }
    public string IpAddress { get; init; }
    public DateTime AttemptedAt { get; init; }
}
```

## 5. API Endpoints

### 5.1 Authentication Endpoints

#### POST /api/v1/auth/register
Register a new user account.

**Request:**
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890"
}
```

**Response (201 Created):**
```json
{
  "userId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "user@example.com",
  "message": "Account created successfully. Please verify your email."
}
```

#### POST /api/v1/auth/login
Authenticate user and return JWT tokens.

**Request:**
```json
{
  "email": "user@example.com",
  "password": "SecurePassword123!"
}
```

**Response (200 OK):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "expiresIn": 3600,
  "tokenType": "Bearer",
  "user": {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "email": "user@example.com",
    "firstName": "John",
    "lastName": "Doe",
    "roles": ["User"]
  }
}
```

#### POST /api/v1/auth/refresh
Refresh access token using refresh token.

**Request:**
```json
{
  "refreshToken": "3fa85f64-5717-4562-b3fc-2c963f66afa6"
}
```

**Response (200 OK):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "4gb96g75-6828-5673-c4gd-3d074g77bgb7",
  "expiresIn": 3600,
  "tokenType": "Bearer"
}
```

#### POST /api/v1/auth/logout
Logout and invalidate session.

**Request Headers:**
```
Authorization: Bearer {accessToken}
```

**Response (204 No Content)**

#### POST /api/v1/auth/forgot-password
Initiate password reset process.

**Request:**
```json
{
  "email": "user@example.com"
}
```

**Response (200 OK):**
```json
{
  "message": "Password reset instructions sent to your email."
}
```

#### POST /api/v1/auth/reset-password
Reset password using reset token.

**Request:**
```json
{
  "resetToken": "abc123...",
  "newPassword": "NewSecurePassword123!"
}
```

**Response (200 OK):**
```json
{
  "message": "Password reset successful. Please login with your new password."
}
```

### 5.2 User Management Endpoints

#### GET /api/v1/users
Get paginated list of users (Admin only).

**Query Parameters:**
- `page` (default: 1)
- `pageSize` (default: 20, max: 100)
- `search` (optional)
- `status` (optional)
- `role` (optional)

**Response (200 OK):**
```json
{
  "items": [
    {
      "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
      "email": "user@example.com",
      "firstName": "John",
      "lastName": "Doe",
      "status": "Active",
      "roles": ["User"],
      "createdAt": "2025-01-01T00:00:00Z",
      "lastLoginAt": "2025-01-15T10:30:00Z"
    }
  ],
  "totalCount": 150,
  "page": 1,
  "pageSize": 20,
  "totalPages": 8
}
```

#### GET /api/v1/users/{userId}
Get user details by ID.

**Response (200 OK):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890",
  "profilePictureUrl": "https://storage.azure.com/...",
  "status": "Active",
  "roles": [
    {
      "id": "role-guid",
      "name": "User",
      "assignedAt": "2025-01-01T00:00:00Z"
    }
  ],
  "permissions": [
    {
      "id": "perm-guid",
      "resource": "Events",
      "action": "Read"
    }
  ],
  "createdAt": "2025-01-01T00:00:00Z",
  "lastLoginAt": "2025-01-15T10:30:00Z"
}
```

#### GET /api/v1/users/me
Get current authenticated user profile.

**Response (200 OK):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890",
  "profilePictureUrl": "https://storage.azure.com/...",
  "roles": ["User"],
  "permissions": ["Events.Read", "Events.Create"]
}
```

#### PUT /api/v1/users/{userId}
Update user account.

**Request:**
```json
{
  "firstName": "John",
  "lastName": "Doe",
  "phoneNumber": "+1234567890"
}
```

**Response (200 OK):**
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "email": "user@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "updatedAt": "2025-01-15T10:30:00Z"
}
```

#### PUT /api/v1/users/me/change-password
Change current user's password.

**Request:**
```json
{
  "currentPassword": "OldPassword123!",
  "newPassword": "NewSecurePassword123!"
}
```

**Response (200 OK):**
```json
{
  "message": "Password changed successfully."
}
```

#### POST /api/v1/users/{userId}/deactivate
Deactivate user account (Admin only).

**Request:**
```json
{
  "reason": "User requested account deletion"
}
```

**Response (200 OK):**
```json
{
  "message": "User account deactivated successfully.",
  "deactivatedAt": "2025-01-15T10:30:00Z"
}
```

#### POST /api/v1/users/{userId}/reactivate
Reactivate user account (Admin only).

**Response (200 OK):**
```json
{
  "message": "User account reactivated successfully.",
  "reactivatedAt": "2025-01-15T10:30:00Z"
}
```

### 5.3 Role Management Endpoints

#### GET /api/v1/roles
Get all roles.

**Response (200 OK):**
```json
{
  "items": [
    {
      "id": "role-guid",
      "name": "Administrator",
      "description": "Full system access",
      "isSystemRole": true,
      "permissionCount": 25,
      "userCount": 5
    }
  ]
}
```

#### POST /api/v1/roles
Create new role (Admin only).

**Request:**
```json
{
  "name": "EventManager",
  "description": "Manages events and attendees"
}
```

**Response (201 Created):**
```json
{
  "id": "role-guid",
  "name": "EventManager",
  "description": "Manages events and attendees",
  "createdAt": "2025-01-15T10:30:00Z"
}
```

#### PUT /api/v1/roles/{roleId}
Update role (Admin only).

#### DELETE /api/v1/roles/{roleId}
Delete role (Admin only, cannot delete system roles).

#### POST /api/v1/users/{userId}/roles
Assign role to user (Admin only).

**Request:**
```json
{
  "roleId": "role-guid"
}
```

**Response (200 OK):**
```json
{
  "message": "Role assigned successfully.",
  "userId": "user-guid",
  "roleId": "role-guid",
  "assignedAt": "2025-01-15T10:30:00Z"
}
```

#### DELETE /api/v1/users/{userId}/roles/{roleId}
Revoke role from user (Admin only).

### 5.4 Permission Management Endpoints

#### GET /api/v1/permissions
Get all permissions.

**Response (200 OK):**
```json
{
  "items": [
    {
      "id": "perm-guid",
      "resource": "Events",
      "action": "Create",
      "description": "Create new events"
    }
  ]
}
```

#### POST /api/v1/roles/{roleId}/permissions
Assign permission to role (Admin only).

**Request:**
```json
{
  "permissionId": "perm-guid"
}
```

#### DELETE /api/v1/roles/{roleId}/permissions/{permissionId}
Remove permission from role (Admin only).

#### POST /api/v1/users/{userId}/permissions
Grant direct permission to user (Admin only).

#### DELETE /api/v1/users/{userId}/permissions/{permissionId}
Revoke direct permission from user (Admin only).

#### GET /api/v1/users/{userId}/effective-permissions
Get all effective permissions for user (including role-based).

**Response (200 OK):**
```json
{
  "userId": "user-guid",
  "permissions": [
    {
      "resource": "Events",
      "action": "Read",
      "source": "Role:User"
    },
    {
      "resource": "Events",
      "action": "Create",
      "source": "Direct"
    }
  ]
}
```

### 5.5 Audit & Monitoring Endpoints

#### GET /api/v1/users/{userId}/audit-logs
Get audit logs for specific user (Admin only).

**Query Parameters:**
- `startDate` (optional)
- `endDate` (optional)
- `eventType` (optional)
- `page` (default: 1)
- `pageSize` (default: 20)

**Response (200 OK):**
```json
{
  "items": [
    {
      "id": "log-guid",
      "eventType": "UserLoginSuccessful",
      "action": "Login",
      "timestamp": "2025-01-15T10:30:00Z",
      "ipAddress": "192.168.1.1",
      "deviceInfo": "Mozilla/5.0...",
      "isSuccessful": true
    }
  ],
  "totalCount": 45,
  "page": 1,
  "pageSize": 20
}
```

#### GET /api/v1/audit/unauthorized-attempts
Get unauthorized access attempts (Admin only).

## 6. Application Services

### 6.1 Command Handlers

```csharp
// User Account Commands
public class CreateUserAccountCommandHandler : IRequestHandler<CreateUserAccountCommand, UserDto>
public class UpdateUserAccountCommandHandler : IRequestHandler<UpdateUserAccountCommand, UserDto>
public class DeactivateUserAccountCommandHandler : IRequestHandler<DeactivateUserAccountCommand, Unit>
public class ReactivateUserAccountCommandHandler : IRequestHandler<ReactivateUserAccountCommand, Unit>

// Authentication Commands
public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponse>
public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthenticationResponse>
public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Unit>
public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Unit>

// Role & Permission Commands
public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, Unit>
public class RevokeRoleFromUserCommandHandler : IRequestHandler<RevokeRoleFromUserCommand, Unit>
public class GrantPermissionToUserCommandHandler : IRequestHandler<GrantPermissionToUserCommand, Unit>
public class RevokePermissionFromUserCommandHandler : IRequestHandler<RevokePermissionFromUserCommand, Unit>
```

### 6.2 Query Handlers

```csharp
public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto>
public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, PagedResult<UserDto>>
public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, IEnumerable<RoleDto>>
public class GetUserPermissionsQueryHandler : IRequestHandler<GetUserPermissionsQuery, IEnumerable<PermissionDto>>
public class GetEffectivePermissionsQueryHandler : IRequestHandler<GetEffectivePermissionsQuery, IEnumerable<PermissionDto>>
public class GetAuditLogsQueryHandler : IRequestHandler<GetAuditLogsQuery, PagedResult<AuditLogDto>>
```

### 6.3 Domain Services

```csharp
public interface IAuthenticationService
{
    Task<AuthenticationResult> AuthenticateAsync(string email, string password);
    Task<string> GenerateAccessTokenAsync(User user);
    Task<string> GenerateRefreshTokenAsync(User user);
    Task<bool> ValidateRefreshTokenAsync(string refreshToken);
    Task RevokeRefreshTokenAsync(string refreshToken);
}

public interface IPasswordService
{
    string HashPassword(string password, out string salt);
    bool VerifyPassword(string password, string hash, string salt);
    bool ValidatePasswordStrength(string password);
    Task<string> GeneratePasswordResetTokenAsync(User user);
    Task<bool> ValidatePasswordResetTokenAsync(string token);
}

public interface IAuthorizationService
{
    Task<bool> HasPermissionAsync(Guid userId, string resource, string action);
    Task<IEnumerable<Permission>> GetEffectivePermissionsAsync(Guid userId);
    Task<bool> IsInRoleAsync(Guid userId, string roleName);
}

public interface ISessionService
{
    Task<UserSession> CreateSessionAsync(User user, string deviceInfo, string ipAddress);
    Task RevokeSessionAsync(Guid sessionId);
    Task RevokeAllUserSessionsAsync(Guid userId);
    Task<bool> IsSessionActiveAsync(Guid sessionId);
    Task CleanupExpiredSessionsAsync();
}

public interface IAuditService
{
    Task LogEventAsync(DomainEvent domainEvent);
    Task LogUnauthorizedAccessAsync(Guid? userId, string resource, string action, string ipAddress);
    Task<PagedResult<UserAuditLog>> GetUserAuditLogsAsync(Guid userId, AuditLogFilter filter);
}
```

## 7. Azure Integration

### 7.1 Azure AD B2C / Entra ID Integration

```csharp
public class AzureAdAuthenticationService : IAuthenticationService
{
    private readonly IConfidentialClientApplication _confidentialClient;

    // SSO implementation
    // External identity provider integration (Google, Microsoft, Facebook)
    // Token validation
    // Claims mapping
}
```

**Configuration:**
```json
{
  "AzureAdB2C": {
    "Instance": "https://login.microsoftonline.com/",
    "ClientId": "your-client-id",
    "Domain": "yourtenant.onmicrosoft.com",
    "SignUpSignInPolicyId": "B2C_1_signupsignin1",
    "ResetPasswordPolicyId": "B2C_1_passwordreset1",
    "EditProfilePolicyId": "B2C_1_profileedit1"
  }
}
```

### 7.2 Azure Key Vault

Store sensitive configuration:
- JWT signing keys
- Database connection strings
- Azure AD client secrets
- API keys

```csharp
public class KeyVaultSecretsProvider
{
    private readonly SecretClient _secretClient;

    public async Task<string> GetSecretAsync(string secretName)
    {
        var secret = await _secretClient.GetSecretAsync(secretName);
        return secret.Value.Value;
    }
}
```

### 7.3 Azure Redis Cache

Cache user permissions, roles, and session data for performance.

```csharp
public class RedisCacheService : ICacheService
{
    private readonly IConnectionMultiplexer _redis;

    public async Task<T> GetAsync<T>(string key)
    {
        var db = _redis.GetDatabase();
        var value = await db.StringGetAsync(key);
        return value.HasValue ? JsonSerializer.Deserialize<T>(value) : default;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan expiration)
    {
        var db = _redis.GetDatabase();
        var serialized = JsonSerializer.Serialize(value);
        await db.StringSetAsync(key, serialized, expiration);
    }
}
```

**Cache Keys:**
- `user:{userId}:permissions` - User permissions (TTL: 15 minutes)
- `user:{userId}:roles` - User roles (TTL: 15 minutes)
- `session:{sessionId}` - Session data (TTL: session expiration)

### 7.4 Azure Service Bus

Publish domain events for asynchronous processing and inter-service communication.

```csharp
public class ServiceBusEventPublisher : IEventPublisher
{
    private readonly ServiceBusSender _sender;

    public async Task PublishAsync<TEvent>(TEvent domainEvent) where TEvent : DomainEvent
    {
        var message = new ServiceBusMessage(JsonSerializer.Serialize(domainEvent))
        {
            Subject = domainEvent.GetType().Name,
            MessageId = Guid.NewGuid().ToString(),
            ContentType = "application/json"
        };

        await _sender.SendMessageAsync(message);
    }
}
```

**Topics & Subscriptions:**
- `user-events` topic
  - `notification-service` subscription
  - `analytics-service` subscription
  - `audit-service` subscription

### 7.5 Azure OpenAI Service Integration

Enhance security and user experience with AI capabilities.

```csharp
public interface IAISecurityService
{
    Task<bool> DetectSuspiciousActivityAsync(UserAuditLog auditLog);
    Task<RiskScore> AssessLoginRiskAsync(LoginAttempt attempt);
    Task<bool> ValidateContentSafetyAsync(string content);
}

public class AzureOpenAISecurityService : IAISecurityService
{
    private readonly OpenAIClient _openAIClient;

    public async Task<RiskScore> AssessLoginRiskAsync(LoginAttempt attempt)
    {
        // Analyze login patterns, location, device, time
        // Use GPT-4 for anomaly detection
        // Return risk score (Low, Medium, High)
    }

    public async Task<bool> DetectSuspiciousActivityAsync(UserAuditLog auditLog)
    {
        // Analyze user behavior patterns
        // Detect potential security threats
        // Trigger alerts for suspicious activity
    }
}
```

### 7.6 Azure AI Content Safety

Validate user-generated content (profile information, usernames).

```csharp
public class ContentSafetyService
{
    private readonly ContentSafetyClient _contentSafetyClient;

    public async Task<bool> ValidateUsernameAsync(string username)
    {
        var response = await _contentSafetyClient.AnalyzeTextAsync(username);
        return !response.Value.HasViolations;
    }
}
```

### 7.7 Azure Application Insights

Monitor and track application performance, errors, and security events.

```csharp
public class ApplicationInsightsLogger
{
    private readonly TelemetryClient _telemetryClient;

    public void TrackSecurityEvent(string eventName, Dictionary<string, string> properties)
    {
        _telemetryClient.TrackEvent(eventName, properties);
    }

    public void TrackLoginAttempt(string email, bool success, string reason)
    {
        var properties = new Dictionary<string, string>
        {
            { "Email", email },
            { "Success", success.ToString() },
            { "Reason", reason }
        };
        _telemetryClient.TrackEvent("LoginAttempt", properties);
    }
}
```

## 8. Security Requirements

### 8.1 Authentication Security

1. **Password Requirements:**
   - Minimum 8 characters
   - At least one uppercase letter
   - At least one lowercase letter
   - At least one number
   - At least one special character
   - Password history: prevent reuse of last 5 passwords

2. **Account Lockout:**
   - Lock account after 5 failed login attempts
   - Lockout duration: 15 minutes
   - Send email notification on account lockout

3. **Session Management:**
   - Access token expiration: 1 hour
   - Refresh token expiration: 7 days
   - Automatic session timeout: 30 minutes of inactivity
   - Single session per user (revoke previous sessions on new login)

4. **Multi-Factor Authentication (MFA):**
   - Support TOTP (Time-based One-Time Password)
   - Support SMS verification
   - Support email verification
   - Optional for regular users, mandatory for administrators

### 8.2 Authorization Security

1. **Permission Enforcement:**
   - Check permissions on every API request
   - Cache permissions in Redis for performance
   - Invalidate cache on permission changes

2. **Role Hierarchy:**
   - Administrator > EventManager > User
   - Higher roles inherit lower role permissions

3. **Audit Trail:**
   - Log all authentication attempts
   - Log all authorization failures
   - Log all administrative actions
   - Retain audit logs for 1 year

### 8.3 Data Protection

1. **Encryption:**
   - Encrypt passwords using bcrypt (cost factor: 12)
   - Encrypt sensitive data at rest using AES-256
   - Use TLS 1.3 for data in transit

2. **PII Protection:**
   - Hash email addresses in audit logs
   - Mask sensitive data in logs
   - Implement GDPR data deletion

3. **Token Security:**
   - Sign JWT tokens using RS256
   - Store signing keys in Azure Key Vault
   - Rotate keys every 90 days

### 8.4 API Security

1. **Rate Limiting:**
   - Authentication endpoints: 5 requests per minute per IP
   - General API endpoints: 100 requests per minute per user

2. **CORS Configuration:**
   - Whitelist allowed origins
   - Restrict allowed methods (GET, POST, PUT, DELETE)

3. **Input Validation:**
   - Validate all input against strict schemas
   - Sanitize input to prevent injection attacks
   - Use FluentValidation for request validation

## 9. Database Schema

### 9.1 Users Table
```sql
CREATE TABLE Users (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Email NVARCHAR(255) NOT NULL UNIQUE,
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100) NOT NULL,
    PhoneNumber NVARCHAR(20),
    ProfilePictureUrl NVARCHAR(500),
    Status INT NOT NULL DEFAULT 1,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,
    LastLoginAt DATETIME2,
    DeactivatedAt DATETIME2,
    DeactivationReason NVARCHAR(500),

    INDEX IX_Users_Email (Email),
    INDEX IX_Users_Status (Status)
);
```

### 9.2 Roles Table
```sql
CREATE TABLE Roles (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Name NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(500),
    IsSystemRole BIT NOT NULL DEFAULT 0,
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt DATETIME2,

    INDEX IX_Roles_Name (Name)
);
```

### 9.3 Permissions Table
```sql
CREATE TABLE Permissions (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Resource NVARCHAR(100) NOT NULL,
    Action NVARCHAR(50) NOT NULL,
    Description NVARCHAR(500),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),

    CONSTRAINT UQ_Permissions_Resource_Action UNIQUE (Resource, Action),
    INDEX IX_Permissions_Resource (Resource)
);
```

### 9.4 UserRoles Table
```sql
CREATE TABLE UserRoles (
    UserId UNIQUEIDENTIFIER NOT NULL,
    RoleId UNIQUEIDENTIFIER NOT NULL,
    AssignedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    AssignedBy UNIQUEIDENTIFIER NOT NULL,

    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE CASCADE,
    FOREIGN KEY (AssignedBy) REFERENCES Users(Id)
);
```

### 9.5 UserPermissions Table
```sql
CREATE TABLE UserPermissions (
    UserId UNIQUEIDENTIFIER NOT NULL,
    PermissionId UNIQUEIDENTIFIER NOT NULL,
    GrantedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    GrantedBy UNIQUEIDENTIFIER NOT NULL,

    PRIMARY KEY (UserId, PermissionId),
    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    FOREIGN KEY (PermissionId) REFERENCES Permissions(Id) ON DELETE CASCADE,
    FOREIGN KEY (GrantedBy) REFERENCES Users(Id)
);
```

### 9.6 RolePermissions Table
```sql
CREATE TABLE RolePermissions (
    RoleId UNIQUEIDENTIFIER NOT NULL,
    PermissionId UNIQUEIDENTIFIER NOT NULL,

    PRIMARY KEY (RoleId, PermissionId),
    FOREIGN KEY (RoleId) REFERENCES Roles(Id) ON DELETE CASCADE,
    FOREIGN KEY (PermissionId) REFERENCES Permissions(Id) ON DELETE CASCADE
);
```

### 9.7 UserSessions Table
```sql
CREATE TABLE UserSessions (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER NOT NULL,
    RefreshToken NVARCHAR(500) NOT NULL UNIQUE,
    DeviceInfo NVARCHAR(500),
    IpAddress NVARCHAR(50),
    CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    ExpiresAt DATETIME2 NOT NULL,
    RevokedAt DATETIME2,
    IsActive BIT NOT NULL DEFAULT 1,

    FOREIGN KEY (UserId) REFERENCES Users(Id) ON DELETE CASCADE,
    INDEX IX_UserSessions_RefreshToken (RefreshToken),
    INDEX IX_UserSessions_UserId (UserId),
    INDEX IX_UserSessions_ExpiresAt (ExpiresAt)
);
```

### 9.8 UserAuditLogs Table
```sql
CREATE TABLE UserAuditLogs (
    Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    UserId UNIQUEIDENTIFIER,
    EventType NVARCHAR(100) NOT NULL,
    Action NVARCHAR(100) NOT NULL,
    Details NVARCHAR(MAX),
    IpAddress NVARCHAR(50),
    UserAgent NVARCHAR(500),
    Timestamp DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    IsSuccessful BIT NOT NULL,
    FailureReason NVARCHAR(500),

    FOREIGN KEY (UserId) REFERENCES Users(Id),
    INDEX IX_UserAuditLogs_UserId (UserId),
    INDEX IX_UserAuditLogs_EventType (EventType),
    INDEX IX_UserAuditLogs_Timestamp (Timestamp)
);
```

## 10. Testing Requirements

### 10.1 Unit Tests
- Domain entity validation
- Value object creation
- Command and query handlers
- Domain services
- Password hashing and validation
- JWT token generation and validation
- Permission evaluation logic

### 10.2 Integration Tests
- API endpoint testing
- Database operations
- Azure Service Bus event publishing
- Azure AD authentication flow
- Redis caching
- Email service integration

### 10.3 Security Tests
- SQL injection prevention
- XSS prevention
- CSRF protection
- Authentication bypass attempts
- Authorization bypass attempts
- Rate limiting enforcement

### 10.4 Performance Tests
- Authentication endpoint load testing
- Permission check performance
- Database query optimization
- Cache effectiveness
- Concurrent user sessions

## 11. Monitoring and Logging

### 11.1 Application Insights Metrics
- User registration rate
- Login success/failure rate
- Average response time per endpoint
- Active user sessions
- Failed authorization attempts
- Password reset requests

### 11.2 Custom Alerts
- High rate of failed login attempts (potential brute force)
- Unusual login patterns (location, time)
- Multiple unauthorized access attempts
- System role modifications
- Mass permission changes

### 11.3 Log Levels
- **Information**: Successful operations
- **Warning**: Failed login attempts, validation failures
- **Error**: Exceptions, service failures
- **Critical**: Security breaches, data corruption

## 12. Non-Functional Requirements

### 12.1 Performance
- API response time: < 200ms (95th percentile)
- Authentication: < 500ms
- Permission check: < 50ms (with caching)
- Support 10,000 concurrent users
- Database query time: < 100ms

### 12.2 Scalability
- Horizontal scaling via Azure App Service
- Database read replicas for query optimization
- Redis cache cluster for high availability
- Service Bus for asynchronous processing

### 12.3 Availability
- 99.9% uptime SLA
- Multi-region deployment for disaster recovery
- Automated failover mechanisms
- Regular database backups (every 6 hours)

### 12.4 Compliance
- GDPR compliance for EU users
- SOC 2 Type II compliance
- ISO 27001 compliance
- Regular security audits

## 13. Deployment

### 13.1 Azure Resources
- Azure App Service (Web API)
- Azure SQL Database (Premium tier)
- Azure Redis Cache (Standard tier)
- Azure Service Bus (Standard tier)
- Azure Key Vault
- Azure Application Insights
- Azure Blob Storage
- Azure OpenAI Service

### 13.2 CI/CD Pipeline
- GitHub Actions for automated builds
- Automated testing on PR
- Blue-green deployment strategy
- Automated database migrations
- Rollback procedures

### 13.3 Environment Configuration
- Development
- Staging
- Production

Each environment has isolated Azure resources and configuration.

## 14. Dependencies

```xml
<ItemGroup>
  <!-- Framework -->
  <PackageReference Include="Microsoft.AspNetCore.App" />

  <!-- Entity Framework -->
  <PackageReference Include="Microsoft.EntityFrameworkCore" Version="8.0.0" />
  <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="8.0.0" />

  <!-- MediatR for CQRS -->
  <PackageReference Include="MediatR" Version="12.2.0" />

  <!-- FluentValidation -->
  <PackageReference Include="FluentValidation.AspNetCore" Version="11.3.0" />

  <!-- Authentication -->
  <PackageReference Include="Microsoft.AspNetCore.Authentication.JwtBearer" Version="8.0.0" />
  <PackageReference Include="Microsoft.Identity.Web" Version="2.16.0" />

  <!-- Azure SDKs -->
  <PackageReference Include="Azure.Identity" Version="1.10.4" />
  <PackageReference Include="Azure.Security.KeyVault.Secrets" Version="4.5.0" />
  <PackageReference Include="Azure.Messaging.ServiceBus" Version="7.17.0" />
  <PackageReference Include="Azure.AI.OpenAI" Version="1.0.0-beta.12" />
  <PackageReference Include="Azure.AI.ContentSafety" Version="1.0.0" />
  <PackageReference Include="Microsoft.ApplicationInsights.AspNetCore" Version="2.21.0" />

  <!-- Redis -->
  <PackageReference Include="StackExchange.Redis" Version="2.7.10" />

  <!-- Mapping -->
  <PackageReference Include="AutoMapper" Version="12.0.1" />

  <!-- Security -->
  <PackageReference Include="BCrypt.Net-Next" Version="4.0.3" />

  <!-- Testing -->
  <PackageReference Include="xUnit" Version="2.6.4" />
  <PackageReference Include="Moq" Version="4.20.70" />
  <PackageReference Include="FluentAssertions" Version="6.12.0" />
</ItemGroup>
```

## 15. Appendix

### 15.1 Default Roles

1. **Administrator**: Full system access
2. **EventManager**: Create and manage events
3. **User**: Basic access to attend events

### 15.2 Default Permissions

| Resource | Actions |
|----------|---------|
| Users | Read, Create, Update, Delete |
| Roles | Read, Create, Update, Delete |
| Permissions | Read, Assign, Revoke |
| Events | Read, Create, Update, Delete |
| Registrations | Read, Create, Cancel |

### 15.3 Error Codes

| Code | Description |
|------|-------------|
| AUTH001 | Invalid credentials |
| AUTH002 | Account locked |
| AUTH003 | Account deactivated |
| AUTH004 | Invalid refresh token |
| AUTH005 | Password expired |
| AUTHZ001 | Insufficient permissions |
| AUTHZ002 | Role not found |
| AUTHZ003 | Permission denied |
| USR001 | User not found |
| USR002 | Email already exists |
| USR003 | Invalid password format |

---

**Document Version**: 1.0
**Last Updated**: 2025-01-15
**Author**: Technical Architecture Team
