#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Runtime.Gameplay.Map
{
    public static class MapZoneStyleEditor
    {
        #region Class Methods

        public static GUIStyle GetStyle(Color color, TextAnchor align = TextAnchor.MiddleCenter, int size = 11, FontStyle st = FontStyle.Normal)
        {
            GUIStyle style = new GUIStyle(GUI.skin.label);
            style.normal.textColor = color;
            style.alignment = align;
            style.fontSize = size;
            style.fontStyle = st;
            style.richText = true;
            return style;
        }

        public static GUIStyle SetBox(this GUIStyle style, Color color, float alpha)
        {
            var bk = new Texture2D(1, 1);
            color.a = alpha;
            bk.SetPixel(0, 0, color);
            bk.Apply();
            style.normal.background = bk;
            style.stretchWidth = true;
            style.stretchHeight = true;
            return style;
        }

        public static GUIStyle GetBoxWindow(Color color, float alpha = 1)
        {
            GUIStyle styleBox = new GUIStyle(GUI.skin.box);
            styleBox.normal.background = GenerateTexture(color, alpha);
            return styleBox;
        }

        public static GUIStyle GetButtonStyle(Color color)
        {
            GUIStyle styleBox = new GUIStyle(GUI.skin.button);
            styleBox.richText = true;
            styleBox.normal.background = GenerateTexture(color, .5f);
            styleBox.hover.background = GenerateTexture(color, .7f);
            var activeCOlor = Color.Lerp(color, Color.black, 0.5f);
            styleBox.active.background = GenerateTexture(activeCOlor, .7f);
            return styleBox;
        }

        public static GUIStyle GetButtonStyle()
        {
            GUIStyle styleBox = new GUIStyle(GUI.skin.button);
            return styleBox;
        }

        public static GUIStyle GetCustomStyle(string style)
        {
            var path = AssetDatabase.FindAssets("CustomEditorSkin");
            var t = AssetDatabase.GUIDToAssetPath(path[0]);
            GUISkin styleBase = AssetDatabase.LoadAssetAtPath<GUISkin>(t);
            return styleBase.GetStyle(style);
        }

        public static Texture2D GenerateTexture(this Texture2D text, Color color, float alpha)
        {
            var bk = new Texture2D(1, 1);
            color.a = alpha;
            bk.SetPixel(0, 0, color);
            bk.Apply();
            return text;
        }

        public static Texture2D GenerateTexture(Color color, float alpha)
        {
            var bk = new Texture2D(1, 1);
            color.a = alpha;
            bk.SetPixel(0, 0, color);
            bk.Apply();
            return bk;
        }

        #endregion Class Methods
    }
}
#endif