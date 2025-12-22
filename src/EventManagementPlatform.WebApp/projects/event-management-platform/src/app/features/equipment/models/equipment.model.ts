export interface EquipmentListItemDto {
  equipmentId: string;
  name: string;
  category: EquipmentCategory;
  quantity: number;
  availableQuantity: number;
  dailyRate: number;
  status: EquipmentStatus;
  condition: EquipmentCondition;
}

export interface EquipmentDetailDto extends EquipmentListItemDto {
  description: string;
  serialNumber: string;
  manufacturer: string;
  model: string;
  purchaseDate: Date;
  purchasePrice: number;
  lastMaintenanceDate: Date;
  nextMaintenanceDate: Date;
  location: string;
  notes: string;
  images: string[];
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateEquipmentRequest {
  name: string;
  description: string;
  category: EquipmentCategory;
  quantity: number;
  dailyRate: number;
  serialNumber: string;
  manufacturer: string;
  model: string;
  purchaseDate: Date;
  purchasePrice: number;
  location: string;
  notes: string;
}

export interface UpdateEquipmentRequest extends CreateEquipmentRequest {
  equipmentId: string;
  status: EquipmentStatus;
  condition: EquipmentCondition;
}

export enum EquipmentCategory {
  AudioVisual = 'AudioVisual',
  Lighting = 'Lighting',
  Staging = 'Staging',
  Furniture = 'Furniture',
  Catering = 'Catering',
  Decoration = 'Decoration',
  Safety = 'Safety',
  Other = 'Other'
}

export enum EquipmentStatus {
  Available = 'Available',
  InUse = 'InUse',
  Reserved = 'Reserved',
  Maintenance = 'Maintenance',
  Retired = 'Retired'
}

export enum EquipmentCondition {
  Excellent = 'Excellent',
  Good = 'Good',
  Fair = 'Fair',
  Poor = 'Poor',
  NeedsRepair = 'NeedsRepair'
}

export const EquipmentCategoryLabels: Record<EquipmentCategory, string> = {
  [EquipmentCategory.AudioVisual]: 'Audio/Visual',
  [EquipmentCategory.Lighting]: 'Lighting',
  [EquipmentCategory.Staging]: 'Staging',
  [EquipmentCategory.Furniture]: 'Furniture',
  [EquipmentCategory.Catering]: 'Catering',
  [EquipmentCategory.Decoration]: 'Decoration',
  [EquipmentCategory.Safety]: 'Safety',
  [EquipmentCategory.Other]: 'Other'
};

export const EquipmentStatusLabels: Record<EquipmentStatus, string> = {
  [EquipmentStatus.Available]: 'Available',
  [EquipmentStatus.InUse]: 'In Use',
  [EquipmentStatus.Reserved]: 'Reserved',
  [EquipmentStatus.Maintenance]: 'Maintenance',
  [EquipmentStatus.Retired]: 'Retired'
};

export const EquipmentConditionLabels: Record<EquipmentCondition, string> = {
  [EquipmentCondition.Excellent]: 'Excellent',
  [EquipmentCondition.Good]: 'Good',
  [EquipmentCondition.Fair]: 'Fair',
  [EquipmentCondition.Poor]: 'Poor',
  [EquipmentCondition.NeedsRepair]: 'Needs Repair'
};
