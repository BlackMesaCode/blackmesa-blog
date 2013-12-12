using System.Web.Mvc;

namespace BlackMesa.Website.Main.Areas.Cards
{
    public class CardsAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Cards";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                name: "CardsDefault",
                url: "{culture}/Cards/{controller}/{action}/{id}",
                defaults: new { culture = "de-DE", controller = "Entry", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}