using UnityEngine;
using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DGMove : DGUtilis
{
    public Vector3 moveTo;
    public override void Play()
    {
        transform.DOMove(moveTo,duration,snapping).OnComplete(()=>onComplete?.Invoke()).SetLoops(loops,loopType);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DGMove))]
public class DGMoveEditor : Editor
{
    private DGMove dg;
    private void OnEnable()
    {
        dg= (DGMove)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Copy base position"))
            dg.moveTo = dg.transform.position;
    }
}
#endif
