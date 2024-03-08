using System.Text;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Data.Entities.Localization;
using GameStore.Data.Extensions;
using GameStore.Data.Interfaces;
using GameStore.Services.Extensions;
using GameStore.Services.Interfaces;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Game;
using GameStore.Shared.DTOs.Genre;
using GameStore.Shared.DTOs.Image;
using GameStore.Shared.DTOs.Platform;
using GameStore.Shared.DTOs.Publisher;
using GameStore.Shared.Exceptions;
using GameStore.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Services;

public class CoreGameService : MultiLingualEntityServiceBase<Game, GameTranslation>, ICoreGameService
{
    private readonly IUnitOfWork _unitOfWork;

    public CoreGameService(IUnitOfWork unitOfWork, IMapper mapper)
        : base(mapper)
    {
        _unitOfWork = unitOfWork;
    }

    public EntityStorage EntityStorage => EntityStorage.SqlServer;

    public async Task<GameFullDto> GetGameByAliasAsync(string alias, string culture)
    {
        var game = await _unitOfWork.Games.GetOneAsync(
            predicate: g => g.Alias == alias,
            include: q => q.Include(g => g.Translations.Where(t => t.LanguageCode == culture)),
            noTracking: false);
        game.PageViews += 1;
        await _unitOfWork.SaveAsync(logChanges: false);
        return Mapper.MapWithTranslation<GameFullDto, GameTranslation>(game, culture);
    }

    public async Task<GameFullDto> GetGameByIdAsync(string id, string culture)
    {
        var gameId = Guid.Parse(id);
        var game = await _unitOfWork.Games.GetOneAsync(
            predicate: g => g.Id == gameId,
            include: q => q.Include(g => g.Translations.Where(t => t.LanguageCode == culture)));
        return Mapper.MapWithTranslation<GameFullDto, GameTranslation>(game, culture);
    }

    public async Task<IList<GenreBriefDto>> GetGenresByGameAliasAsync(string alias)
    {
        var genresByGame = (await _unitOfWork.GamesGenres.GetAsync(
                predicate: gg => gg.Game.Alias == alias,
                include: q => q.Include(gg => gg.Genre)))
            .Select(gg => gg.Genre);
        return Mapper.Map<IList<GenreBriefDto>>(genresByGame);
    }

    public async Task<IList<PlatformBriefDto>> GetPlatformsByGameAliasAsync(string alias)
    {
        var platformsByGame = (await _unitOfWork.GamesPlatforms.GetAsync(
                predicate: gp => gp.Game.Alias == alias,
                include: q => q.Include(gp => gp.Platform)))
            .Select(gp => gp.Platform);
        return Mapper.Map<IList<PlatformBriefDto>>(platformsByGame);
    }

    public async Task<PublisherBriefDto> GetPublisherByGameAliasAsync(string alias)
    {
        var game = await _unitOfWork.Games.GetOneAsync(
            predicate: g => g.Alias == alias,
            include: q => q.Include(g => g.Publisher));
        return Mapper.Map<PublisherBriefDto>(game.Publisher);
    }

    public async Task<EntityFilteringResult<GameFullDto>> GetFilteredGamesAsync(GamesFilter filter)
    {
        var filteringResult = await _unitOfWork.Games.GetFilteredGamesAsync(filter);
        return new EntityFilteringResult<GameFullDto>
        {
            Records = Mapper.MapWithTranslation<IList<GameFullDto>, GameTranslation>(filteringResult.Records, filter.Culture),
            TotalNoLimit = filteringResult.TotalNoLimit,
            MongoBlacklist = filteringResult.MongoBlacklist,
        };
    }

    public async Task<IList<ImageBriefDto>> GetImagesByGameAliasAsync(string gameAlias)
    {
        var game = await _unitOfWork.Games.GetOneAsync(
            predicate: g => g.Alias == gameAlias,
            include: q => q.Include(g => g.Images.OrderBy(img => img.Order)));
        return Mapper.Map<IList<ImageBriefDto>>(game.Images);
    }

