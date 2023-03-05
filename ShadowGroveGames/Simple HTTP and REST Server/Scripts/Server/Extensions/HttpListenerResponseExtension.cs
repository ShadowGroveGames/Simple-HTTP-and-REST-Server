using Newtonsoft.Json.Linq;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

namespace ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server.Extensions
{
    public static class HttpListenerResponseExtension
    {
        public static void JsonResponse(this HttpListenerResponse response, JObject data)
        {
            string json = data.ToString();

            WriteUTF8TextToResponseBody(response, json, HttpConstants.ContentTypeApplicationJson + "; charset=utf-8");
        }

        public static void JsonResponse<T>(this HttpListenerResponse response, T data)
        {
            string json = JsonUtility.ToJson(data);

            WriteUTF8TextToResponseBody(response, json, HttpConstants.ContentTypeApplicationJson + "; charset=utf-8");
        }

        public static void TextResponse(this HttpListenerResponse response, string text)
        {
            WriteUTF8TextToResponseBody(response, text, HttpConstants.ContentTypeTextPlain + "; charset=utf-8");
        }

        public static void HtmlResponse(this HttpListenerResponse response, string text)
        {
            WriteUTF8TextToResponseBody(response, text, HttpConstants.ContentTypeTextHtml + "; charset=utf-8");
        }

        public static void ImageResponse(this HttpListenerResponse response, Sprite sprite)
        {
            Texture2D texture = sprite.texture;
            ImageResponse(response, texture);
        }

        public static void ImageResponse(this HttpListenerResponse response, Texture2D texture)
        {
            var bufferedTexture = new Texture2D(texture.width, texture.height);
            bufferedTexture.SetPixels32(texture.GetPixels32(0));
            bufferedTexture.Apply();

            Stream imageStream = new MemoryStream(bufferedTexture.EncodeToPNG());
            StreamResponse(response, imageStream, HttpConstants.ContentTypeImagePng);
        }

        public static void WriteUTF8TextToResponseBody(HttpListenerResponse response, string text, string contentType)
        {
            Stream stream = new MemoryStream(Encoding.UTF8.GetBytes(text));
            response.ContentEncoding = Encoding.UTF8;

            StreamResponse(response, stream, contentType);
        }

        public static void StreamResponse(this HttpListenerResponse response, Stream stram, string contentType = HttpConstants.ContentTypeApplicationOctetStream)
        {
            response.ContentType = contentType;
            response.ContentLength64 = stram.Length;

            byte[] buffer = new byte[1024 * 16];
            int byteCount;

            while ((byteCount = stram.Read(buffer, 0, buffer.Length)) > 0)
                response.OutputStream.Write(buffer, 0, byteCount);


            stram.Close();
            response.OutputStream.Flush();
            response.OutputStream.Close();
            response.Close();
        }
    }
}
