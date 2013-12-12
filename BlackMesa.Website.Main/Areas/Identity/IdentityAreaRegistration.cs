using System.Web.Mvc;

namespace BlackMesa.Website.Main.Areas.Identity
{
    public class IdentityAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Identity";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "IdentityDefault",
                "{culture}/Identity/{controller}/{action}/{id}",
                new { culture = "de-DE", controller = "Account", action = "Login", id = UrlParameter.Optional }
            );
        }
    }
}