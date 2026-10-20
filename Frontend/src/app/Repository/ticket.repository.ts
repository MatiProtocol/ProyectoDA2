import { Injectable } from "@angular/core";;
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment.development";
import { Repository } from "./repository";
import { Observable } from "rxjs";
import { TicketModel } from "../Models/ticket.model";

@Injectable({
  providedIn: 'root'
})
export class TicketRepository extends Repository<TicketModel> {
  constructor(public override http: HttpClient) {
    super(http, environment.apiControllers.Ticket);
  }

  public GetTicketByQR(qr : string) : Observable<TicketModel> {
    return this.http.get<TicketModel>(`${this.Endpoint}/QR`, { params: { qr } });
  }
}
