using Framework.Core;
using Framework.UI;
using GameFolders.Scripts.Events;

namespace GameFolders.Scripts.Presenters
{
    public class FailPresenter : BasePresenter
    {
        public override void Receive(BaseEventArgs baseEventArgs)
        {
        }

        public void RestartLevelButtonHandler()
        {
            BroadcastUpward(new RestartLevelButtonClickedEventArgs());
        }
    }
}