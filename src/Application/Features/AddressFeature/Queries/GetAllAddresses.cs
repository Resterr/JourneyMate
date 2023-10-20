using AutoMapper;
using FluentValidation;
using JourneyMate.Application.Common.Models;
using JourneyMate.Domain.Repositories;
using MediatR;

namespace JourneyMate.Application.Features.AddressFeature.Queries;
public record GetAllAddresses(int PageNumber, int PageSize) : IRequest<PaginatedList<AddressDto>>;

internal sealed class GetAllAddressesHandler : IRequestHandler<GetAllAddresses, PaginatedList<AddressDto>>
{
    private readonly IAddressRepository _addressRepository;
    private readonly IMapper _mapper;

    public GetAllAddressesHandler(IAddressRepository addressRepository, IMapper mapper)
    {
        _addressRepository = addressRepository;
        _mapper = mapper;
    }
    public async Task<PaginatedList<AddressDto>> Handle(GetAllAddresses request, CancellationToken cancellationToken)
    {
        var addresses = await _addressRepository.GetAll(request.PageNumber, request.PageSize);

        var result = _mapper.Map<PaginatedList<AddressDto>>(addresses);

        return result;
    }
}

public class GetAllAddressesValidator : AbstractValidator<GetAllAddresses>
{
	public GetAllAddressesValidator()
	{
		RuleFor(x => x.PageNumber).GreaterThan(0);
		RuleFor(x => x.PageSize).GreaterThan(0);
	}
}