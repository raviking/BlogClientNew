using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text;
using System.ServiceModel.Syndication;

using BlogClient.Core;
using BlogClientNew.Models;
using BlogClient.Core.Objects;
using System.Configuration;
using System.Net.Mail;

namespace BlogClientNew.Controllers
{
    public class BlogController : Controller
    {
        private readonly IBlogRepository _blogrepository;

        //Default constructor
        public BlogController(IBlogRepository blogrepository) {
            _blogrepository = blogrepository;
        }

        public ViewResult Posts(int p=1)
        {
            var ViewModel = new ListViewModel(_blogrepository, p);
            ViewBag.Title = "Latest Posts";
            return View("List",ViewModel);
        }

        public ActionResult Category(string categoryslug, int p=1) {
            var ViewModel = new ListViewModel(_blogrepository, categoryslug, p,"Category");
            if (ViewModel.Category == null) {
                throw new HttpException(404, "Category not found!");
            }
            ViewBag.Title = String.Format("Latest Posts on this category: {0}", ViewModel.Category.Name);
            return View("List",ViewModel);
        }

        public ActionResult Tag(string Tagslug, int p = 1) {
            var ViewModel = new ListViewModel(_blogrepository, Tagslug, p, "Tag");
            if (ViewModel.Tag == null) {
                throw new HttpException(404, "Tag not Found!");
            }
            ViewBag.Title = String.Format("Latest Posts under this Tag: {0}",ViewModel.Tag.Name);
            return View("List",ViewModel);
        }

        public ActionResult Search(string s, int p = 1) {
            var ViewModel = new ListViewModel(_blogrepository, s, p, "search");
            ViewBag.Title = String.Format(@"Lists of posts found
                        for search text ""{0}""", s);
            return View("List", ViewModel);
        }

        public ActionResult Post(int year, int month, string title) {
            var post = _blogrepository.Post(year, month, title);
            if (post == null) {
                throw new HttpException(404, "Post Not Found!");
            }
            if (post.Published==false && User.Identity.IsAuthenticated==false) {
                throw new HttpException(401, "The post is not published");
            }
            return View(post);
        }
        
        //Sidebar
        [ChildActionOnly]
        public PartialViewResult SideBar() {
            var WidgetModel = new WidgetViewModel(_blogrepository);
            return PartialView("_Sidebar",WidgetModel);
        }

        public ActionResult Feed() { 
            var blogTitle=ConfigurationManager.AppSettings["BlogTitle"];
            var blogDescription=ConfigurationManager.AppSettings["BlogDescription"];
            var blogUrl=ConfigurationManager.AppSettings["BlogUrl"];
            // Create a collection of SyndicationItemobjects from the latest posts
            var posts = _blogrepository.Posts(0, 25).Select
            (
              p => new SyndicationItem
                  (
                      p.Title,
                      p.Description,
                      new Uri(string.Concat(blogUrl, p.Href(Url)))
                  )
            );

            // Create an instance of SyndicationFeed class passing the SyndicationItem collection
            var feed = new SyndicationFeed(blogTitle, blogDescription, new Uri(blogUrl), posts)
            {
                Copyright = new TextSyndicationContent(String.Format("Copyright ï¿½ {0}", blogTitle)),
                Language = "en-US"
            };

            // Format feed in RSS format through Rss20FeedFormatter formatter
            var feedFormatter = new Rss20FeedFormatter(feed);

            // Call the custom action that write the feed to the response
            return new FeedResult(feedFormatter);
        }

        //Contact
        public ActionResult Contact() {

            return View();
        }

        [HttpPost]
        public ViewResult Contact(Contact contact)
        {
            if (ModelState.IsValid)
            {
                using (var client = new SmtpClient())
                {
                    var adminEmail = ConfigurationManager.AppSettings["AdminEmail"];
                    var from = new MailAddress(adminEmail, "JustBlog Messenger");
                    var to = new MailAddress(adminEmail, "JustBlog Admin");
 
                    using (var message = new MailMessage(from, to))
                    {
                        message.Body = contact.Body;
                        message.IsBodyHtml = true;
                        message.BodyEncoding = Encoding.UTF8;
 
                        message.Subject = contact.Subject;
                        message.SubjectEncoding = Encoding.UTF8;
 
                        message.ReplyTo = new MailAddress(contact.Email);
 
                        client.Send(message);
                    }
                }
 
                return View("Thanks");
            }
 
            return View();
        }
        public ActionResult AboutMe() {
            return View();
        }
    }
}
