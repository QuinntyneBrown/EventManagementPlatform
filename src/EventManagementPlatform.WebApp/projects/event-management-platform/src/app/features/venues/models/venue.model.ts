export interface VenueListItemDto {
  venueId: string;
  name: string;
  address: string;
  city: string;
  state: string;
  capacity: number;
  hourlyRate: number;
  venueType: VenueType;
  status: VenueStatus;
  rating: number;
  totalEvents: number;
}

export interface VenueDetailDto extends VenueListItemDto {
  description: string;
  zipCode: string;
  phone: string;
  email: string;
  website: string;
  amenities: string[];
  images: string[];
  contactPerson: string;
  notes: string;
  createdAt: Date;
  updatedAt: Date;
}

export interface CreateVenueRequest {
  name: string;
  description: string;
  address: string;
  city: string;
  state: string;
  zipCode: string;
  phone: string;
  email: string;
  website: string;
  capacity: number;
  hourlyRate: number;
  venueType: VenueType;
  amenities: string[];
  contactPerson: string;
  notes: string;
}

export interface UpdateVenueRequest extends CreateVenueRequest {
  venueId: string;
}

export enum VenueType {
  ConferenceCenter = 'ConferenceCenter',
  BanquetHall = 'BanquetHall',
  Hotel = 'Hotel',
  Outdoor = 'Outdoor',
  Theater = 'Theater',
  Restaurant = 'Restaurant',
  Museum = 'Museum',
  Other = 'Other'
}

export enum VenueStatus {
  Active = 'Active',
  Inactive = 'Inactive',
  UnderMaintenance = 'UnderMaintenance'
}

export const VenueTypeLabels: Record<VenueType, string> = {
  [VenueType.ConferenceCenter]: 'Conference Center',
  [VenueType.BanquetHall]: 'Banquet Hall',
  [VenueType.Hotel]: 'Hotel',
  [VenueType.Outdoor]: 'Outdoor',
  [VenueType.Theater]: 'Theater',
  [VenueType.Restaurant]: 'Restaurant',
  [VenueType.Museum]: 'Museum',
  [VenueType.Other]: 'Other'
};

export const VenueStatusLabels: Record<VenueStatus, string> = {
  [VenueStatus.Active]: 'Active',
  [VenueStatus.Inactive]: 'Inactive',
  [VenueStatus.UnderMaintenance]: 'Under Maintenance'
};
