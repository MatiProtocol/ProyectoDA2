import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { RewardModel } from '../Models/reward.model';
import { RewardRedemptionModel } from '../Models/reward-redemption.model';
import { RewardRedemptionResponse } from '../Models/reward-redemption.response';
import { Repository } from './repository';
import { RewardValidationResponse } from '../Models/reward-validation.response';
import { RedeemRewardRequest } from '../Models/redeem-reward.request';

@Injectable({
  providedIn: 'root'
})
export class RewardRepository extends Repository<RewardModel>{
  constructor(public override http: HttpClient) {
     super(http, environment.apiControllers.Reward);
   }

  
  public GetAvailableRewards(id: number): Observable<RewardModel[]> {
    return this.http.get<RewardModel[]>(`${this.Endpoint}/available/${id}`);
  }

  public RedeemReward(data: RedeemRewardRequest): Observable<RewardRedemptionResponse> {
    return this.http.post<RewardRedemptionResponse>(`${this.Endpoint}/redeem`, data);
  }

  public GetVisitorRedemptions(visitorId: number): Observable<RewardRedemptionModel[]> {
    return this.http.get<RewardRedemptionModel[]>(`${this.Endpoint}/redemptions/${visitorId}`);
  }

  public ValidateRedemption(visitorId: number, rewardId: number): Observable<RewardValidationResponse> {
    return this.http.get<RewardValidationResponse>(`${this.Endpoint}/validate/${visitorId}/${rewardId}`);
  }
}