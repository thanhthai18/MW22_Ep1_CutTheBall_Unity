using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Runtime.Definition;

namespace Runtime.Gameplay.EntitySystem
{
    [Flags]
    public enum CharacterStatus
    {
        Stunned = 1 << 0,
        KnockedUp = 1 << 1,
        Taunted = 1 << 2,
        Freezed = 1 << 3,
        HardCC = Stunned | KnockedUp | Freezed
    }

    public abstract partial class CharacterModel : EntityModel
    {
        #region Members

        protected List<StatusEffectType> statusEffectsStackData = new();
        protected CharacterStatus characterStatus;

        #endregion Members

        #region Properties 

        public bool IsInHardCCStatus
            => (characterStatus & CharacterStatus.HardCC) > 0;

        public bool IsInBleedStatus
        {
            get
            {
                var bleedAttackStatusEffectCount = GetStatusEffectStackCount(StatusEffectType.BleedAttack);
                return bleedAttackStatusEffectCount > 0;
            }
        }

        #endregion Properties

        #region Class Methods

        public void StackStatusEffect(StatusEffectType statusEffectType)
            => statusEffectsStackData.Add(statusEffectType);

        public void ClearStatusEffectStack(StatusEffectType statusEffectType)
        {
            for (int i = statusEffectsStackData.Count - 1; i >= 0; i--)
                if (statusEffectsStackData[i] == statusEffectType)
                    statusEffectsStackData.RemoveAt(i);
        }

        public int GetStatusEffectStackCount(StatusEffectType statusEffectType)
            => statusEffectsStackData.Count(x => x == statusEffectType);

        public bool CheckContainStatusEffectInStack(StatusEffectType statusEffectType)
           => statusEffectsStackData.Any(x => x == statusEffectType);

        public virtual void StartGettingKnockUp()
        {
            characterStatus |= CharacterStatus.KnockedUp;
            HardCCImpactedEvent.Invoke(StatusEffectType.KnockUp);
            UpdateState(CharacterState.HardCC);
        }

        public virtual void GettingKnockUp(Vector2 knockUpPosition)
            => SetTransformMovePosition(knockUpPosition);

        public virtual void StopGettingKnockUp()
        {
            characterStatus &= ~CharacterStatus.KnockedUp;
            StopGettingHardCC();
        }

        public virtual void StartGettingStun()
        {
            characterStatus |= CharacterStatus.Stunned;
            HardCCImpactedEvent.Invoke(StatusEffectType.Stun);
            UpdateState(CharacterState.HardCC);
        }

        public virtual void StopGettingStun()
        {
            characterStatus &= ~CharacterStatus.Stunned;
            StopGettingHardCC();
        }

        public virtual void StartGettingFreeze()
        {
            characterStatus |= CharacterStatus.Freezed;
            HardCCImpactedEvent.Invoke(StatusEffectType.Freeze);
            UpdateState(CharacterState.HardCC);
        }

        public virtual void StopGettingFreeze()
        {
            characterStatus &= ~CharacterStatus.Freezed;
            StopGettingHardCC();
        }

        public virtual void StartGettingTaunt()
            => characterStatus |= CharacterStatus.Taunted;

        public virtual void StopGettingTaunt()
            => characterStatus &= ~CharacterStatus.Taunted;

        protected virtual void StopGettingHardCC()
        {
            if (!IsInHardCCStatus)
                UpdateState(CharacterState.Idle);
        }

        #endregion Class Methods
    }
}