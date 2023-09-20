﻿using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Service.Commons.Mappers;
using GameStore.Service.Interfaces.Files;
using GameStore.Service.Interfaces.Games;
using GameStore.Service.Interfaces.Orders;
using GameStore.Service.Interfaces.Users;
using GameStore.Service.Services.Files;
using GameStore.Service.Services.Games;
using GameStore.Service.Services.Orders;
using GameStore.Service.Services.Users;

namespace GameStore.Web.Configurations
{
    public static class CustomServiceConfiguration
    {
        public static void AddCustomService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IGameService, GameService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<IImageService, ImageService>();

            services.AddAutoMapper(typeof(MapperProfile));
        }
    }
}
