import { Injectable } from "@angular/core";
import { Repository } from "./repository";
import { HttpClient } from "@angular/common/http";  
import { environment } from "../../environments/environment.development";
import { Observable } from "rxjs";
import { IncidenceModel } from "../Models/incidence.model";

@Injectable({
  providedIn: 'root'
})

export class IncidenceRepository extends Repository<IncidenceModel>{
  constructor(public override http: HttpClient) {
    super(http, environment.apiControllers.Incidences);
  }

  public GetActiveIncidences(): Observable<IncidenceModel[]> {
    return this.http.get<IncidenceModel[]>(`${this.Endpoint}/active`);
  }

  public GetByAttractionId(id: number): Observable<IncidenceModel[]> {
    return this.http.get<IncidenceModel[]>(`${this.Endpoint}/attraction/${id}`);
  }

  public ActivateIncidence(id: number): Observable<IncidenceModel> {
    return this.http.patch<IncidenceModel>(`${this.Endpoint}/${id}/activate`, {});
  }

  public DeactivateIncidence(id: number): Observable<IncidenceModel> {
    return this.http.patch<IncidenceModel>(`${this.Endpoint}/${id}/deactivate`, {});
  }
}
