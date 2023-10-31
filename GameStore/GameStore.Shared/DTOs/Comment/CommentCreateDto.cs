namespace GameStore.Shared.DTOs.Comment;

public class CommentCreateDto
{
    public CommentCreateInnerDto Comment { get; set; }

    public string? ParentId { get; set; }

    public string Action { get; set; }
}