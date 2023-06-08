using System.Collections.Generic;
using DG.Tweening;
using FluffyUnderware.Curvy.Controllers;
using Framework.Core;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Helpers;
using GameFolders.Scripts.Models;
using GameFolders.Scripts.Objects;
using Lean.Touch;
using Unity.Mathematics;
using UnityEngine;

namespace GameFolders.Scripts.Managers.MidLevelManagers
{
    public class PlayerManager : BaseManager
    {
        #region Public Variables

        #endregion

        #region Const Variables

        #endregion

        #region Properties

        private float SetSpeed
        {
            get => player.SplineController.Speed;
            set
            {
                player.SplineController.Speed = value;
                splineController.Speed = value;
            }
        }

        public int SetPowerTitle
        {
            set
            {
                if (value <= 4)
                {
                    player.PowerTitleText.text = ((PowerTitles)value).ToString().Replace("_", " ");
                    _gameModel.CharacterPowerTitleIndex = value;
                }
            }
        }

        public int PowerTitleBarValueRelegation
        {
            set
            {
                if (DOTween.IsTweening(11))
                {
                    powerTitleBarValueRelegationQueue.Enqueue(value);
                    return;
                }


                if (player.PowerTitleBarValue == 0)
                {
                    player.PowerTitleBarValue = 10;
                    player.PowerTitleBarSizeDeltaX = 10;
                    if (_gameModel.CharacterPowerTitleIndex > 0)
                        SetPowerTitle = _gameModel.CharacterPowerTitleIndex - 1;
                }

                player.PowerTitleBarValue -= value;

                if (player.PowerTitleBarValue < 0)
                {
                    powerTitleBarValueRelegationQueue.Enqueue(Mathf.Abs(player.PowerTitleBarValue));
                    player.PowerTitleBarValue = 0;
                }


                player.SetSmoothPowerTitleBarValue(player.PowerTitleBarValue / 10f)
                    .OnComplete(() =>
                    {
                        if (player.PowerTitleBarValue <= 0)
                        {
                            SetPowerTitle = _gameModel.CharacterPowerTitleIndex - 1;
                            player.PowerTitleBarValue = 10;
                            player.PowerTitleBarSizeDeltaX = 10;
                        }

                        if (powerTitleBarValueRelegationQueue.Count > 0)
                            PowerTitleBarValueUpgrade = powerTitleBarValueRelegationQueue.Dequeue();
                    });
            }
        }

        public int PowerTitleBarValueUpgrade
        {
            set
            {
                if (_gameModel.CharacterPowerTitleIndex >= 4)
                    return;

                if (DOTween.IsTweening(11))
                {
                    powerTitleBarValueUpgradeQueue.Enqueue(value);
                    return;
                }

                player.PowerTitleBarValue += value;

                if (player.PowerTitleBarValue > 10)
                {
                    powerTitleBarValueUpgradeQueue.Enqueue(player.PowerTitleBarValue - 10);
                    player.PowerTitleBarValue = 10;
                }

                player.SetSmoothPowerTitleBarValue(player.PowerTitleBarValue / 10f)
                    .OnComplete(() =>
                    {
                        if (player.PowerTitleBarValue >= 10)
                        {
                            SetPowerTitle = _gameModel.CharacterPowerTitleIndex + 1;
                            if (_gameModel.CharacterPowerTitleIndex < 4)
                            {
                                player.PowerTitleBarValue = 0;
                                player.PowerTitleBarSizeDeltaX = 0;
                            }
                            else
                            {
                                player.PowerTitleBarValue = 10;
                                return;
                            }
                        }

                        if (powerTitleBarValueUpgradeQueue.Count > 0)
                            PowerTitleBarValueUpgrade = powerTitleBarValueUpgradeQueue.Dequeue();
                    });
            }
        }

        #endregion

        #region Private Variables

        [SerializeField] [Range(5, 15)] private float initializeSpeed;

        [SerializeField] [Range(20, 100)] private int boostDistance = 60;

