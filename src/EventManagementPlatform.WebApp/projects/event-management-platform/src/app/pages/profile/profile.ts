import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { Router } from '@angular/router';
import { PageHeader } from '../../shared/components/page-header';
import { AuthService } from '../../core/services/auth.service';
import { HasUnsavedChanges } from '../../core/guards/unsaved-changes.guard';

/**
 * Profile Page Component - PHASE B STUB
 *
 * This component is a placeholder for Phase B implementation.
 * Profile functionality requires backend endpoints that are not yet implemented:
 * - GET /api/identity/profile
 * - PUT /api/identity/profile
 *
 * Phase A does not include user profile management.
 */
@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [
    CommonModule,
    MatCardModule,
    MatIconModule,
    MatButtonModule,
    PageHeader
  ],
  template: `
    <div class="profile-placeholder">
      <app-page-header
        title="User Profile"
        subtitle="Profile management coming in Phase B"
      ></app-page-header>

      <mat-card class="profile-placeholder__card">
        <mat-card-content>
          <div class="profile-placeholder__content">
            <mat-icon class="profile-placeholder__icon">construction</mat-icon>
            <h2>Profile Management</h2>
            <p>This feature is planned for Phase B.</p>
            <p class="profile-placeholder__info">
              @if (currentUser$ | async; as user) {
                <strong>Current User:</strong> {{ user.username }}<br>
                <strong>Roles:</strong> {{ user.roles.join(', ') }}
              }
            </p>
            <button mat-raised-button color="primary" (click)="goBack()">
              <mat-icon>arrow_back</mat-icon>
              Go Back
            </button>
          </div>
        </mat-card-content>
      </mat-card>
    </div>
  `,
  styles: [`
    .profile-placeholder {
      padding: 24px;
    }
    .profile-placeholder__card {
      max-width: 600px;
      margin: 0 auto;
    }
    .profile-placeholder__content {
      text-align: center;
      padding: 48px 24px;
    }
    .profile-placeholder__icon {
      font-size: 64px;
      width: 64px;
      height: 64px;
      color: #757575;
      margin-bottom: 16px;
    }
    .profile-placeholder__info {
      margin: 24px 0;
      padding: 16px;
      background: #f5f5f5;
      border-radius: 8px;
      text-align: left;
    }
  `]
})
export class Profile implements HasUnsavedChanges {
  private readonly authService = inject(AuthService);
  private readonly router = inject(Router);

  currentUser$ = this.authService.currentUser$;

  hasUnsavedChanges(): boolean {
    return false;
  }

  goBack(): void {
    this.router.navigate(['/dashboard']);
  }
}
