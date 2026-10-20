import { Injectable } from "@angular/core";;
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment.development";
import { Repository } from "./repository";
import { Observable } from "rxjs";
import { VisitorNfcResponse } from "../Models/visitor-nfc.response";
import { VisitorModel } from "../Models/visitor.model";
import { AvailableTicketResponse } from "../Models/availabletTicket.response";

@Injectable({
  providedIn: 'root'
})
export class VisitorRepository extends Repository<VisitorModel> {
  constructor(public override http: HttpClient) {
    super(http, environment.apiControllers.Visitors);
  }

  GetVisitorNFC(id : number): Observable<VisitorNfcResponse> {
    return this.http.get<VisitorNfcResponse>(`${this.Endpoint}/${id}/NFC`);
  }

  GetByEmail(email: string): Observable<VisitorModel[]> {
    return this.http.get<VisitorModel[]>(`${this.Endpoint}`, { params: { email: email } });
  }

  GetAvailableTickets(id: number): Observable<AvailableTicketResponse[]> {
    return this.http.get<AvailableTicketResponse[]>(`${this.Endpoint}/${id}/Tickets`);
  }
}
