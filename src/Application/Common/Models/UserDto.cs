using AutoMapper;
using JourneyMate.Application.Common.Mappings;
using JourneyMate.Domain.Entities;

namespace JourneyMate.Application.Common.Models;
public class UserDto : IMapFrom<User>
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string UserName { get; private set; }
    public DateTime Created { get; private set; }
    public DateTime? Verified { get; private set; }
}
