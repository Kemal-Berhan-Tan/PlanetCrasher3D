using DG.Tweening;
using Framework.Core;
using FluffyUnderware.Curvy;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Managers.MidLevelManagers;
using GameFolders.Scripts.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace GameFolders.Scripts.Managers.HighLevelManagers
{
    public class GameManager : BaseManager
    {
        #region Private Variables

        [SerializeField] private LevelManager levelManager;
        [SerializeField] private InputManager inputManager;
        [SerializeField] private PlayerManager playerManager;
        [SerializeField] private CameraManager cameraManager;
        [SerializeField] private AudioManager audioManager;
        [SerializeField] private HapticManager hapticManager;
        [SerializeField] private CharacterUpgradeRegionManager characterUpgradeRegionManager;
        [SerializeField] private GameplayManager gameplayManager;

        private CurvyGlobalManager curvyGlobal;

        private GameModel _gameModel;

        #endregion

        #region Begining

        protected override void Awake()
        {
            levelManager.InjectManager(this);
            inputManager.InjectManager(this);
            playerManager.InjectManager(this);
            cameraManager.InjectManager(this);
            audioManager.InjectManager(this);
            hapticManager.InjectManager(this);
            characterUpgradeRegionManager.InjectManager(this);
            gameplayManager.InjectManager(this);

            var mediator = new BaseMediator();
            levelManager.InjectMediator(mediator);
            inputManager.InjectMediator(mediator);
            playerManager.InjectMediator(mediator);
            cameraManager.InjectMediator(mediator);
            audioManager.InjectMediator(mediator);
            hapticManager.InjectMediator(mediator);
            characterUpgradeRegionManager.InjectMediator(mediator);
            gameplayManager.InjectMediator(mediator);
        }

        protected override void Start()
        {
            curvyGlobal = GameObject.Find("_CurvyGlobal_").GetComponent<CurvyGlobalManager>();
        }

        #endregion

        #region Implemented Methods

        public override void Receive(BaseEventArgs baseEventArgs)
        {
            switch (baseEventArgs)
            {
                case OnLevelCreatedEventArgs onLevelCreatedEventArgs:
                    Broadcast(onLevelCreatedEventArgs);
                    break;
                case OnStartTheTutorialLevelEventArgs onStartTheTutorialLevelEventArgs:
                    Broadcast(onStartTheTutorialLevelEventArgs);
                    break;
                case ResetTheManagersEventArgs resetTheManagersEventArgs:
                    ResetTheGame(true);
                    break;
                case RestartLevelButtonClickedEventArgs restartLevelButtonClickedEventArgs:
                    ResetTheGame(true);
                    break;
                case NextLevelButtonClickedEventArgs nextLevelButtonClickedEventArgs:
                    NextLevel();
                    break;
                case PlayButtonClickedEventArgs playButtonClickedEventArgs:
                    BroadcastDownward(playButtonClickedEventArgs);
                    break;
                case StartTheGamePlayEventArgs startTheGamePlayEventArgs:
                    BroadcastDownward(startTheGamePlayEventArgs);
                    break;
                case OnLevelComplatedEventArgs onLevelComplatedEventArgs:
                    Broadcast(onLevelComplatedEventArgs);
                    break;
                case OnPlayerCollectedFuelEventArgs onPlayerCollectedFuelEventArgs:
                    Broadcast(onPlayerCollectedFuelEventArgs);
                    break;
            }
        }

        public void InjectModel(GameModel gameModel)
        {
            _gameModel = gameModel;

            levelManager.InjectModel(gameModel);
            playerManager.InjectModel(gameModel);
            inputManager.InjectModel(gameModel);
            audioManager.InjectModel(gameModel);
            hapticManager.InjectModel(gameModel);
            characterUpgradeRegionManager.InjectModel(gameModel);
            gameplayManager.InjectModel(gameModel);

            LoadLevel(true);
        }

        #endregion

        #region Private Methods

        private void AnalyticsEventSender(AnalyticsEventTypesEnum eventType)
        {
            switch (eventType)
            {
                case AnalyticsEventTypesEnum.StartEvent:
                    //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start,"Level" + gameModel.ShowingLevelNumber.ToString("D5"));
                    //Elephant.LevelStarted(PlayerPrefs.GetInt("Level"));
                    break;
                case AnalyticsEventTypesEnum.CompleteEvent:
                    //GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete,"Level" + gameModel.ShowingLevelNumber.ToString("D5"));
                    //Elephant.LevelCompleted(PlayerPrefs.GetInt("Level"));
                    break;
                case AnalyticsEventTypesEnum.DesignEvent:
                    //GameAnalytics.NewDesignEvent("Level:" + gameModel.ShowingLevelNumber.ToString("D5") + ":Money",gameModel.Money);
                    break;
            }
        }

        #endregion

        #region Incoming Receive Events

        private void LoadLevel(bool isSendAnalyticsEvent)
        {
            if (isSendAnalyticsEvent)
            {
                AnalyticsEventSender(AnalyticsEventTypesEnum.StartEvent);
                AnalyticsEventSender(AnalyticsEventTypesEnum.DesignEvent);
            }

            levelManager.LoadLevel();
        }

        private void NextLevel()
        {
            _gameModel.LevelID++;
            ResetTheGame(false);
            LoadLevel(true);
        }

        private void ResetTheGame(bool isLoadLevel)
        {
            // Destroy(curvyGlobal.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            DOTween.KillAll();
            if (isLoadLevel)
                LoadLevel(false);
            BaseEventArgs eventArgs = new ResetTheManagersEventArgs();
            Broadcast(eventArgs);
            BroadcastDownward(eventArgs);
        }

        #endregion
    }
}