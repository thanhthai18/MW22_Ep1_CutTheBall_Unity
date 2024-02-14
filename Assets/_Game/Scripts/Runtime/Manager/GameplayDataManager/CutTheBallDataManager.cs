using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Runtime.Manager.Data;
using Cysharp.Threading.Tasks;
using Runtime.Config;
using Runtime.Gameplay.EntitySystem;

namespace Runtime.Gameplay.Manager
{
    public class CutTheBallDataManager : GameplayDataManager
    {
        #region Class Methods

        protected override async UniTask LoadConfig(CancellationToken cancellationToken)
        {
            await DataManager.Config.LoadEntityConfig(cancellationToken);
            await base.LoadConfig(cancellationToken);
        }
        
        #endregion Class Methods
    }
}