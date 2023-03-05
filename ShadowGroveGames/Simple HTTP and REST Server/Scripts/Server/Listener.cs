using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

namespace ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server
{
    public class Listener : IDisposable
    {
        private HttpListener _listener;
        private Thread _listenerThread;
        private ListenerLogLevel _logLevel;
        public event OnReceiveRequest OnReceiveRequestEvent;

        public Listener(ListenerLogLevel logLevel = ListenerLogLevel.Info)
        {
            _listener = new HttpListener();
            _listener.AuthenticationSchemes = AuthenticationSchemes.Anonymous;
            _logLevel = logLevel;
        }

        /// <summary>
        /// Start the listener
        /// </summary>
        /// <returns>Returns false if no listening address was added.</returns>
        public bool Start()
        {
            if (_listener.Prefixes.Count == 0)
            {
                if (_logLevel >= ListenerLogLevel.Error)
                    Debug.LogError("The listener needs at least one address. Use AddListeningAddress to add one!");

                return false;
            }

            // Cleanup before start
            Stop();

            try
            {
                _listener.Start();
            }
            catch (SocketException ex)
            {
                Debug.LogError($"REST/HTTP Listener ports are already used!\n{string.Join(" | ", _listener.Prefixes)}");
                Debug.LogException(ex);
                return false;
            }

            _listenerThread = new Thread(ListenerLoop);
            _listenerThread.Start();

            if (_logLevel >= ListenerLogLevel.Info)
                Debug.Log("REST/HTTP Listener started");

            return true;
        }

        /// <summary>
        /// Add listening address to the listener
        /// </summary>
        /// <param name="port">Port on which the listener should listen. 1 - 49150</param>
        /// <param name="host">The hostname is optional. If none is specified, the server listens to all hostnames and network IPs</param>
        /// <returns>Returns true if the listening address is valid and was successfully added to the listener</returns>
        public bool AddListeningAddress(int port, string host = "*")
        {
            if (port <= 0 || port > 49150)
            {
                if (_logLevel >= ListenerLogLevel.Error)
                    Debug.LogError($"The port \"{port}\" is invalid. A port can be between 1-49150");

                return false;
            }

            string hostPortCombination = $"{host}:{port}";
            _listener.Prefixes.Add($"http://{hostPortCombination}/");


            return true;
        }

        /// <summary>
        /// Stop the listener
        /// </summary>
        public void Stop()
        {
            _listenerThread?.Abort();
            _listener?.Abort();
        }

        private void ListenerLoop()
        {
            while (true)
            {
                HandleIncomingConnections();
            }
        }

        private void HandleIncomingConnections()
        {
            // Will wait here until we hear from a connection
            HttpListenerContext context = _listener.GetContext();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;

            try
            {
                LogRequest(request);

                response.Headers.Add("ServerName", HttpConstants.ServerName);

                bool closeConnection = OnReceiveRequestEvent?.Invoke(context) ?? true;
                if (!closeConnection)
                    return;

                response.Close();
            }
            catch (Exception ex)
            {
                if (_logLevel >= ListenerLogLevel.Error)
                    Debug.LogException(ex);
            }
        }

        private void LogRequest(HttpListenerRequest request)
        {
            if (_logLevel < ListenerLogLevel.Info)
                return;

            string logRequest = $"[{request.HttpMethod}] {request.Url.LocalPath}";

            if (_logLevel < ListenerLogLevel.Verbos)
            {
                Debug.Log(logRequest);

                return;
            }

            logRequest += "\n";

            if (request.QueryString.AllKeys.Length > 0)
            {
                logRequest += "Query Parameters:";
                foreach (var key in request.QueryString.AllKeys)
                    logRequest += $"\t{key}: {request.QueryString.GetValues(key)[0]}\n";
            }


            if (request.ContentLength64 == 0)
            {
                Debug.Log(logRequest);

                return;
            }

            if (request.Headers["Content-Type"] == null)
            {
                logRequest += $"Content body of size {request.ContentLength64} bytes with unknown Content-Type";
                Debug.Log(logRequest);

                return;
            }

            logRequest += $"Content body of size {request.ContentLength64} bytes with Content-Type \"{request.Headers["Content-Type"]}\"";
        }

        public void Dispose()
        {
            _listener.Stop();
        }
    }

    public enum ListenerLogLevel : uint
    {
        None = 0,
        Error = 1,
        Warning = 2,
        Info = 3,
        Verbos = 4
    }

    public delegate bool OnReceiveRequest(HttpListenerContext context);
}
