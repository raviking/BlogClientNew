using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using BlogClientNew.Models;
using System.Web.Security;
using BlogClientNew.Providers;
using BlogClient.Core;
using Newtonsoft.Json;
using BlogClient.Core.Objects;
using System.Text;

namespace BlogClientNew.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        // GET: /Admin/
        private readonly IAuthProvider _authprovider;
        private readonly IBlogRepository _blogrepository;

        public AdminController(IAuthProvider authprovider,IBlogRepository blogrepostiroy=null) {
            _authprovider = authprovider;
            _blogrepository = blogrepostiroy;
        }
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (_authprovider.IsLoggedIn) {
                return RedirectToUrl(returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel loginmodel, string returnurl) {
            if (ModelState.IsValid && _authprovider.Login(loginmodel.UserName, loginmodel.Password)) {
                return RedirectToUrl(returnurl);
            }
            ModelState.AddModelError("", "UserName and Password Entered are incorrect");
            return View();
        }

        public ActionResult Logout()
        {
            _authprovider.Logout();
            return RedirectToAction("Login","Admin");
        }

        public ActionResult RedirectToUrl(string returnurl) {
            if (Url.IsLocalUrl(returnurl))
                return Redirect(returnurl);
            else
                return RedirectToAction("Manage");
        }

        public ActionResult Manage() {

            return View();
        }

        //Posts Actions
        public ActionResult Posts(JqViewModel jqparams) {
            var posts = _blogrepository.Posts(jqparams.page - 1, jqparams.rows, jqparams.sidx, jqparams.sord == "asc");
            var totalposts = _blogrepository.TotalPosts(false);
            return Content(JsonConvert.SerializeObject(new
            {
                page = jqparams.page,
                rows=posts,
                records=totalposts,
                total=Math.Ceiling(Convert.ToDouble(totalposts)/jqparams.rows)
            },new CustomDateTimeConvertor()),"application/json");
        }

        [HttpPost,ValidateInput(false)]
        public ActionResult AddPost(Post post) {           
            string Json;
            ModelState.Clear();
            if (TryValidateModel(post))
            {
                var id = _blogrepository.AddPost(post);
                Json = JsonConvert.SerializeObject(new
                {
                    id = id,
                    success = true,
                    message = "Post added successfully!"
                });
            }
            else {
                Json = JsonConvert.SerializeObject(new
                {
                    id=0,
                    success=false,
                    message="Failed to add Post!"
                });
            }
            return Content(Json, "application/json");
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditPost(Post post) {
            string Json;
            ModelState.Clear();
            if (TryValidateModel(post))
            {
                _blogrepository.EditPost(post);
                Json = JsonConvert.SerializeObject(new
                {
                    id = post.Id,
                    success = true,
                    message = "Post Updated Successfully!"
                });
            }
            else {
                Json = JsonConvert.SerializeObject(new
                {
                    id=0,
                    success=false,
                    message="Post Updation failed!"
                });
            }
            return Content(Json,"application/json");
        }

        [HttpPost]
        public ActionResult DeletePost(int id) {
            _blogrepository.DeletePost(id);
            var Json = JsonConvert.SerializeObject(new
            {
                id=0,
                success=true,
                message="Post Deleted Successfully!"
            });
            return Content(Json, "application/json");
        }

        public ContentResult GetCategoriesHtml()
        {
            var categories = _blogrepository.Categories().OrderBy(x => x.Name);
            var sb = new StringBuilder();
            sb.AppendLine(@"<select>");
            foreach (var category in categories) { 
                sb.AppendLine(String.Format(@"<option value=""{0}"">{1}</option>",category.Id,category.Name));
            }
            sb.AppendLine(@"<select>");
            return Content(sb.ToString(),"text/html");
        }

        public ContentResult GetTagsHtml() {
            var tags = _blogrepository.Tags().OrderBy(x => x.Name);
            var sb = new StringBuilder();
            sb.AppendLine(@"<select>");
            foreach (var tag in tags) {
                sb.AppendLine(String.Format(@"<option value=""{0}"">{1}</option>", tag.Id, tag.Name));
            }
            sb.AppendLine(@"<select>");
            return Content(sb.ToString(),"text/html");
        }

        //Categories Actions
        public ContentResult Categories() {
            var categories = _blogrepository.Categories();
            return Content(JsonConvert.SerializeObject(new
            {
                page=1,
                records=categories.Count,
                rows=categories,
                total=1
            }),"application/json");
        }

        [HttpPost]
        public ContentResult AddCategory([Bind(Exclude="Id")]Category category)
        {
            string json;

            if (ModelState.IsValid)
            {
                var id = _blogrepository.AddCategory(category);
                json = JsonConvert.SerializeObject(new
                {
                    id = id,
                    success = true,
                    message = "Category added successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to add the category."
                });
            }
            return Content(json, "application/json");
        }

        [HttpPost]
        public ContentResult EditCategory(Category category)
        {
            string json;

            if (ModelState.IsValid)
            {
                _blogrepository.EditCategory(category);
                json = JsonConvert.SerializeObject(new
                {
                    id = category.Id,
                    success = true,
                    message = "Changes saved successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to save the changes."
                });
            }
            return Content(json, "application/json");
        }
        [HttpPost]
        public ContentResult DeleteCategory(int id)
        {
            _blogrepository.DeleteCategory(id);

            var json = JsonConvert.SerializeObject(new
            {
                id = 0,
                success = true,
                message = "Category deleted successfully."
            });

            return Content(json, "application/json");
        }

        //Tags Actions
        public ContentResult Tags()
        {
            var tags = _blogrepository.Tags();

            return Content(JsonConvert.SerializeObject(new
            {
                page = 1,
                records = tags.Count,
                rows = tags,
                total = 1
            }), "application/json");
        }

        [HttpPost]
        public ContentResult AddTag([Bind(Exclude = "Id")]Tag tag)
        {
            string json;
            if (ModelState.IsValid)
            {
                var id = _blogrepository.AddTag(tag);
                json = JsonConvert.SerializeObject(new
                {
                    id = id,
                    success = true,
                    message = "Tag added successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to add the tag."
                });
            }
            return Content(json, "application/json");
        }

        [HttpPost]
        public ContentResult EditTag(Tag tag)
        {
            string json;

            if (ModelState.IsValid)
            {
                _blogrepository.EditTag(tag);
                json = JsonConvert.SerializeObject(new
                {
                    id = tag.Id,
                    success = true,
                    message = "Changes saved successfully."
                });
            }
            else
            {
                json = JsonConvert.SerializeObject(new
                {
                    id = 0,
                    success = false,
                    message = "Failed to save the changes."
                });
            }
            return Content(json, "application/json");
        }

        [HttpPost]
        public ContentResult DeleteTag(int id)
        {
            _blogrepository.DeleteTag(id);

            var json = JsonConvert.SerializeObject(new
            {
                id = 0,
                success = true,
                message = "Tag deleted successfully."
            });

            return Content(json, "application/json");
        }
    }
}
