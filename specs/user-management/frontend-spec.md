# User & Access Management - Frontend Specification

## 1. Introduction

### 1.1 Purpose
This Software Requirements Specification (SRS) document provides comprehensive requirements for the frontend implementation of the User & Access Management feature of the Event Management Platform. This document specifies the user interface, user experience, component architecture, and integration requirements for Angular 18+ and Angular Material.

### 1.2 Scope
The User & Access Management frontend provides:
- User registration and authentication interfaces
- Profile management and settings
- Role and permission management (admin interfaces)
- Security features (password management, session management)
- Responsive and accessible user interfaces
- Real-time notifications and feedback
- Audit log viewing capabilities

### 1.3 Technology Stack
- **Framework**: Angular 18+
- **Language**: TypeScript 5.4+
- **UI Library**: Angular Material 18+
- **State Management**: NgRx (Store, Effects, Entity)
- **Forms**: Reactive Forms with Angular Material
- **HTTP Client**: Angular HttpClient with Interceptors
- **Authentication**: JWT token-based with refresh token rotation
- **Routing**: Angular Router with guards
- **Styling**: SCSS with Angular Material theming
- **Icons**: Material Icons
- **Accessibility**: WCAG 2.1 Level AA compliant

### 1.4 Definitions and Acronyms
- **SPA**: Single Page Application
- **RBAC**: Role-Based Access Control
- **JWT**: JSON Web Token
- **MFA**: Multi-Factor Authentication
- **UX**: User Experience
- **UI**: User Interface
- **WCAG**: Web Content Accessibility Guidelines

## 2. Application Architecture

### 2.1 Module Structure
```
src/app/
├── core/                           # Core module (singleton services)
│   ├── auth/
│   │   ├── guards/
│   │   ├── interceptors/
│   │   ├── services/
│   │   └── models/
│   ├── services/
│   └── interceptors/
├── features/                       # Feature modules
│   ├── auth/                       # Authentication module
│   │   ├── components/
│   │   │   ├── login/
│   │   │   ├── register/
│   │   │   ├── forgot-password/
│   │   │   └── reset-password/
│   │   ├── store/
│   │   ├── services/
│   │   └── auth.module.ts
│   ├── user-management/            # User management module
│   │   ├── components/
│   │   │   ├── user-list/
│   │   │   ├── user-detail/
│   │   │   ├── user-profile/
│   │   │   ├── change-password/
│   │   │   └── user-form/
│   │   ├── store/
│   │   ├── services/
│   │   └── user-management.module.ts
│   ├── role-management/            # Role management module
│   │   ├── components/
│   │   │   ├── role-list/
│   │   │   ├── role-detail/
│   │   │   ├── role-form/
│   │   │   └── assign-permissions/
│   │   ├── store/
│   │   ├── services/
│   │   └── role-management.module.ts
│   └── audit-logs/                 # Audit logs module
│       ├── components/
│       ├── store/
│       ├── services/
│       └── audit-logs.module.ts
├── shared/                         # Shared module
│   ├── components/
│   ├── directives/
│   ├── pipes/
│   ├── models/
│   └── shared.module.ts
└── app.component.ts
```

### 2.2 State Management (NgRx)

#### 2.2.1 Auth State
```typescript
export interface AuthState {
  user: User | null;
  accessToken: string | null;
  refreshToken: string | null;
  isAuthenticated: boolean;
  isLoading: boolean;
  error: string | null;
}
```

#### 2.2.2 User State
```typescript
export interface UserState {
  users: EntityState<User>;
  selectedUser: User | null;
  currentUserProfile: User | null;
  isLoading: boolean;
  error: string | null;
  pagination: {
    page: number;
    pageSize: number;
    totalCount: number;
  };
  filters: UserFilters;
}
```

#### 2.2.3 Role State
```typescript
export interface RoleState {
  roles: EntityState<Role>;
  selectedRole: Role | null;
  permissions: Permission[];
  isLoading: boolean;
  error: string | null;
}
```

### 2.3 Routing Configuration

```typescript
const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () => import('./features/auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'profile',
    loadChildren: () => import('./features/user-management/user-management.module')
      .then(m => m.UserManagementModule),
    canActivate: [AuthGuard]
  },
  {
    path: 'admin',
    canActivate: [AuthGuard, RoleGuard],
    data: { requiredRole: 'Administrator' },
    children: [
      {
        path: 'users',
        loadChildren: () => import('./features/user-management/user-management.module')
          .then(m => m.UserManagementModule)
      },
      {
        path: 'roles',
        loadChildren: () => import('./features/role-management/role-management.module')
          .then(m => m.RoleManagementModule)
      },
      {
        path: 'audit-logs',
        loadChildren: () => import('./features/audit-logs/audit-logs.module')
          .then(m => m.AuditLogsModule)
      }
    ]
  }
];
```

## 3. User Interface Components

### 3.1 Authentication Components

#### 3.1.1 Login Component
**Route**: `/auth/login`

