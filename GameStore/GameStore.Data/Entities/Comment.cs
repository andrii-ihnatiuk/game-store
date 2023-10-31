namespace GameStore.Data.Entities;

public class Comment
{
    public Guid Id { get; set; }

    public string Author { get; set; }

    public string Body { get; set; }

    public Guid? ParentId { get; set; }

    public Guid GameId { get; set; }

    public Game Game { get; set; }

    public IList<Comment> Replies { get; set; } = new List<Comment>();
}