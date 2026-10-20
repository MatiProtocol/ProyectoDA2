import { Injectable } from '@angular/core';
import { Repository } from './repository';
import { environment } from '../../environments/environment.development';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { StrategyActiveResponse } from '../Models/strategy-active.response';
import { StrategyListResponse } from '../Models/strategy-list.response';
import { PointsSummaryResponse } from '../Models/points-summary.response';
import { DailyRankingResponse } from '../Models/daily-ranking.response';
import { AttractionAccessResponse } from '../Models/attraction-access.response';
import { ProcessAttractionAccessRequest } from '../Models/process-attraction-access.request';

@Injectable({
  providedIn: 'root'
})

export class PunctuationRepository extends Repository<unknown>{
  constructor(public override http: HttpClient) {
    super(http, environment.apiControllers.Punctuation);
  }

  GetActiveStrategy(): Observable<StrategyActiveResponse> {
    return this.http.get<StrategyActiveResponse>(`${this.Endpoint}/active`);
  }

  SetStrategy(data: { strategyname: string }): Observable<StrategyActiveResponse> {
    return this.http.post<StrategyActiveResponse>(`${this.Endpoint}`, data);
  }

  GetAvailableStrategies(): Observable<StrategyListResponse> {
    return this.http.get<StrategyListResponse>(`${this.Endpoint}`);
  }

  GetVisitorPoints(visitorId: number): Observable<PointsSummaryResponse> {
    return this.http.get<PointsSummaryResponse>(`${this.Endpoint}/visitors/${visitorId}`);
  }

  GetDailyRanking(): Observable<DailyRankingResponse> {
    return this.http.get<DailyRankingResponse>(`${this.Endpoint}/ranking/daily`);
  }
  
  public ProcessAttractionAccess(data: ProcessAttractionAccessRequest): Observable<AttractionAccessResponse> {
    return this.http.post<AttractionAccessResponse>(`${this.Endpoint}/access`, data);
  }
}
