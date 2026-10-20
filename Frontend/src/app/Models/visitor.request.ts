export interface VisitorRequest {
  name: string;
  email: string;
  birthdate: Date;
  oldPassword?: string;
  newPassword?: string;
}
