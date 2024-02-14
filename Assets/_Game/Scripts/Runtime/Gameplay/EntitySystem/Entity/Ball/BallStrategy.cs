namespace Runtime.Gameplay.EntitySystem
{
    public class BallStrategy : EntityStrategy<BallModel>
    {
        #region Class Methods

        public override void Collision()
        {
            base.Collision();
        }

        public override void Missed()
        {
            base.Missed();
        }

        #endregion  
    }
}