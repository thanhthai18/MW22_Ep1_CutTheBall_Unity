#if UNITY_EDITOR

using UnityEditor;

namespace Runtime.Gameplay.Map
{
    [CustomEditor(typeof(MapZoneUnrespawnable))]
    public class MapZoneUnrespawnableEditor : MapZoneEditor { }
}

#endif