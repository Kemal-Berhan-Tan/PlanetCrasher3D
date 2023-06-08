using System.Collections.Generic;
using System.ComponentModel;
using Framework.Core;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Models;
using UnityEngine;

namespace GameFolders.Scripts.Managers.MidLevelManagers
{
    public class AudioManager : BaseManager
    {
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private List<AudioClip> sounds;

        public override void Receive(BaseEventArgs baseEventArgs)
        {
        }

        public void InjectModel(GameModel gameModel)
        {
            AudioListener.pause = !gameModel.IsSoundOn;
            gameModel.PropertyChanged += delegate(object sender, PropertyChangedEventArgs args)
            {
                if (args.PropertyName == nameof(gameModel.IsSoundOn)) AudioListener.pause = !gameModel.IsSoundOn;
            };
        }

        private void PlaySound(SoundTypesEnum soundAndHapticTypesEnum)
        {
            var tempClip = sounds[(int)soundAndHapticTypesEnum];
            if (tempClip != null) audioSource.PlayOneShot(tempClip);
        }
    }
}