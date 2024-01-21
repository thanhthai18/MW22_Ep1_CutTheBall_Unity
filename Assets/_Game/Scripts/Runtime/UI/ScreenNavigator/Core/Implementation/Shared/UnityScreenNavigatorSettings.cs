﻿using System;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityScreenNavigator.Runtime.Core.Modals;
using UnityScreenNavigator.Runtime.Foundation.AssetLoaders;
#if UNITY_EDITOR
using UnityEditor;

#endif

namespace UnityScreenNavigator.Runtime.Core.Shared
{
    internal sealed class UnityScreenNavigatorSettings : ScriptableObject
    {
        private const string DefaultModalBackdropPrefabKey = "DefaultModalBackdrop";
        private static UnityScreenNavigatorSettings _instance;

        [SerializeField] private TransitionAnimationObject _sheetEnterAnimation;

        [SerializeField] private TransitionAnimationObject _sheetExitAnimation;

        [SerializeField] private TransitionAnimationObject _screenPushEnterAnimation;

        [SerializeField] private TransitionAnimationObject _screenPushExitAnimation;

        [SerializeField] private TransitionAnimationObject _screenPopEnterAnimation;

        [SerializeField] private TransitionAnimationObject _screenPopExitAnimation;

        [SerializeField] private TransitionAnimationObject _modalEnterAnimation;

        [SerializeField] private TransitionAnimationObject _modalExitAnimation;

        [SerializeField] private TransitionAnimationObject _modalBackdropEnterAnimation;

        [SerializeField] private TransitionAnimationObject _modalBackdropExitAnimation;

        [SerializeField] private TransitionAnimationObject _activityEnterAnimation;

        [SerializeField] private TransitionAnimationObject _activityExitAnimation;

        [SerializeField] private ModalBackdrop _modalBackdropPrefab;

        [SerializeField] private AssetLoaderObject _assetLoader;

        [SerializeField] private bool _enableInteractionInTransition;
        
        private IAssetLoader _defaultAssetLoader;
        private ModalBackdrop _defaultModalBackdrop;

