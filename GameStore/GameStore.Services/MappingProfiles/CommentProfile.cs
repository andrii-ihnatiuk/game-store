using System.Diagnostics.CodeAnalysis;
using AutoMapper;
using GameStore.Data.Entities;
using GameStore.Shared.Constants;
using GameStore.Shared.DTOs.Comment;

namespace GameStore.Services.MappingProfiles;

[ExcludeFromCodeCoverage]
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
                opts => opts.MapFrom(src => src.Author))
            .ForMember(
                dest => dest.Body,
                opts => opts.MapFrom(src => src.Intro + src.Body));

        CreateMap<CommentCreateInnerDto, Comment>();
        CreateMap<CommentCreateDto, Comment>()
            .IncludeMembers(m => m.Comment)
            .ForMember(
                dest => dest.ParentId,
                opts => opts.MapFrom(src => ConstructNullableGuidFromString(src.ParentId)))
            .ForMember(
                dest => dest.Author,
                opts => opts.MapFrom(src => src.Comment.Name))
            .ForMember(
                dest => dest.Type,
                opts => opts.MapFrom(src => GetCommentTypeFromString(src.Action)));
    }

    private static Guid? ConstructNullableGuidFromString(string? str)
    {
        return string.IsNullOrEmpty(str) ? null : new Guid(str);
    }

    private static CommentType GetCommentTypeFromString(string value)
    {
        return Enum.TryParse(value, out CommentType result) ? result : CommentType.Plain;
    }
}