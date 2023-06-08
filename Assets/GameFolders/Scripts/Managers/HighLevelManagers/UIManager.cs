using System.ComponentModel;
using Framework.Core;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Helpers;
using GameFolders.Scripts.Models;
using GameFolders.Scripts.Presenters;
using UnityEngine;

namespace GameFolders.Scripts.Managers.HighLevelManagers
{
    public class UIManager : BaseManager
    {
        #region Private Methods

        [SerializeField] private CharacterUpgradeRegionPresenter characterUpgradeRegionPresenter;
        [SerializeField] private GamePlayPresenter gamePlayPresenter;
        [SerializeField] private SettingsPresenter settingsPresenter;
        [SerializeField] private SuccessPresenter successPresenter;
        [SerializeField] private FailPresenter failPresenter;

        private GameModel _gameModel;

        #endregion

        #region Begining

        protected override void Awake()
        {
            characterUpgradeRegionPresenter.InjectManager(this);
            gamePlayPresenter.InjectManager(this);
            //settingsPresenter.InjectManager(this);
            successPresenter.InjectManager(this);
            failPresenter.InjectManager(this);
        }

        protected override void Start()
        {
            characterUpgradeRegionPresenter.ShowView();
        }

        #endregion

        #region Implemented Methods

        public override void Receive(BaseEventArgs baseEventArgs)
        {
            switch (baseEventArgs)
            {
                case OnStartTheTutorialLevelEventArgs onStartTheTutorialLevelEventArgs:
                    characterUpgradeRegionPresenter.HideView();
                    gamePlayPresenter.ShowView();
                    Broadcast(new PlayButtonClickedEventArgs());
                    CoroutineController.DoAfterGivenTime(1.5f, () => Broadcast(new StartTheGamePlayEventArgs()));
                    break;
                case StartTheGamePlayEventArgs startThePlayerFallingEventArgs:
                    Broadcast(new StartTheGamePlayEventArgs());
                    break;
                case PlayButtonClickedEventArgs playButtonClickedEventArgs:
                    characterUpgradeRegionPresenter.HideView();
                    gamePlayPresenter.ShowView();
                    Broadcast(playButtonClickedEventArgs);
                    CoroutineController.DoAfterGivenTime(1f, () =>
                    {
                        if (_gameModel.ShowingLevelNumber - 1 <= 10 && _gameModel.LevelID != 0)
                            gamePlayPresenter.NextLevelToRoadMap(_gameModel.ShowingLevelNumber - 1);
                        else
                            Broadcast(new StartTheGamePlayEventArgs());
                    });
                    break;
                case OnLevelComplatedEventArgs onLevelComplatedEventArgs:
                    CoroutineController.DoAfterGivenTime(1f, () =>
                    {
                        gamePlayPresenter.HideView();
                        if (onLevelComplatedEventArgs.IsSuccessed)
                        {
                            successPresenter.ShowView();
                            if (_gameModel.ShowingLevelNumber == 6)
                                successPresenter.ActivateBossMap();
                        }
                        else
                            failPresenter.ShowView();
                    });
                    break;
                case RestartLevelButtonClickedEventArgs restartLevelButtonClickedEventArgs:
                    // if (_gameModel.IsTutorialLevel)
                    //     Broadcast(new NextLevelButtonClickedEventArgs());
                    // else
                    Broadcast(restartLevelButtonClickedEventArgs);
                    break;
                case NextLevelButtonClickedEventArgs nextLevelButtonClickedEventArgs:
                    Broadcast(nextLevelButtonClickedEventArgs);
                    break;
                case OnPlayerCollectedFuelEventArgs eventArgs:
                    if (eventArgs.IsBoostActivated)
                        gamePlayPresenter.FuelBarFull();
                    else
                        gamePlayPresenter.UpdateFuelBar(eventArgs.FuelAmount);
                    break;
            }
        }

        private void OnGameModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_gameModel.Money))
            {
                characterUpgradeRegionPresenter.SetCurrentMoney(_gameModel.Money);
                //gamePlayPresenter.SetCurrentMoney(_gameModel.Money);
            }
            else if (e.PropertyName == nameof(_gameModel.ShowingLevelNumber))
            {
                //gamePlayPresenter.SetCurrentShowingLevelNumber(_gameModel.ShowingLevelNumber);
            }
        }

        #endregion

        #region Public Methods

        public void InjectModel(GameModel gameModel)
        {
            _gameModel = gameModel;

            characterUpgradeRegionPresenter.SetCurrentMoney(gameModel.Money);
            //gamePlayPresenter.SetCurrentMoney(gameModel.Money);
            //gamePlayPresenter.SetCurrentShowingLevelNumber(gameModel.ShowingLevelNumber);
            //settingsPresenter.SetJoystickToggleState(gameModel.IsJoystickOn);
            //settingsPresenter.SetMusicToggleState(gameModel.IsMusicOn);

            _gameModel.PropertyChanged += OnGameModelPropertyChanged;
        }

        #endregion
    }
}