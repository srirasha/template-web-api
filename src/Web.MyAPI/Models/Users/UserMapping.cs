using AutoMapper;
using Domain.Entities;

namespace Web.MyAPI.Models.Users
{
    public class UserMapping : Profile
    {
        public UserMapping()
        {
            CreateMap<User, UserModel>();
        }
    }
}