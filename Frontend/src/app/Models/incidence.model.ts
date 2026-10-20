export interface IncidenceModel {
  id: number;
  description: string;
  isMaintenance: boolean;
  startDate: Date;
  startTime: string;
  duration: number;
  active: boolean;
  attractionId: number;
  attractionName: string;
}

export interface CreateIncidenceRequest {
  description: string;
  attractionId: number;
  isMaintenance: boolean;
  startDate?: Date;
  startTime?: string;
  duration?: number;
}