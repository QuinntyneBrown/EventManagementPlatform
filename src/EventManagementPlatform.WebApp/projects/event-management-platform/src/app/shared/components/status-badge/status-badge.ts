import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

export type BadgeColor = 'primary' | 'accent' | 'warn' | 'success' | 'info' | 'default' | 'danger' | 'warning' | 'neutral';

@Component({
  selector: 'app-status-badge',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './status-badge.html',
  styleUrl: './status-badge.scss'
})
export class StatusBadge {
  @Input() status = '';
  @Input() set text(value: string) { this.status = value; }
  @Input() color: BadgeColor = 'default';
  @Input() size: 'small' | 'medium' = 'medium';
}