**Features**:
- Email and password input fields
- Remember me checkbox
- Forgot password link
- Social login buttons (Google, Microsoft)
- Form validation with real-time feedback
- Loading state during authentication
- Error message display

**UI Layout**:
```
┌─────────────────────────────────────┐
│         Event Platform Logo         │
│                                     │
│        Welcome Back!                │
│                                     │
│  ┌─────────────────────────────┐   │
│  │ Email                       │   │
│  └─────────────────────────────┘   │
│                                     │
│  ┌─────────────────────────────┐   │
│  │ Password              [👁]  │   │
│  └─────────────────────────────┘   │
│                                     │
│  ☐ Remember me    Forgot password?  │
│                                     │
│  ┌─────────────────────────────┐   │
│  │        LOGIN                │   │
│  └─────────────────────────────┘   │
│                                     │
│     ─────── or ───────              │
│                                     │
│  [🔵 Continue with Google]          │
│  [🔷 Continue with Microsoft]       │
│                                     │
│  Don't have an account? Sign up     │
└─────────────────────────────────────┘
```

**Component Code**:
```typescript
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  isLoading$ = this.store.select(selectAuthIsLoading);
  error$ = this.store.select(selectAuthError);
  hidePassword = true;

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private router: Router
  ) {
    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
      rememberMe: [false]
    });
  }

  onSubmit(): void {
    if (this.loginForm.valid) {
      this.store.dispatch(AuthActions.login({
        credentials: this.loginForm.value
      }));
    }
  }

  onSocialLogin(provider: 'google' | 'microsoft'): void {
    this.store.dispatch(AuthActions.socialLogin({ provider }));
  }
}
```

#### 3.1.2 Register Component
**Route**: `/auth/register`

**Features**:
- First name, last name, email, phone, password fields
- Password strength indicator
- Terms and conditions acceptance
- Email verification flow
- Form validation with custom validators

**UI Layout**:
```
┌─────────────────────────────────────┐
│        Create Your Account          │
│                                     │
│  ┌──────────┐  ┌──────────┐        │
│  │First Name│  │Last Name │        │
│  └──────────┘  └──────────┘        │
│                                     │
│  ┌─────────────────────────────┐   │
│  │ Email                       │   │
│  └─────────────────────────────┘   │
│                                     │
│  ┌─────────────────────────────┐   │
│  │ Phone Number                │   │
│  └─────────────────────────────┘   │
│                                     │
│  ┌─────────────────────────────┐   │
│  │ Password              [👁]  │   │
│  └─────────────────────────────┘   │
│  [████████░░] Strong                │
│                                     │
│  ┌─────────────────────────────┐   │
│  │ Confirm Password      [👁]  │   │
│  └─────────────────────────────┘   │
│                                     │
│  ☑ I accept the Terms & Conditions  │
│                                     │
│  ┌─────────────────────────────┐   │
│  │      CREATE ACCOUNT         │   │
│  └─────────────────────────────┘   │
│                                     │
│  Already have an account? Login     │
└─────────────────────────────────────┘
```

**Password Strength Component**:
```typescript
@Component({
  selector: 'app-password-strength',
  template: `
    <div class="password-strength">
      <mat-progress-bar
        [value]="strength"
        [color]="color">
      </mat-progress-bar>
      <span class="strength-label">{{ label }}</span>
    </div>
  `
})
export class PasswordStrengthComponent {
  @Input() password: string = '';

  get strength(): number {
    return this.calculateStrength(this.password);
  }

  get color(): 'warn' | 'accent' | 'primary' {
    if (this.strength < 40) return 'warn';
    if (this.strength < 70) return 'accent';
    return 'primary';
  }

  get label(): string {
    if (this.strength < 40) return 'Weak';
    if (this.strength < 70) return 'Medium';
    return 'Strong';
  }
}
```

#### 3.1.3 Forgot Password Component
**Route**: `/auth/forgot-password`

**Features**:
- Email input for password reset
- Success message after submission
- Resend link functionality

#### 3.1.4 Reset Password Component
**Route**: `/auth/reset-password/:token`

**Features**:
- New password input
- Password confirmation
- Token validation
- Success/error messaging

### 3.2 User Profile Components

#### 3.2.1 User Profile Component
**Route**: `/profile`

**Features**:
- View and edit user information
- Profile picture upload
- Change password
- Session management
- Account deactivation

