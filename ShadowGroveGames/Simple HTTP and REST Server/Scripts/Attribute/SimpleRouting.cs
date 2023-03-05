using System;

namespace ShadowGroveGames.SimpleHttpAndRestServer.Scripts
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class SimpleEventServerRoutingAttribute : System.Attribute
    {
        public SimpleEventServerRoutingAttribute(string method, string route)
        {
            Method = method;
            Route = route;
        }

        public string Method { get; }
        public string Route { get; }
    }
}
