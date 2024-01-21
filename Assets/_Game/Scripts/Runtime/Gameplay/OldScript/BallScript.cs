using Cysharp.Threading.Tasks;
using DG.Tweening;
using Runtime.AssetLoader;
using Runtime.Definition;
using Runtime.Message;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallScript : MonoBehaviour
{
    // private Rigidbody2D rb;
    // private bool isHit;
    // [SerializeField] private HitObjectType _hitObjectType;
    // [SerializeField] private int _scoreAddValue;
    // private SpriteRenderer _selfSpriteRenderer;
    // public static event Action<int> ActionOnHit;
    //
    //
    // public void InitBallScript(HitObjectType type)
    // {
    //     _hitObjectType = type;
    //     _selfSpriteRenderer = GetComponent<SpriteRenderer>();
    //     LoadDataSpriteAssets().Forget();
    //     if (_hitObjectType == HitObjectType.Ball)
    //     {
    //         _scoreAddValue = 1;
    //     }
    //     else if (_hitObjectType == HitObjectType.Boom)
    //     {
    //         _scoreAddValue = -1;
    //     }
    //     isHit = false;
    //     rb = GetComponent<Rigidbody2D>();
    //     Jump(Random.Range(15, 20), new Vector2(Random.Range(-0.3f, 0.3f), 1));
    //     transform.DORotate(new Vector3(0, 0, 360), 2, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1, LoopType.Restart);
    // }
    //
    // public void Jump(float jumpPower, Vector2 force)
    // {
    //     rb.AddForce(force * jumpPower, ForceMode2D.Impulse);
    // }
    // public void OnHit()
    // {
    //     if (!isHit)
    //     {
    //         isHit = true;
    //         _selfSpriteRenderer.color = Color.red;
    //         Messenger.Publish(new HitMessage(_hitObjectType, _scoreAddValue));
    //     }
    // }
    //
    // private async UniTask LoadDataSpriteAssets()
    // {
    //     _selfSpriteRenderer.sprite = await SpriteAssetsLoader.LoadAsset($"{SpriteAtlasKey.SPRITE_COMMON_ATLAS}[{_hitObjectType}]");
    // }
    // private void OnDestroy()
    // {
    //     transform.DOKill();
    // }
}
