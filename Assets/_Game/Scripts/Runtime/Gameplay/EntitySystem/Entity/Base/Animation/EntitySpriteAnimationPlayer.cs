using System.Linq;
using UnityEngine;
using Runtime.Animation;

namespace Runtime.Gameplay.EntitySystem
{
    public class EntitySpriteAnimationPlayer : MonoBehaviour, IEntityAnimationPlayer
    {
        #region Members

        [SerializeField]
        private SpriteAnimator _spriteAnimator;
        [SerializeField]
        private StateSpriteAnimation[] _stateSpriteAnimations;

        #endregion Members

        #region Properties

        public SpriteAnimator SpriteAnimator
            => _spriteAnimator;

        #endregion Properties

        #region Class Methods

        public void Init() { }

        public void Play(EntityAnimationState state)
        {
            var stateAnimation = _stateSpriteAnimations.FirstOrDefault(x => x.state == state);
            if (stateAnimation != null)
                _spriteAnimator.Play(stateAnimation.spriteAnimationName, playOneShot: !stateAnimation.isLoop);
        }

        public void SetUnscaled(bool isUnscaled)
        {
            if (isUnscaled)
                _spriteAnimator.UpdateUseScaledDeltaTime(false);
            else
                _spriteAnimator.ResetUseScaledDeltaTime();
        }

        public void Pause()
            => _spriteAnimator.Stop();

        public void Continue()
            => _spriteAnimator.Resume();

        public void TintColor(Color color)
            => _spriteAnimator?.TintColor(color);

        public void SetNewAnimationName(EntityAnimationState state, string newMappingName)
        {
            var stateSpriteAnimation = _stateSpriteAnimations.FirstOrDefault(s => s.state == state);
            if (stateSpriteAnimation != null)
            {
                stateSpriteAnimation.spriteAnimationName = newMappingName;
            }
        }

        #endregion Class Methods
    }
}