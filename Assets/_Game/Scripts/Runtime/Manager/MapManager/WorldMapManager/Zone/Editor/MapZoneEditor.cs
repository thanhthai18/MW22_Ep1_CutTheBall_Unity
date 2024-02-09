#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

namespace Runtime.Gameplay.Map
{
    [CustomEditor(typeof(MapZone))]
    public class MapZoneEditor : Editor
    {
        #region Members

        private static readonly int s_headerTextSize = 15;
        private static readonly int s_titleTextSize = 13;
        private MapZone _target;
        private bool _isShiftPress;
        private bool _isControlPress;

        #endregion Members

        #region API Methods

        protected virtual void OnEnable()
            => _target = target as MapZone;

        #endregion API Methods

        #region Class Methods

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            EditorGUILayout.BeginVertical(MapZoneStyleEditor.GetBoxWindow(Color.black, 0.4f));
            DrawHeaderTittle();
            DrawButtons();
            DrawGizmosSection();
            DrawSections();
            DrawShortCuts();
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }

        protected virtual void DrawHeaderTittle()
        {
            var tittleStyle = MapZoneStyleEditor.GetStyle(Color.white, TextAnchor.MiddleCenter, s_headerTextSize, FontStyle.Bold);
            EditorGUILayout.LabelField("--- Map Zone ---", tittleStyle);
        }

