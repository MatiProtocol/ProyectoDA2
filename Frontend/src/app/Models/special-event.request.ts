export interface SpecialEventRequest {
  name: string;
  date: Date;
  startTime: string;
  endTime: string;
  maxCapacity: number;
  additionalCost: number;
  attractionsIds: number[];
}
