export interface AttractionModel {
  id: number;
  name: string;
  description: string;
  type: string;
  minAge: number;
  maxCapacity: number;
  currentCapacity: number;
}

export interface CreateAttractionRequest {
  name: string;
  description: string;
  type: string;
  minAge: number;
  maxCapacity: number;
  currentCapacity: number;
}