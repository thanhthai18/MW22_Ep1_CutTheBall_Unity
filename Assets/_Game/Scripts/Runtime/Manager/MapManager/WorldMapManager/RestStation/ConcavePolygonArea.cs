using UnityEngine;

namespace Runtime.Gameplay.Map
{
    public struct ConcavePolygonArea
    {
        #region Members

        private readonly Vector2[] _points;

        #endregion Members

        #region Struct Methods

        public ConcavePolygonArea(Vector2[] points)
            => _points = points;

        public bool IsPositionInArea(Vector2 point)
        {
            var pointsLength = _points.Length;
            var isInside = false;
            for (int i = 0, j = pointsLength - 1; i < pointsLength; j = i++)
            {
                if (((_points[i].y > point.y) != (_points[j].y > point.y)) &&
                    (point.x < (_points[j].x - _points[i].x) * (point.y - _points[i].y) / (_points[j].y - _points[i].y) + _points[i].x))
                {
                    isInside = !isInside;
                }
            }
            return isInside;
        }

        #endregion Struct Methods
    }
}