using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class DGRotate : DGUtilis
{
    public Vector3 rotateTo;
    public RotateMode rotateMode = RotateMode.Fast;
    public override void Play()
    {
        transform.DORotate(rotateTo, duration, rotateMode).OnComplete(() => onComplete?.Invoke()).SetLoops(loops, loopType);
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(DGRotate))]
public class DGRotateEditor : Editor
{
    private DGRotate dg;
    private void OnEnable()
    {
        dg = (DGRotate)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Copy base Rotation"))
            dg.rotateTo = dg.transform.rotation.eulerAngles;
    }
}
#endif
