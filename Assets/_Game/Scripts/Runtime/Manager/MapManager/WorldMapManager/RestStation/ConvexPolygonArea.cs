using UnityEngine;

namespace Runtime.Gameplay.Map
{
    public struct ConvexPolygonArea
    {
        #region Members

        private readonly Vector2[] _points;

        #endregion Members

        #region Struct Methods

        public ConvexPolygonArea(Vector2[] points)
            => _points = points;

        /// <summary>
        /// Sử dụng toán vector, nhân chéo vector để ra hướng rẽ.
        /// Nếu chỉ cần 1 lần mà hướng rẽ của điểm xét và cạnh tiếp theo của polygon khác nhau thì tức là nằm ngoài
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool IsPositionInArea(Vector2 position)
        {
            if (this._points == null)
                return false;

            var length = this._points.Length;
            for (int i = 0; i < this._points.Length; i++)
            {
                var pointOne = this._points[i];
                var vectorBase = this._points[(i + 1) % length] - pointOne;
                var vectorSecond = this._points[(i + 2) % length] - pointOne;
                var vectorToPlayer = position - pointOne;
                var turnValue1 = vectorBase.x * vectorSecond.y - vectorBase.y * vectorSecond.x;
                var turnValue2 = vectorBase.x * vectorToPlayer.y - vectorBase.y * vectorToPlayer.x;

                // Nếu 2 bên rẽ khác nhau thì không nằm trong
                if (turnValue1 * turnValue2 < 0)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion Struct Methods
    }
}