using System.Collections.Generic;
using DG.Tweening;
using Framework.Core;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Models;
using GameFolders.Scripts.Objects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace GameFolders.Scripts.Managers.MidLevelManagers
{
    public class GameplayManager : BaseManager
    {
        #region Public Variables

        #endregion

        #region Const Variables

        #endregion

        #region Properties

        private PlanetItem GetRandomPlanet => planetItems[Random.Range(1, planetItems.Count)];

        #endregion

        #region Private Variables

        [SerializeField] private GameObject UI;

        [SerializeField] private TapTimingBarIndicatorItem tapTimingBarIndicatorItem;
        [SerializeField] private GameObject tapToScreenTextObject;

        [SerializeField] private GameObject tapTimingBar;

        [SerializeField] private List<PlanetItem> planetItems = new List<PlanetItem>();
        // [SerializeField] private List<GameObject> tapTimingBars = new List<GameObject>();

        private int characterBodyPartLevelAverage;
        private bool isStopGame, isInputDisabled = true;

        private GameModel gameModel;

        private PlanetItem currentPlanetItem;

        private TapTimingBarTypes targetTapTimingBarTypeForTutorial;

        #endregion

        #region Actions

        #endregion

        #region Begining

        protected override void Start()
        {
            base.Start();

            Begining();
        }

        private void Begining()
        {
            UI.SetActive(false);
            tapTimingBarIndicatorItem.SetMovementAnimEnabled(false);
        }

        #endregion

        #region Implemented Methods

        public override void Receive(BaseEventArgs baseEventArgs)
        {
            switch (baseEventArgs)
            {
                case OnLevelCreatedEventArgs onLevelCreatedEventArgs:
                    OnLevelCreated();
                    break;
                case PlayButtonClickedEventArgs startThePlayerFallingEventArgs:
                    StartTheFalling();
                    break;
                case StartTheGamePlayEventArgs startTheGamePlayEventArgs:
                    StartGamePlay();
                    break;
                case LeanOnFingerEventArgs leanOnFingerEventArgs:
                    if (leanOnFingerEventArgs.Finger.Tap)
                        OnInputTap();
                    break;
                case OnCharacterBodyPartUpdatedEventArgs onCharacterBodyPartUpdatedEventArgs:
                    characterBodyPartLevelAverage = onCharacterBodyPartUpdatedEventArgs.BodyPartLevelsAverage;
                    break;
                case OnLevelComplatedEventArgs onLevelComplatedEventArgs:
                    OnLevelComplated();
                    break;
                case StartThePlanetFightEventArgs startThePlanetFightEventArgs:
                    StartThePlanetFight();
                    break;
                case OnPlayerCollidedPlanetEventArgs onPlayerCollidedPlanetEventArgs:
                    OnPlayerCollidedPlanet();
                    break;
            }
        }

        public void InjectModel(GameModel gameModel)
        {
            this.gameModel = gameModel;
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        private void StartThePlanetFight()
        {
            //miyav
            if (isStopGame)
                return;

            isInputDisabled = true;

            PlanetItem tempPlanetItem = GetRandomPlanet;

            currentPlanetItem = Instantiate(tempPlanetItem, transform);

            currentPlanetItem.StartSequence().OnComplete(() =>
            {
                isInputDisabled = false;
                currentPlanetItem.SetRotation(5);
                // isInputDisabled = gameModel.IsTutorialLevel;
                // SetTapTimingBar(characterBodyPartLevelAverage);
                tapTimingBar.SetActive(true);
                if (gameModel.IsTutorialLevel)
                {
                    tapTimingBarIndicatorItem.targetStopType = targetTapTimingBarTypeForTutorial;
                    tapTimingBarIndicatorItem.SetMovementAnimEnabled(true);
                    tapTimingBarIndicatorItem.OnTapTimingBarStoppedEvent += () =>
                    {
                        tapToScreenTextObject.SetActive(true);
                        tapToScreenTextObject.transform.DOScale(Vector3.one, .5f).SetEase(Ease.Linear).OnComplete(() =>
                        {
                            tapToScreenTextObject.transform.DOScale(Vector3.one * 1.25f, .5f).SetEase(Ease.Linear)
                                .SetLoops(-1, LoopType.Yoyo).SetId(7);
                        });
                        targetTapTimingBarTypeForTutorial = TapTimingBarTypes.Red;
                        isInputDisabled = false;
                    };
                }
                else
                    tapTimingBarIndicatorItem.SetMovementAnimEnabled(true);
            });
        }

        private void StartTapTimingBarAction(TapTimingBarTypes tapTimingBarType)
        {
            Broadcast(new OnPlanetFightActionEventArgs(InputTypes.Tap));

            // TODO : Tap timinin tutturulduğu yere göre multipler verilecek.

            // if (gameModel.IsTutorialLevel)
            // {
            //     DOTween.Kill(7);
            //     tapToScreenTextObject.transform.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack).OnComplete(() =>
            //     {
            //         tapToScreenTextObject.SetActive(false);
            //     });
            // }
            isInputDisabled = true;
        }

        // private void SetTapTimingBar(int index)
        // {
        //     foreach (var tapTimingBar in tapTimingBars)
        //         tapTimingBar.gameObject.SetActive(false);
        //
        //     tapTimingBars[index].SetActive(true);
        // }

        #endregion

        #region Incoming Receive Events

        private void OnLevelCreated()
        {
            // if (gameModel.IsTutorialLevel)
            //     targetTapTimingBarTypeForTutorial = TapTimingBarTypes.Green;
        }

        private void StartTheFalling()
        {
            UI.SetActive(true);
        }

        private void StartGamePlay()
        {
            Broadcast(new CameraTransisitonEventArgs(CameraTransitionTypesEnum.RunnerCamera, true, 2));
        }

        private void OnInputTap()
        {
            if (isStopGame || isInputDisabled)
                return;

            StartTapTimingBarAction(tapTimingBarIndicatorItem.GetTapTimingBarType());
        }

        private void OnPlayerCollidedPlanet()
        {
            tapTimingBarIndicatorItem.SetMovementAnimEnabled(false);

            currentPlanetItem.Crack();
            currentPlanetItem.Break();
        }

        private void OnLevelComplated()
        {
            isStopGame = true;
        }

        #endregion

        #region IEnumerators

        #endregion
    }
}