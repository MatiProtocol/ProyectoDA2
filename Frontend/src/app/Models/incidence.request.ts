
export interface IncidenceRequest {
  attractionId: number;
  description: string;
  isMaintenance: boolean;
  startDate?: Date;
  startTime?: string;
  duration?: number;
}
