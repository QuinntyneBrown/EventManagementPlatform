import { Injectable, inject } from '@angular/core';
import { MatSnackBar, MatSnackBarConfig } from '@angular/material/snack-bar';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly snackBar = inject(MatSnackBar);

  showSuccess(message: string, action = 'Close'): void {
    this.show(message, action, {
      panelClass: ['success-snackbar']
    });
  }

  showError(message: string, action = 'Close'): void {
    this.show(message, action, {
      panelClass: ['error-snackbar'],
      duration: 8000
    });
  }

  showInfo(message: string, action = 'Close'): void {
    this.show(message, action, {
      panelClass: ['info-snackbar']
    });
  }

  showWarning(message: string, action = 'Close'): void {
    this.show(message, action, {
      panelClass: ['error-snackbar'],
      duration: 6000
    });
  }

  private show(message: string, action: string, config: MatSnackBarConfig = {}): void {
    this.snackBar.open(message, action, {
      duration: 5000,
      horizontalPosition: 'end',
      verticalPosition: 'top',
      ...config
    });
  }
}
