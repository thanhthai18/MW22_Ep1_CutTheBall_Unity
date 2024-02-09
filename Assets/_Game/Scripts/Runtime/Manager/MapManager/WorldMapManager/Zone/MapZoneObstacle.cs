using UnityEngine;

namespace Runtime.Gameplay.Map
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class MapZoneObstacle : MonoBehaviour
    {
        #region Members

        public BoxCollider2D boxCollider;

        #endregion Members
    }
}