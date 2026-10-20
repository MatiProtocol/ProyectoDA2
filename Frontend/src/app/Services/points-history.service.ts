import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { PointsHistoryRepository } from '../Repository/points-history.repository';
import { GetPointsHistoryResponse, PointsHistoryModel } from '../Models/points-history.model';
import { PointsSummaryResponse } from '../Models/points-summary.response';

@Injectable({
  providedIn: 'root'
})

export class PointsHistoryService {
  private repository = inject(PointsHistoryRepository);

  public GetVisitorHistory(visitorId: number): Observable<GetPointsHistoryResponse> {
    return this.repository.GetVisitorHistory(visitorId);
  }

  public GetHistoryByDateRange(
    visitorId: number,
    startDate?: Date,
    endDate?: Date
  ): Observable<GetPointsHistoryResponse> {
    return this.repository.GetHistoryByDateRange({
      visitorId,
      startDate,
      endDate
    });
  }

  public GetPointsSummary(visitorId: number): Observable<PointsSummaryResponse> {
    return this.repository.GetPointsSummary(visitorId);
  }
}