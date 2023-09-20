using AutoMapper;
using GameStore.Domain.Entities.Files;
using GameStore.Domain.Entities.Games;
using GameStore.Domain.Entities.Orders;
using GameStore.Domain.Entities.Users;
using GameStore.Service.DTOs.Carts;
using GameStore.Service.DTOs.Comments;
using GameStore.Service.DTOs.Files;
using GameStore.Service.DTOs.Games;
using GameStore.Service.DTOs.Genres;
using GameStore.Service.DTOs.Users;

namespace GameStore.Service.Commons.Mappers
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserUpdateDto>().ReverseMap();
            CreateMap<User, UserResultDto>().ReverseMap();
            CreateMap<User, UserCreationDto>().ReverseMap();

            CreateMap<Game, GameResultDto>().ReverseMap();
            CreateMap<Game, GameUpdateDto>().ReverseMap();
            CreateMap<Game, GameCreationDto>().ReverseMap();

            CreateMap<Genre, GenreResultDto>().ReverseMap();
            CreateMap<Genre, GenreUpdateDto>().ReverseMap();
            CreateMap<Genre, GenreCreationDto>().ReverseMap();

            CreateMap<Image, ImageCreationDto>().ReverseMap();
            CreateMap<Image, ImageResultDto>().ReverseMap();

            CreateMap<Comment, CommentResultDto>().ReverseMap();
            CreateMap<Comment, CommentUpdateDto>().ReverseMap();
            CreateMap<Comment, CommentCreationDto>().ReverseMap();

            CreateMap<Cart, CartResultDto>().ReverseMap();
            CreateMap<CartItem, CartItemResultDto>().ReverseMap();
            CreateMap<CartItem, CartItemUpdateDto>().ReverseMap();
            CreateMap<CartItem, CartItemCreationDto>().ReverseMap();
        }
    }
}
