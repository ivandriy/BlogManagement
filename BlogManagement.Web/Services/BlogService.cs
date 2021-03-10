using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BlogManagement.DataAccess.Abstract;
using BlogManagement.DataAccess.DTO.Response;
using BlogManagement.DataAccess.Models;

namespace BlogManagement.Services
{
    public class BlogService : IBlogService
    {
        private readonly IBlogRepository _repository;
        private readonly IMapper _mapper;

        public BlogService(IBlogRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<IEnumerable<PostViewModel>> GetBlogPosts(int blogId)
        {
            var postsDto= (await _repository.GetAllBlogPosts(blogId)).ToList();
            var postsViewModels = _mapper.Map<List<Post>, List<PostViewModel>>(postsDto);
            return postsViewModels;
        }
    }
}