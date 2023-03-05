using ShadowGroveGames.SimpleHttpAndRestServer.Scripts;
using ShadowGroveGames.SimpleHttpAndRestServer.Scripts.Control;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ShadowGroveGames.SimpleHttpAndRestServer.Editor
{
    [CustomPropertyDrawer(typeof(SimpleEventServerEndpoints<HttpListenerContext>), true)]
    public class SimpleEventServerEndpointsDrawer : UnityEventDrawer
    {
        private static FieldInfo _listenersArrayListFieldInfo;
        private SerializedProperty _listenersArray;
        private HashSet<string> _usedRoutes = new HashSet<string>();

        private Color _colorDarkGreen = new Color(0.1f, 0.5f, 0.12f);
        private Color _colorDarkRed = new Color(0.59f, 0.01f, 0.08f);
        private GUIStyle _labelStyleBold;

        void PrepareGUIStyles()
        {
            _labelStyleBold = new GUIStyle(EditorStyles.boldLabel);
            _labelStyleBold.normal.textColor = Color.white;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            PrepareGUIStyles();
            base.OnGUI(position, property, label);
            _usedRoutes.Clear();

            _listenersArray = GetListenersArray();
        }

        protected override void DrawEvent(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (_listenersArray == null)
            {
                base.DrawEvent(rect, index, isActive, isFocused);
                return;
            }

            base.DrawEvent(rect, index, isActive, isFocused);
            Rect[] rowRects = GetRowRects(rect);
            Rect positionBackground = rowRects[3];
            Rect positionText = rowRects[3];
            positionText.x += 5f;

            SerializedProperty arrayElementAtIndex = _listenersArray.GetArrayElementAtIndex(index);
            SerializedProperty serializedProperty2 = arrayElementAtIndex.FindPropertyRelative("m_Target");
            SerializedProperty serializedProperty3 = arrayElementAtIndex.FindPropertyRelative("m_MethodName");


            if (serializedProperty2.objectReferenceValue == null || string.IsNullOrEmpty(serializedProperty3.stringValue))
            {
                EditorGUI.DrawRect(positionBackground, _colorDarkRed);
                EditorGUI.LabelField(positionText, "⚠️ Please select an object and a function!", _labelStyleBold);
                return;
            }

            var targetObjectType = serializedProperty2.objectReferenceValue.GetType();
            if (targetObjectType == null)
            {
                EditorGUI.DrawRect(positionBackground, _colorDarkRed);
                EditorGUI.LabelField(positionText, "⚠️ Selected object not found!", _labelStyleBold);
                return;
            }

            var targetObjectMethodType = targetObjectType.GetMethod(serializedProperty3.stringValue);
            if (targetObjectMethodType == null)
            {
                EditorGUI.DrawRect(positionBackground, _colorDarkRed);
                EditorGUI.LabelField(positionText, "⚠️ Selected function not found!", _labelStyleBold);
                return;
            }

            SimpleEventServerRoutingAttribute simpleRouting = targetObjectMethodType.GetCustomAttributes(true).OfType<SimpleEventServerRoutingAttribute>().FirstOrDefault();
            if (simpleRouting == null)
            {
                EditorGUI.DrawRect(positionBackground, _colorDarkRed);
                EditorGUI.LabelField(positionText, "⚠️ Missing SimpleEventServerRouting attribute!", _labelStyleBold);
                return;
            }

            string routeKey = $"{simpleRouting.Method}#{simpleRouting.Route}";
            if (_usedRoutes.Contains(routeKey))
            {
                EditorGUI.DrawRect(positionBackground, _colorDarkRed);
                EditorGUI.LabelField(positionText, "⚠️ This function is already registered!", _labelStyleBold);
                return;
            }

            EditorGUI.DrawRect(positionBackground, _colorDarkGreen);
            EditorGUI.LabelField(positionText, $"Method: {simpleRouting.Method} | Route: {simpleRouting.Route}", _labelStyleBold);
            _usedRoutes.Add(routeKey);
        }

        private Rect[] GetRowRects(Rect rect)
        {
            Rect[] array = new Rect[4];
            rect.height = 18f;
            rect.y += 2f;
            Rect rect2 = rect;
            rect2.width *= 0.3f;
            Rect rect3 = rect2;
            rect3.y += EditorGUIUtility.singleLineHeight + (float)3;
            Rect rect4 = rect;
            rect4.xMin = rect3.xMax + 5f;
            Rect rect5 = rect4;
            rect5.y += EditorGUIUtility.singleLineHeight + (float)3;
            array[0] = rect2;
            array[1] = rect3;
            array[2] = rect4;
            array[3] = rect5;

            return array;
        }

        /// <summary>
		/// Gets the internal instance of the ListenersArray
        /// </summary>
        private SerializedProperty GetListenersArray()
        {
            if (_listenersArrayListFieldInfo == null)
                _listenersArrayListFieldInfo = typeof(UnityEventDrawer).GetField("m_ListenersArray", BindingFlags.NonPublic | BindingFlags.Instance);

            return (SerializedProperty)_listenersArrayListFieldInfo.GetValue(this);
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }
    }
}
