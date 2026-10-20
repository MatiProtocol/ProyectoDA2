export interface RewardRequest {
  name: string;
  description: string;
  pointsCost: number;
  availableQuantity: number;
  requiredMembershipLevel?: string;
}
