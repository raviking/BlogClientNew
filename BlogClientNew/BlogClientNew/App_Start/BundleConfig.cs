using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace BlogClientNew.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles) {
            // Use the CDN file for bundles if specified.
            bundles.UseCdn = true;

            // jquery library bundle
            var jqueryBundle = new ScriptBundle("~/jquery", "http://ajax.aspnetcdn.com/ajax/jQuery/jquery-1.8.2.min.js")
                                    .Include("~/Scripts/jquery-{version}.js");
            bundles.Add(jqueryBundle);

            // jquery validation library bundle
            var jqueryValBundle = new ScriptBundle("~/jqueryval", "http://ajax.aspnetcdn.com/ajax/jquery.validate/1.10.0/jquery.validate.min.js")
                                        .Include("~/Scripts/jquery.validate.js");
            bundles.Add(jqueryValBundle);

            // jquery unobtrusive validation library
            var jqueryUnobtrusiveValBundle = new ScriptBundle("~/jqueryunobtrusiveval", "http://ajax.aspnetcdn.com/ajax/mvc/3.0/jquery.validate.unobtrusive.min.js")
                                                .Include("~/Scripts/jquery.validate.unobtrusive.js");
            bundles.Add(jqueryUnobtrusiveValBundle);

            // application script bundle
            var layoutJsBundle = new ScriptBundle("~/js").Include("~/Scripts/app.js");
            bundles.Add(layoutJsBundle);

            // css bundle
            var layoutCssBundle = new StyleBundle("~/css").Include("~/Content/themes/simple/style.css");
            bundles.Add(layoutCssBundle);

            // TODO: bundles for other pages

            //Login page
            var loginCssBundle = new StyleBundle("~/Content/themes/simple/admin").Include("~/Content/themes/simple/admin.css");
            bundles.Add(loginCssBundle);

            //Manage page
            // jQuery UI library bundle
            var jqueryUIBundle = new ScriptBundle("~/jqueryui", "http://ajax.aspnetcdn.com/ajax/jquery.ui/1.9.1/jquery-ui.min.js").Include("~/Scripts/jquery-ui.js");
            bundles.Add(jqueryUIBundle );
 
            // CSS bundle
            var manageCssBundle = new StyleBundle("~/Scripts/jqgrid/css/bundle").Include("~/Scripts/jqgrid/css/ui.jqgrid.css");
            bundles.Add(manageCssBundle);
 
            // jQuery UI library CSS bundle
            var jqueryUICssBundle =
            new StyleBundle("~/Content/themes/simple/jqueryuicustom/css/sunny/bundle").Include("~/Content/themes/simple/jqueryuicustom/css/sunny/jquery-ui-1.9.2.custom.css");
            bundles.Add(jqueryUICssBundle);
 
            // tinyMCE library bundle
            var tinyMceBundle = new ScriptBundle("~/Scripts/tiny_mce/js").Include("~/Scripts/tiny_mce/tiny_mce.js");
            bundles.Add(tinyMceBundle);
 
            // Other scripts
            var manageJsBundle = new ScriptBundle("~/manage/js").Include("~/Scripts/jqgrid/js/jquery.jqGrid.js").Include("~/Scripts/jqgrid/js/i18n/grid.locale-en.js").Include("~/Scripts/admin.js");
            bundles.Add(manageJsBundle);


        }
    }
}