        public ITransitionAnimation SheetEnterAnimation => _sheetEnterAnimation != null
            ? Instantiate(_sheetEnterAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(beforeAlpha: 0.0f, easeType: EaseType.Linear);

        public ITransitionAnimation SheetExitAnimation => _sheetExitAnimation != null
            ? Instantiate(_sheetExitAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(afterAlpha: 0.0f, easeType: EaseType.Linear);

        public ITransitionAnimation ScreenPushEnterAnimation => _screenPushEnterAnimation != null
            ? Instantiate(_screenPushEnterAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(beforeAlignment: SheetAlignment.Right,
                afterAlignment: SheetAlignment.Center);

        public ITransitionAnimation ScreenPushExitAnimation => _screenPushExitAnimation != null
            ? Instantiate(_screenPushExitAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(beforeAlignment: SheetAlignment.Center,
                afterAlignment: SheetAlignment.Left);

        public ITransitionAnimation ScreenPopEnterAnimation => _screenPopEnterAnimation != null
            ? Instantiate(_screenPopEnterAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(beforeAlignment: SheetAlignment.Left,
                afterAlignment: SheetAlignment.Center);

        public ITransitionAnimation ScreenPopExitAnimation => _screenPopExitAnimation != null
            ? Instantiate(_screenPopExitAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(beforeAlignment: SheetAlignment.Center,
                afterAlignment: SheetAlignment.Right);

        public ITransitionAnimation ModalEnterAnimation => _modalEnterAnimation != null
            ? Instantiate(_modalEnterAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(beforeScale: Vector3.one * 0.3f, beforeAlpha: 0.0f);

        public ITransitionAnimation ModalExitAnimation => _modalExitAnimation != null
            ? Instantiate(_modalExitAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(afterScale: Vector3.one * 0.3f, afterAlpha: 0.0f);

        public ITransitionAnimation ModalBackdropEnterAnimation => _modalBackdropEnterAnimation != null
            ? Instantiate(_modalBackdropEnterAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(beforeAlpha: 0.0f, easeType: EaseType.Linear);

        public ITransitionAnimation ModalBackdropExitAnimation => _modalBackdropExitAnimation != null
            ? Instantiate(_modalBackdropExitAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(afterAlpha: 0.0f, easeType: EaseType.Linear);

        public ITransitionAnimation ActivityEnterAnimation => _activityEnterAnimation != null
            ? Instantiate(_activityEnterAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(beforeScale: Vector3.one * 0.3f, beforeAlpha: 0.0f);

        public ITransitionAnimation ActivityExitAnimation => _activityExitAnimation != null
            ? Instantiate(_activityExitAnimation)
            : SimpleTransitionAnimationObject.CreateInstance(afterScale: Vector3.one * 0.3f, afterAlpha: 0.0f);

        public ModalBackdrop ModalBackdropPrefab
        {
            get
            {
                if (_modalBackdropPrefab != null)
                {
                    return _modalBackdropPrefab;
                }

                if (_defaultModalBackdrop == null)
                {
                    _defaultModalBackdrop = Resources.Load<ModalBackdrop>(DefaultModalBackdropPrefabKey);
                }

                return _defaultModalBackdrop;
            }
        }

        public IAssetLoader AssetLoader
        {
            get
            {
                if (_assetLoader != null)
                {
                    return _assetLoader;
                }
                if (_defaultAssetLoader == null)
                {
#if USN_USE_ADDRESSABLES
                    _defaultAssetLoader = CreateInstance<AddressableAssetLoaderObject>();
#else
                    _defaultAssetLoader = CreateInstance<ResourcesAssetLoaderObject>();
#endif
                }
                return _defaultAssetLoader;
            }
        }

        public bool EnableInteractionInTransition => _enableInteractionInTransition;

        public static UnityScreenNavigatorSettings Instance
        {
            get
            {
#if UNITY_EDITOR
                if (_instance == null)
                {
                    var asset = PlayerSettings.GetPreloadedAssets().OfType<UnityScreenNavigatorSettings>()
                        .FirstOrDefault();
                    _instance = asset != null ? asset : CreateInstance<UnityScreenNavigatorSettings>();
                }

                return _instance;

#else
                if (_instance == null)
                {
                    _instance = CreateInstance<UnityScreenNavigatorSettings>();
                }

                return _instance;
#endif
            }
            private set => _instance = value;
        }

        private void OnEnable()
        {
            _instance = this;
        }

        public ITransitionAnimation GetDefaultScreenTransitionAnimation(bool push, bool enter)
        {
            if (push)
            {
                return enter ? ScreenPushEnterAnimation : ScreenPushExitAnimation;
            }

            return enter ? ScreenPopEnterAnimation : ScreenPopExitAnimation;
        }

        public ITransitionAnimation GetDefaultModalTransitionAnimation(bool enter)
        {
            return enter ? ModalEnterAnimation : ModalExitAnimation;
        }

        public ITransitionAnimation GetDefaultSheetTransitionAnimation(bool enter)
        {
            return enter ? SheetEnterAnimation : SheetExitAnimation;
        }

        public ITransitionAnimation GetDefaultActivityTransitionAnimation(bool enter)
        {
            return enter ? ActivityEnterAnimation : ActivityExitAnimation;
        }

#if UNITY_EDITOR

        [MenuItem("Assets/Create/Screen Navigator/Screen Navigator Settings", priority = -1)]
        private static void Create()
        {
            var asset = PlayerSettings.GetPreloadedAssets().OfType<UnityScreenNavigatorSettings>().FirstOrDefault();
            if (asset != null)
            {
                var path = AssetDatabase.GetAssetPath(asset);
                throw new InvalidOperationException($"{nameof(UnityScreenNavigatorSettings)} already exists at {path}");
            }

            var assetPath = EditorUtility.SaveFilePanelInProject($"Save {nameof(UnityScreenNavigatorSettings)}",
                nameof(UnityScreenNavigatorSettings),
                "asset", "", "Assets");

            if (string.IsNullOrEmpty(assetPath))
            {
                // Return if canceled.
                return;
            }

            // Create folders if needed.
            var folderPath = Path.GetDirectoryName(assetPath);
            if (!string.IsNullOrEmpty(folderPath) && !Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            var instance = CreateInstance<UnityScreenNavigatorSettings>();
            AssetDatabase.CreateAsset(instance, assetPath);
            var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
            preloadedAssets.Add(instance);
            PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
            AssetDatabase.SaveAssets();
        }
#endif
    }

}