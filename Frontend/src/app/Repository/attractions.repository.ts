import { Injectable } from "@angular/core";
import { Repository } from "./repository";
import { AttractionModel } from "../Models/attraction.model";
import { environment } from "../../environments/environment.development";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { IncidenceModel } from "../Models/incidence.model";
import { IncidenceRequest } from "../Models/incidence.request";
import { AttractionAccessRequest, AttractionAccessResponse } from "../Models/attraction-access.response";
import { AttractionExitsResponse } from "../Models/attraction-exits.response";
import { VisitorsBetweenResponse } from "../Models/visit-entry.response";

@Injectable({
  providedIn: 'root'
})
export class AttractionsRepository extends Repository<AttractionModel> {
  constructor(public override http: HttpClient) {
    super(http, environment.apiControllers.Attractions);
  }
  
  public ValidateAttractionAccess(id: number, data : AttractionAccessRequest): Observable<AttractionAccessResponse> {
    return this.http.post<AttractionAccessResponse>(`${this.Endpoint}/${id}/entry`, data);
  }

  public AttractionExits(id: number): Observable<AttractionExitsResponse> {
    return this.http.get<AttractionExitsResponse>(`${this.Endpoint}/${id}/exits`);
  }

  public RegisterIncident(id: number, data: IncidenceRequest): Observable<IncidenceModel> {
    return this.http.post<IncidenceModel>(`${this.Endpoint}/${id}/Incidences`, data);
  }

  public DeactivateIncidence(incidenceId: number): Observable<void> {
    return this.http.delete<void>(`${this.Endpoint}/Incidences/${incidenceId}`);
  }

  public GetVisitorsBetween(id: number, startDate: string, endDate: string): Observable<VisitorsBetweenResponse> {
    return this.http.get<VisitorsBetweenResponse>(`${this.Endpoint}/${id}/visits?startDate=${startDate}&endDate=${endDate}`);
  }

  public ExitAttraction(id: number): Observable<AttractionExitsResponse> {
    return this.http.post<AttractionExitsResponse>(`${this.Endpoint}/${id}/exit`, {});
  }
}