        [SerializeField] private Player player;

        [SerializeField] private SplineController splineController;

        private Queue<int> powerTitleBarValueUpgradeQueue = new Queue<int>();
        private Queue<int> powerTitleBarValueRelegationQueue = new Queue<int>();

        private bool isPlayerRunning, isDie, isBosstedRunActivated;

        private int fuelAmount, currentBoostDistance;

        private float boostStartPos;

        private GameModel _gameModel;

        #endregion

        #region Actions

        #endregion

        #region Monobehaviour Methods

        private void FixedUpdate()
        {
            // player.transform.rotation = Quaternion.Euler(0,
            //     player.transform.rotation.eulerAngles.y + player.TheWillBeAddRotationY, 0);

            if (isBosstedRunActivated)
            {
                if (boostStartPos + currentBoostDistance <= player.SplineController.Position)
                    SetBoostedRunActivate(false);
            }
        }

        #endregion

        #region Begining

        protected override void Start()
        {
            base.Start();

            Begining();

            SetSpeed = initializeSpeed;
        }

        private void Begining()
        {
            player.PlayerIsOnCollidedEvent += PlayerIsOnCollided;
        }

        #endregion

        #region Implemented Methods

        public override void Receive(BaseEventArgs baseEventArgs)
        {
            switch (baseEventArgs)
            {
                case OnCharacterBodyPartUpdatedEventArgs onCharacterBodyPartUpdatedEventArgs:
                    OnCharacterBodyPartUpdated(onCharacterBodyPartUpdatedEventArgs);
                    break;
                case PlayButtonClickedEventArgs playButtonClickedEventArgs:
                    StartFalling();
                    break;
                case OnPlanetFightActionEventArgs fightEndedEventArgs:
                    OnPlanetFightAction(fightEndedEventArgs);
                    break;
                case LeanOnDeltaEventArgs leanOnDeltaEventArgs:
                    LeanOnDelta(leanOnDeltaEventArgs.Delta, leanOnDeltaEventArgs.IsSwerve);
                    break;
                case LeanOnFingerEventArgs leanOnFingerEventArgs:
                    LeanOnFinger(leanOnFingerEventArgs.Finger);
                    break;
                case OnLevelCreatedEventArgs onLevelCreatedEventArgs:
                    OnLevelCreated(onLevelCreatedEventArgs);
                    break;
            }
        }

