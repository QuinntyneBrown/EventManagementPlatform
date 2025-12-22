import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';

export type BadgeColor = 'primary' | 'accent' | 'warn' | 'success' | 'info' | 'neutral';

@Component({
  selector: 'app-status-badge',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './status-badge.html',
  styleUrl: './status-badge.scss'
})
export class StatusBadge {
  @Input() text = '';
  @Input() color: BadgeColor = 'neutral';
  @Input() size: 'small' | 'medium' = 'medium';
}
