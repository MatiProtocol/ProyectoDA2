export interface VisitorModel {
  id: number;
  name: string;
  birthDate: string;
  email: string;
  membership: string;
  points: number;
  todayPoints: number;
}

export interface CreateVisitorRequest {
  name: string;
  email: string;
  birthdate: Date;
  oldPassword?: string;
  newPassword?: string;
}
