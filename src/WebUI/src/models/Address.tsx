export type Address = {
    id: string;
    placeId: string;
    location: {
        latitude: number;
        longitude: number;
    };
    locality: string;
    administrativeAreaLevel2: string;
    administrativeAreaLevel1: string;
    country: string;
    postalCode: string;
};
