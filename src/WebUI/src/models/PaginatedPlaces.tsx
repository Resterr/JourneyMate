import { Place } from "./Place";

export type PaginatedPlaces = {
    items: Place[];
    pageNumber: number;
    totalPages: number;
    totalCount: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
};
