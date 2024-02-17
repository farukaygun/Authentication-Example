using AutoMapper;
using Authentication_Example.Controllers;
using Authentication_Example.Entities;
using Microsoft.AspNetCore.Identity;
using Authentication_Example.Application.User_Operations.Create;
using Authentication_Example.Application.User_Operations.Queris;

namespace Authentication_Example.Common
{
	public class AutoMapperProfile : Profile
	{
		public AutoMapperProfile()
		{
			CreateMap<User, CreateUserModel>();
			CreateMap<CreateUserModel, User>();
			CreateMap<User, GetUserDto>();
		}
	}
}
