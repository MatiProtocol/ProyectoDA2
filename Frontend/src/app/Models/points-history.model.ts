export interface PointsHistoryModel {
  id: number;
  transactionDate: Date;
  points: number;
  origin: string;
  description: string;
  activeStrategy: string;
}

export interface GetPointsHistoryResponse {
  history: PointsHistoryModel[];
  totalPointsEarned: number;
  totalPointsSpent: number;
  currentBalance: number;
}