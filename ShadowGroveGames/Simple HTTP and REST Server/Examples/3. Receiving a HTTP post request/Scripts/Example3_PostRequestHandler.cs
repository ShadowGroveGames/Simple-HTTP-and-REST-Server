using Newtonsoft.Json;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server.Extensions;
using System.Net;
using UnityEngine;
using UnityEngine.UI;

namespace ShadowGroveGames.SimpleHttpAndRestServer.Examples
{
    public class Example3_PostRequestHandler : MonoBehaviour
    {
        [SerializeField]
        private Text _title;

        [SerializeField]
        private Text _body;

        [SimpleEventServerRouting(HttpConstants.MethodPost, "/rawString")]
        public void PostRawStringEndpoint(HttpListenerContext context)
        {
            _title.text = "Recivied raw string";
            _body.text = context.Request.GetStringBody();
            Debug.Log(_body.text);
        }

        [SimpleEventServerRouting(HttpConstants.MethodPost, "/json")]
        public void PostJsonEndpoint(HttpListenerContext context)
        {
            var jsonObject = context.Request.GetJsonBody<PostJsonBody>();

            _title.text = "Recivied json";

            if (jsonObject == null)
            {
                _body.text = "Invalid Body!";
                return;
            }

            _body.text = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
            Debug.Log(_body.text);
        }

        public struct PostJsonBody
        {
            public string playerName;
            public int level;
        }
    }
}
