﻿using GameStore.Shared.DTOs;

namespace GameStore.Services.Services;

public interface IGameService
{
    Task<GameViewDto> GetGameByAliasAsync(string alias);

    Task<GameViewDto> AddGameAsync(GameCreateDto dto);
}