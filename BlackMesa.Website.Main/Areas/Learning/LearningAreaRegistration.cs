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
                "LearningDefault",
                "{culture}/Learning/{controller}/{action}/{id}",
                new { culture = "de-DE", controller = "Learning_Folders", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}