#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Runtime.Gameplay.Map
{
    [CustomEditor(typeof(MapFog))]
    public class MapFogEditor : Editor
    {
        #region Members

        private static readonly int s_headerTextSize = 15;
        private MapFog _target;

        #endregion Members

        #region API Methods

        private void OnEnable()
            => _target = target as MapFog;

        #endregion API Methods

        #region Class Methods

        public override void OnInspectorGUI()
        {
            EditorGUILayout.BeginVertical(MapZoneStyleEditor.GetBoxWindow(Color.black, 0.4f));
            DrawHeaderTittle();
            DrawGizmosSection();
            EditorGUILayout.EndVertical();
        }

        private void DrawHeaderTittle()
        {
            var tittleStyle = MapZoneStyleEditor.GetStyle(Color.white, TextAnchor.MiddleCenter, s_headerTextSize, FontStyle.Bold);
            EditorGUILayout.LabelField("--- Map Fog --- ", tittleStyle);
        }

        private void DrawGizmosSection()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            _target.fogId = EditorGUILayout.TextField("Fog Id: ", _target.fogId);
            SceneView.RepaintAll();
            EditorGUILayout.EndVertical();
        }

        #endregion Class Methods
    }
}
#endif