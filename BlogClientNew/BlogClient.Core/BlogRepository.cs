using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NHibernate;
using NHibernate.Criterion;
using NHibernate.Linq;
using NHibernate.Transform;

using BlogClient.Core.Objects;

namespace BlogClient.Core
{
    public class BlogRepository:IBlogRepository
    {
        //NHibernate object
        private readonly ISession _session;

        public BlogRepository(ISession session) {
            _session = session;
        }

        //Returns all the Posts based pagesize and pagenumber
        public IList<Post> Posts(int PageNo, int PageSize) {
            var posts = _session.Query<Post>()
                        .Where(x => x.Published)
                        .OrderByDescending(x => x.PostedOn)
                        .Skip(PageNo * PageSize)
                        .Take(PageSize)
                        .Fetch(x => x.Category).ToList();

            var postids = posts.Select(x => x.Id).ToList();
            return _session.Query<Post>()
                        .Where(x => postids.Contains(x.Id))
                        .OrderByDescending(x => x.PostedOn)
                        .FetchMany(x => x.Tags).ToList();
        }
        public int TotalPosts(bool checkIsPulished=true)
        {
            return _session.Query<Post>()
                        .Where(x => !checkIsPulished || x.Published == true).Count();
        }

        public IList<Post> TotalPostsForCategory(string CategoryUrlslug, int PageNo, int PageSize) {
            var posts = _session.Query<Post>()
                            .Where(x => x.Published && x.Category.UrlSlug.Equals(CategoryUrlslug))
                            .OrderByDescending(x => x.PostedOn)
                            .Skip(PageNo * PageSize)
                            .Take(PageSize)
                            .Fetch(x => x.Category).ToList();
            var postids = posts.Select(x => x.Id).ToList();
            return _session.Query<Post>()
                            .Where(x => postids.Contains(x.Id))
                            .OrderByDescending(x => x.PostedOn)
                            .FetchMany(x => x.Tags).ToList();
        }

        public int TotalNoPostsForCategory(string CategoryUrlSlug) {
            return _session.Query<Post>()
                          .Where(x => x.Published && x.Category.UrlSlug.Equals(CategoryUrlSlug)).Count();   
        }

        public Category Category(string CategoryUrlSlug)
        {
            return _session.Query<Category>()
                            .FirstOrDefault(x => x.UrlSlug.Equals(CategoryUrlSlug));
        }

        public IList<Post> TotalPostsForTag(string TagUrlSlug, int PageNo, int PageSize) {
            var posts = _session.Query<Post>()
                            .Where(x => x.Published && x.Tags.Any(p => p.UrlSlug.Equals(TagUrlSlug)))
                            .OrderByDescending(x => x.PostedOn)
                            .Skip(PageNo * PageSize)
                            .Take(PageSize)
                            .Fetch(x => x.Category).ToList();
            var postids = posts.Select(x => x.Id).ToList();
            return _session.Query<Post>()
                            .Where(x => postids.Contains(x.Id))
                            .OrderByDescending(x => x.PostedOn)
                            .FetchMany(x => x.Tags).ToList();
        }

        public int TotalNoPostsForTag(string TagUrlSlug) {
            return _session.Query<Post>()
                            .Where(x => x.Published && x.Tags.Any(p => p.UrlSlug.Equals(TagUrlSlug))).Count();
        }

        public Tag Tag(string TagUrlSlug) {
            return _session.Query<Tag>()
                            .FirstOrDefault(x => x.UrlSlug.Equals(TagUrlSlug));
        }

        public IList<Post> TotalPostsForSearch(string searchstring, int PageNo, int PageSize) { 
            var posts=_session.Query<Post>()
                               .Where(x=>x.Published && x.Title.Contains(searchstring) || x.Category.Name.Equals(searchstring) || x.Tags.Any(p=>p.Name.Equals(searchstring)))
                               .OrderByDescending(x=>x.PostedOn)
                               .Skip(PageNo*PageSize)
                               .Take(PageSize)
                               .Fetch(x => x.Category).ToList();
            var postids = posts.Select(x => x.Id).ToList();
            return _session.Query<Post>()
                            .Where(p => postids.Contains(p.Id))
                            .OrderByDescending(p => p.PostedOn)
                            .FetchMany(p => p.Tags)
                            .ToList();
        }

        public int TotalNoSearchPosts(string searchstring) {
            return _session.Query<Post>()
                            .Where(x => x.Published && x.Title.Contains(searchstring) || x.Category.Name.Equals(searchstring) || x.Tags.Any(p => p.Name.Equals(searchstring)))
                            .Count();
        }

