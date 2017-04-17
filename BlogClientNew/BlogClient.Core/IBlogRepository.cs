using BlogClient.Core.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogClient.Core
{
    public interface IBlogRepository
    {
        //REturns all the posts with pagination values
        IList<Post> Posts(int PageNo, int PageSize);

        //Returns total posts number
        int TotalPosts(bool checkIsPulished);

        //Returns all posts in Category based on urlslug
        IList<Post> TotalPostsForCategory(string CategoryUrlslug, int PageNo, int PageSize);

        //Returns all posts num in category
        int TotalNoPostsForCategory(string CategoryUrlSlug);

        //REturns single category
        Category Category(string CategoryUrlSlug);
        Category Category(int categoryid);      

        //Returns all Posts belongs to Tag
        IList<Post> TotalPostsForTag(string TagUrlSlug, int PageNo, int PageSize);

        //Returns total number of posts for Tag
        int TotalNoPostsForTag(string TagUrlSlug);

        //Returns Tag based on TagUrl
        Tag Tag(string TagUrlSlug);
        Tag Tag(int tagid);

        //Search
        IList<Post> TotalPostsForSearch(string searchstring,int PageNo,int PageSize);
        int TotalNoSearchPosts(string searchstring);

        //Returns and displays single post
        Post Post(int year, int month, string titleslug);

        //Widget methods 
        //gets all categories
        IList<Category> Categories();

        //gets tags
        IList<Tag> Tags();

        //Posts grid 
        IList<Post> Posts(int pageNo, int pageSize, string sortColumn, bool sortByAscending);

        //Add Post
        int AddPost(Post post);
        void EditPost(Post post);
        void DeletePost(int postid);

        //Categories
        int AddCategory(Category category);
        void EditCategory(Category category);
        void DeleteCategory(int id);

        //Tags
        int AddTag(Tag tag);
        void EditTag(Tag tag);
        void DeleteTag(int id);
    }
}
