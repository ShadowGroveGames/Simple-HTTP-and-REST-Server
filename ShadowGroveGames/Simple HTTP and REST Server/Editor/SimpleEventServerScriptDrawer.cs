using ShadowGroveGames.SimpleHttpAndRestServer.Scripts;
using UnityEditor;
using UnityEngine;

namespace ShadowGroveGames.SimpleHttpAndRestServer.Editor
{
    [CustomEditor(typeof(SimpleEventServerScript))]
    public class RealWeatherAndTimeEventsScriptEditor : UnityEditor.Editor
    {
        GUIStyle _labelWordWrapStyle;
        GUIStyle _smallLabelWordWrapStyle;

        public override void OnInspectorGUI()
        {
            PrepareGUIStyles();
            DrawHeaderImage();
            DrawNote();
            base.OnInspectorGUI();
        }

        void PrepareGUIStyles()
        {
            _labelWordWrapStyle = new GUIStyle(EditorStyles.boldLabel);
            _labelWordWrapStyle.wordWrap = true;
            _smallLabelWordWrapStyle = new GUIStyle(EditorStyles.label);
            _smallLabelWordWrapStyle.wordWrap = true;
        }

        void DrawHeaderImage()
        {
            Texture2D headerImage = (Texture2D)Resources.Load("simple-http-and-rest-server-banner", typeof(Texture2D));

            GUI.DrawTexture(new Rect(10, 10, headerImage.width, headerImage.height), headerImage, ScaleMode.ScaleToFit, true, headerImage.width / headerImage.height);
            EditorGUILayout.Space(headerImage.height + 10);
        }

        void DrawNote()
        {
#if (UNITY_2021_1_OR_NEWER)
            EditorGUILayout.BeginHorizontal();
            if (EditorGUILayout.LinkButton("For support you can join our ShadowGroveGames Discord"))
                Application.OpenURL("https://discord.shadow-grove.org/");
            EditorGUILayout.EndHorizontal();
#else
            EditorGUILayout.LabelField("For support you can join our ShadowGroveGames Discord:", _smallLabelWordWrapStyle);
            EditorGUILayout.LabelField("https://discord.shadow-grove.org/", _labelWordWrapStyle);
#endif
            EditorGUILayout.Space();
        }
    }

}