        public Post Post(int year, int month, string titleslug) {
            var query = _session.Query<Post>()
                            .Where(x => x.PostedOn.Year == year && x.PostedOn.Month == month && x.UrlSlug.Equals(titleslug))
                            .Fetch(x => x.Category);
            query.FetchMany(x => x.Tags).ToFuture();
            return query.ToFuture().Single();        
        }

        public IList<Category> Categories() {
            return _session.Query<Category>()
                            .OrderBy(x => x.Name).ToList();
        }

        public IList<Tag> Tags() {
            return _session.Query<Tag>()
                            .OrderByDescending(x => x.Name).ToList();
        }

        public IList<Post> Posts(int pageNo, int pageSize, string sortColumn, bool sortByAscending)
        {
            IList<Post> posts;
            IList<int> Postids;
            switch (sortColumn) {
                case "Title":
                    if (sortByAscending)
                    {
                        posts = _session.Query<Post>()
                                       .OrderBy(x => x.Title)
                                       .Skip(pageNo * pageSize)
                                       .Take(pageSize)
                                       .Fetch(x => x.Category).ToList();
                        Postids = posts.Select(x => x.Id).ToList();
                        return _session.Query<Post>()
                                        .Where(x => Postids.Contains(x.Id))
                                        .OrderBy(x => x.Title)
                                        .FetchMany(x => x.Tags).ToList();
                    }
                    else {
                        posts = _session.Query<Post>()
                                       .OrderByDescending(x => x.Title)
                                       .Skip(pageNo * pageSize)
                                       .Take(pageSize)
                                       .Fetch(x => x.Category).ToList();
                        Postids = posts.Select(x => x.Id).ToList();
                        return _session.Query<Post>()
                                        .Where(x => Postids.Contains(x.Id))
                                        .OrderByDescending(x => x.Title)
                                        .FetchMany(x => x.Tags).ToList();
                    }
                    break;
                case "Published":
                    if (sortByAscending)
                    {
                        posts = _session.Query<Post>()
                                       .OrderBy(x => x.Published)
                                       .Skip(pageNo * pageSize)
                                       .Take(pageSize)
                                       .Fetch(x => x.Category).ToList();
                        Postids = posts.Select(x => x.Id).ToList();
                        return _session.Query<Post>()
                                        .Where(x => Postids.Contains(x.Id))
                                        .OrderBy(x => x.Published)
                                        .FetchMany(x => x.Tags).ToList();
                    }
                    else {
                        posts = _session.Query<Post>()
                                       .OrderByDescending(x => x.Published)
                                       .Skip(pageNo * pageSize)
                                       .Take(pageSize)
                                       .Fetch(x => x.Category).ToList();
                        Postids = posts.Select(x => x.Id).ToList();
                        return _session.Query<Post>()
                                        .Where(x => Postids.Contains(x.Id))
                                        .OrderByDescending(x => x.Published)
                                        .FetchMany(x => x.Tags).ToList();
                    }
                    break;
                case "PostedOn":
                    if (sortByAscending)
                    {
                        posts = _session.Query<Post>()
                                       .OrderBy(x => x.PostedOn)
                                       .Skip(pageNo * pageSize)
                                       .Take(pageSize)
                                       .Fetch(x => x.Category).ToList();
                        Postids = posts.Select(x => x.Id).ToList();
                        return _session.Query<Post>()
                                        .Where(x => Postids.Contains(x.Id))
                                        .OrderBy(x => x.PostedOn)
                                        .FetchMany(x => x.Tags).ToList();
                    }
                    else {
                        posts = _session.Query<Post>()
                                       .OrderByDescending(x => x.PostedOn)
                                       .Skip(pageNo * pageSize)
                                       .Take(pageSize)
                                       .Fetch(x => x.Category).ToList();
                        Postids = posts.Select(x => x.Id).ToList();
                        return _session.Query<Post>()
                                        .Where(x => Postids.Contains(x.Id))
                                        .OrderByDescending(x => x.PostedOn)
                                        .FetchMany(x => x.Tags).ToList();
                    }
                    break;
                case "Modified":
                    if (sortByAscending)
                    {
                        posts = _session.Query<Post>()
                                       .OrderBy(x => x.Modified)
                                       .Skip(pageNo * pageSize)
                                       .Take(pageSize)
                                       .Fetch(x => x.Category).ToList();
                        Postids = posts.Select(x => x.Id).ToList();
                        return _session.Query<Post>()
                                        .Where(x => Postids.Contains(x.Id))
                                        .OrderBy(x => x.Modified)
                                        .FetchMany(x => x.Tags).ToList();
                    }
                    else {
                        posts = _session.Query<Post>()
                                       .OrderByDescending(x => x.Modified)
                                       .Skip(pageNo * pageSize)
                                       .Take(pageSize)
                                       .Fetch(x => x.Category).ToList();
                        Postids = posts.Select(x => x.Id).ToList();
                        return _session.Query<Post>()
                                        .Where(x => Postids.Contains(x.Id))
                                        .OrderByDescending(x => x.Modified)
                                        .FetchMany(x => x.Tags).ToList();
                    }
                    break;
                case "Category":
                     if (sortByAscending)
                    {
                        posts = _session.Query<Post>()
                                       .OrderBy(x => x.Category.Name)
                                       .Skip(pageNo * pageSize)
                                       .Take(pageSize)
                                       .Fetch(x => x.Category).ToList();
                        Postids = posts.Select(x => x.Id).ToList();
                        return _session.Query<Post>()
                                        .Where(x => Postids.Contains(x.Id))
                                        .OrderBy(x => x.Category.Name)
                                        .FetchMany(x => x.Tags).ToList();
                    }
                    else {
                        posts = _session.Query<Post>()
                                       .OrderByDescending(x => x.Category.Name)
                                       .Skip(pageNo * pageSize)
                                       .Take(pageSize)
                                       .Fetch(x => x.Category).ToList();
                        Postids = posts.Select(x => x.Id).ToList();
                        return _session.Query<Post>()
                                        .Where(x => Postids.Contains(x.Id))
                                        .OrderByDescending(x => x.Category.Name)
                                        .FetchMany(x => x.Tags).ToList();
                    }
                    break;
                default:
                   
                        posts = _session.Query<Post>()
                                       .OrderByDescending(x => x.PostedOn)
                                       .Skip(pageNo * pageSize)
                                       .Take(pageSize)
                                       .Fetch(x => x.Category).ToList();
                        Postids = posts.Select(x => x.Id).ToList();
                        return _session.Query<Post>()
                                        .Where(x => Postids.Contains(x.Id))
                                        .OrderByDescending(x => x.PostedOn)
                                        .FetchMany(x => x.Tags).ToList();                   
                    break;
            }
            return posts;
        }

