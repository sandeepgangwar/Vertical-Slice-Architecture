using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VerticalSliceArchitecture.Core.Domain.Identity;

namespace VerticalSliceArchitecture.API.Features.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, GetAll.Result>();
            CreateMap<User, GetByEmail.Result>();
            CreateMap<Create.Command, User>();
        }
    }
}
