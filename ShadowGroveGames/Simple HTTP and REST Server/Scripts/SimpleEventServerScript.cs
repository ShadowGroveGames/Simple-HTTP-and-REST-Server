using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Control;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using UnityEngine;

namespace ShadowGroveGames.SimpleHttpAndRestServer.Scripts
{
    public class SimpleEventServerScript : AbstractServerScript
    {
        [Header("Events")]
        public OnReceiveRequestEvent OnReceiveRequest = new OnReceiveRequestEvent();

        protected override void OnEnable()
        {
            base.OnEnable();

            listener.Start();
        }

        protected override void OnDisable()
        {
            base.OnDisable();
        }

        protected void OnDestroy()
        {
            base.OnDisable();
        }

        protected override void ProcessReceivedRequest(HttpListenerContext context)
        {
            int eventCount = OnReceiveRequest?.GetPersistentEventCount() ?? 0;
            if (eventCount == 0)
            {
                base.ProcessReceivedRequest(context);
                return;
            }

            var matchedRoutes = new HashSet<string>();

            for (int index = 0; index < eventCount; index++)
            {
                #region Validate target
                var targetObject = OnReceiveRequest.GetPersistentTarget(index);
                if (targetObject == null)
                    continue;

                string targetMethodName = OnReceiveRequest.GetPersistentMethodName(index);
                if (targetMethodName == null)
                    continue;

                var targetObjectType = targetObject.GetType();
                if (targetObjectType == null)
                    continue;

                var targetObjectMethodType = targetObjectType.GetMethod(targetMethodName);
                if (targetObjectMethodType == null)
                    continue;
                #endregion

                #region Get and validate SimpleEventServerRoutingAttribute
                SimpleEventServerRoutingAttribute simpleRouting = targetObjectMethodType.GetCustomAttributes(true).OfType<SimpleEventServerRoutingAttribute>().FirstOrDefault();
                if (simpleRouting == null)
                {
                    Debug.LogError($"Method \"{targetMethodName}\" in Listener \"{targetObjectType.FullName}\" missing SimpleEventServerRouting attribute!");
                    base.ProcessReceivedRequest(context);
                    return;
                }
                #endregion

                if (context.Request.HttpMethod != simpleRouting.Method)
                    continue;

                if (context.Request.Url.AbsolutePath.TrimStart('/') != simpleRouting.Route.Trim('/'))
                    continue;

                string routeKey = $"{context.Request.HttpMethod}-{context.Request.Url.AbsolutePath}";
                if (matchedRoutes.Contains(routeKey))
                {
                    Debug.LogError($"On the route \"{context.Request.Url.AbsolutePath}\" with the method \"{context.Request.HttpMethod}\" is more than one listener! More than one listener is not allowed.");
                    continue;
                }

                matchedRoutes.Add($"{context.Request.HttpMethod}-{context.Request.Url.AbsolutePath}");
                targetObjectMethodType.Invoke(targetObject, new object[1] { context });
            }

            if (matchedRoutes.Count == 0)
                base.ProcessReceivedRequest(context);
        }
    }

    [Serializable]
    public class OnReceiveRequestEvent : SimpleEventServerEndpoints<HttpListenerContext> { }
}