**UI Layout**:
```
┌─────────────────────────────────────────────────────┐
│  Profile Settings                          [✓ Save] │
├─────────────────────────────────────────────────────┤
│                                                     │
│  ┌──────┐                                           │
│  │ 👤   │  Upload Photo                            │
│  └──────┘  Remove                                   │
│                                                     │
│  Personal Information                               │
│  ┌──────────────┐  ┌──────────────┐               │
│  │ First Name   │  │ Last Name    │               │
│  └──────────────┘  └──────────────┘               │
│                                                     │
│  ┌─────────────────────────────────────┐           │
│  │ Email                               │           │
│  └─────────────────────────────────────┘           │
│                                                     │
│  ┌─────────────────────────────────────┐           │
│  │ Phone Number                        │           │
│  └─────────────────────────────────────┘           │
│                                                     │
│  ─────────────────────────────────────────         │
│                                                     │
│  Security                                           │
│  ┌─────────────────────────────────────┐           │
│  │ [🔒] Change Password                │           │
│  └─────────────────────────────────────┘           │
│                                                     │
│  ┌─────────────────────────────────────┐           │
│  │ [📱] Manage Sessions                │           │
│  └─────────────────────────────────────┘           │
│                                                     │
│  ─────────────────────────────────────────         │
│                                                     │
│  Account                                            │
│  ┌─────────────────────────────────────┐           │
│  │ [⚠️] Deactivate Account             │           │
│  └─────────────────────────────────────┘           │
└─────────────────────────────────────────────────────┘
```

**Component Code**:
```typescript
@Component({
  selector: 'app-user-profile',
  templateUrl: './user-profile.component.html',
  styleUrls: ['./user-profile.component.scss']
})
export class UserProfileComponent implements OnInit {
  profileForm: FormGroup;
  user$ = this.store.select(selectCurrentUser);
  isLoading$ = this.store.select(selectUserIsLoading);

  constructor(
    private fb: FormBuilder,
    private store: Store,
    private dialog: MatDialog
  ) {
    this.profileForm = this.fb.group({
      firstName: ['', Validators.required],
      lastName: ['', Validators.required],
      email: [{ value: '', disabled: true }],
      phoneNumber: ['']
    });
  }

  ngOnInit(): void {
    this.user$.pipe(
      filter(user => !!user),
      take(1)
    ).subscribe(user => {
      this.profileForm.patchValue(user);
    });
  }

  onSubmit(): void {
    if (this.profileForm.valid) {
      this.store.dispatch(UserActions.updateProfile({
        updates: this.profileForm.getRawValue()
      }));
    }
  }

  onChangePassword(): void {
    this.dialog.open(ChangePasswordDialogComponent, {
      width: '500px'
    });
  }

  onManageSessions(): void {
    this.dialog.open(SessionManagementDialogComponent, {
      width: '700px'
    });
  }

  onUploadPhoto(file: File): void {
    this.store.dispatch(UserActions.uploadProfilePicture({ file }));
  }
}
```

#### 3.2.2 Change Password Dialog Component

**Features**:
- Current password verification
- New password input
- Password confirmation
- Real-time validation

**UI Layout**:
```
┌─────────────────────────────────┐
│  Change Password           [×]  │
├─────────────────────────────────┤
│                                 │
│  ┌──────────────────────────┐  │
│  │ Current Password   [👁]  │  │
│  └──────────────────────────┘  │
│                                 │
│  ┌──────────────────────────┐  │
│  │ New Password       [👁]  │  │
│  └──────────────────────────┘  │
│  [████████░░] Strong            │
│                                 │
│  ┌──────────────────────────┐  │
│  │ Confirm Password   [👁]  │  │
│  └──────────────────────────┘  │
│                                 │
│  Requirements:                  │
│  ✓ At least 8 characters        │
│  ✓ One uppercase letter         │
│  ✓ One lowercase letter         │
│  ✓ One number                   │
│  ✓ One special character        │
│                                 │
│        [Cancel]  [Change]       │
└─────────────────────────────────┘
```

### 3.3 User Management Components (Admin)

#### 3.3.1 User List Component
**Route**: `/admin/users`

**Features**:
- Paginated user table
- Search and filter capabilities
- Sort by multiple columns
- Bulk actions
- User status indicators
- Quick actions (edit, deactivate, view details)

**UI Layout**:
```
┌──────────────────────────────────────────────────────────────┐
│  User Management                            [+ Add User]     │
├──────────────────────────────────────────────────────────────┤
│  ┌────────────┐  [Status ▼]  [Role ▼]     🔍 [Search...]    │
│  └────────────┘                                              │
├──────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ ☐ │ Name          │ Email       │ Role  │ Status │ •  │ │
│  ├───┼───────────────┼─────────────┼───────┼────────┼────┤ │
│  │ ☐ │ John Doe      │ john@...    │ Admin │ 🟢     │ ⋮  │ │
│  │ ☐ │ Jane Smith    │ jane@...    │ User  │ 🟢     │ ⋮  │ │
│  │ ☐ │ Bob Johnson   │ bob@...     │ Event │ 🔴     │ ⋮  │ │
│  │ ☐ │ Alice Brown   │ alice@...   │ User  │ 🟢     │ ⋮  │ │
│  └────────────────────────────────────────────────────────┘ │
│                                                              │
│  Showing 1-20 of 150        [< 1 2 3 4 5 >]                 │
└──────────────────────────────────────────────────────────────┘
```

