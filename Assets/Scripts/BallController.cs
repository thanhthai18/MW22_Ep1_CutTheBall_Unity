using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public static BallController instance;
    public BallScript ballPrefab;
    public List<BallScript> listBall = new List<BallScript>();


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance.gameObject);
    }
    public BallScript SpawnBall(Vector2 posSpawn)
    {
        BallScript ball = Instantiate(ballPrefab, posSpawn, Quaternion.identity);
        listBall.Add(ball);
        Destroy(ball.gameObject, 10);
        return ball;
    }

    public Vector2 GetPosSpawn()
    {
        float posX = Random.Range(-GlobalValue.sizeCamera.x * 0.8f / 2, GlobalValue.sizeCamera.x * 0.8f / 2);
        float posY = -GlobalValue.sizeCamera.y * 2 / 2;
        return new Vector2(posX, posY);
    }

}
