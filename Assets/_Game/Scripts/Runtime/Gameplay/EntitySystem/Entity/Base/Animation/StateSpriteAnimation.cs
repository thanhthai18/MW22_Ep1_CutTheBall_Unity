using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Runtime.Gameplay.EntitySystem
{
    [Serializable]
    public class StateSpriteAnimation
    {
        #region Members

        public string spriteAnimationName;
        public EntityAnimationState state;
        public bool isLoop;

        public bool haveEvent;
        [ShowIf(nameof(haveEvent))]
        public int frameTriggeredEvent;
        [ShowIf(nameof(haveEvent))]
        public Transform[] spawnPointsTransform;

        #endregion Members
    }
}