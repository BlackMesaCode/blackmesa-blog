using System.Web.Http;

namespace BlackMesa.Blog.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();


            config.Routes.MapHttpRoute(
                "LearningApiDefault",
                "{culture}/Learning/Api/{controller}/{id}",
                new { culture = "de-DE", id = RouteParameter.Optional }
            );

        }
    }
}
