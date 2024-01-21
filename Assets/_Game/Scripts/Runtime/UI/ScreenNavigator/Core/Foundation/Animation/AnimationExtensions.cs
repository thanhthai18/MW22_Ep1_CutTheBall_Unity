﻿using System.Collections;
using UnityEngine;

namespace UnityScreenNavigator.Runtime.Foundation.Animation
{
    internal static class AnimationExtensions
    {
        public static IEnumerator CreatePlayRoutine(this IAnimation self)
        {
            var player = Pool<AnimationPlayer>.Shared.Rent();
            player.Initialize(self);

            UpdateDispatcher.Instance.Register(player);
            player.Play();

            yield return new WaitUntil(() => player.IsFinished);

            UpdateDispatcher.Instance.Unregister(player);
        }
    }
}