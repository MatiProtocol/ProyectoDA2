export interface SpecialEvent {
  id: number;
  name: string;
  date: string;
  startTime: string;
  endTime: string;
  maxCapacity: number;
  currentCapacity: number;
  additionalCost: number;
  includedAttractionIds: number[];
  active: boolean;
}

export interface CreateSpecialEventRequest {
  name: string;
  date: Date;
  startTime: string;
  endTime: string;
  maxCapacity: number;
  additionalCost: number;
  attractionsIds: number[];
}