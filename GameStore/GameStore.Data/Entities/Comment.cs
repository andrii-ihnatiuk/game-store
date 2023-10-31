using GameStore.Shared.Constants;

namespace GameStore.Data.Entities;

public class Comment
{
    public Guid Id { get; set; }

    public string Author { get; set; }

    public string Body { get; set; }

    public CommentType Type { get; set; } = CommentType.Plain;

    public string Intro { get; set; } = string.Empty;

    public Guid? ParentId { get; set; }

    public Guid GameId { get; set; }

    public Game Game { get; set; }

    public IList<Comment> Replies { get; set; } = new List<Comment>();
}