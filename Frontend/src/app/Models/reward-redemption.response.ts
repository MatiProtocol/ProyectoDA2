export interface RewardRedemptionResponse {
  redemptionId: number;
  rewardName: string;
  pointsSpent: number;
  redemptionDate: Date;
  status: string;
  remainingPoints: number;
}
