using AutoMapper;
using src.PeliculasApi.Models;
using src.PeliculasApi.Models.Dtos;

namespace src.PeliculasApi.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Genero, GeneroDto>().ReverseMap();
            CreateMap<GeneroCreateDto, Genero>()
            .ForMember(x => x.Id, opt => opt.Ignore());
        }
    }
}