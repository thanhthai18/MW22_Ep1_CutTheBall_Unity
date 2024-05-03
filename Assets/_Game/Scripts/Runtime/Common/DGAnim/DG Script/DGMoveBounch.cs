using DG.Tweening;
using UnityEngine;

public class DGMoveBounch : MonoBehaviour
{
    public bool playOnStart;
    public Renderer render;
    public enum BounchType
    {
        Top,
        Left,
        Bottom,
        Right
    }

    public BounchType bounchType;

    private Camera mainCam;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Start()
    {
        if (playOnStart) Move();
    }

    public void Move()
    {
        gameObject.SetActive(true);
        float pos = 0f;
        switch (bounchType)
        {
            case BounchType.Top:
                pos = mainCam.transform.position.y + mainCam.orthographicSize - render.bounds.size.y / 2f;
                transform.DOMoveY(pos, 1);
                break;
            case BounchType.Left:
                pos = mainCam.transform.position.x + mainCam.orthographicSize * mainCam.aspect - render.bounds.size.x / 2f;
                transform.DOMoveX(pos, 1);
                break;
            case BounchType.Bottom:
                pos = mainCam.transform.position.y - mainCam.orthographicSize + render.bounds.size.y / 2f;
                transform.DOMoveY(pos, 1);
                break;
            case BounchType.Right:
                pos = mainCam.transform.position.x - mainCam.orthographicSize * mainCam.aspect + render.bounds.size.x / 2f;
                transform.DOMoveX(pos, 1);
                break;
        }
    }
}