        public int AddPost(Post post) {
            using (var tran = _session.BeginTransaction()) {
                _session.Save(post);
                tran.Commit();
                return post.Id;
            }
        }

        public Category Category(int categoryid) {
            return _session.Query<Category>()
                            .FirstOrDefault(x => x.Id == categoryid);
        }

        public Tag Tag(int tagid) {
            return _session.Query<Tag>()
                            .FirstOrDefault(x => x.Id == tagid);
        }

        public void EditPost(Post post) {
            using (var trans = _session.BeginTransaction()) {
                _session.SaveOrUpdate(post);
                trans.Commit();
            }
        }

        public void DeletePost(int postid) {
            using (var trans = _session.BeginTransaction()) { 
                //get the post to delete
                var post=_session.Get<Post>(postid);
                _session.Delete(post);
                trans.Commit();
            }
        }

        public int AddCategory(Category category) {
            using (var tran = _session.BeginTransaction())
            {
                _session.Save(category);
                tran.Commit();
                return category.Id;
            }
        }

        public void EditCategory(Category category)
        {
            using (var tran = _session.BeginTransaction())
            {
                _session.SaveOrUpdate(category);
                tran.Commit();
            }
        }

        public void DeleteCategory(int id)
        {
            using (var tran = _session.BeginTransaction())
            {
                var category = _session.Get<Category>(id);
                _session.Delete(category);
                tran.Commit();
            }
        }

        //Tags Actions
        public int AddTag(Tag tag)
        {
            using (var tran = _session.BeginTransaction())
            {
                _session.Save(tag);
                tran.Commit();
                return tag.Id;
            }
        }

        public void EditTag(Tag tag)
        {
            using (var tran = _session.BeginTransaction())
            {
                _session.SaveOrUpdate(tag);
                tran.Commit();
            }
        }

        public void DeleteTag(int id)
        {
            using (var tran = _session.BeginTransaction())
            {
                var tag = _session.Get<Tag>(id);
                _session.Delete(tag);
                tran.Commit();
            }
        }
    }
}
