using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Application.Common.Models;

public class UserDto : IMapFrom<User>
{
	public Guid Id { get; }
	public string Email { get; }
	public string UserName { get; }
	public DateTime Created { get; }
	public DateTime? Verified { get; }
}