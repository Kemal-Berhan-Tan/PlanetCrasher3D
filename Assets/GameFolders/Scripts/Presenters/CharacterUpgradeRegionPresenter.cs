using Framework.Core;
using Framework.UI;
using Framework.Utility;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Views;

namespace GameFolders.Scripts.Presenters
{
    public class CharacterUpgradeRegionPresenter : BasePresenter
    {
        public override void Receive(BaseEventArgs baseEventArgs)
        {
        }

        public void SetCurrentMoney(int currentMoney)
        {
            ((CharacterUpgradeRegionView)view).currentMoneyText.text =
                $"{AbbrevationUtility.AbbreviateNumber(currentMoney)}";
        }

        public void OnPlayButtonClickHandler()
        {
            BroadcastUpward(new PlayButtonClickedEventArgs());
        }
    }
}