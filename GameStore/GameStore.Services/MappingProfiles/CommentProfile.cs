using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.DTOs.Comment;

namespace GameStore.Services.MappingProfiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentBriefDto>()
            .ForMember(
                dest => dest.ChildComments,
                opts => opts.MapFrom(src => src.Replies))
            .ForMember(
                dest => dest.Name,
                opts => opts.MapFrom(src => src.Author));

        CreateMap<CommentCreateInnerDto, Comment>();
        CreateMap<CommentCreateDto, Comment>()
            .IncludeMembers(m => m.Comment)
            .ForMember(
                dest => dest.ParentId,
                opts => opts.MapFrom(src => ConstructNullableGuidFromString(src.ParentId)))
            .ForMember(
                dest => dest.Author,
                opts => opts.MapFrom(src => src.Comment.Name));
    }

    private static Guid? ConstructNullableGuidFromString(string? str)
    {
        return string.IsNullOrEmpty(str) ? null : new Guid(str);
    }
}