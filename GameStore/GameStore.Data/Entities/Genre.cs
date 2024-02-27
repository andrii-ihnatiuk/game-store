using GameStore.Data.Entities.Localization;
using GameStore.Data.Interfaces;

namespace GameStore.Data.Entities;

public class Genre : IMigrationTrackable, IMultiLingualEntity<GenreTranslation>
{
    public Guid Id { get; set; }

    public string? LegacyId { get; set; }

    public string Name { get; set; }

    public string? Picture { get; set; }

    public Guid? ParentGenreId { get; set; }

    public IList<Genre> SubGenres { get; set; } = new List<Genre>();

    public IList<GameGenre> GenreGames { get; set; } = new List<GameGenre>();

    public IList<GenreTranslation> Translations { get; set; } = new List<GenreTranslation>();
}