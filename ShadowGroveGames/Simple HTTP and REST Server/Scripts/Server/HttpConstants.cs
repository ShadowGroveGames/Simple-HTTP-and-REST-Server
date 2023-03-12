using System.Collections.Generic;

namespace ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server
{
    public static class HttpConstants
    {
        public const string ServerName = "ShadowGroveGames UnityServer";

        public const string MethodGet = "GET";
        public const string MethodPost = "POST";
        public const string MethodPut = "PUT";
        public const string MethodPatch = "PATCH";
        public const string MethodDelete = "DELETE";
        public const string MethodOptions = "OPTIONS";
        public const string MethodHead = "HEAD";

        public static readonly List<string> HttpMethodsWithRequstBody = new List<string>()
        {
            MethodPost,
            MethodPut,
            MethodPatch
        };

        public const string ContentTypeApplicationOctetStream = "application/octet-stream";
        public const string ContentTypeApplicationJson = "application/json";
        public const string ContentTypeTextHtml = "text/html";
        public const string ContentTypeTextPlain = "text/plain";
        public const string ContentTypeImageJpeg = "image/jpeg";
        public const string ContentTypeImageGif = "image/gif";
        public const string ContentTypeImagePng = "image/png";
        public const string ContentTypeImageWebp = "image/webp";
        public const string ContentTypeImageSvg = "image/svg+xml";
    }
}