**Component Code**:
```typescript
@Component({
  selector: 'app-user-list',
  templateUrl: './user-list.component.html',
  styleUrls: ['./user-list.component.scss']
})
export class UserListComponent implements OnInit, OnDestroy {
  displayedColumns = ['select', 'name', 'email', 'role', 'status', 'actions'];
  dataSource: MatTableDataSource<User>;
  selection = new SelectionModel<User>(true, []);

  users$ = this.store.select(selectAllUsers);
  isLoading$ = this.store.select(selectUserIsLoading);
  pagination$ = this.store.select(selectUserPagination);

  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  searchControl = new FormControl('');
  statusFilter = new FormControl('all');
  roleFilter = new FormControl('all');

  constructor(
    private store: Store,
    private dialog: MatDialog
  ) {}

  ngOnInit(): void {
    this.store.dispatch(UserActions.loadUsers());

    this.searchControl.valueChanges.pipe(
      debounceTime(300),
      distinctUntilChanged()
    ).subscribe(search => {
      this.store.dispatch(UserActions.setSearchFilter({ search }));
    });
  }

  onPageChange(event: PageEvent): void {
    this.store.dispatch(UserActions.setPage({
      page: event.pageIndex + 1,
      pageSize: event.pageSize
    }));
  }

  onEdit(user: User): void {
    this.dialog.open(UserFormDialogComponent, {
      width: '600px',
      data: { user }
    });
  }

  onDeactivate(user: User): void {
    this.dialog.open(ConfirmDialogComponent, {
      data: {
        title: 'Deactivate User',
        message: `Are you sure you want to deactivate ${user.firstName} ${user.lastName}?`
      }
    }).afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.store.dispatch(UserActions.deactivateUser({ userId: user.id }));
      }
    });
  }

  onViewDetails(user: User): void {
    this.router.navigate(['/admin/users', user.id]);
  }
}
```

#### 3.3.2 User Detail Component
**Route**: `/admin/users/:id`

**Features**:
- Comprehensive user information display
- Role assignment interface
- Permission management
- Audit log timeline
- Activity history

**UI Layout**:
```
┌──────────────────────────────────────────────────────────────┐
│  ← Back to Users                                    [Edit]   │
├──────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌────┐  John Doe                              Status: 🟢   │
│  │ 👤 │  john.doe@example.com                                │
│  └────┘  +1 (555) 123-4567                                  │
│          Member since: Jan 1, 2025                           │
│                                                              │
│  ┌─ Roles ────────────────────────────────────────────────┐ │
│  │ [Administrator] [×]                      [+ Add Role]  │ │
│  └────────────────────────────────────────────────────────┘ │
│                                                              │
│  ┌─ Direct Permissions ──────────────────────────────────┐  │
│  │ • Events.Create                    [+ Add Permission]│  │
│  │ • Events.Delete                                      │  │
│  └────────────────────────────────────────────────────────┘ │
│                                                              │
│  ┌─ Recent Activity ─────────────────────────────────────┐  │
│  │ ⦿ Logged in                           2 hours ago    │  │
│  │ ○ Updated profile                     1 day ago      │  │
│  │ ○ Changed password                    3 days ago     │  │
│  │                                      [View All Logs] │  │
│  └────────────────────────────────────────────────────────┘ │
│                                                              │
│  ┌─ Sessions ────────────────────────────────────────────┐  │
│  │ 🖥️  Windows • Chrome        Active now              │  │
│  │ 📱  iOS • Safari             Last active: 1 day ago  │  │
│  └────────────────────────────────────────────────────────┘ │
└──────────────────────────────────────────────────────────────┘
```

### 3.4 Role Management Components (Admin)

#### 3.4.1 Role List Component
**Route**: `/admin/roles`

**Features**:
- List all roles
- Create new roles
- Edit existing roles
- Assign permissions to roles
- View users with each role

**UI Layout**:
```
┌──────────────────────────────────────────────────────────────┐
│  Role Management                            [+ Create Role]  │
├──────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ Administrator                           [System Role]  │ │
│  │ Full system access                              ⋮      │ │
│  │ 🔑 25 permissions  │  👥 5 users                       │ │
│  └────────────────────────────────────────────────────────┘ │
│                                                              │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ Event Manager                                          │ │
│  │ Manages events and attendees                    ⋮      │ │
│  │ 🔑 12 permissions  │  👥 15 users                      │ │
│  └────────────────────────────────────────────────────────┘ │
│                                                              │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ User                                    [System Role]  │ │
│  │ Basic user access                               ⋮      │ │
│  │ 🔑 5 permissions   │  👥 130 users                     │ │
│  └────────────────────────────────────────────────────────┘ │
└──────────────────────────────────────────────────────────────┘
```

#### 3.4.2 Role Form Dialog Component

**Features**:
- Role name and description
- Permission selection with categories
- User count display
- System role indicator

