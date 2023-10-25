using JourneyMate.Application.Common.Interfaces;
using MediatR;
using Serilog;

namespace JourneyMate.Application.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
	private readonly ICurrentUserService _currentUserService;

	public LoggingBehaviour(ICurrentUserService currentUserService)
	{
		_currentUserService = currentUserService;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var requestName = typeof(TRequest).Name;
		var userId = _currentUserService.UserId;
		if (userId != null)
			Log.Information($"Request: {requestName} User: {userId.ToString()}");
		else
			Log.Information($"Request: {requestName} User: Unknown");


		return await next();
	}
}