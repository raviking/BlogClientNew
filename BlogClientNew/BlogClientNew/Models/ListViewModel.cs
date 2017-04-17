using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BlogClient.Core;
using BlogClient.Core.Objects;

namespace BlogClientNew.Models
{
    public class ListViewModel
    {
        public ListViewModel(IBlogRepository _blogrepository,int p) {
            Posts = _blogrepository.Posts(p - 1, 10);
            TotalPosts = _blogrepository.TotalPosts(true);
        }
        public ListViewModel(IBlogRepository _blogrepository, string Text_UrlSlugOrSearch,int p,string type) {
            switch (type) { 
                case "Tag":
                    Posts = _blogrepository.TotalPostsForTag(Text_UrlSlugOrSearch, p - 1, 10);
                    TotalPosts = _blogrepository.TotalNoPostsForTag(Text_UrlSlugOrSearch);
                    Tag = _blogrepository.Tag(Text_UrlSlugOrSearch);
                    break;
                case "Category":
                    Posts = _blogrepository.TotalPostsForCategory(Text_UrlSlugOrSearch, p - 1, 10);
                    TotalPosts = _blogrepository.TotalNoPostsForCategory(Text_UrlSlugOrSearch);
                    Category = _blogrepository.Category(Text_UrlSlugOrSearch);
                    break;
                default:
                    Posts = _blogrepository.TotalPostsForSearch(Text_UrlSlugOrSearch, p - 1, 10);
                    TotalPosts = _blogrepository.TotalNoSearchPosts(Text_UrlSlugOrSearch);
                    SearchText = Text_UrlSlugOrSearch;
                    break;
            }
        }
        public IList<Post> Posts { get; set; }
        public int TotalPosts { get; set; }
        public Category Category { get; set; }
        public Tag Tag { get; set; }
        public string SearchText{get;set;}
    }
}