**UI Layout**:
```
┌─────────────────────────────────────────┐
│  Create Role                       [×]  │
├─────────────────────────────────────────┤
│                                         │
│  ┌──────────────────────────────────┐  │
│  │ Role Name                        │  │
│  └──────────────────────────────────┘  │
│                                         │
│  ┌──────────────────────────────────┐  │
│  │ Description                      │  │
│  │                                  │  │
│  └──────────────────────────────────┘  │
│                                         │
│  Permissions (8 selected)               │
│  ┌──────────────────────────────────┐  │
│  │ [Search permissions...]          │  │
│  ├──────────────────────────────────┤  │
│  │ ▼ Events                         │  │
│  │   ☑ Read                         │  │
│  │   ☑ Create                       │  │
│  │   ☑ Update                       │  │
│  │   ☐ Delete                       │  │
│  │ ▼ Users                          │  │
│  │   ☑ Read                         │  │
│  │   ☐ Create                       │  │
│  │   ☐ Update                       │  │
│  │   ☐ Delete                       │  │
│  └──────────────────────────────────┘  │
│                                         │
│        [Cancel]  [Create Role]          │
└─────────────────────────────────────────┘
```

### 3.5 Audit Log Components (Admin)

#### 3.5.1 Audit Log List Component
**Route**: `/admin/audit-logs`

**Features**:
- Filterable audit log table
- Date range picker
- Event type filter
- User filter
- Export functionality
- Detail view for each log entry

**UI Layout**:
```
┌──────────────────────────────────────────────────────────────┐
│  Audit Logs                                    [📥 Export]   │
├──────────────────────────────────────────────────────────────┤
│  [Date Range]  [Event Type ▼]  [User ▼]  🔍 [Search...]     │
├──────────────────────────────────────────────────────────────┤
│                                                              │
│  ┌────────────────────────────────────────────────────────┐ │
│  │ Time      │ User      │ Event           │ Status │ IP  │ │
│  ├───────────┼───────────┼─────────────────┼────────┼─────┤ │
│  │ 10:30 AM  │ John Doe  │ Login Success   │ ✓      │ ... │ │
│  │ 10:25 AM  │ Jane S.   │ Role Assigned   │ ✓      │ ... │ │
│  │ 10:20 AM  │ Bob J.    │ Login Failed    │ ✗      │ ... │ │
│  │ 10:15 AM  │ Alice B.  │ Password Change │ ✓      │ ... │ │
│  │ 10:10 AM  │ System    │ Session Expired │ ✓      │ ... │ │
│  └────────────────────────────────────────────────────────┘ │
│                                                              │
│  Showing 1-50 of 1,523         [< 1 2 3 ... 31 >]           │
└──────────────────────────────────────────────────────────────┘
```

## 4. Services

### 4.1 Authentication Service

```typescript
@Injectable({ providedIn: 'root' })
export class AuthService {
  private readonly API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}

  login(credentials: LoginCredentials): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.API_URL}/auth/login`,
      credentials
    );
  }

  register(userData: RegisterData): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.API_URL}/auth/register`,
      userData
    );
  }

  refreshToken(refreshToken: string): Observable<AuthResponse> {
    return this.http.post<AuthResponse>(
      `${this.API_URL}/auth/refresh`,
      { refreshToken }
    );
  }

  logout(): Observable<void> {
    return this.http.post<void>(`${this.API_URL}/auth/logout`, {});
  }

  forgotPassword(email: string): Observable<void> {
    return this.http.post<void>(
      `${this.API_URL}/auth/forgot-password`,
      { email }
    );
  }

  resetPassword(token: string, newPassword: string): Observable<void> {
    return this.http.post<void>(
      `${this.API_URL}/auth/reset-password`,
      { resetToken: token, newPassword }
    );
  }
}
```

### 4.2 User Service

```typescript
@Injectable({ providedIn: 'root' })
export class UserService {
  private readonly API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUsers(params: UserQueryParams): Observable<PagedResponse<User>> {
    const httpParams = new HttpParams({ fromObject: params as any });
    return this.http.get<PagedResponse<User>>(
      `${this.API_URL}/users`,
      { params: httpParams }
    );
  }

  getUserById(userId: string): Observable<User> {
    return this.http.get<User>(`${this.API_URL}/users/${userId}`);
  }

  getCurrentUser(): Observable<User> {
    return this.http.get<User>(`${this.API_URL}/users/me`);
  }

  updateUser(userId: string, updates: Partial<User>): Observable<User> {
    return this.http.put<User>(`${this.API_URL}/users/${userId}`, updates);
  }

  updateCurrentUser(updates: Partial<User>): Observable<User> {
    return this.http.put<User>(`${this.API_URL}/users/me`, updates);
  }

  changePassword(data: ChangePasswordData): Observable<void> {
    return this.http.put<void>(
      `${this.API_URL}/users/me/change-password`,
      data
    );
  }

  uploadProfilePicture(file: File): Observable<{ url: string }> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post<{ url: string }>(
      `${this.API_URL}/users/me/profile-picture`,
      formData
    );
  }

  deactivateUser(userId: string, reason: string): Observable<void> {
    return this.http.post<void>(
      `${this.API_URL}/users/${userId}/deactivate`,
      { reason }
    );
  }

  reactivateUser(userId: string): Observable<void> {
    return this.http.post<void>(
      `${this.API_URL}/users/${userId}/reactivate`,
      {}
    );
  }
}
```

