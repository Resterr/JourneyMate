namespace JourneyMate.Application.Common.Security;
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class AllowAnonymousAttribute : Attribute
{
    public AllowAnonymousAttribute() { }
}
