using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FakeTwitterAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{max_id}/{screen_name}",
                defaults: new { max_id = RouteParameter.Optional, screen_name = RouteParameter.Optional }
            );
        }
    }
}
