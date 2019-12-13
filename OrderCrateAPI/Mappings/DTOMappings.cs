using AutoMapper;
using OrderCrateAPI.Entities;
using OrderCrateAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OrderCrateAPI.Mappings
{
    public class DTOMappings : Profile
    {
        public DTOMappings()
        {
            CreateMap<Login, LoginDTO>().ReverseMap();

            CreateMap<Login, LoginDTO>().ForMember(dest => dest.UserID, opts => opts.MapFrom(src => new User
            {
                ID = src.ID,
                Lastname = src.User.Lastname,
                Firstname = src.User.Firstname,
                Birthdate = src.User.Birthdate,
                Gender = src.User.Gender,
                Email = src.User.Email,
                Date_Joined = src.User.Date_Joined
            }));
        }
    }
}