        protected virtual void DrawButtons()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            var tittleStyle = MapZoneStyleEditor.GetStyle(Color.white, TextAnchor.MiddleCenter, s_titleTextSize);
            EditorGUILayout.LabelField("- Control Buttons -", tittleStyle);
            if (GUILayout.Button("Add Section"))
            {
                NewSection();
            }
            if (GUILayout.Button("Clear Zone"))
            {
                _target.ClearPath();
                EditorUtility.SetDirty(_target);
            }
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawGizmosSection()
        {
            var subtittleStyle = MapZoneStyleEditor.GetStyle(Color.white, TextAnchor.MiddleCenter, s_titleTextSize);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("- Area Visual -", subtittleStyle);
            EditorGUILayout.LabelField("Area Id: " + _target.MapAreaId);
            _target.confinedAreaType = (ConfinedAreaType)EditorGUILayout.EnumPopup("Area Type", _target.confinedAreaType);
            _target.zoneId = EditorGUILayout.TextField("Zone Id: ", _target.zoneId);
            _target.zoneId = !string.IsNullOrEmpty(_target.zoneId) ? _target.zoneId.Trim() : _target.zoneId;
            _target.distanceBetweenObjects = Mathf.Max(0.1f, EditorGUILayout.FloatField("Distance Between Objects: ", _target.distanceBetweenObjects));
            _target.objectVisualLength = Mathf.Max(0.1f, EditorGUILayout.FloatField("Object Visual Radius: ", _target.objectVisualLength));
            _target.minTestNumberSpawnedObjects = Mathf.Max(0, EditorGUILayout.IntField("{Editor Test} - Min Spawned Objects: ", _target.minTestNumberSpawnedObjects));
            _target.maxTestNumberSpawnedObjects = Mathf.Max(0, EditorGUILayout.IntField("{Editor Test} - Max Spawned Objects: ", _target.maxTestNumberSpawnedObjects));

            var otherText = "Show Points Visual";
            if (_target.alwaysShowAreaPointVisual)
                otherText = "Hide Points Visual";
            if (GUILayout.Button(otherText))
                _target.alwaysShowAreaPointVisual = !_target.alwaysShowAreaPointVisual;

            if (GUI.changed)
            {
                EditorUtility.SetDirty(target);
                SceneView.RepaintAll();
            }

            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawSections()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            var subtittleStyle = MapZoneStyleEditor.GetStyle(Color.white, TextAnchor.MiddleCenter, s_titleTextSize);
            EditorGUILayout.LabelField("- Sections -", subtittleStyle);

            var buttonStyle = MapZoneStyleEditor.GetButtonStyle(Color.black);

            var text = "Show Sections";
            if (_target.showSections)
                text = "Hide Sections";

            if (GUILayout.Button(text + ": " + _target.sections.Count))
                _target.showSections = !_target.showSections;

            if (_target.showSections)
            {
                for (int i = 0; i < _target.sections.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button("Section [" + i + "]", buttonStyle))
                        _target.sections[i].showPoint = !_target.sections[i].showPoint;
                    if (GUILayout.Button("Delete", buttonStyle))
                    {
                        _target.sections.RemoveAt(i);
                        return;
                    }
                    EditorGUILayout.EndHorizontal();
                    Repaint();
                    if (_target.sections[i].showPoint)
                    {
                        EditorGUILayout.BeginVertical(EditorStyles.helpBox);
                        _target.sections[i].point = EditorGUILayout.Vector3Field("Center: ", _target.sections[i].point);
                        _target.sections[i].limitUp = EditorGUILayout.Vector3Field("Up: ", _target.sections[i].limitUp);
                        _target.sections[i].limitDown = EditorGUILayout.Vector3Field("Down: ", _target.sections[i].limitDown);

                        if (GUI.changed)
                        {
                            EditorUtility.SetDirty(target);
                            SceneView.RepaintAll();
                        }

                        EditorGUILayout.EndVertical();
                    }
                }
            }
            EditorGUILayout.EndVertical();
        }

        protected virtual void DrawShortCuts()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            var tittleStyle = MapZoneStyleEditor.GetStyle(Color.white, TextAnchor.MiddleCenter, s_titleTextSize);
            EditorGUILayout.LabelField("- Shortcuts -", tittleStyle);
            EditorGUILayout.LabelField("Shift: Keep Y position");
            EditorGUILayout.EndVertical();
        }

        private void NewSection()
        {
            bool isFristPoint = false;
            var newSection = new MapZoneSection();
            var newPos = _target.transform.position;
            if (_target.sections.Count > 0)
            {
                newPos = _target.sections[_target.sections.Count - 1].point + new Vector3(5, 0);
                var posUp = _target.sections[_target.sections.Count - 1].limitUp + new Vector3(5, 0);
                var posDown = _target.sections[_target.sections.Count - 1].limitDown + new Vector3(5, 0);

                newSection.point = newPos;
                newSection.limitUp = posUp;
                newSection.limitDown = posDown;
            }
            else
            {
                newSection.point = newPos;
                newSection.limitUp = newPos + new Vector3(0, 5);
                newSection.limitDown = newPos + new Vector3(0, -5);
                isFristPoint = true;
            }

            _target.sections.Add(newSection);

            EditorUtility.SetDirty(_target);
            if (isFristPoint)
                NewSection();
        }

        protected virtual void OnSceneGUI()
        {
            Tools.current = UnityEditor.Tool.None;
            CheckInput();

            // Edit path points.
            for (int i = 0; i < _target.sections.Count; i++)
            {
                EditorGUI.BeginChangeCheck();
                var prevY = _target.sections[i].point.y;
                Handles.color = Color.green;
                var newTargetPosition = Handles.PositionHandle(_target.sections[i].point, Quaternion.identity);
                if (_isShiftPress)
                    newTargetPosition.y = prevY;
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_target, "Change Look At Target Position");
                    _target.sections[i].point = newTargetPosition;
                    if (_isControlPress)
                        _target.UpdateLimitUpDownToCenter(i);
                    EditorUtility.SetDirty(_target);
                }
            }

            // Edit path up points.
            for (int i = 0; i < _target.sections.Count; i++)
            {
                EditorGUI.BeginChangeCheck();
                var prevY = _target.sections[i].limitUp.y;
                Handles.color = Color.blue;
                var newTargetPosition = Handles.PositionHandle(_target.sections[i].limitUp, Quaternion.identity);

                if (_isShiftPress)
                    newTargetPosition.y = prevY;
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_target, "Change Look At Target Position");
                    _target.sections[i].limitUp = newTargetPosition;
                    EditorUtility.SetDirty(_target);
                }
            }

            // Edit path down points.
            for (int i = 0; i < _target.sections.Count; i++)
            {
                EditorGUI.BeginChangeCheck();
                var prevY = _target.sections[i].limitDown.y;
                Handles.color = Color.blue;
                var newTargetPosition = Handles.PositionHandle(_target.sections[i].limitDown, Quaternion.identity);

                if (_isShiftPress)
                    newTargetPosition.y = prevY;
                if (EditorGUI.EndChangeCheck())
                {
                    Undo.RecordObject(_target, "Change Look At Target Position");
                    _target.sections[i].limitDown = newTargetPosition;
                }
            }
        }

        private void CheckInput()
        {
            Event e = Event.current;
            switch (e.type)
            {
                case EventType.KeyDown:
                {
                    if (Event.current.keyCode == (KeyCode.LeftShift))
                    {
                        _isShiftPress = true;
                        e.Use();
                    }
                    if (Event.current.keyCode == (KeyCode.LeftControl))
                    {
                        _isControlPress = true;
                        e.Use();
                    }
                    break;
                }
                case EventType.KeyUp:
                {
                    if (Event.current.keyCode == (KeyCode.LeftShift))
                    {
                        _isShiftPress = false;
                        e.Use();
                    }
                    if (Event.current.keyCode == (KeyCode.LeftControl))
                    {
                        _isControlPress = false;
                        e.Use();
                    }
                    break;
                }
            }
        }

        #endregion Class Methods
    }
}

#endif