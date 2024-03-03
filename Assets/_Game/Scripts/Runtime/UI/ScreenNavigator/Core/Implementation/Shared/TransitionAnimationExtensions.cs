using System;
using System.Collections;
using UnityScreenNavigator.Runtime.Foundation;
using UnityScreenNavigator.Runtime.Foundation.Animation;

namespace UnityScreenNavigator.Runtime.Core.Shared
{
    internal static class TransitionAnimationExtensions
    {
        public static IEnumerator CreatePlayRoutine(this ITransitionAnimation self, IProgress<float> progress = null)
        {
            var player = Pool<AnimationPlayer>.Shared.Rent();
            player.Initialize(self);

            UpdateDispatcher.Instance.Register(player);
            progress?.Report(0.0f);
            player.Play();

            while (!player.IsFinished)
            {
                yield return null;
                progress?.Report(player.Time / self.Duration);
            }

            UpdateDispatcher.Instance.Unregister(player);
        }
    }
}