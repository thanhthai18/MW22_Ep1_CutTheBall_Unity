namespace Runtime.Gameplay.Map
{
    public class MapZoneUnrespawnable : MapZone
    {
        #region Properties

        public override bool MarkRespawnable => false;

        #endregion Properties
    }
}