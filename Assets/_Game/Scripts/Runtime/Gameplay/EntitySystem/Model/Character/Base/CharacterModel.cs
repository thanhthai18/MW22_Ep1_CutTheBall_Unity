using UnityEngine;
using Runtime.Definition;

namespace Runtime.Gameplay.EntitySystem
{
    public enum CharacterState
    {
        Idle,
        Move,
        Attack,
        HardCC,
        StandStill,
    }

    public abstract partial class CharacterModel : EntityModel
    {
        #region Members

        protected string visualId;
        protected AttackType attackType;
        protected float maxHp;
        protected float currentHp;
        protected float baseMultiplyHp;
        protected float baseBonusHp;
        protected int level;
        protected bool hasMoveInput;
        protected bool isAttacking;
        protected bool isUsingSkill;
        protected Vector2 movePosition;
        protected Vector2 moveDirection;
        protected Vector2 faceDirection;
        protected bool faceRight;
        protected float respawnDelay;
        protected bool isInDamageReductionAreaAffect;
        protected bool isInGoddessAuraAreaAffect;
        protected IInteractable currentTargetedTarget;
        protected IInteractable currentAttackedTarget;
        protected CharacterState characterState;

        #endregion Members

        #region Properties

        public override int Level => level;
        public string VisualId => visualId;
        public virtual bool IsHeroBoss => false;

        public bool IsInDamageReductionAreaAffect
        {
            get => isInDamageReductionAreaAffect;
            set
            {
                if (isInDamageReductionAreaAffect != value)
                {
                    isInDamageReductionAreaAffect = value;
                }
            }
        }

        public bool IsInGoddessAuraAreaAffect
        {
            get => isInGoddessAuraAreaAffect;
            set
            {
                if (isInGoddessAuraAreaAffect != value)
                {
                    isInGoddessAuraAreaAffect = value;
                }
            }
        }

        public bool FaceRight
        {
            get => faceRight;
            set
            {
                if (faceRight != value)
                {
                    faceRight = value;
                    DirectionChangedEvent.Invoke();
                }
            }
        }

        public bool IsAttacking
        {
            get => isAttacking;
            set
            {
                if (isAttacking != value)
                    isAttacking = value;
            }
        }

        public bool IsUsingSkill
        {
            get => isUsingSkill;
            set
            {
                if (isUsingSkill != value)
                    isUsingSkill = value;
            }
        }

        public virtual Vector2 MoveDirection
        {
            get => moveDirection;
            set
            {
                if (value != Vector2.zero && faceDirection != value)
                {
                    faceDirection = value;
                    FaceRight = value.x > 0.0f;
                }

                if (moveDirection != value)
                {
                    moveDirection = value;
                    if (hasMoveInput && moveDirection == Vector2.zero)
                    {
                        hasMoveInput = false;
                        MovementChangedEvent.Invoke();
                    }
                    else if (!hasMoveInput && moveDirection != Vector2.zero)
                    {
                        hasMoveInput = true;
                        MovementChangedEvent.Invoke();
                    }
                }
            }
        }

        public Vector2 MovePosition
        {
            get => movePosition;
            set => movePosition = value;
        }

        public bool CheckCanAttack => !(IsDead || isAttacking || IsUsingSkill);
        public float RespawnDelay => respawnDelay;
        public float MaxHp => (maxHp + baseBonusHp) * baseMultiplyHp;
        public float CurrentHp => currentHp;
        public override bool IsDead => currentHp <= 0;
        public bool HasMoveInput => hasMoveInput;
        public AttackType AttackType => attackType;
        public CharacterState CharacterState => characterState;
        public IInteractable CurrentTargetedTarget => currentTargetedTarget;
        public IInteractable CurrentAttackedTarget => currentAttackedTarget;

        #endregion Properties

        #region Class Methods

        public CharacterModel(uint characterUId, string characterId, CharacterModelData characterModelData)
            : base(characterUId, characterId, characterModelData.DetectedPriority)
        {
            visualId = characterModelData.CharacterVisualId;
            level = characterModelData.CharacterLevelModel.Level;
            attackType = characterModelData.AttackType;
            faceRight = true;
            faceDirection = new Vector2(1, 0);
            respawnDelay = characterModelData.RespawnDelay;
            maxHp = characterModelData.CharacterLevelModel.CharacterStats.GetStatTotalValue(StatType.HealthPoint);
            currentHp = maxHp;
            baseMultiplyHp = 1f;
            baseBonusHp = 0f;
            UpdateState(CharacterState.Idle);
            InitStats(characterModelData.CharacterLevelModel.CharacterStats);
        }

        public void MissedDamage()
        {
            if (!IsDead)
                ReactionChangedEvent.Invoke(CharacterReactionType.JustMissDamage);
        }

        public void UpdateState(CharacterState characterState)
        {
            if (this.characterState != characterState)
            {
                this.characterState = characterState;
                switch (characterState)
                {
                    case CharacterState.Idle:
                        ReactionChangedEvent.Invoke(CharacterReactionType.JustIdle);
                        break;

                    case CharacterState.Move:
                        ReactionChangedEvent.Invoke(CharacterReactionType.JustMove);
                        break;

                    case CharacterState.StandStill:
                        ReactionChangedEvent.Invoke(CharacterReactionType.JustStandStill);
                        break;
                }
            }
        }

        public bool CanUpdateTargetedTarget()
            => (characterStatus & CharacterStatus.Taunted) == 0;

        public void UpdateTargetedTarget(IInteractable targetedTarget)
            => currentTargetedTarget = targetedTarget;

        public void UpdateAttackedTarget(IInteractable attackedTarget)
        {
            currentAttackedTarget = attackedTarget;
            FaceRight = (attackedTarget.Position - Position).x > 0.0f;
        }

        public void SetTransformMovePosition(Vector2 transformMovePosition)
        {
            MovePosition = transformMovePosition;
            MovePositionUpdatedEvent.Invoke();
        }

        protected partial void InitStats(CharacterStats characterStats);

        #endregion Class Methods
    }

    public class CharacterModelData
    {
        #region Members

        protected string characterId;
        protected string characterVisualId;
        protected int detectedPriority;
        protected AttackType attackType;
        protected float respawnDelay;
        protected CharacterLevelModel characterLevelModel;

        #endregion Members

        #region Properties

        public int DetectedPriority => detectedPriority;
        public AttackType AttackType => attackType;
        public float RespawnDelay => respawnDelay;
        public CharacterLevelModel CharacterLevelModel => characterLevelModel;

        public string CharacterVisualId
        {
            get
            {
                if (string.IsNullOrEmpty(characterVisualId))
                    return characterId;
                else
                    return characterVisualId;
            }
        }

        #endregion Properties

        #region Class Methods

        public CharacterModelData(string characterId, string characterVisualId, int detectedPriority,
                                  AttackType attackType, float respawnDelay, CharacterLevelModel characterLevelModel)
        {
            this.characterId = characterId;
            this.characterVisualId = characterVisualId;
            this.detectedPriority = detectedPriority;
            this.attackType = attackType;
            this.respawnDelay = respawnDelay;
            this.characterLevelModel = characterLevelModel;
        }

        #endregion Class Methods
    }

    public class CharacterLevelModel
    {
        #region Members

        protected int level;
        protected CharacterStats characterStats;

        #endregion Members

        #region Properties

        public int Level => level;
        public CharacterStats CharacterStats => characterStats;

        #endregion Properties

        #region Class Methods

        public CharacterLevelModel(int level, CharacterStats characterStats)
        {
            this.level = level;
            this.characterStats = characterStats;
        }

        #endregion Class Methods
    }
}