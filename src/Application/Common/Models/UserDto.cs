using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Application.Common.Models;

public class UserDto : IMapFrom<User>
{
	public Guid Id { get; set; }
	public string Email { get; set; }
	public string UserName { get; set; }
	public DateTime Created { get; set; }
}