### 4.3 Role Service

```typescript
@Injectable({ providedIn: 'root' })
export class RoleService {
  private readonly API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getRoles(): Observable<Role[]> {
    return this.http.get<{ items: Role[] }>(`${this.API_URL}/roles`)
      .pipe(map(response => response.items));
  }

  getRoleById(roleId: string): Observable<Role> {
    return this.http.get<Role>(`${this.API_URL}/roles/${roleId}`);
  }

  createRole(role: CreateRoleData): Observable<Role> {
    return this.http.post<Role>(`${this.API_URL}/roles`, role);
  }

  updateRole(roleId: string, updates: Partial<Role>): Observable<Role> {
    return this.http.put<Role>(`${this.API_URL}/roles/${roleId}`, updates);
  }

  deleteRole(roleId: string): Observable<void> {
    return this.http.delete<void>(`${this.API_URL}/roles/${roleId}`);
  }

  assignRoleToUser(userId: string, roleId: string): Observable<void> {
    return this.http.post<void>(
      `${this.API_URL}/users/${userId}/roles`,
      { roleId }
    );
  }

  revokeRoleFromUser(userId: string, roleId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.API_URL}/users/${userId}/roles/${roleId}`
    );
  }

  assignPermissionToRole(roleId: string, permissionId: string): Observable<void> {
    return this.http.post<void>(
      `${this.API_URL}/roles/${roleId}/permissions`,
      { permissionId }
    );
  }

  removePermissionFromRole(roleId: string, permissionId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.API_URL}/roles/${roleId}/permissions/${permissionId}`
    );
  }
}
```

### 4.4 Permission Service

```typescript
@Injectable({ providedIn: 'root' })
export class PermissionService {
  private readonly API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getPermissions(): Observable<Permission[]> {
    return this.http.get<{ items: Permission[] }>(`${this.API_URL}/permissions`)
      .pipe(map(response => response.items));
  }

  getUserEffectivePermissions(userId: string): Observable<Permission[]> {
    return this.http.get<{ permissions: Permission[] }>(
      `${this.API_URL}/users/${userId}/effective-permissions`
    ).pipe(map(response => response.permissions));
  }

  grantPermissionToUser(userId: string, permissionId: string): Observable<void> {
    return this.http.post<void>(
      `${this.API_URL}/users/${userId}/permissions`,
      { permissionId }
    );
  }

  revokePermissionFromUser(userId: string, permissionId: string): Observable<void> {
    return this.http.delete<void>(
      `${this.API_URL}/users/${userId}/permissions/${permissionId}`
    );
  }
}
```

### 4.5 Audit Service

```typescript
@Injectable({ providedIn: 'root' })
export class AuditService {
  private readonly API_URL = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getUserAuditLogs(
    userId: string,
    params: AuditLogQueryParams
  ): Observable<PagedResponse<AuditLog>> {
    const httpParams = new HttpParams({ fromObject: params as any });
    return this.http.get<PagedResponse<AuditLog>>(
      `${this.API_URL}/users/${userId}/audit-logs`,
      { params: httpParams }
    );
  }

  getUnauthorizedAttempts(
    params: AuditLogQueryParams
  ): Observable<PagedResponse<AuditLog>> {
    const httpParams = new HttpParams({ fromObject: params as any });
    return this.http.get<PagedResponse<AuditLog>>(
      `${this.API_URL}/audit/unauthorized-attempts`,
      { params: httpParams }
    );
  }
}
```

## 5. Guards and Interceptors

### 5.1 Auth Guard

```typescript
@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(
    private store: Store,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {
    return this.store.select(selectIsAuthenticated).pipe(
      map(isAuthenticated => {
        if (!isAuthenticated) {
          this.router.navigate(['/auth/login'], {
            queryParams: { returnUrl: state.url }
          });
          return false;
        }
        return true;
      })
    );
  }
}
```

### 5.2 Role Guard

```typescript
@Injectable({ providedIn: 'root' })
export class RoleGuard implements CanActivate {
  constructor(
    private store: Store,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  canActivate(route: ActivatedRouteSnapshot): Observable<boolean> {
    const requiredRole = route.data['requiredRole'] as string;

    return this.store.select(selectCurrentUser).pipe(
      map(user => {
        if (!user) return false;

        const hasRole = user.roles.some(role => role.name === requiredRole);

        if (!hasRole) {
          this.snackBar.open('Access denied. Insufficient permissions.', 'Close', {
            duration: 3000
          });
          this.router.navigate(['/']);
          return false;
        }

        return true;
      })
    );
  }
}
```

### 5.3 JWT Interceptor

```typescript
@Injectable()
export class JwtInterceptor implements HttpInterceptor {
  constructor(private store: Store) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return this.store.select(selectAccessToken).pipe(
      take(1),
      switchMap(token => {
        if (token) {
          request = request.clone({
            setHeaders: {
              Authorization: `Bearer ${token}`
            }
          });
        }
        return next.handle(request);
      })
    );
  }
}
```

