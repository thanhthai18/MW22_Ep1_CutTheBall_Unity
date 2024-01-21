using System;
using Runtime.Definition;

namespace Runtime.Gameplay.EntitySystem
{
    public enum CharacterReactionType
    {
        JustDie,
        JustMove,
        justMoveInGroup,
        JustIdle,
        JustFinishAttack,
        JustStandStill,
        JustMissDamage,
        JustAnimateUnscaled,
        JustResetAnimateUnscaled,
        JustSawHeroTeleported,
    }

    public abstract partial class CharacterModel : EntityModel
    {
        #region Properties

        public Action MovementChangedEvent { get; set; }
        public Action DirectionChangedEvent { get; set; }
        public Action MovePositionUpdatedEvent { get; set; }
        public Action<CharacterReactionType> ReactionChangedEvent { get; set; }
        public Action<StatType, float> StatChangedEvent { get; set; }
        public Action<StatusEffectType> HardCCImpactedEvent { get; set; }

        #endregion Properties

        #region Class Methods

        protected override void InitEvents()
        {
            base.InitEvents();
            MovementChangedEvent = () => { };
            MovePositionUpdatedEvent = () => { };
            DirectionChangedEvent = () => { };
            ReactionChangedEvent = _ => { };
            StatChangedEvent = (_, _) => { };
            HardCCImpactedEvent = _ => { };
        }

        #endregion Class Methods
    }
}