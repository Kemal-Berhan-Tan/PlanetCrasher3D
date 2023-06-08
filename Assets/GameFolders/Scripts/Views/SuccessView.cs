using Framework.UI;
using GameFolders.Scripts.Presenters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFolders.Scripts.Views
{
    public class SuccessView : BaseView
    {
        public TextMeshProUGUI RewardText;
        [SerializeField] private Button nextLevelButton;
        public Transform Beam;
        public GameObject SuccessContainer, BossMapContainer;
        public Transform BossRef;

        protected override void Initialize()
        {
            nextLevelButton.onClick.AddListener((_presenter as SuccessPresenter)
                .NextLevelButtonHandler);
        }
    }
}