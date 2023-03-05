using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Server.Extensions;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Struct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Unity.Profiling;
using UnityEngine;

namespace ShadowGroveGames.SimpleHttpAndRestServer.Scripts
{
    public abstract class AbstractServerScript : MonoBehaviour
    {
        [Header("General Settings")]
        [SerializeField]
        protected List<ListeningAddress> _listeningAddresses = new List<ListeningAddress>();

        [SerializeField]
        protected ListenerLogLevel _logLevel = ListenerLogLevel.Info;

        protected Listener listener;

        public List<ListeningAddress> ListeningAddresses { get { return _listeningAddresses; } }

        /// <summary>
        /// The processRequestQueue contains open requests that have not yet been processed.
        /// </summary>
        private Queue<HttpListenerContext> _processRequestQueue = new Queue<HttpListenerContext>();

        static readonly ProfilerMarker _processReceivedRequestPerfMarker = new ProfilerMarker("SimpleHttpAndRestServer.ProcessReceivedRequest");

        protected virtual void OnEnable()
        {

            listener = new Listener(_logLevel);
            foreach (var listeningAddress in _listeningAddresses)
                listener.AddListeningAddress(listeningAddress.Port);

            listener.OnReceiveRequestEvent += OnReceiveRequestEvent;
        }

        protected virtual void OnDisable()
        {
            _processRequestQueue.Clear();
            listener.OnReceiveRequestEvent -= OnReceiveRequestEvent;
            listener.Stop();
        }

        protected bool OnReceiveRequestEvent(HttpListenerContext context)
        {
            _processRequestQueue.Enqueue(context);

            return false;
        }

        /// <summary>
        /// In order to process the requests that have been accepted in the Async mode, we need to process them here one by one
        /// </summary>
        protected void FixedUpdate()
        {
            if (_processRequestQueue.Count == 0)
                return;

            var context = _processRequestQueue.Dequeue();

            using (_processReceivedRequestPerfMarker.Auto())
            {
                try
                {
                    if (!ValidateConnection(context))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        context.Response.Close();
                        return;
                    }

                    ProcessReceivedRequest(context);
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.TextResponse($"{ex.Message}\n\nStacktrace:\n{ex.StackTrace}");
                }

                context.Response.Close();
            }
        }

        protected virtual bool ValidateConnection(HttpListenerContext context)
        {
            var requestOverListeningAddress = _listeningAddresses.FirstOrDefault(a => a.Port == context.Request.LocalEndPoint.Port);

            if (requestOverListeningAddress != null && requestOverListeningAddress.Host == ListeningAddressType.Any)
                return true;

            if (!IPAddress.IsLoopback(context.Request.RemoteEndPoint.Address))
                return false;

            return true;
        }


        /// <summary>
        /// Our default response is 404 not found
        /// </summary>
        protected virtual void ProcessReceivedRequest(HttpListenerContext context)
        {
            context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            context.Response.Close();
        }
    }
}