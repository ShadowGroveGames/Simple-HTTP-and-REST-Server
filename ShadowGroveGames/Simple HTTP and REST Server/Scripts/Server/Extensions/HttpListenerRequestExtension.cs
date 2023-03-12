using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server.Extensions
{
    public static class HttpListenerRequestExtension
    {
        public static string? GetStringBody(this HttpListenerRequest request)
        {
            if (request.InputStream == null || request.ContentLength64 == 0)
                return null;

            using (StreamReader reader = new StreamReader(request.InputStream, request.ContentEncoding))
            {
                return reader.ReadToEnd();
            }
        }

        public static T? GetJsonBody<T>(this HttpListenerRequest request) where T : struct
        {
            if (request.InputStream == null || request.ContentLength64 == 0)
                return null;

            if (request.ContentType != HttpConstants.ContentTypeApplicationJson)
            {
                Debug.LogError($"Request missing Content-Type: {HttpConstants.ContentTypeApplicationJson}");
                return null;
            }

            string rawString = GetStringBody(request);
            if (string.IsNullOrEmpty(rawString))
                return null;

            return JsonConvert.DeserializeObject<T>(rawString);
        }
    }
}
