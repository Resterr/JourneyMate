namespace JourneyMate.Infrastructure.Common.Models;

internal class TerytReadModel
{
	public string County { get; set; }
	public string Municipality { get; set; }
	public bool IsCity { get; set; }

	public TerytReadModel(string county, string municipality, bool isCity)
	{
		County = county;
		Municipality = municipality;
		IsCity = isCity;
	}
		
	public string ToComponent()
	{
		if (IsCity)
		{
			var result = $"locality:{Municipality}|administrative_area:{County}|country:Polska";
			return result;
		}
		else
		{
			var result = $"locality:Gmina {Municipality}|administrative_area:{County}|country:Polska";
			return result;
		}
	}
}