using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Tweetbook.Contracts.V1;
using Tweetbook.Contracts.V1.Request;
using Tweetbook.Contracts.V1.Response;
using Tweetbook.Domain;
using Tweetbook.Services;

namespace Tweetbook.Controllers.V1
{
    public class PostsController : Controller
    {
        private IPostService _postService;

        public PostsController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpGet(ApiRoutes.Posts.GelAll)]
        public IActionResult GetAll()
        {
            return Ok( _postService.GetPosts());
        }

        [HttpDelete(ApiRoutes.Posts.Delete)]
        public IActionResult Delete([FromRoute] Guid postId)
        {
            var deleted = _postService.DeletePost(postId);

            if (deleted)
                return NoContent();
            return NotFound("Post not found.");
        }

        [HttpPut(ApiRoutes.Posts.Update)]
        public IActionResult Get([FromRoute] Guid postId, UpdatePostRequest request )
        {
            var post = new Post
            {
                Id = postId,
                Name = request.Name
            };

            var updated = _postService.UpdatePost(post);

            if(updated)
               return Ok(post);

            return NotFound("Post not found.");
        }

        [HttpGet(ApiRoutes.Posts.Get)]
        public IActionResult Get([FromRoute] Guid postId)
        {
            var post = _postService.GetPostById(postId);

            if(post == null)
            {
                return NotFound("Post not found.");
            }
            return Ok(post);
        }

        [HttpPost(ApiRoutes.Posts.Create)]
        public IActionResult Create([FromBody] CreatePostRequest postRequest)
        {
            var post = new Post { Id = postRequest.Id };
            if (post.Id != Guid.Empty)
                post.Id = Guid.NewGuid();

            _postService.GetPosts().Add(post);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Posts.Get.Replace("{postId}", post.Id.ToString());

            var response = new PostResponse { Id = post.Id };

            return Created(locationUrl, response);
        }
    }
}
