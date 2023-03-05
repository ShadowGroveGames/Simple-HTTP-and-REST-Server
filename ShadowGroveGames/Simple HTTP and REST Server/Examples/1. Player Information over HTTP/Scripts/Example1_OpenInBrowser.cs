using ShadowGroveGames.SimpleHttpAndRestServer.Scripts;
using System.Linq;
using UnityEngine;

namespace ShadowGroveGames.SimpleHttpAndRestServer.Examples
{
    public class Example1_OpenInBrowser : MonoBehaviour
    {
        public SimpleEventServerScript simpleUnityEventServerScript;

        private int _port = 0;

        private void Start()
        {
            var listeningAddress = simpleUnityEventServerScript.ListeningAddresses.FirstOrDefault();
            if (listeningAddress == null)
                return;

            _port = listeningAddress.Port;
        }

        public void PlayerStatsButtonClick()
        {
            if (_port == 0)
            {
                Debug.LogError("Add a port to Simple Unity Events Server!");
                return;
            }

            Application.OpenURL($"http://127.0.0.1:{_port}/player/stats");
        }

        public void PlayerPositionButtonClick()
        {
            if (_port == 0)
            {
                Debug.LogError("Add a port to Simple Unity Events Server!");
                return;
            }
            Application.OpenURL($"http://127.0.0.1:{_port}/player/position");
        }

        public void PlayerRotationButtonClick()
        {
            if (_port == 0)
            {
                Debug.LogError("Add a port to Simple Unity Events Server!");
                return;
            }

            Application.OpenURL($"http://127.0.0.1:{_port}/player/rotation");
        }
    }
}
