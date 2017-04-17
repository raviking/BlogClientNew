using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using BlogClient.Core;
using BlogClient.Core.Objects;

namespace BlogClientNew.Models
{
    public class WidgetViewModel
    {
        public WidgetViewModel(IBlogRepository _blogrepository) {
            Categories = _blogrepository.Categories();
            Tags = _blogrepository.Tags();
            LatestPosts = _blogrepository.Posts(0,10);
        }
        public IList<Category> Categories { get; set; }
        public IList<Tag> Tags { get; set; }
        public IList<Post> LatestPosts { get; set; }
    }
}