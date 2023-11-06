﻿using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;

namespace GameStore.Services.Interfaces;

public interface IGameService
{
    Task<GameFullDto> GetGameByAliasAsync(string alias);

    Task<GameFullDto> GetGameByIdAsync(Guid id);

    Task<IList<GenreBriefDto>> GetGenresByGameAliasAsync(string alias);

    Task<IList<PlatformBriefDto>> GetPlatformsByGameAliasAsync(string alias);

    Task<PublisherBriefDto> GetPublisherByGameAliasAsync(string alias);

    Task<FilteredGamesDto> GetAllGamesAsync(GamesFilterOptions filterOptions);

    Task<GameBriefDto> AddGameAsync(GameCreateDto dto);

    Task UpdateGameAsync(GameUpdateDto dto);

    Task DeleteGameAsync(string alias);

    Task<Tuple<byte[], string>> DownloadAsync(string gameAlias);
}