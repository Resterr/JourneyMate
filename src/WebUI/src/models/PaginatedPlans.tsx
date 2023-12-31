import { Plan } from "./Plan";

export type PaginatedPlans = {
  items: Plan[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
};
