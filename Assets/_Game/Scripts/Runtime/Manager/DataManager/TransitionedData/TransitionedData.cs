using System.Collections.Generic;
using UnityEngine;

namespace Runtime.Manager.Data
{
    public class TransitionedData
    {
        #region Properties

        public string GoneToDungeonId { get; set; }
        public bool HasGoneToDungeonAndPassed { get; set; }
        public List<Vector3> LastStandDungeonPositions { get; set; }

        #endregion Properties

        #region Class Methods

        public void ClearStandDungeonPositions()
            => LastStandDungeonPositions = new List<Vector3>();

        #endregion Class Methods
    }
}