using System.Web.Http;

namespace CrudGridExample.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "MetadataApi",
                routeTemplate: "api/Metadata/{id}",
                defaults: new { controller = "Metadata", action = "GetMetadataItem" });

            //config.Routes.MapHttpRoute(
            //  name: "CustomMetadataApi",
            //  routeTemplate: "api/{controller}/Metadata",
            //  defaults: new { action = "GetMetadata" });

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.Routes.MapHttpRoute(
                name: "ControllerAndAction",
                routeTemplate: "api/{controller}/{action}"
            );

            config.Routes.MapHttpRoute(
                name: "ActionApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { id = RouteParameter.Optional });

            var json = config.Formatters.JsonFormatter;
            json.SerializerSettings.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects;
            json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            
            config.EnableSystemDiagnosticsTracing();
        }
    }
}