        public void InjectModel(GameModel gameModel)
        {
            _gameModel = gameModel;

            Broadcast(new GetCharacterBodyPartsTotalPowerScoreEventArgs(i =>
                player.TotalPowerScoreShower = i));

            SetPowerTitle = _gameModel.CharacterPowerTitleIndex;
            if (_gameModel.CharacterPowerTitleIndex >= 4)
            {
                player.PowerTitleBarValue = 10;
                player.PowerTitleBarSizeDeltaX = 10;
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        private void OnDamageTakenPlayer(int damage)
        {
            if (damage == 0)
                return;
            Broadcast(new OnDamageTakenPlayerEventArgs(damage));
        }

        private void Die(bool isRunner)
        {
            player.Die(isRunner);
            BaseEventArgs eventArgs = new OnLevelComplatedEventArgs(false);
            Broadcast(eventArgs);
            BroadcastUpward(eventArgs);
            isDie = true;
            if (isRunner)
            {
                player.SplineController.Pause();
                splineController.Pause();
            }
        }

        private void PlanetCollided()
        {
            if (!isDie)
            {
                //Broadcast(new CameraTransisitonEventArgs(CameraTransitionTypesEnum.FallingCamera, true));
                player.GoDefFallingPos().OnComplete(() => { player.CapsuleCollider.enabled = true; });
            }
        }

        private void InteractableItemInteractAction(InteractableItem item)
        {
            switch (item.ItemType)
            {
                case InteractableItemTypes.Alien:
                    VFXManager.Instance.CreateParticle(VFXTypesEnum.AlienHit, player.Container.position);

                    if (isBosstedRunActivated)
                    {
                        item.FallToPlatformOut();
                        break;
                    }

                    if (DOTween.IsTweening(11))
                        DOTween.Complete(11);

                    if (_gameModel.CharacterPowerTitleIndex == 0 && player.PowerTitleBarValue <= item.PowerTitleValue)
                    {
                        Die(true);
                        break;
                    }

                    if (_gameModel.CharacterPowerTitleIndex == (int)item.PowerTitle)
                    {
                        PowerTitleBarValueRelegation = item.PowerTitleValue;
                        OnDamageTakenPlayer(item.PodyPartDamageValue);
                    }
                    else if (_gameModel.CharacterPowerTitleIndex < 4)
                        PowerTitleBarValueUpgrade = item.PowerTitleValue;

                    item.FallToPlatformOut();
                    break;
                case InteractableItemTypes.Obstacle:
                    VFXManager.Instance.CreateParticle(VFXTypesEnum.AlienHit, player.Container.position);
                    if (isBosstedRunActivated)
                    {
                        item.FallToPlatformOut();
                        break;
                    }

                    if (DOTween.IsTweening(11))
                        DOTween.Complete(11);

                    if (_gameModel.CharacterPowerTitleIndex == 0 && player.PowerTitleBarValue <= item.PowerTitleValue)
                    {
                        Die(true);
                        break;
                    }

                    PowerTitleBarValueRelegation = item.PowerTitleValue;
                    OnDamageTakenPlayer(item.PodyPartDamageValue);
                    item.Break();

                    break;
                case InteractableItemTypes.Fuel:
                    VFXManager.Instance.CreateParticle(VFXTypesEnum.ItemCollected, item.GetPos());
                    if (isBosstedRunActivated)
                    {
                        item.DoScaleToZero();
                        break;
                    }

                    fuelAmount++;
                    BroadcastUpward(new OnPlayerCollectedFuelEventArgs(fuelAmount));
                    if (fuelAmount == 10)
                    {
                        fuelAmount = 0;
                        SetBoostedRunActivate();
                    }


                    // item.DoScaleToZero();
                    item.gameObject.SetActive(false);
                    break;
                case InteractableItemTypes.Coin:
                    VFXManager.Instance.CreateParticle(VFXTypesEnum.CoinCollected, item.GetPos());
                    _gameModel.Money += 100;
                    item.gameObject.SetActive(false);
                    break;
                case InteractableItemTypes.TitleUpgradeItems:
                    VFXManager.Instance.CreateParticle(VFXTypesEnum.AlienHit, player.Container.position);
                    if (isBosstedRunActivated)
                    {
                        item.FallToPlatformOut();
                        break;
                    }

                    if (DOTween.IsTweening(11))
                        DOTween.Complete(11);

                    PowerTitleBarValueUpgrade = item.PowerTitleValue;
                    item.Break();
                    break;
            }
        }

        private void SetBoostedRunActivate(bool isActive = true)
        {
            if (isActive)
            {
                isBosstedRunActivated = true;
                boostStartPos = player.SplineController.Position;
                Broadcast(new GetCharacterBodyPartItemLevelAverageEventArgs(
                    i => { currentBoostDistance = boostDistance + (i * 10); },
                    CharacterBodyPartTypes.Leg));
                SetSpeed += 5;
                BroadcastUpward(new OnPlayerCollectedFuelEventArgs(isBosstedRunActivated));
                player.FallingParticle.transform.parent = splineController.transform;
                player.SetBoostedRunActivate();
            }
            else
            {
                isBosstedRunActivated = false;
                boostStartPos = 0;
                SetSpeed = initializeSpeed;
                currentBoostDistance = boostDistance;
                BroadcastUpward(new OnPlayerCollectedFuelEventArgs(0));
                player.SetBoostedRunActivate(false);
            }
        }

        private void SplineEndReached(CurvySplineMoveEventArgs arg)
        {
            if (isBosstedRunActivated)
                SetBoostedRunActivate(false);

            isPlayerRunning = false;

            player.SplineController.Pause();
            splineController.Pause();
            player.JetpackParticlesEnabled(false);

            player.SplineController.enabled = false;
            splineController.enabled = false;

            player.transform.rotation = Quaternion.Euler(0, 0, 0);


            Broadcast(new CameraTransisitonEventArgs(CameraTransitionTypesEnum.LevelEndTransition, true));

            player.StartPlanetFight().OnComplete(() =>
            {
                player.transform.position = new Vector3(0, -20, 0);
                Broadcast(new StartThePlanetFightEventArgs());
                // Broadcast(new CameraTransisitonEventArgs(CameraTransitionTypesEnum.FallingCamera,
                //     new Vector3(0, -30, 30), Quaternion.Euler(-18.435f, 180, 0)));

                Broadcast(new CameraTransisitonEventArgs(CameraTransitionTypesEnum.LevelEndTransition,
                    new Vector3(0, -5, 30), Quaternion.Euler(35, 180, 0)));
                Broadcast(new CameraTransisitonEventArgs(CameraTransitionTypesEnum.FightCamera, true));
            });
        }

        #endregion

        #region Incoming Player Class Actions

        private void PlayerIsOnCollided(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Obstacle"))
            {
                player.CapsuleCollider.enabled = false;

                player.SetAnimSpeed(1);

                Broadcast(new CameraShakeEventArgs());
                CoroutineController.DoAfterGivenTime(.25f, () =>
                {
                    PlanetCollided();
                    Broadcast(new OnPlayerCollidedPlanetEventArgs());

                    CoroutineController.DoAfterGivenTime(.75f, () =>
                    {
                        BaseEventArgs eventArgs = new OnLevelComplatedEventArgs(true);
                        Broadcast(eventArgs);
                        BroadcastUpward(eventArgs);
                    });
                });
            }
            else if (other.gameObject.layer == LayerMask.NameToLayer("InteractableItem"))
            {
                InteractableItem item = other.GetComponent<InteractableItem>();
                InteractableItemInteractAction(item);
            }
        }

        #endregion

        #region Incoming Receive Events

        private void OnLevelCreated(OnLevelCreatedEventArgs eventArgs)
        {
            player.SplineController.Spline = splineController.Spline = eventArgs.Spline;
        }

        private void OnCharacterBodyPartUpdated(OnCharacterBodyPartUpdatedEventArgs eventArgs)
        {
            player.SetCharacterWeaponEnabled(eventArgs.DirectionType, eventArgs.BodyPartLevel);
            player.TotalPowerScoreShower = eventArgs.BodyPartTotalPowerScore;
        }

        private void StartFalling()
        {
            player.FallingJump().OnComplete(() =>
            {
                VFXManager.Instance.CreateParticle(VFXTypesEnum.PlayerBossFightRegionLanded, player.transform.position);
                Broadcast(new CameraShakeEventArgs());
                player.LandDownRunnerPlatform();
                // CoroutineController.DoAfterGivenTime(.15f, () =>
                // {
                player.SplineController.enabled = true;
                splineController.enabled = true;
                player.SplineController.Play();
                splineController.Play();
                isPlayerRunning = true;
                // });
                player.SplineController.OnEndReached.AddListener(SplineEndReached);
            });
        }

        private void OnPlanetFightAction(OnPlanetFightActionEventArgs eventArgs)
        {
            switch (eventArgs.PlayerReaction)
            {
                case InputTypes.Tap:
                    player.BreakPlanet();
                    break;
            }
        }

        private void LeanOnDelta(Vector2 delta, bool isSwerve)
        {
            if (!isPlayerRunning || !isSwerve)
                return;
            player.SetSwerveMovement(delta.x);
        }

        private void LeanOnFinger(LeanFinger finger)
        {
            if (!isPlayerRunning)
                return;

            if (finger.Up)
                player.DoSmoothRotationY(0);
        }

        #endregion

        #region IEnumerators

        #endregion
    }
}