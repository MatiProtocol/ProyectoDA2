export interface DailyRankingEntry {
  name: string;
  todayPoints: number;
}
export interface DailyRankingResponse {
  ranking: DailyRankingEntry[];
}
