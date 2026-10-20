export interface AttractionAccessRequest {
  TicketQR?: string;
  VisitorNFC?: string;
}
export interface AttractionAccessResponse {
  allowed: boolean;
  message: string;
  pointsAwarded?: number;
}
