namespace JourneyMate.Application.Common.Models.ReadModels;

public class AddressReadModel
{
	public List<Result> Results { get; set; }
	public string Status { get; set; }

	public class AddressComponent
	{
		public string Long_Name { get; set; }
		public string Short_Name { get; set; }
		public List<string> Types { get; set; }
	}

	public class Bounds
	{
		public Location Northeast { get; set; }
		public Location Southwest { get; set; }
	}

	public class Location
	{
		public double Lat { get; set; }
		public double Lng { get; set; }
	}

	public class Geometry
	{
		public Bounds Bounds { get; set; }
		public Location Location { get; set; }
		public string Location_Type { get; set; }
		public Bounds Viewport { get; set; }
	}

	public class Result
	{
		public List<AddressComponent> Address_Components { get; set; }
		public string Formatted_Address { get; set; }
		public Geometry Geometry { get; set; }
		public string Place_Id { get; set; }
		public List<string> Types { get; set; }
	}
}