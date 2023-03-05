using Newtonsoft.Json.Linq;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server.Extensions;
using System.Net;
using UnityEngine;

namespace ShadowGroveGames.SimpleHttpAndRestServer.Examples
{
    public class Example1_PlayerPositionRequestHandler : MonoBehaviour
    {
        [SerializeField]
        private GameObject _player;

        private Example1_PlayerController _playerController;

        void Start()
        {
            _playerController = _player.GetComponent<Example1_PlayerController>();

            if (_playerController == null)
                Debug.LogError("Cant find player controller!");
        }

        [SimpleEventServerRouting(HttpConstants.MethodGet, "/player/stats")]
        public void GetPlayerStats(HttpListenerContext context)
        {
            context.Response.JsonResponse(new JObject()
            {
                new JProperty("MaxHealth", 10),
                new JProperty("CurrentHealth", _playerController.CurrentHealth),
            });
        }

        [SimpleEventServerRouting(HttpConstants.MethodGet, "/player/position")]
        public void GetPlayerPosition(HttpListenerContext context)
        {
            context.Response.JsonResponse(_player.transform.position);
        }

        [SimpleEventServerRouting(HttpConstants.MethodGet, "/player/rotation")]
        public void GetPlayerRotation(HttpListenerContext context)
        {
            context.Response.JsonResponse(_player.transform.rotation);
        }
    }
}
