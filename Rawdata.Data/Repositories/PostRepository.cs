﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rawdata.Data.Models;
using Rawdata.Data.Repositories.Generic;
using Rawdata.Data.Repositories.Interfaces;

namespace Rawdata.Data.Repositories
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        public PostRepository(DataContext context) : base(context)
        {
        }

        public virtual Task<Post> GetById(int id)
        {
            return Context.Posts.SingleOrDefaultAsync(a => a.Id == id);
        }

        public virtual void Add(Post post)
        {
            Context.Set<Post>().Add(post);
        }

        public virtual async Task<IEnumerable<Post>> GetAllAsync()
        {
            return await Context.Posts
                .Include(p => p.ChildrenPosts)
                .Include(p => p.Parent)
                .Include(p => p.Comments)
                .Include(p => p.AcceptedAnswer)
                .Include(p => p.PostTags)
                .Include(p => p.LinkedByPosts)
                .Include(p => p.LinkedToPosts)
                .ToListAsync();
        }

        public virtual void Update(Post post)
        {
            Context.Set<Post>().Update(post);
        }

        public virtual void Remove(Post post)
        {
            Context.Set<Post>().Remove(post);
        }
        public IEnumerable<Post> FilterByTags(IList<string> tags)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Post> FilterByWords(int userId, string searchString, IList<string> tags)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Post> GetLinkedPosts(int postId)
        {
            throw new System.NotImplementedException();
        }
    }
}