namespace JourneyMate.Application.Common.Security;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class AuthorizeAttribute : Attribute
{
	public AuthorizeAttribute() { }

	public string Role { get; set; } = string.Empty;
}