### 5.4 Error Interceptor

```typescript
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(
    private store: Store,
    private snackBar: MatSnackBar
  ) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) {
          // Attempt token refresh
          this.store.dispatch(AuthActions.refreshToken());
        } else if (error.status === 403) {
          this.snackBar.open('Access denied', 'Close', { duration: 3000 });
        } else if (error.status === 500) {
          this.snackBar.open('Server error. Please try again later.', 'Close', {
            duration: 5000
          });
        }

        return throwError(() => error);
      })
    );
  }
}
```

### 5.5 Token Refresh Interceptor

```typescript
@Injectable()
export class TokenRefreshInterceptor implements HttpInterceptor {
  private refreshTokenInProgress = false;
  private refreshTokenSubject = new BehaviorSubject<string | null>(null);

  constructor(private store: Store) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401 && !request.url.includes('auth/refresh')) {
          return this.handle401Error(request, next);
        }
        return throwError(() => error);
      })
    );
  }

  private handle401Error(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {
    if (!this.refreshTokenInProgress) {
      this.refreshTokenInProgress = true;
      this.refreshTokenSubject.next(null);

      return this.store.select(selectRefreshToken).pipe(
        take(1),
        switchMap(refreshToken => {
          if (!refreshToken) {
            this.store.dispatch(AuthActions.logout());
            return throwError(() => new Error('No refresh token'));
          }

          this.store.dispatch(AuthActions.refreshToken());

          return this.refreshTokenSubject.pipe(
            filter(token => token !== null),
            take(1),
            switchMap(token => {
              this.refreshTokenInProgress = false;
              return next.handle(this.addToken(request, token!));
            })
          );
        })
      );
    } else {
      return this.refreshTokenSubject.pipe(
        filter(token => token !== null),
        take(1),
        switchMap(token => next.handle(this.addToken(request, token!)))
      );
    }
  }

  private addToken(request: HttpRequest<any>, token: string): HttpRequest<any> {
    return request.clone({
      setHeaders: { Authorization: `Bearer ${token}` }
    });
  }
}
```

## 6. State Management (NgRx)

### 6.1 Auth Store

**Actions**:
```typescript
export const AuthActions = createActionGroup({
  source: 'Auth',
  events: {
    'Login': props<{ credentials: LoginCredentials }>(),
    'Login Success': props<{ response: AuthResponse }>(),
    'Login Failure': props<{ error: string }>(),
    'Register': props<{ userData: RegisterData }>(),
    'Register Success': props<{ response: AuthResponse }>(),
    'Register Failure': props<{ error: string }>(),
    'Logout': emptyProps(),
    'Logout Success': emptyProps(),
    'Refresh Token': emptyProps(),
    'Refresh Token Success': props<{ response: AuthResponse }>(),
    'Refresh Token Failure': props<{ error: string }>()
  }
});
```

**Reducer**:
```typescript
export const authReducer = createReducer(
  initialState,
  on(AuthActions.login, (state) => ({
    ...state,
    isLoading: true,
    error: null
  })),
  on(AuthActions.loginSuccess, (state, { response }) => ({
    ...state,
    user: response.user,
    accessToken: response.accessToken,
    refreshToken: response.refreshToken,
    isAuthenticated: true,
    isLoading: false,
    error: null
  })),
  on(AuthActions.loginFailure, (state, { error }) => ({
    ...state,
    isLoading: false,
    error
  })),
  on(AuthActions.logoutSuccess, () => initialState)
);
```

**Effects**:
```typescript
@Injectable()
export class AuthEffects {
  login$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.login),
      switchMap(({ credentials }) =>
        this.authService.login(credentials).pipe(
          map(response => {
            this.tokenStorage.setTokens(
              response.accessToken,
              response.refreshToken
            );
            return AuthActions.loginSuccess({ response });
          }),
          catchError(error =>
            of(AuthActions.loginFailure({ error: error.message }))
          )
        )
      )
    )
  );

  loginSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.loginSuccess),
        tap(() => {
          this.router.navigate(['/']);
          this.snackBar.open('Login successful', 'Close', { duration: 3000 });
        })
      ),
    { dispatch: false }
  );

  logout$ = createEffect(() =>
    this.actions$.pipe(
      ofType(AuthActions.logout),
      switchMap(() =>
        this.authService.logout().pipe(
          map(() => {
            this.tokenStorage.clearTokens();
            return AuthActions.logoutSuccess();
          }),
          catchError(() => {
            this.tokenStorage.clearTokens();
            return of(AuthActions.logoutSuccess());
          })
        )
      )
    )
  );

  logoutSuccess$ = createEffect(
    () =>
      this.actions$.pipe(
        ofType(AuthActions.logoutSuccess),
        tap(() => {
          this.router.navigate(['/auth/login']);
        })
      ),
    { dispatch: false }
  );

  constructor(
    private actions$: Actions,
    private authService: AuthService,
    private tokenStorage: TokenStorageService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}
}
```

