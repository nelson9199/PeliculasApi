using System.Collections.Generic;
using System.Runtime.InteropServices;
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

            CreateMap<Actor, ActorDto>().ReverseMap();
            CreateMap<ActorCreateDto, Actor>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Foto, opt => opt.Ignore());
            CreateMap<ActorPatchDto, Actor>().ReverseMap();

            CreateMap<Pelicula, PeliculaDto>().ReverseMap();
            CreateMap<PeliculaCreateDto, Pelicula>()
            .ForMember(x => x.Id, opt => opt.Ignore())
            .ForMember(x => x.Poster, opt => opt.Ignore())
            .ForMember(x => x.PeliculasGeneros, opt => opt.MapFrom(MapPeliculasGeneros))
            .ForMember(x => x.PeliculasActores, opt => opt.MapFrom(MapPeliculasActores));
            CreateMap<PeliculaPatchDto, Pelicula>().ReverseMap();
        }

        private List<PeliculasActores> MapPeliculasActores(PeliculaCreateDto peliculaCreateDto, Pelicula pelicula)
        {
            var resultado = new List<PeliculasActores>();

            if (peliculaCreateDto.Actores == null)
            {
                return resultado;
            }

            foreach (var actor in peliculaCreateDto.Actores)
            {
                resultado.Add(new PeliculasActores() { ActorId = actor.ActorId, Personaje = actor.Personaje });
            }

            return resultado;
        }

        private List<PeliculasGeneros> MapPeliculasGeneros(PeliculaCreateDto peliculaCreateDto, Pelicula pelicula)
        {
            var resultado = new List<PeliculasGeneros>();

            if (peliculaCreateDto.GenerosId == null)
            {
                return resultado;
            }

            foreach (var id in peliculaCreateDto.GenerosId)
            {
                resultado.Add(new PeliculasGeneros() { GeneroId = id });
            }

            return resultado;
        }
    }
}