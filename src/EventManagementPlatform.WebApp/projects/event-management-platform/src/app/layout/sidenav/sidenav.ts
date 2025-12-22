import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatListModule } from '@angular/material/list';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { AuthService } from '../../core/services/auth.service';
import { UserRole } from '../../core/models/auth.model';

interface NavItem {
  label: string;
  icon: string;
  route: string;
  roles?: UserRole[];
}

@Component({
  selector: 'app-sidenav',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatListModule,
    MatIconModule,
    MatDividerModule
  ],
  templateUrl: './sidenav.html',
  styleUrl: './sidenav.scss'
})
export class Sidenav {
  private readonly authService = inject(AuthService);

  readonly navItems: NavItem[] = [
    { label: 'Dashboard', icon: 'dashboard', route: '/dashboard' },
    { label: 'Events', icon: 'event', route: '/events' },
    { label: 'Customers', icon: 'people', route: '/customers' },
    { label: 'Venues', icon: 'location_on', route: '/venues' },
    { label: 'Staff', icon: 'badge', route: '/staff', roles: ['Admin', 'Manager'] },
    { label: 'Equipment', icon: 'inventory_2', route: '/equipment' }
  ];

  isVisible(item: NavItem): boolean {
    if (!item.roles || item.roles.length === 0) {
      return true;
    }
    return this.authService.hasAnyRole(item.roles);
  }
}
