export interface RewardValidationResponse {
  canRedeem: boolean;
  reasonIfCannot?: string;
  visitorPoints: number;
  rewardCost: number;
}
