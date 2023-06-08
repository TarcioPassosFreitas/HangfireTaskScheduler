using AutoMapper;
using HangfireTaskScheduler.API.Models.User;
using HangfireTaskScheduler.Core.Aggregate.UserAggregate;

namespace HangfireTaskScheduler.API.Mappers;

public class UserMapper : Profile
{
    public UserMapper()
    {
        CreateMap<User, UserRequestModel>().ReverseMap();
        CreateMap<User, UserResponseModel>().ReverseMap();
    }
}