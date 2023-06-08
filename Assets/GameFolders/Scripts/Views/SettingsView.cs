using Framework.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameFolders.Scripts.Views
{
    public class SettingsView : BaseView
    {
        public Button closeButton;
        public Button versionTextButton;
        public TextMeshProUGUI versionText;
        public GameObject mainPanel, adminPanel;

        //public CustomToggle MusicToggle;

        protected override void Initialize()
        {
            //CloseButton.onClick.AddListener((_presenter as SettingsPresenter).CloseButtonHandler);
            //VersionTextButton.onClick.AddListener((_presenter as SettingsPresenter).VersionTextButtonHandler);
            //MusicToggle.onValueChanged.AddListener((_presenter as SettingsPresenter).MusicToggleHandler);
        }
    }
}