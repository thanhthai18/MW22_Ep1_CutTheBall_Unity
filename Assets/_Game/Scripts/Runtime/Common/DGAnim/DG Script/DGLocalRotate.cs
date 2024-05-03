using DG.Tweening;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class DGLocalRotate : DGUtilis
{
    public Vector3 rotateTo;
    public RotateMode rotateMode = RotateMode.Fast;
    public override void Play()
    {
        transform.DOLocalRotate(rotateTo, duration, rotateMode).OnComplete(() => onComplete?.Invoke()).SetLoops(loops, loopType);
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DGLocalRotate))]
public class DGLocalRotateEditor : Editor
{
    private DGLocalRotate dg;
    private void OnEnable()
    {
        dg = (DGLocalRotate)target;
    }
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Copy base Rotation"))
            dg.rotateTo = dg.transform.rotation.eulerAngles;
    }
}
#endif
