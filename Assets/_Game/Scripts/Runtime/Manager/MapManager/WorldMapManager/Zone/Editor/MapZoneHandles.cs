#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Runtime.Gameplay.Map
{
    public class MapZoneHandlesFreeCircle
    {
        #region Members

        public enum DragHandleResult
        {
            none = 0,

            LMBPress,
            LMBClick,
            LMBDoubleClick,
            LMBDrag,
            LMBRelease,

            RMBPress,
            RMBClick,
            RMBDoubleClick,
            RMBDrag,
            RMBRelease,
        };

        private static int s_dragHandleHash = "DragHandleHash".GetHashCode();
        private static Vector2 s_dragHandleMouseStart;
        private static Vector2 s_dragHandleMouseCurrent;
        private static Vector3 s_dragHandleWorldStart;
        private static float s_dragHandleClickTime = 0;
        private static int s_dragHandleClickID;
        private static float s_dragHandleDoubleClickInterval = 0.5f;
        private static bool s_dragHandleHasMoved;

        #endregion Members

        #region Class Methods

        public static Vector3 DragHandle(Vector3 position, float handleSize, Handles.CapFunction capFunc, Color colorBase, Color colorSelected, out DragHandleResult result)
        {
            int id = GUIUtility.GetControlID(s_dragHandleHash, FocusType.Passive);
            Vector3 screenPosition = Handles.matrix.MultiplyPoint(position);
            Matrix4x4 cachedMatrix = Handles.matrix;

            result = DragHandleResult.none;

            switch (Event.current.GetTypeForControl(id))
            {
                case EventType.MouseDown:
                    if (HandleUtility.nearestControl == id && (Event.current.button == 0 || Event.current.button == 1))
                    {
                        GUIUtility.hotControl = id;
                        s_dragHandleMouseCurrent = s_dragHandleMouseStart = Event.current.mousePosition;
                        s_dragHandleWorldStart = position;
                        s_dragHandleHasMoved = false;

                        Event.current.Use();
                        EditorGUIUtility.SetWantsMouseJumping(1);

                        if (Event.current.button == 0)
                            result = DragHandleResult.LMBPress;
                        else if (Event.current.button == 1)
                            result = DragHandleResult.RMBPress;
                    }
                    break;

                case EventType.MouseUp:
                    if (GUIUtility.hotControl == id && (Event.current.button == 0 || Event.current.button == 1))
                    {
                        GUIUtility.hotControl = 0;
                        Event.current.Use();
                        EditorGUIUtility.SetWantsMouseJumping(0);

                        if (Event.current.button == 0)
                            result = DragHandleResult.LMBRelease;
                        else if (Event.current.button == 1)
                            result = DragHandleResult.RMBRelease;

                        if (Event.current.mousePosition == s_dragHandleMouseStart)
                        {
                            bool doubleClick = (s_dragHandleClickID == id) &&
                                (Time.realtimeSinceStartup - s_dragHandleClickTime < s_dragHandleDoubleClickInterval);

                            s_dragHandleClickID = id;
                            s_dragHandleClickTime = Time.realtimeSinceStartup;

                            if (Event.current.button == 0)
                                result = doubleClick ? DragHandleResult.LMBDoubleClick : DragHandleResult.LMBClick;
                            else if (Event.current.button == 1)
                                result = doubleClick ? DragHandleResult.RMBDoubleClick : DragHandleResult.RMBClick;
                        }
                    }
                    break;

                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == id)
                    {
                        if (Event.current.button == 0)
                            result = DragHandleResult.LMBDrag;
                        else if (Event.current.button == 1)
                            result = DragHandleResult.RMBDrag;
                        s_dragHandleMouseCurrent += new Vector2(Event.current.delta.x, -Event.current.delta.y);
                        Vector3 position2 = Camera.current.WorldToScreenPoint(Handles.matrix.MultiplyPoint(s_dragHandleWorldStart))
                            + (Vector3)(s_dragHandleMouseCurrent - s_dragHandleMouseStart);
                        position = Handles.matrix.inverse.MultiplyPoint(Camera.current.ScreenToWorldPoint(position2));

                        if (Camera.current.transform.forward == Vector3.forward || Camera.current.transform.forward == -Vector3.forward)
                            position.z = s_dragHandleWorldStart.z;
                        if (Camera.current.transform.forward == Vector3.up || Camera.current.transform.forward == -Vector3.up)
                            position.y = s_dragHandleWorldStart.y;
                        if (Camera.current.transform.forward == Vector3.right || Camera.current.transform.forward == -Vector3.right)
                            position.x = s_dragHandleWorldStart.x;

                        s_dragHandleHasMoved = true;

                        GUI.changed = true;
                        Event.current.Use();
                    }
                    break;

                case EventType.Repaint:
                    Color currentColour = colorBase;
                    if (id == GUIUtility.hotControl && s_dragHandleHasMoved)
                        Handles.color = colorSelected;

                    Handles.matrix = Matrix4x4.identity;
                    capFunc(id, screenPosition, Quaternion.identity, handleSize, Event.current.GetTypeForControl(id));
                    Handles.matrix = cachedMatrix;
                    Handles.color = currentColour;
                    break;

                case EventType.Layout:
                    Handles.matrix = Matrix4x4.identity;
                    HandleUtility.AddControl(id, HandleUtility.DistanceToCircle(screenPosition, handleSize));
                    Handles.matrix = cachedMatrix;
                    break;
            }

            return position;
        }

        #endregion Class Methods
    }
}
#endif