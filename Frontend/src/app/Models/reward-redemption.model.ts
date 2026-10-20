export interface RewardRedemptionModel {
  id: number;
  rewardId: number;
  redemptionDate: Date;
  pointsSpent: number;
  status: string;
}

export interface GetVisitorRedemptionsResponse {
  redemptions: RewardRedemptionModel[];
  totalPointsRedeemed: number;
}