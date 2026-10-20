export interface WorkerRequest {
  Name: string;
  Email: string;
  Roles: string;
  OldPassword?: string | null;
  NewPassword?: string | null;
}
