﻿using GameStore.Data.Repositories;
using GameStore.Data.UnitOfWorks;
using GameStore.Service.Commons.Mappers;
using GameStore.Service.Interfaces.Games;
using GameStore.Service.Services.Games;

namespace GameStore.Api.Configurations
{
    public static class CustomServiceConfiguration
    {
        public static void AddCustomService(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IGameService, GameService>();
            services.AddScoped<IGenreService, GenreService>();
            services.AddScoped<ICommentService, CommentService>();

            services.AddAutoMapper(typeof(MapperProfile));
        }
    }
}