### 6.2 User Store

Similar patterns for:
- User actions, reducer, effects
- Role actions, reducer, effects
- Permission actions, reducer, effects

## 7. Models and Interfaces

```typescript
export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
  profilePictureUrl: string;
  status: UserStatus;
  roles: Role[];
  permissions: Permission[];
  createdAt: string;
  updatedAt: string;
  lastLoginAt: string;
}

export interface Role {
  id: string;
  name: string;
  description: string;
  isSystemRole: boolean;
  permissions: Permission[];
  userCount: number;
}

export interface Permission {
  id: string;
  resource: string;
  action: string;
  description: string;
}

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresIn: number;
  tokenType: string;
  user: User;
}

export interface LoginCredentials {
  email: string;
  password: string;
  rememberMe: boolean;
}

export interface RegisterData {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
  phoneNumber: string;
}

export interface PagedResponse<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
}

export enum UserStatus {
  Active = 'Active',
  Inactive = 'Inactive',
  Deactivated = 'Deactivated',
  Suspended = 'Suspended',
  Locked = 'Locked'
}
```

## 8. Validation and Form Requirements

### 8.1 Email Validation
- Valid email format
- Unique email check (on registration)
- Case-insensitive

### 8.2 Password Validation
- Minimum 8 characters
- At least one uppercase letter
- At least one lowercase letter
- At least one number
- At least one special character
- Password strength indicator

### 8.3 Form Error Messages
- Display errors below fields
- Real-time validation
- Clear, user-friendly messages
- Highlight invalid fields

## 9. Accessibility Requirements

### 9.1 WCAG 2.1 Level AA Compliance
- Keyboard navigation support
- Screen reader compatibility
- ARIA labels and roles
- Focus indicators
- Color contrast ratios (4.5:1 for normal text)
- Skip navigation links

### 9.2 Semantic HTML
- Proper heading hierarchy
- Form labels
- Button roles
- Link text

## 10. Responsive Design

### 10.1 Breakpoints
- Mobile: < 600px
- Tablet: 600px - 960px
- Desktop: > 960px

### 10.2 Mobile Optimizations
- Touch-friendly buttons (min 44x44px)
- Simplified navigation
- Collapsible sections
- Bottom sheets for actions

## 11. Performance Requirements

### 11.1 Metrics
- First Contentful Paint: < 1.5s
- Time to Interactive: < 3s
- Lighthouse Performance Score: > 90

### 11.2 Optimizations
- Lazy loading of modules
- Virtual scrolling for large lists
- Image optimization
- Code splitting
- AOT compilation
- Tree shaking

## 12. Testing Requirements

### 12.1 Unit Tests (Jasmine/Karma)
- Component logic
- Service methods
- State management (reducers, effects, selectors)
- Form validation
- Guards and interceptors

### 12.2 Integration Tests
- Component integration
- API integration
- Router navigation
- Form submission

### 12.3 E2E Tests (Cypress/Playwright)
- Login flow
- Registration flow
- Password reset flow
- User management workflow
- Role assignment workflow

## 13. Deployment

### 13.1 Build Configuration
```json
{
  "configurations": {
    "production": {
      "optimization": true,
      "outputHashing": "all",
      "sourceMap": false,
      "namedChunks": false,
      "aot": true,
      "extractLicenses": true,
      "buildOptimizer": true
    }
  }
}
```

### 13.2 Environment Configuration
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.eventplatform.com',
  azureAdB2C: {
    clientId: 'your-client-id',
    authority: 'https://login.microsoftonline.com/...',
    redirectUri: 'https://app.eventplatform.com/auth/callback'
  }
};
```

## 14. Dependencies

```json
{
  "dependencies": {
    "@angular/animations": "^18.0.0",
    "@angular/cdk": "^18.0.0",
    "@angular/common": "^18.0.0",
    "@angular/core": "^18.0.0",
    "@angular/forms": "^18.0.0",
    "@angular/material": "^18.0.0",
    "@angular/platform-browser": "^18.0.0",
    "@angular/router": "^18.0.0",
    "@ngrx/store": "^18.0.0",
    "@ngrx/effects": "^18.0.0",
    "@ngrx/entity": "^18.0.0",
    "@ngrx/store-devtools": "^18.0.0",
    "rxjs": "^7.8.0",
    "tslib": "^2.6.0",
    "zone.js": "^0.14.0"
  },
  "devDependencies": {
    "@angular/cli": "^18.0.0",
    "@angular/compiler-cli": "^18.0.0",
    "@types/jasmine": "^5.1.0",
    "jasmine-core": "^5.1.0",
    "karma": "^6.4.0",
    "karma-jasmine": "^5.1.0",
    "typescript": "^5.4.0"
  }
}
```

---

**Document Version**: 1.0
**Last Updated**: 2025-01-15
**Author**: Frontend Architecture Team
