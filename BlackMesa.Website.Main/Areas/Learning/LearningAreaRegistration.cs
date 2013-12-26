using System.Web.Mvc;

namespace BlackMesa.Website.Main.Areas.Learning
{
    public class LearningAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Learning";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "LearningDefault",
                url: "{culture}/Learning/{controller}/{action}/{id}",
                defaults: new { culture = "de-DE", controller = "Learning_Folders", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "BlackMesa.Website.Main.Areas.Learning.Controllers" }
            );
        }
    }
}