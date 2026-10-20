export interface AvailableTicketResponse {
  ticketId: number;
  qr: string;
  visitorParkInformationId: number;
  consumed: boolean;
  ticketType: string;
  date: Date;
  specialEventName?: string;
}