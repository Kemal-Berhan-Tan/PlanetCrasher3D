using Framework.UI;
using GameFolders.Scripts.Presenters;
using UnityEngine;
using UnityEngine.UI;

namespace GameFolders.Scripts.Views
{
    public class FailView : BaseView
    {
        [SerializeField] private Button RestartLevelButton;

        protected override void Initialize()
        {
            RestartLevelButton.onClick.AddListener((_presenter as FailPresenter).RestartLevelButtonHandler);
        }
    }
}