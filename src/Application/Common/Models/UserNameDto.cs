using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Application.Common.Models;

public class UserNameDto : IMapFrom<User>
{
	public string UserName { get; set; }
}