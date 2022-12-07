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
    }

    public void Jump(float jumpPower, Vector2 force)
    {
        rb.AddForce(force * jumpPower, ForceMode2D.Impulse);
    }
}
