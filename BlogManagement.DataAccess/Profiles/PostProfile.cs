using System.Linq;
using AutoMapper;
using BlogManagement.DataAccess.DTO.Response;
using BlogManagement.DataAccess.Models;

namespace BlogManagement.DataAccess.Profiles
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostViewModel>()
                .ForMember(dst => dst.CategoryNames,
                    opt => opt.MapFrom(src => src.Categories.Select(c => c.Name)));
        }
    }
}