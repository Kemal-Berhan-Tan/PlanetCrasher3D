using Framework.Core;
using Framework.UI;
using GameFolders.Scripts.Events;

namespace GameFolders.Scripts.Presenters
{
    public class SettingsPresenter : BasePresenter
    {
        //private int remaninglCountForPanelChange = 3;
        //private bool isAdminPanelOpened = false;

        private void Start()
        {
            //(view as SettingsView).VersionText.text = "v" + Application.version;
        }

        public override void Receive(BaseEventArgs baseEventArgs)
        {
        }

        public override void ShowView()
        {
            base.ShowView();
            // if (!isAdminPanelOpened)
            // {
            //     (view as SettingsView).AdminPanel.gameObject.SetActive(false);
            //     (view as SettingsView).MainPanel.gameObject.SetActive(true);
            // }
        }

        public void SoundToggleHandler(bool isOn)
        {
            BroadcastUpward(new SoundToggleClickedEventArgs(isOn));
        }

        public void HapticToggleHandler(bool isOn)
        {
            BroadcastUpward(new HapticToggleClickedEventArgs(isOn));
        }

        public void JoystickToggleHandler(bool isOn)
        {
            BroadcastUpward(new JoystickToggleClickedEventArgs(isOn));
        }

        // public void MusicToggleHandler(bool isOn)
        // {
        //     BroadcastUpward(new MusicToggleClickedEventArgs(isOn));
        // }

        public void CloseButtonHandler()
        {
            BroadcastUpward(new SettingsButtonCloseClickedEventArgs());
        }

        // public void VersionTextButtonHandler()
        // {
        //     remaninglCountForPanelChange--;
        //     if (remaninglCountForPanelChange <= 0)
        //     {
        //         if (isAdminPanelOpened)
        //         {
        //             (view as SettingsView).MainPanel.gameObject.SetActive(true);
        //             (view as SettingsView).AdminPanel.gameObject.SetActive(false);
        //             isAdminPanelOpened = false;
        //         }
        //         else
        //         {
        //             (view as SettingsView).MainPanel.gameObject.SetActive(false);
        //             (view as SettingsView).AdminPanel.gameObject.SetActive(true);
        //             isAdminPanelOpened = true;
        //         }
        //
        //         remaninglCountForPanelChange = 3;
        //     }
        // }

        // public void SetJoystickToggleState(bool gameModelIsJoystickOn)
        // {
        //     (view as SettingsView).JoystickToggle.isOn = gameModelIsJoystickOn;
        // }

        // public void SetMusicToggleState(bool gameModelIsSoundOn)
        // {
        //     (view as SettingsView).SoundToggle.isOn = gameModelIsSoundOn;
        // }
    }
}