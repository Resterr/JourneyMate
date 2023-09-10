using JourneyMate.Application.Common.Security;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.AddressFeature.Commands.RemoveAddress;
[Authorize(Role = "Admin")]
public record RemoveAddress(Guid Id) : IRequest<Unit>;

internal sealed class RemoveAddressHandler : IRequestHandler<RemoveAddress, Unit>
{
	private readonly IAddressRepository _addressRepository;

	public RemoveAddressHandler(IAddressRepository addressRepository)
	{
		_addressRepository = addressRepository;
	}

	public async Task<Unit> Handle(RemoveAddress request, CancellationToken cancellationToken)
	{
		var address = await _addressRepository.GetByIdAsync(request.Id);

		await _addressRepository.DeleteAsync(address);

		return Unit.Value;
	}
}
