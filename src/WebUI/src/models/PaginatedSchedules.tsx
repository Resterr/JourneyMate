import { Schedule } from "./Schedule";

export type PaginatedSchedules = {
  items: Schedule[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
};
