using Framework.UI;
using GameFolders.Scripts.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFolders.Scripts.Views
{
    public class CharacterUpgradeRegionView : BaseView 
    {
        public TextMeshProUGUI currentMoneyText;
        
        [SerializeField] private Button PlayButton;

        protected override void Initialize()
        {
            PlayButton.onClick.AddListener((_presenter as CharacterUpgradeRegionPresenter).OnPlayButtonClickHandler);
        }
    }
}