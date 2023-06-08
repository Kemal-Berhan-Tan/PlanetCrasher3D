using Framework.App;
using Framework.Core;
using GameFolders.Scripts.Models;

namespace GameFolders.Scripts.Managers.HighLevelManagers
{
    public class AppManager : BaseAppManager
    {
        private GameModel _gameModel;

        public override void Receive(BaseEventArgs baseEventArgs)
        {
        }

        public void InjectModel(GameModel gameModel)
        {
            _gameModel = gameModel;
        }
    }
}