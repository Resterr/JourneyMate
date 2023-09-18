namespace JourneyMate.Application.Common.Exceptions;
public static class ExceptionTemplates
{
	public static string InvalidObject(object? name = null)
	{
		string messege;
		if(name is null)
		{
			messege = "Invalid object";
		}
		else
		{
			if(name is string) 
			{
				messege = $"Invalid {name}";
			}
			else
			{
				messege = $"Invalid {nameof(name)}";
			}
		}

		return messege;
	}

	public static string NotFoundObject(string? name = null, object? key = null)
	{
		string messege;
		if (name is null)
		{
			messege = "Object not found";
		}
		else 
		{
			if (key is null)
			{
				messege = $"Entity {name} was not found.";
			}
			else
			{
				if (key is string)
				{
					messege = $"Entity {name} ({key}) was not found.";
				}
				else
				{
					messege = $"Entity {name} ({nameof(key)}) was not found.";
				}
				
			}
		}

		return messege;
	}

	public static string AlreadyTakenObject(object? name = null)
	{
		string messege;
		if (name is null)
		{
			messege = "Object already taken";
		}
		else
		{
			if (name is string)
			{
				messege = $"Already taken {name}";
			}
			else
			{
				messege = $"Already taken {nameof(name)}";
			}
		}

		return messege;
	}
}
