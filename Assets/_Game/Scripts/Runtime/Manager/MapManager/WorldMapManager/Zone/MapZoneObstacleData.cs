using UnityEngine;

namespace Runtime.Gameplay.Map
{
    public class MapZoneObstacleData
    {
        #region Members

        public bool hitObstacle;
        public Vector3 point;

        #endregion Members

        #region Class Methods

        public MapZoneObstacleData(bool hitObstacle, Vector3 point)
        {
            this.hitObstacle = hitObstacle;
            this.point = point;
        }

        #endregion Class Methods
    }
}