using Microsoft.EntityFrameworkCore;
using PostBook.Data;
using PostBook.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostBook.Services
{
    public class PostService : IPostService
    {

        private readonly DataContext context;

        public PostService(DataContext dataContext)
        {
            context = dataContext;
        }

        public async Task<List<Post>> GetPostsAsync()
        {
            return await context.Posts
                .Include(post => post.PostTags)
                .ToListAsync();
        }

        public async Task<Post> GetPostByIdAsync(Guid postId)
        {
            return await context.Posts
                .Include(post => post.PostTags)
                .SingleOrDefaultAsync(post => post.Id == postId);
        }

        public async Task<bool> CreatePostAsync(Post post)
        {
            post.PostTags?.ForEach(x => x.TagName = x.TagName.ToLower());

            await AddNewTags(post);

            await context.Posts.AddAsync(post);
            var created = await context.SaveChangesAsync();

            return created > 0;
        }

        public async Task<bool> UpdatePostAsync(Post postToUpdate)
        {
            context.Posts.Update(postToUpdate);
            var updated = await context.SaveChangesAsync();

            return updated > 0;
        }

        public async Task<bool> DeletePostAsync(Guid postId)
        {
            var post = await GetPostByIdAsync(postId);
            if (post == null)
                return false;

            context.Posts.Remove(post);
            var deleted = await context.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<bool> UserOwnsPost(Guid postId, string UserId)
        {
            var post = await context.Posts.AsNoTracking().SingleOrDefaultAsync(post => post.Id == postId);

            Console.WriteLine(postId);

            if (post == null)
            {
                Console.WriteLine("can't find post");
                return false;
            }
            Console.WriteLine(post.UserId);
            Console.WriteLine(UserId);
            return post.UserId == UserId;
        }

        // Tags

        private async Task AddNewTags(Post post)
        {
            foreach (var postTag in post.PostTags)
            {
                var existingTag = await context.Tags.SingleOrDefaultAsync(x => x.Name == postTag.TagName);
                if (existingTag != null)
                    continue;

                await context.Tags.AddAsync(new Tag { Name = postTag.TagName, CreatedAt = DateTime.UtcNow });
            }
        }

        public async Task<Tag> GetTagByNameAsync(string tagName)
        {
            return await context.Tags.AsNoTracking().SingleOrDefaultAsync(tag => tag.Name == tagName.ToLower());
        }

        public async Task<Tag> GetPostByTag(string tagName)
        {
            //var posts = await context.Posts.Select(post => post.PostTags.Where(item => item.TagName == tagName).First().Post).ToListAsync();

            var posts = await context.Tags.Include(tag => tag.PostTags)
                .SingleOrDefaultAsync(tag => tag.Name == tagName);

            return posts;
        }
    }
}