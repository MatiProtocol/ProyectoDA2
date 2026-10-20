import { Injectable } from "@angular/core";
import { Repository } from "./repository";
import { HttpClient } from "@angular/common/http";
import { environment } from "../../environments/environment.development";
import { Observable } from "rxjs/internal/Observable";
import { WorkerModel } from "../Models/worker.model";

@Injectable({
  providedIn: 'root'
})
export class WorkerRepository extends Repository<WorkerModel> {
  constructor(public override http: HttpClient) {
    super(http, environment.apiControllers.Workers);
  }

  GetByEmail(email: string): Observable<WorkerModel[]> {
    return this.http.get<WorkerModel[]>(`${this.Endpoint}`, { params: { email: email } });
  }
}
