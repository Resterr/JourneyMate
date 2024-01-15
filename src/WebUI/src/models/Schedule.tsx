export type Schedule = {
    id: string;
    placeId: string;
    placeName: string;
    planId: string;
    planName: string;
    startingDate: Date;
    endingDate: Date | null;
};