    public async Task<GameBriefDto> AddGameAsync(GameCreateDto dto)
    {
        var game = Mapper.Map<Game>(dto);
        await _unitOfWork.Games.ThrowIfGameAliasIsNotUnique(game.Alias);
        await ThrowIfForeignKeyConstraintViolation(game);
        await _unitOfWork.Games.AddAsync(game);
        await _unitOfWork.SaveAsync();
        return Mapper.Map<GameBriefDto>(game);
    }

    public async Task UpdateGameAsync(GameUpdateDto dto)
    {
        await using var transaction = await _unitOfWork.BeginTransactionAsync();
        try
        {
            var existingGame = await _unitOfWork.Games.GetOneAsync(
                predicate: g => g.Id == Guid.Parse(dto.Game.Id),
                include: q => q.Include(g => g.GameGenres)
                        .Include(g => g.GamePlatforms)
                        .Include(g => g.Translations.Where(t => t.LanguageCode == dto.Culture))
                        .Include(g => g.Images),
                noTracking: false);

            if (existingGame.Alias != dto.Game.Key)
            {
                await _unitOfWork.Games.ThrowIfGameAliasIsNotUnique(dto.Game.Key);
            }

            UpdateMultiLingualEntity(existingGame, dto, dto.Culture);
            if (dto.Images is not null)
            {
                var images = await GetImagesForUpdateAsync(dto.Images);
                UpdateGameImages(existingGame, images, dto.Images.First(img => img.IsCover).Id);
            }

            await ThrowIfForeignKeyConstraintViolation(existingGame);
            await _unitOfWork.SaveAsync();
            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task DeleteGameAsync(string alias)
    {
        var gameToRemove = await _unitOfWork.Games.GetOneAsync(g => g.Alias == alias, noTracking: false);
        if (gameToRemove.Deleted)
        {
            await _unitOfWork.Games.DeleteAsync(gameToRemove.Id);
        }

        gameToRemove.Deleted = true;
        await _unitOfWork.SaveAsync();
    }

    public async Task<Tuple<byte[], string>> DownloadAsync(string gameAlias)
    {
        var game = await _unitOfWork.Games.GetOneAsync(g => g.Alias == gameAlias);
        string content = $"Game: {game.Name}\n\nDescription: {game.Description}";
        byte[] bytes = Encoding.UTF8.GetBytes(content);
        var timestamp = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
        string fileName = $"{game.Name}_{timestamp}.txt";
        return new Tuple<byte[], string>(bytes, fileName);
    }

    private async Task ThrowIfForeignKeyConstraintViolation(Game game)
    {
        await _unitOfWork.Genres.ThrowIfForeignKeyConstraintViolation(game.GameGenres);
        await _unitOfWork.Platforms.ThrowIfForeignKeyConstraintViolation(game.GamePlatforms);
        await _unitOfWork.Publishers.ThrowIfForeignKeyConstraintViolation(game);
    }

    private async Task<IList<AppImage>> GetImagesForUpdateAsync(ICollection<ImageDto> dtoImages)
    {
        var images = await _unitOfWork.Images.GetAsync(
            predicate: img => dtoImages.Select(x => x.Id).Contains(img.Id),
            noTracking: false);

        if (images.Count != dtoImages.Count)
        {
            throw new EntityNotFoundException();
        }

        return dtoImages.Join(
            images,
            dto => dto.Id,
            img => img.Id,
            (_, img) => img).ToList(); // keep original order as in dto
    }

    private static void UpdateGameImages(Game game, IList<AppImage> images, Guid coverImageId)
    {
        foreach (var image in game.Images)
        {
            image.ResetOwner();
        }

        for (var i = 0; i < images.Count; i++)
        {
            var image = images[i];
            if (image.GameId is not null)
            {
                throw new ForeignKeyException(nameof(image.GameId));
            }

            image.GameId = game.Id;
            image.IsCover = image.Id == coverImageId;
            image.Order = (ushort)(i + 1);

            if (image.IsCover)
            {
                image.Order = 0;
                game.PreviewImgUrl = image.Small ?? image.Large;
            }
        }
    }
}