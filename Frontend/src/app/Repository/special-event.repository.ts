import { Injectable } from "@angular/core";
import { Repository } from "./repository";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment.development";
import { Observable } from "rxjs";
import { SpecialEvent } from "../Models/special-event.model";

@Injectable({
  providedIn: 'root'
})
export class SpecialEventRepository extends Repository<SpecialEvent> {
  constructor(public override http: HttpClient) {
    super(http, environment.apiControllers.SpecialEvents);
  }

  public GetActiveEvents(): Observable<SpecialEvent[]> {
    return this.http.get<SpecialEvent[]>(`${this.Endpoint}/active`);
  }
}
