using System;
using UnityEngine;

namespace ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Struct
{
    [Serializable]
    public class ListeningAddress
    {
        /// <summary>
        /// Port on which the listener should listen. 1 - 49150
        /// </summary>
        [Min(1)]
        [SerializeField]
        [Tooltip("Port on which the listener should listen. 1 - 49150")]
        private int _port;

        /// <summary>
        /// Port on which the listener should listen. 1 - 49150
        /// </summary>
        [SerializeField]
        [Tooltip("Select who is allowed to connect to your web server.\n\nLocal: Allows connections only from the Device running the game.\nAny: Allows connections from any device in the local network.")]
        private ListeningAddressType _host;

        public int Port { get { return _port; } }

        public ListeningAddressType Host { get { return _host; } }
    }

    public enum ListeningAddressType
    {
        Local,
        Any
    }
}
