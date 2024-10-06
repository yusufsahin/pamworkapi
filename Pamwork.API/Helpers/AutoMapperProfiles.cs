using AutoMapper;
using Pamwork.API.Dtos;
using Pamwork.Entities.Concrete;

namespace Pamwork.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {

            CreateMap<Note, NoteDto>().ReverseMap();

        }
    }
}
