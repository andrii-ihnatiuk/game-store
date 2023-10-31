namespace GameStore.Shared.DTOs.Comment;

public class CommentBriefDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Body { get; set; }

    public IList<CommentBriefDto> ChildComments { get; set; }
}