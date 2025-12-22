export interface StaffListItemDto {
  staffId: string;
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  role: StaffRole;
  status: StaffStatus;
  hireDate: Date;
  hourlyRate: number;
  totalEventsAssigned: number;
}

export interface StaffDetailDto extends StaffListItemDto {
  address: string;
  city: string;
  state: string;
  zipCode: string;
  emergencyContact: string;
  emergencyPhone: string;
  skills: string[];
  certifications: string[];
  notes: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateStaffRequest {
  firstName: string;
  lastName: string;
  email: string;
  phone: string;
  role: StaffRole;
  hireDate: Date;
  hourlyRate: number;
  address: string;
  city: string;
  state: string;
  zipCode: string;
  emergencyContact: string;
  emergencyPhone: string;
  skills: string[];
  certifications: string[];
  notes: string;
}

export interface UpdateStaffRequest extends CreateStaffRequest {
  staffId: string;
}

export enum StaffRole {
  Manager = 'Manager',
  Coordinator = 'Coordinator',
  Technician = 'Technician',
  Security = 'Security',
  Catering = 'Catering',
  Cleaning = 'Cleaning',
  Driver = 'Driver',
  General = 'General'
}

export enum StaffStatus {
  Active = 'Active',
  OnLeave = 'OnLeave',
  Inactive = 'Inactive',
  Terminated = 'Terminated'
}

export const StaffRoleLabels: Record<StaffRole, string> = {
  [StaffRole.Manager]: 'Manager',
  [StaffRole.Coordinator]: 'Coordinator',
  [StaffRole.Technician]: 'Technician',
  [StaffRole.Security]: 'Security',
  [StaffRole.Catering]: 'Catering',
  [StaffRole.Cleaning]: 'Cleaning',
  [StaffRole.Driver]: 'Driver',
  [StaffRole.General]: 'General'
};

export const StaffStatusLabels: Record<StaffStatus, string> = {
  [StaffStatus.Active]: 'Active',
  [StaffStatus.OnLeave]: 'On Leave',
  [StaffStatus.Inactive]: 'Inactive',
  [StaffStatus.Terminated]: 'Terminated'
};
