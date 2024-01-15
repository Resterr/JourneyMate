import { Report } from "./Report";

export type PaginatedReports = {
    items: Report[];
    pageNumber: number;
    totalPages: number;
    totalCount: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
};
