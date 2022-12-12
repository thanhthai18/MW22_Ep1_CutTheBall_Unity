using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayContrl : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private int life;
    private Vector2 mouseCurrentPos;
    private RaycastHit2D[] hit;
    private bool isStart;
    private bool isLose;
    private Camera mainCamera;
    [SerializeField] private MouseTrail mouseTrail;
    public bool isHoldMouse;
    private Coroutine spawnBallCoroutine;

    public int Score
    {
        get => score; set { if (value >= 0) score = value; }
    }
    public int Life
    {
        get => life; set { if (value >= 0 && value <= 2) life = value; }
    }

    private void Awake()
    {
        isStart = false;
        isLose = false;
        isHoldMouse = false;
        GameController.eventStartGame += Begin;
    }

    public void Begin()
    {
        mainCamera = GameController.instance.mainCamera;
        Score = 0;
        Life = 3;
        spawnBallCoroutine = StartCoroutine(AutoSpawnBall());
        isStart = true;
    }

    public void Lose()
    {
        isLose = true;
        StopCoroutine(spawnBallCoroutine);
    }

    public IEnumerator AutoSpawnBall()
    {
        while (!isLose)
        {
            BallController.instance.SpawnBall(BallController.instance.GetPosSpawn());
            yield return new WaitForSeconds(1);
        }
    }

    private void Update()
    {
        if (!isLose && isStart)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isHoldMouse = true;
            }
            if (isHoldMouse && mouseTrail.trail.positionCount > 1)
            {
                mouseCurrentPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                hit = Physics2D.RaycastAll(mouseCurrentPos, Vector2.zero);
                if (hit.Length != 0)
                {
                    for (int i = 0; i < hit.Length; i++)
                    {
                        if (hit[i].collider.gameObject.CompareTag("Ball"))
                        {
                            hit[i].collider.GetComponent<SpriteRenderer>().color = Color.red;
                            Score++;
                            Debug.Log("Score: " + Score);
                        }
                    }
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                isHoldMouse = false;
            }
        }
    }

    private void OnDisable()
    {
        GameController.eventStartGame -= Begin;
    }
}
