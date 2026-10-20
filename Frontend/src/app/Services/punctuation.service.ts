import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { inject } from '@angular/core';
import { PunctuationRepository } from '../Repository/punctuation-repository';
import { AttractionAccessResponse } from '../Models/attraction-access.response';
import { ProcessAttractionAccessRequest } from '../Models/process-attraction-access.request';
import { StrategyActiveResponse } from '../Models/strategy-active.response';
import { StrategyListResponse } from '../Models/strategy-list.response';
import { DailyRankingResponse } from '../Models/daily-ranking.response';

@Injectable({
  providedIn: 'root'
})
export class PunctuationService {
  private repository = inject(PunctuationRepository);

  public ProcessAttractionEntrance(data: ProcessAttractionAccessRequest): Observable<AttractionAccessResponse> {
    return this.repository.ProcessAttractionAccess(data);
  }

  GetStrategyActive(): Observable<StrategyActiveResponse> {
    return this.repository.GetActiveStrategy();
  }

  GetAvailableStrategies(): Observable<StrategyListResponse> {
    return this.repository.GetAvailableStrategies();
  }

  SetStrategy(data: { strategyname: string }): Observable<StrategyActiveResponse> {
    return this.repository.SetStrategy(data);
  }

  public GetDailyRanking(): Observable<DailyRankingResponse> {
    return this.repository.GetDailyRanking();
  }
}