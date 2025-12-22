import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { PageHeader } from '../../shared/components/page-header';
import { AuthService } from '../../core/services/auth.service';

interface DashboardCard {
  title: string;
  icon: string;
  route: string;
  description: string;
  color: string;
}

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [
    CommonModule,
    RouterModule,
    MatCardModule,
    MatButtonModule,
    MatIconModule,
    PageHeader
  ],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss'
})
export class Dashboard {
  private readonly authService = inject(AuthService);

  currentUser$ = this.authService.currentUser$;

  readonly cards: DashboardCard[] = [
    {
      title: 'Events',
      icon: 'event',
      route: '/events',
      description: 'Manage events, schedules, and bookings',
      color: '#3f51b5'
    },
    {
      title: 'Customers',
      icon: 'people',
      route: '/customers',
      description: 'View and manage customer information',
      color: '#e91e63'
    },
    {
      title: 'Venues',
      icon: 'location_on',
      route: '/venues',
      description: 'Browse and manage venue locations',
      color: '#4caf50'
    },
    {
      title: 'Staff',
      icon: 'badge',
      route: '/staff',
      description: 'Manage staff members and assignments',
      color: '#ff9800'
    },
    {
      title: 'Equipment',
      icon: 'inventory_2',
      route: '/equipment',
      description: 'Track equipment inventory and bookings',
      color: '#9c27b0'
    }
  ];
}
