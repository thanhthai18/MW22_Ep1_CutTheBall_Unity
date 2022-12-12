using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        Jump(Random.Range(15, 20), new Vector2(Random.Range(-0.3f, 0.3f), 1));
        transform.DORotate(new Vector3(0, 0, 360), 2, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    }

    public void Jump(float jumpPower, Vector2 force)
    {
        rb.AddForce(force * jumpPower, ForceMode2D.Impulse);
    }
    private void OnDestroy()
    {
        transform.DOKill();
    }
}
