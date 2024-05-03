using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class DGLocalMove : DGUtilis
{
    public Vector3 moveTo;
    public override void Play()
    {
        transform.DOLocalMove(moveTo, duration,snapping).OnComplete(() => onComplete?.Invoke()).SetLoops(loops, loopType); 
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DGLocalMove)),CanEditMultipleObjects]
public class DGLocalMoveEditor : Editor
{
    private Object[] dgs;
    private void OnEnable()
    {
        dgs = targets;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Copy base position"))
        {
            foreach (Object obj in dgs)
            {
                var tobj=obj as DGLocalMove;
                tobj.moveTo = tobj.transform.localPosition;
            }
        }    
    }
}
#endif
