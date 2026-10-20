export interface TicketModel {
  id: number;
  qr: string;
  visitorParkInformationId: number;
  consumed: boolean;
  ticketType: string;
  date: Date | string;
  specialEventId: number;
}
