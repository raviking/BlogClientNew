using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using BlogClient.Core.Objects;
using System.Web.Mvc;

namespace BlogClientNew
{
    public static class Extensions
    {
        public static string ToConfigLocalTime(this DateTime utcDT) {
            var isTZ = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["TimeZone"]);
            return string.Format("{0}",TimeZoneInfo.ConvertTimeFromUtc(utcDT,isTZ).ToShortDateString(),ConfigurationManager.AppSettings["TomeZoneAbbr"]);
        }

        public static string Href(this Post post, UrlHelper helper) {
            return helper.RouteUrl(new { controller="Blog",action="Post",year=post.PostedOn.Year,month=post.PostedOn.Month,title=post.UrlSlug});
        }
    }
}