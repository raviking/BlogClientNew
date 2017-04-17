using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

using Ninject;
using BlogClient.Core;
using BlogClient.Core.Objects;
using System;

namespace BlogClientNew
{
    public class PostModelBinder:DefaultModelBinder
    {
        private readonly IKernel _kernel;
        public PostModelBinder(IKernel kernel) {
            _kernel = kernel;
        }
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var post = (Post)base.BindModel(controllerContext, bindingContext);
            if (post.PostedOn.ToString() == DateTime.Parse("1/1/0001").ToString())
            {
                post.PostedOn = DateTime.UtcNow;
            }
            //create instance for IBlogRepository..
            var _blogrepository = _kernel.Get<IBlogRepository>();
            if (post.Category != null) {
                post.Category = _blogrepository.Category(post.Category.Id);
            }
            var tags = bindingContext.ValueProvider.GetValue("Tags").AttemptedValue.Split(',');
            post.Tags = new List<Tag>();
            foreach (var tag in tags) {
                post.Tags.Add(_blogrepository.Tag(int.Parse(tag.Trim())));
            }
            if (bindingContext.ValueProvider.GetValue("oper").AttemptedValue.Equals("edit"))
                post.Modified = DateTime.UtcNow;
            else
                post.Modified = DateTime.UtcNow;
            return post;
        }
    }
}