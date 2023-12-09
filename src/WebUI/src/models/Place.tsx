export type Location = {
	latitude : number;
	longitude : number;
};

export type PlusCode = {
	compoundCode : string;
	globalCode : string;
};

export type Place = {
	id : string;
	apiPlaceId : string;
	businessStatus : string;
	name : string;
	rating : number;
	userRatingsTotal : number;
	vicinity : string;
	distanceFromAddress : number;
	location : Location;
	plusCode : PlusCode;
	types : Array<{
		id : string;
		name : string;
	}>;
};