namespace GameStore.Shared.DTOs.Comment;

public class CommentBriefDto
{
    public Guid Id { get; set; }

    public string Author { get; set; }

    public string Body { get; set; }

    public DateTime CreationDate { get; set; }

    public IList<CommentBriefDto> ChildComments { get; set; }
}