using AutoMapper;
using GameStore.Domain.Entities.Games;
using GameStore.Service.DTOs.Comments;
using GameStore.Service.DTOs.Games;
using GameStore.Service.DTOs.Genres;

namespace GameStore.Service.Commons.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Game, GameCreationDto>().ReverseMap();
            CreateMap<Game, GameResultDto>().ReverseMap();
            CreateMap<Game, GameUpdateDto>().ReverseMap();

            CreateMap<Genre, GenreCreationDto>().ReverseMap();
            CreateMap<Genre, GenreResultDto>().ReverseMap();
            CreateMap<Genre, GenreUpdateDto>().ReverseMap();

            CreateMap<Comment, CommentCreationDto>().ReverseMap();
            CreateMap<Comment, CommentResultDto>().ReverseMap();
            CreateMap<Comment, CommentUpdateDto>().ReverseMap();
        }
    }
}
