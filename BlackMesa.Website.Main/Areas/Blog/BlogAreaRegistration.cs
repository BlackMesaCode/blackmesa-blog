using System.Web.Mvc;

namespace BlackMesa.Website.Main.Areas.Blog
{
    public class BlogAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Blog";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {

            context.MapRoute(
                name: "EntryWithLanguage",
                url: "{culture}/Blog/Entry/{id}/{title}",
                defaults: new { culture = "de-DE", controller = "Entry", action = "Details", title = UrlParameter.Optional},
                constraints: new { culture = Global.CultureConstraints, id = Global.IdConstraints }
            );

            context.MapRoute(
                name: "TagWithLanguage",
                url: "{culture}/Blog/Tag/{selectedTag}",
                defaults: new { culture="de-DE", controller = "Entry", action = "Index", page = UrlParameter.Optional, orderBy = UrlParameter.Optional },
                constraints: new { culture = Global.CultureConstraints }
            );


            context.MapRoute(
                name: "BlogDefault",
                url: "{culture}/Blog/{controller}/{action}/{id}",
                defaults: new { culture = "de-DE", controller = "Entry", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}