using System;
using System.Web.Mvc;
using System.Web.Mvc.Html;

using BlogClient.Core.Objects;

namespace BlogClientNew
{
    public static class ActionLinkExtensions
    {
        public static MvcHtmlString PostLink(this HtmlHelper helper, Post post) {
            return helper.ActionLink(post.Title, "Post", "Blog",
                    new
                    {
                        year = post.PostedOn.Year,
                        month = post.PostedOn.Month,
                        title = post.UrlSlug
                    },
                    new { title = post.Title }
                );
        }
        public static MvcHtmlString CategoryLink(this HtmlHelper helper, Category category) { 
            return helper.ActionLink(category.Name,"Category","Blog",
                    new 
                    {
                        category=category.UrlSlug
                    },
                    new{title="See all the posts in {0}",category.Name}
                );
        }
        public static MvcHtmlString TagLink(this HtmlHelper helper, Tag tag) {
            return helper.ActionLink(tag.Name, "Tag", "Blog",
                    new
                    {
                        tag = tag.UrlSlug
                    },
                    new { title="See all the posts in {0}",tag.Name}
                );
        }
    }
}