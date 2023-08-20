namespace JourneyMate.Application.Common.Interfaces;
public interface IPasswordManager
{
	string Secure(string password);
	bool Validate(string password, string securedPassword);
}