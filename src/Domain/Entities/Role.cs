using JourneyMate.Domain.Common;

namespace JourneyMate.Domain.Entities;

public class Role : BaseEntity
{
	public string Name { get; private set; }
	public List<User> Users { get; private set; } = new();

	private Role() { }

	public Role(string name)
	{
		Name = name;
	}
}