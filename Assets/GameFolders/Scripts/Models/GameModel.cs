using Framework.Game;
using UnityEngine;

namespace GameFolders.Scripts.Models
{
    public class GameModel : BaseGameModel
    {
        #region Private Variables

        private readonly int _maxLevel = 0;
        private string _jsonFileName;

        private readonly string _prefKeyLevelID = "LevelID";
        private readonly string _prefKeyShowingLevelNumber = "ShowingLevelID";
        private readonly string _prefKeyMoney = "Money";
        private readonly string _prefKeyIsFirstSession = "IsFirstSession";
        private readonly string _prefKeyIsFirstWronglyKnitted = "IsFirstWronglyKnitted";
        protected string PrefKeyIsJoysticOn = "IsJoystickOn";
        private readonly string _prefKeyCharacterBodyPartBuyPrice = "CharacterBodyPartBuyPrice";
        private readonly string _prefKeyCharacterPowerTitleIndex = "CharacterPowerTitle";

        #endregion

        #region Properties

        public int LevelID
        {
            get =>
                // if (!PlayerPrefs.HasKey(prefKey_LevelID))
                //     return 1;
                // else
                PlayerPrefs.GetInt(_prefKeyLevelID);
            set
            {
                var tempValue = value;
                if (tempValue > _maxLevel)
                    tempValue = 0;
                //tempValue = Random.Range(0, maxLevel);
                PlayerPrefs.SetInt(_prefKeyLevelID, tempValue);
                ShowingLevelNumber++;
                OnPropertyChanged();
            }
        }

        public int ShowingLevelNumber
        {
            get => PlayerPrefs.GetInt(_prefKeyShowingLevelNumber, 1);
            set
            {
                PlayerPrefs.SetInt(_prefKeyShowingLevelNumber, value);
                OnPropertyChanged();
            }
        }

        public int Money
        {
            get => 99999;
            //get => PlayerPrefs.GetInt(_prefKeyMoney, 99999);

            set
            {
                PlayerPrefs.SetInt(_prefKeyMoney, value);
                OnPropertyChanged();
            }
        }

        public bool IsFirstSession
        {
            get => PlayerPrefs.GetInt(_prefKeyIsFirstSession, 1) == 1;
            set => PlayerPrefs.SetInt(_prefKeyIsFirstSession, value ? 1 : 0);
        }

        public bool IsFirstWronglyKnitted
        {
            get => PlayerPrefs.GetInt(_prefKeyIsFirstWronglyKnitted, 1) == 1;
            set => PlayerPrefs.SetInt(_prefKeyIsFirstWronglyKnitted, value ? 1 : 0);
        }

        public int PercentageOfTheRequiredCompletion { get; } = 90;

        public bool IsJoystickOn
        {
            get => PlayerPrefs.GetInt(prefKey_IsAdminOn) == 1 ? true : false;
            set
            {
                PlayerPrefs.SetInt(PrefKeyIsJoysticOn, value ? 1 : 0);
                OnPropertyChanged();
            }
        }

        public int CharacterBodyPartBuyPrice
        {
            get => PlayerPrefs.GetInt(_prefKeyCharacterBodyPartBuyPrice, 100);
            set
            {
                PlayerPrefs.SetInt(_prefKeyCharacterBodyPartBuyPrice, value);
                OnPropertyChanged();
            }
        }

        public int CharacterPowerTitleIndex
        {
            get => PlayerPrefs.GetInt(_prefKeyCharacterPowerTitleIndex);
            set => PlayerPrefs.SetInt(_prefKeyCharacterPowerTitleIndex, value);
        }
        public bool IsTutorialLevel => LevelID == 0;

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        #endregion
    }
}