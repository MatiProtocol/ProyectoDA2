export interface RewardModel {
  id: number;
  name: string;
  description: string;
  pointsCost: number;
  availableQuantity: number;
  requiredMembershipLevel?: string;
  active: boolean;
  isAvailable: boolean;
}
export interface CreateRewardRequest {
  name: string;
  description: string;
  pointsCost: number;
  availableQuantity: number;
  requiredMembershipLevel?: string;
}
export interface UpdateRewardRequest {
  name: string;
  description: string;
  pointsCost: number;
  availableQuantity: number;
  requiredMembershipLevel?: string;
  active: boolean;
}
export interface GetAvailableRewardsResponse {
  rewards: RewardItemResponse[];
  visitorPoints: number;
  visitorMembership: string;
}

export interface RewardItemResponse {
  id: number;
  name: string;
  description: string;
  pointsCost: number;
  availableQuantity: number;
  canRedeem: boolean;
}