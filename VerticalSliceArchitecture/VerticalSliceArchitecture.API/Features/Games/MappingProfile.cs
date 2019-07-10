using AutoMapper;
using VerticalSliceArchitecture.Core.Domain.Games;

namespace VerticalSliceArchitecture.API.Features.Games
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Game, GetAll.Result>();
            CreateMap<Game, GetById.Result>();
            CreateMap<Create.Command, Game>();
            CreateMap<Game, Edit.Command>();
            CreateMap<Game, Delete.Command>();
        }
    }
}
