import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { GetPointsHistoryResponse, PointsHistoryModel } from '../Models/points-history.model';
import { PointsSummaryResponse } from '../Models/points-summary.response';
import { Repository } from './repository';

@Injectable({
  providedIn: 'root'
})
export class PointsHistoryRepository extends Repository<PointsHistoryModel> {
  constructor(public override http: HttpClient) {
    super(http, environment.apiControllers.PointsHistory);
  }

  public GetVisitorHistory(visitorId: number): Observable<GetPointsHistoryResponse> {
    return this.http.get<GetPointsHistoryResponse>(`${this.Endpoint}/${visitorId}`);
  }

  public GetHistoryByDateRange(data: {
    visitorId: number;
    startDate?: Date;
    endDate?: Date;
  }): Observable<GetPointsHistoryResponse> {
    return this.http.post<GetPointsHistoryResponse>(`${this.Endpoint}/range`, data);
  }

  public GetPointsSummary(visitorId: number): Observable<PointsSummaryResponse> {
    return this.http.get<PointsSummaryResponse>(`${this.Endpoint}/summary/${visitorId}`);
  }
}