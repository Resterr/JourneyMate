import { UserName } from "./UserName";

export type PaginatedFollow = {
    items: UserName[];
    pageNumber: number;
    totalPages: number;
    totalCount: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
};
