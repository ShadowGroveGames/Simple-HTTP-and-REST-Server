using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts;
using System.Net;
using UnityEngine;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server.Extensions;

namespace ShadowGroveGames.SimpleHttpAndRestServer.Examples
{
    public class Example2_ResourceRequestHandler : MonoBehaviour
    {

        [SimpleEventServerRouting(HttpConstants.MethodGet, "/resource-file")]
        public void GetHtmlResource(HttpListenerContext context)
        {
            var content = (TextAsset)Resources.Load("shrs-example2-wwwroot/resource-file", typeof(TextAsset));
            context.Response.HtmlResponse(content.text);
        }

        [SimpleEventServerRouting(HttpConstants.MethodGet, "/banner.png")]
        public void GetImageResource(HttpListenerContext context)
        {
            var texture = (Texture2D)Resources.Load("shrs-example2-wwwroot/banner", typeof(Texture2D));
            context.Response.ImageResponse(texture);
        }
    }
}
