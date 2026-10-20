import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { RewardRepository } from '../Repository/reward.repository';
import { RewardModel } from '../Models/reward.model';
import { RewardRedemptionModel } from '../Models/reward-redemption.model';
import { RewardRedemptionResponse } from '../Models/reward-redemption.response';
import { RewardValidationResponse } from '../Models/reward-validation.response';
import { RewardRequest } from '../Models/reward.request';
import { RedeemRewardRequest } from '../Models/redeem-reward.request';

@Injectable({
  providedIn: 'root'
})
export class RewardService {
  private repository = inject(RewardRepository);

  public GetById(id: number): Observable<RewardModel> {
    return this.repository.Get(id);
  }

  public GetAll(): Observable<RewardModel[]> {
    return this.repository.GetAll();
  }

  public Create(data: RewardRequest): Observable<RewardModel> {
    return this.repository.Post<RewardRequest>(data);
  }

  public Update(id: number, data: RewardRequest): Observable<RewardModel> {
    return this.repository.Put<RewardRequest>(id, data);
  }

  public Delete(id: number): Observable<void> {
    return this.repository.Delete(id);
  }

  public GetAvailableRewards(visitorId: number): Observable<RewardModel[]> {
    return this.repository.GetAvailableRewards(visitorId);
  }

  public RedeemReward(visitorId: number, rewardId: number): Observable<RewardRedemptionResponse> {
    return this.repository.RedeemReward({ visitorId, rewardId } as RedeemRewardRequest);
  }

  public GetVisitorRedemptions(visitorId: number): Observable<RewardRedemptionModel[]> {
    return this.repository.GetVisitorRedemptions(visitorId);
  }

  public ValidateRedemption(visitorId: number, rewardId: number): Observable<RewardValidationResponse> {
    return this.repository.ValidateRedemption(visitorId, rewardId);
  }
}