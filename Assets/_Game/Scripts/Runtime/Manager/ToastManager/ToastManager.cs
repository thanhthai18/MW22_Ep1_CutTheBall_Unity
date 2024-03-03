using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Runtime.Common.Singleton;
using Runtime.Definition;
using Runtime.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using static Runtime.Manager.Toast.ToastManager;

namespace Runtime.Manager.Toast
{
    public class ToastManager : MonoSingleton<ToastManager>
    {
        #region Members

        [SerializeField]
        private Transform _rewardPosition;
        [SerializeField]
        private Transform _textPosition;
        [SerializeField]
        private Transform _mapPosition;
        private Dictionary<ToastVisualType, ToastQueue> _toastQueueDictionary;

        #endregion Members

        #region API Methods

        protected override void Awake()
        {
            base.Awake();
            _toastQueueDictionary = new();
        }


        #endregion API Methods

        #region Class Methods


        public void Show(string toastContent, ToastVisualType visualToastType = ToastVisualType.Text)
            => LoadToastAsync(toastContent, visualToastType).Forget();
      
        private async UniTask LoadToastAsync(string toastContent, ToastVisualType visualToastType = ToastVisualType.Text)
        {
            if (visualToastType == ToastVisualType.Reward) return;
            var result = _toastQueueDictionary.TryGetValue(visualToastType, out var toastQueue);
            if (!result)
            {
                var prefabId = $"toast_{visualToastType.ToString().ToLower()}";
                var prefab = await Addressables.LoadAssetAsync<GameObject>(prefabId).WithCancellation(this.GetCancellationTokenOnDestroy());
                toastQueue = new ToastQueue(prefab.GetComponent<Toast>());
                _toastQueueDictionary.TryAdd(visualToastType, toastQueue);
            }

            var dequeueResult = toastQueue.queue.TryDequeue(out var toast);
            if (!dequeueResult)
                toast = Instantiate(toastQueue.prefab);

            Transform positionTransform;
            switch (visualToastType)
            {
                case ToastVisualType.Text:
                    positionTransform = _textPosition;
                    toast.Init(toastContent, positionTransform, ReturnPool);
                    break;
                case ToastVisualType.Map:
                    positionTransform = _mapPosition;
                    toast.Init(toastContent, positionTransform, ReturnPool);
                    break;
                default:
                    positionTransform = _textPosition;
                    break;
            }
        }
      

        private void ReturnPool(Toast toast)
        {
            var result = _toastQueueDictionary.TryGetValue(toast.ToastVisualType, out var toastQueue);
            if (result)
                toastQueue.queue.Enqueue(toast);
        }


        #endregion Class Methods

        #region Owner Classes

        private class ToastQueue
        {
            #region Members

            public Toast prefab;
            public Queue<Toast> queue;

            #endregion Members

            #region Class Methods

            public ToastQueue(Toast prefab)
            {
                this.prefab = prefab;
                queue = new Queue<Toast>();
            }

            #endregion Class Methods
        }

        public class ToastContent
        {
            public string content;
            public Sprite sprite;
            public int number;

            public ToastContent(string content, Sprite sprite, int number)
            {
                this.content = content;
                this.sprite = sprite;
                this.number = number;
            }
        }

        #endregion Owner Classes
    }
}