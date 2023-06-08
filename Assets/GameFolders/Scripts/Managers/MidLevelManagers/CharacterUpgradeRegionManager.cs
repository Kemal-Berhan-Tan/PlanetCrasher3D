using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DG.Tweening;
using Framework.Core;
using Framework.Utility;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Helpers;
using GameFolders.Scripts.Models;
using GameFolders.Scripts.Objects;
using Lean.Common;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameFolders.Scripts.Managers.MidLevelManagers
{
    public class CharacterUpgradeRegionManager : BaseManager
    {
        #region Public Variables

        #endregion

        #region Const Variables

        private const string prefkey_Stands = "Stand";
        private const string prefkey_Slots = "Slot";

        #endregion

        #region Properties

        private bool IsEnabledInputBlocker
        {
            get => isEnabledInputBlocker;
            set
            {
                foreach (var item in allUsedItems)
                {
                    item.SetSelectable(!value);
                }
            }
        }

        private CharacterBodyPartItemStand GetSlotClosestToTheTargetTransform(Transform targetTransform)
        {
            CharacterBodyPartItemStand closestItemStand = allStands.Select(item =>
                    new { n = item, distance = Vector3.Distance(item.transform.position, targetTransform.position) })
                .OrderBy(p => p.distance)
                .First().n;

            float distance = Vector3.Distance(closestItemStand.transform.position, targetTransform.position);
            if (distance > 5)
                return null;
            else
                return closestItemStand;
        }

        public int GetBodyPartsTotalPowerScore
        {
            get
            {
                int totalPowerScore = 0;
                foreach (var slot in characterBodyPartAssignmentSlots)
                {
                    if (!slot.CurrentItem)
                    {
                        totalPowerScore += listOfBodyParPowerScorePerLevel[0];
                    }
                    else
                        totalPowerScore += slot.CurrentItem.BodyPartPowerScore;
                }

                return totalPowerScore;
            }
        }

        public float LabCricleBlendshape
        {
            get { return LabCricle.GetBlendShapeWeight(0); }
            set { LabCricle.SetBlendShapeWeight(0, value); }
        }

        #endregion

        #region Private Variables

        //[SerializeField] private GameObject fallingRegion;
        [SerializeField] private GameObject characterUpgradeRegion, labEnviroment;
        [SerializeField] private SkinnedMeshRenderer LabCricle;
        [SerializeField] private Transform bodyPartItemsParent;
        [SerializeField] private Button buyCharacterBodyPartButton, playButton;
        [SerializeField] private TextMeshProUGUI buyCharacterBodyPartButtonText;
        [SerializeField] private Image handGuestureImage;
        [SerializeField] private Material standMaterial_Arm, standMaterial_Leg;
        [SerializeField] LeanPlane leanPlane;
        [SerializeField] private List<int> listOfBodyParPowerScorePerLevel = new List<int>();
        [SerializeField] private List<CharacterBodyPartItem> characterBodyArmPartItemPrefabs;
        [SerializeField] private List<CharacterBodyPartItem> characterBodyLegPartItemPrefabs;
        [SerializeField] private List<CharacterBodyPartItemStand> characterBodyPartStands;

        [SerializeField] private List<CharacterBodyPartItemStand> characterBodyPartAssignmentSlots;
        //[SerializeField] private List<CharacterBodyPartItemStand> characterBodyPartFallingSlots;

        private List<CharacterBodyPartItem> allUsedItems = new List<CharacterBodyPartItem>();

        private List<CharacterBodyPartItemStand> allStands = new List<CharacterBodyPartItemStand>();

        private List<CharacterBodyPartDirectionTypes> willBeResetStandTypes =
            new List<CharacterBodyPartDirectionTypes>();
        //private List<CharacterBodyPartItemStand> activeFallingItems = new List<CharacterBodyPartItemStand>();

        private GameModel gameModel;

        private CharacterBodyPartItemStand standOfCurrentSelectedItem;

        private bool isEnabledInputBlocker = false, isHandGuestureTutorialShown;

        private int currentStepIndexTheHandGesture;

        private WillBeResetStandsListHolder WillBeResetStandTypesHolder;

        #endregion

        #region Actions

        #endregion

        #region Begining

        protected override void Start()
        {
            base.Start();

            buyCharacterBodyPartButton.onClick.AddListener(BuyCharacterBodyPart);
            allStands.AddRange(characterBodyPartStands);
            allStands.AddRange(characterBodyPartAssignmentSlots);

            GameObject tempObject = GameObject.Find("WillBeResetStandsListHolder");
            if (tempObject)
            {
                WillBeResetStandTypesHolder = tempObject.GetComponent<WillBeResetStandsListHolder>();
                isEnabledInputBlocker = true;
                willBeResetStandTypes.AddRange(WillBeResetStandTypesHolder.standDirectionTypes);
                CoroutineController.DoAfterGivenTime(.5f, () =>
                    StartCoroutine(StartResetTheStandsWithDelay()));
            }
        }

        private void Begining()
        {
            if (PlayerPrefs.HasKey($"{prefkey_Stands}ItemsJson"))
                LoadTheBodyPartItem(characterBodyPartStands, prefkey_Stands);
            if (PlayerPrefs.HasKey($"{prefkey_Slots}ItemsJson"))
                LoadTheBodyPartItem(characterBodyPartAssignmentSlots, prefkey_Slots);

            characterUpgradeRegion.SetActive(true);
            //fallingRegion.SetActive(false);
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
                case ResetTheManagersEventArgs resetTheManagersEventArgs:
                    ResetTheManagers();
                    break;
                case PlayButtonClickedEventArgs playButtonClickedEventArgs:
                    StartFalling();
                    break;
                case OnDamageTakenPlayerEventArgs onDamageTakenPlayerEventArgs:
                    DamageTakenBodyPart(onDamageTakenPlayerEventArgs.Damage);
                    break;
                case GetCharacterBodyPartsTotalPowerScoreEventArgs getCharacterBodyPartsTotalPowerScoreEventArgs:
                    getCharacterBodyPartsTotalPowerScoreEventArgs.Callback?.Invoke(GetBodyPartsTotalPowerScore);
                    break;
                case GetCharacterBodyPartItemLevelAverageEventArgs eventArgs:
                    eventArgs.Callback?.Invoke(GetBodyPartItemLevelAverage(eventArgs.BodyPartType));
                    break;
            }
        }

        public void InjectModel(GameModel gameModel)
        {
            this.gameModel = gameModel;

            SetBuyCharacterBodyPartButtonText(this.gameModel.CharacterBodyPartBuyPrice);

            this.gameModel.PropertyChanged += OnGameModelPropertyChanged;

            Begining();
        }

        private void OnGameModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(gameModel.CharacterBodyPartBuyPrice))
            {
                SetBuyCharacterBodyPartButtonText(gameModel.CharacterBodyPartBuyPrice);
                CheckBuyBodyPartButtonInteractable();
            }
        }

        #endregion

        #region Public Methods

        #endregion

        #region Private Methods

        private void BuyCharacterBodyPart()
        {
            if (gameModel.Money < gameModel.CharacterBodyPartBuyPrice && IsEnabledInputBlocker)
                return;
            GenerateTheBodyPartToEmptyStand(0);
            gameModel.Money -= gameModel.CharacterBodyPartBuyPrice;
            gameModel.CharacterBodyPartBuyPrice += 50;
            if (isHandGuestureTutorialShown)
            {
                currentStepIndexTheHandGesture++;
                NextStepToHandGestureTutorial();
            }
        }

        private void GenerateTheBodyPartToEmptyStand(int bodyPartLevel)
        {
            CharacterBodyPartTypes bodyPartType = CharacterBodyPartTypes.None;
            if (isHandGuestureTutorialShown)
                bodyPartType = CharacterBodyPartTypes.Arm;
            else
                bodyPartType = (CharacterBodyPartTypes)Random.Range(1, 3);
            CharacterBodyPartItemStand itemStand =
                characterBodyPartStands.First(i => i.IsSlotEmpty);
            SetBodyPartStand(itemStand, GenerateBodyPart(bodyPartType, 0), false);
        }

        private void SetBodyPartStand(CharacterBodyPartItemStand itemStand, CharacterBodyPartItem item,
            bool isSmoothMove = true)
        {
            Material tempMaterial =
                item.BodyPartType == CharacterBodyPartTypes.Arm ? standMaterial_Arm : standMaterial_Leg;

            itemStand.RemaningHP = item.BodyPartLevel + 2;

            Vector3 targetPosition = itemStand.GetItemPlacePos();
            if (isSmoothMove)
            {
                IsEnabledInputBlocker = true;
                item.DoMoveTargetPos(targetPosition)
                    .OnComplete(() =>
                    {
                        itemStand.StandFill(item, tempMaterial);
                        CheckBuyBodyPartButtonInteractable();
                        if (itemStand.BodyPartAssignmentSlotDirectionType != CharacterBodyPartDirectionTypes.None)
                            OnCharacterBodyPartUpdated(itemStand.BodyPartAssignmentSlotDirectionType,
                                item.BodyPartLevel);
                        IsEnabledInputBlocker = false;
                    });
            }
            else
            {
                item.transform.position = targetPosition;
                itemStand.StandFill(item, tempMaterial);
                CheckBuyBodyPartButtonInteractable();
                if (itemStand.BodyPartAssignmentSlotDirectionType != CharacterBodyPartDirectionTypes.None)
                    OnCharacterBodyPartUpdated(itemStand.BodyPartAssignmentSlotDirectionType,
                        item.BodyPartLevel);
            }
        }

        private void ResetTheBodyPartItemStand(CharacterBodyPartItemStand ıtemStand)
        {
            CharacterBodyPartDirectionTypes tempDirectionType = ıtemStand.BodyPartAssignmentSlotDirectionType;
            ıtemStand.ResetItem();

            if (tempDirectionType != CharacterBodyPartDirectionTypes.None)
                OnCharacterBodyPartUpdated(tempDirectionType, -1);
        }

        private void ResetTheBodyPartItem(CharacterBodyPartItem item)
        {
            item.IsSelectingChangedEvent -= OnBodyPartItemSelectingChanged;
            if (allUsedItems.Contains(item))
                allUsedItems.Remove(item);
            Destroy(item.gameObject);
        }

        private void SetBuyCharacterBodyPartButtonText(int price)
        {
            buyCharacterBodyPartButtonText.text = AbbrevationUtility.AbbreviateNumber(price);
        }

        private void OnCharacterBodyPartUpdated(CharacterBodyPartDirectionTypes directionType, int level)
        {
            Broadcast(new OnCharacterBodyPartUpdatedEventArgs(directionType, level, GetBodyPartItemLevelAverage(),
                GetBodyPartsTotalPowerScore));

            SaveTheBodyPartItem(characterBodyPartAssignmentSlots, prefkey_Slots);
        }

        private void CheckBuyBodyPartButtonInteractable()
        {
            if (isHandGuestureTutorialShown)
                return;
            buyCharacterBodyPartButton.interactable = gameModel.Money >= gameModel.CharacterBodyPartBuyPrice &&
                                                      characterBodyPartStands.Any(i => i.IsSlotEmpty);
        }

        // private void SetBodyPartFallingItems()
        // {
        //     foreach (var fallingItem in characterBodyPartFallingSlots)
        //     {
        //         ResetTheBodyPartItemStand(fallingItem);
        //         fallingItem.transform.gameObject.SetActive(false);
        //     }
        //
        //     activeFallingItems.Clear();
        //
        //     int i = 0;
        //     foreach (var refItem in characterBodyPartAssignmentSlots)
        //     {
        //         CharacterBodyPartItemStand targetItemStand = characterBodyPartFallingSlots[i];
        //         if (!refItem.IsSlotEmpty)
        //         {
        //             targetItemStand.transform.gameObject.SetActive(true);
        //             SetBodyPartStand(targetItemStand, refItem.CurrentItem, false);
        //             targetItemStand.RemaningHP = 2 + targetItemStand.CurrentItem.BodyPartLevel;
        //             activeFallingItems.Add(targetItemStand);
        //             i++;
        //         }
        //     }
        // }

        private int GetBodyPartItemLevelAverage(CharacterBodyPartTypes type = CharacterBodyPartTypes.None)
        {
            if (characterBodyPartAssignmentSlots.Any(i => i.IsSlotEmpty))
                return 0;

            List<CharacterBodyPartItemStand> tempList = new List<CharacterBodyPartItemStand>();
            if (type == CharacterBodyPartTypes.None)
                tempList.AddRange(characterBodyPartAssignmentSlots);
            else
            {
                if (characterBodyPartAssignmentSlots.Any(i => i.CurrentItem.BodyPartType == type))
                    return 0;
                else
                {
                    tempList.AddRange(characterBodyPartAssignmentSlots.Where(i =>
                        i.BodyPartAssignmentSlotType == type));
                }
            }

            int levelSum = 0;
            int itemCount = 0;
            foreach (var slot in tempList)
            {
                if (!slot.IsSlotEmpty)
                {
                    levelSum += slot.CurrentItem.BodyPartLevel;
                    itemCount++;
                }
            }

            return levelSum / itemCount;
        }

        private void OnApplicationQuit()
        {
            SaveTheBodyPartItem(characterBodyPartStands, prefkey_Stands);
            SaveTheBodyPartItem(characterBodyPartAssignmentSlots, prefkey_Slots);
        }

        private void StartTheTutorial()
        {
            foreach (var slot in characterBodyPartAssignmentSlots)
            {
                SetBodyPartStand(slot, GenerateBodyPart(slot.BodyPartAssignmentSlotType, 3), false);
            }

            CoroutineController.DoAfterGivenTime(.25f, () =>
            {
                BaseEventArgs eventArgs = new OnStartTheTutorialLevelEventArgs();
                //Broadcast(eventArgs);
                BroadcastUpward(eventArgs);
            });
        }

        private void NextStepToHandGestureTutorial(CharacterBodyPartItem item = null)
        {
            Vector3 tempPos = Vector3.zero;
            switch (currentStepIndexTheHandGesture)
            {
                case 0:
                    tempPos = buyCharacterBodyPartButton.transform.position;
                    tempPos.y -= 75;
                    handGuestureImage.transform.position = tempPos;
                    handGuestureImage.transform.localScale = Vector3.one;
                    handGuestureImage.gameObject.SetActive(true);
                    handGuestureImage.transform.DOScale(Vector3.one * 1.25f, .5f).SetLoops(-1, LoopType.Yoyo).SetId(8);
                    // DOVirtual.DelayedCall(1.5f, () => { handGuestureImage.sprite = defHandGuestureSprite; })
                    //     .SetLoops(-1, LoopType.Restart).SetId(8);
                    break;
                case 2:
                    DOTween.Kill(8);
                    buyCharacterBodyPartButton.interactable = false;
                    tempPos = Camera.main.WorldToScreenPoint(characterBodyPartStands[0].transform.position);
                    Vector3 tempPos_1 = Camera.main.WorldToScreenPoint(characterBodyPartStands[1].transform.position);
                    handGuestureImage.transform.position = tempPos;
                    handGuestureImage.transform.localScale = Vector3.one;
                    handGuestureImage.transform.DOMove(tempPos_1, 1).SetLoops(-1).SetId(8);
                    break;
                case 3:
                    DOTween.Kill(8);
                    handGuestureImage.gameObject.SetActive(true);
                    tempPos = Camera.main.WorldToScreenPoint(item.transform.position);
                    Vector3 tempPos_2 =
                        Camera.main.WorldToScreenPoint(characterBodyPartAssignmentSlots[0].transform.position);
                    tempPos_2.y -= 75;
                    handGuestureImage.transform.position = tempPos;
                    handGuestureImage.transform.localScale = Vector3.one;
                    handGuestureImage.transform.DOMove(tempPos_2, 1).SetLoops(-1).SetId(8);
                    break;
                case 4:
                    DOTween.Kill(8);
                    playButton.interactable = true;
                    tempPos = playButton.transform.position;
                    tempPos.y -= 75;
                    handGuestureImage.transform.position = tempPos;
                    handGuestureImage.transform.localScale = Vector3.one;
                    handGuestureImage.transform.DOScale(Vector3.one * 1.25f, .5f).SetLoops(-1, LoopType.Yoyo).SetId(8);
                    playButton.onClick.AddListener(() =>
                    {
                        currentStepIndexTheHandGesture++;
                        NextStepToHandGestureTutorial();
                    });
                    break;
                case 5:
                    DOTween.Kill(8);
                    handGuestureImage.gameObject.SetActive(false);
                    isHandGuestureTutorialShown = false;
                    CheckBuyBodyPartButtonInteractable();
                    break;
            }
        }

        #endregion

        #region Incoming Character Body Part Item Class Actions

        private void OnBodyPartItemSelectingChanged(CharacterBodyPartItem item, bool isSelected)
        {
            if (isSelected)
            {
                //item.SetPosOffsetEnable(false);
                if (isHandGuestureTutorialShown)
                {
                    DOTween.Pause(8);
                    Vector3 tempPos = Vector3.zero;
                    if (currentStepIndexTheHandGesture == 3)
                    {
                        tempPos = Camera.main.WorldToScreenPoint(characterBodyPartAssignmentSlots[0].transform
                            .position);
                        tempPos.y -= 75;
                    }
                    else if (item != characterBodyPartStands[0].CurrentItem)
                        tempPos = Camera.main.WorldToScreenPoint(characterBodyPartStands[0].transform.position);
                    else if (item != characterBodyPartStands[1].CurrentItem)
                        tempPos = Camera.main.WorldToScreenPoint(characterBodyPartStands[1].transform.position);

                    handGuestureImage.transform.position = tempPos;
                    handGuestureImage.transform.DOScale(Vector3.one * 1.25f, .5f).SetLoops(-1, LoopType.Yoyo).SetId(8);
                }

                item.SetStandRotation(false);
                standOfCurrentSelectedItem = allStands.First(i => i.CurrentItem == item);
                ResetTheBodyPartItemStand(standOfCurrentSelectedItem);
            }
            else if (!isSelected)
            {
                CharacterBodyPartItemStand targetItemStand =
                    GetSlotClosestToTheTargetTransform(item.transform);

                if (!targetItemStand)
                {
                    SetBodyPartStand(standOfCurrentSelectedItem, item);
                    if (isHandGuestureTutorialShown)
                    {
                        handGuestureImage.gameObject.SetActive(true);
                        DOTween.Play(8);
                    }
                }
                else
                {
                    if (characterBodyPartAssignmentSlots.Contains(targetItemStand))
                    {
                        if (targetItemStand.BodyPartAssignmentSlotType ==
                            item.BodyPartType &&
                            (!isHandGuestureTutorialShown ||
                             (currentStepIndexTheHandGesture == 3 &&
                              (targetItemStand == characterBodyPartAssignmentSlots[0] ||
                               targetItemStand == characterBodyPartAssignmentSlots[1]))))
                        {
                            if (targetItemStand.IsSlotEmpty)
                                SetBodyPartStand(targetItemStand, item);
                            else
                            {
                                SetBodyPartStand(standOfCurrentSelectedItem, targetItemStand.CurrentItem);
                                ResetTheBodyPartItemStand(targetItemStand);
                                SetBodyPartStand(targetItemStand, item);
                            }

                            if (isHandGuestureTutorialShown)
                            {
                                currentStepIndexTheHandGesture++;
                                NextStepToHandGestureTutorial();
                            }
                        }
                        else
                        {
                            SetBodyPartStand(standOfCurrentSelectedItem, item);
                            if (isHandGuestureTutorialShown)
                            {
                                handGuestureImage.gameObject.SetActive(true);
                                DOTween.Play(8);
                            }
                        }
                    }
                    else
                    {
                        if (targetItemStand.IsSlotEmpty)
                        {
                            if (isHandGuestureTutorialShown)
                            {
                                SetBodyPartStand(standOfCurrentSelectedItem, item);
                                handGuestureImage.gameObject.SetActive(true);
                                DOTween.Play(8);
                            }
                            else
                                SetBodyPartStand(targetItemStand, item);
                        }
                        else if (item.BodyPartType == targetItemStand.CurrentItem.BodyPartType &&
                                 item.BodyPartLevel == targetItemStand.CurrentItem.BodyPartLevel &&
                                 targetItemStand.CurrentItem.BodyPartLevel < 3)
                        {
                            isEnabledInputBlocker = true;
                            item.DoMoveTargetPos(targetItemStand.GetItemPlacePos()).OnComplete(() =>
                            {
                                ResetTheBodyPartItem(targetItemStand.CurrentItem);
                                ResetTheBodyPartItemStand(targetItemStand);
                                CharacterBodyPartItem tempItem =
                                    GenerateBodyPart(item.BodyPartType, item.BodyPartLevel + 1);
                                SetBodyPartStand(targetItemStand, tempItem, false);
                                VFXManager.Instance.CreateParticle(VFXTypesEnum.BodyPartItemMerge,
                                    tempItem.transform.position);
                                ResetTheBodyPartItem(item);
                                isEnabledInputBlocker = false;

                                if (isHandGuestureTutorialShown)
                                {
                                    currentStepIndexTheHandGesture++;
                                    NextStepToHandGestureTutorial(tempItem);
                                }
                            });
                        }
                        else
                        {
                            SetBodyPartStand(standOfCurrentSelectedItem, item);
                            if (isHandGuestureTutorialShown)
                            {
                                handGuestureImage.gameObject.SetActive(true);
                                DOTween.Play(8);
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region Incoming Receive Events

        private void OnLevelCreated()
        {
            // if (gameModel.IsTutorialLevel)
            // {
            //     StartTheTutorial();
            // }
            // if (gameModel.ShowingLevelNumber == 2)
            // {
            //     isHandGuestureTutorialShown = true;
            //     playButton.interactable = false;
            //     NextStepToHandGestureTutorial();
            // }
        }

        private void ResetTheManagers()
        {
            if (willBeResetStandTypes.Count != 0)
            {
                if (!WillBeResetStandTypesHolder)
                {
                    GameObject tempObject = new GameObject();
                    tempObject.name = "WillBeResetStandsListHolder";
                    DontDestroyOnLoad(tempObject);
                    WillBeResetStandTypesHolder = tempObject.AddComponent<WillBeResetStandsListHolder>();
                }

                WillBeResetStandTypesHolder.standDirectionTypes.Clear();
                WillBeResetStandTypesHolder.standDirectionTypes.AddRange(willBeResetStandTypes);
            }
        }

        private void StartFalling()
        {
            if (IsEnabledInputBlocker)
                return;
            IsEnabledInputBlocker = true;
            DOTween.To(() => LabCricleBlendshape, x => LabCricleBlendshape = x, 100, .75f);
            //fallingRegion.SetActive(true);
            //SetBodyPartFallingItems();

            CoroutineController.DoAfterGivenTime(1.5f, () =>
            {
                characterUpgradeRegion.SetActive(false);
                foreach (var slot in characterBodyPartAssignmentSlots)
                {
                    if (slot.CurrentItem)
                        slot.CurrentItem.SetStandRotation(false);
                }

                labEnviroment.SetActive(false);
                SaveTheBodyPartItem(characterBodyPartStands, prefkey_Stands);
                SaveTheBodyPartItem(characterBodyPartAssignmentSlots, prefkey_Slots);
            });
        }

        private void DamageTakenBodyPart(int damage)
        {
            CharacterBodyPartItemStand lowerItemStand = null;
            try
            {
                lowerItemStand = characterBodyPartAssignmentSlots
                    .FindAll(x =>
                        x.CurrentItem != null & !willBeResetStandTypes.Contains(x.BodyPartAssignmentSlotDirectionType))
                    .Select(
                        item =>
                            new { n = item, hp = item.RemaningHP })
                    .OrderBy(p => p.hp)
                    .First().n;
            }
            catch
            {
                return;
            }

            foreach (var stand in characterBodyPartAssignmentSlots)
            {
                if (!stand.CurrentItem & willBeResetStandTypes.Contains(stand.BodyPartAssignmentSlotDirectionType))
                    continue;
                if (lowerItemStand.RemaningHP == stand.RemaningHP)
                    if (lowerItemStand.CurrentItem.BodyPartLevel > stand.CurrentItem.BodyPartLevel)
                        lowerItemStand = stand;
            }

            if (damage >= lowerItemStand.RemaningHP)
            {
                // DOTween.Kill(0);
                CharacterBodyPartItemStand stand = null;
                // try
                // {
                //     stand = characterBodyPartAssignmentSlots
                //         .First(stand =>
                //             stand.CurrentItem.BodyPartType == lowerItemStand.CurrentItem.BodyPartType);
                // }
                // catch (Exception e)
                // {
                //     return;
                // }

                // if (!lowerItemStand.CurrentItem)
                // {
                //     stand = characterBodyPartAssignmentSlots
                //         .First(stand =>
                //             stand.CurrentItem.BodyPartType == lowerItemStand.CurrentItem.BodyPartType);
                // }

                damage -= lowerItemStand.RemaningHP;

                Debug.Log($"Item Broken: {lowerItemStand.BodyPartAssignmentSlotDirectionType}");
                willBeResetStandTypes.Add(lowerItemStand.BodyPartAssignmentSlotDirectionType);
                // ResetTheBodyPartItem(lowerItemStand.CurrentItem);
                // ResetTheBodyPartItemStand(lowerItemStand, true);

                // if (gameModel.IsTutorialLevel)
                // DamageTakenBodyPart(damage);

                //SetBodyPartFallingItems();
            }
            else
            {
                lowerItemStand.RemaningHP -= damage;
                // if (lowerItemStand.RemaningHP <= 2)
                //     lowerItemStand.ImageAlphaBlink();
            }
        }

        // private void CloseTheBodyPartFallingItems()
        // {
        //     foreach (var item in activeFallingItems)
        //     {
        //         ResetTheBodyPartItemStand(item);
        //         item.transform.parent.gameObject.SetActive(false);
        //     }
        // }

        #endregion

        #region IEnumerators

        private IEnumerator StartResetTheStandsWithDelay(float delay = .5f)
        {
            int index = 0;
            for (int i = 0; i < willBeResetStandTypes.Count; i++)
            {
                CharacterBodyPartItemStand stand = characterBodyPartAssignmentSlots
                    .Find(x => x.BodyPartAssignmentSlotDirectionType == willBeResetStandTypes[i]);
                ResetTheBodyPartItem(stand.CurrentItem);
                ResetTheBodyPartItemStand(stand);
                yield return new WaitForSeconds(delay);
            }

            isEnabledInputBlocker = false;
        }

        #endregion

        #region Tools

        private CharacterBodyPartItem GenerateBodyPart(CharacterBodyPartTypes type, int bodyPartLevel)
        {
            if (type == CharacterBodyPartTypes.None)
                return null;

            CharacterBodyPartItem item = Instantiate(GetBodyPartPrefab(type, bodyPartLevel), bodyPartItemsParent);
            item.Initialize(leanPlane, listOfBodyParPowerScorePerLevel[bodyPartLevel + 1]);
            item.IsSelectingChangedEvent += OnBodyPartItemSelectingChanged;
            allUsedItems.Add(item);
            return item;
        }

        private CharacterBodyPartItem GetBodyPartPrefab(CharacterBodyPartTypes type, int bodyPartLevel)
        {
            if (type == CharacterBodyPartTypes.None)
                return null;

            List<CharacterBodyPartItem> tempItemPrefabs =
                new List<CharacterBodyPartItem>(type == CharacterBodyPartTypes.Arm
                    ? characterBodyArmPartItemPrefabs
                    : characterBodyLegPartItemPrefabs);

            return tempItemPrefabs.First(item => item.BodyPartLevel == bodyPartLevel);
        }

        private void SaveTheBodyPartItem(List<CharacterBodyPartItemStand> standList, string key)
        {
            // if (gameModel.IsTutorialLevel || isHandGuestureTutorialShown)
            //     return;

            BodyPartStandDatas datas = new BodyPartStandDatas();
            foreach (var stand in standList)
            {
                if (!stand.CurrentItem)
                    continue;
                BodyPartStandData data = new BodyPartStandData()
                {
                    BodyPartType = stand.CurrentItem.BodyPartType,
                    BodyPartLevel = stand.CurrentItem.BodyPartLevel,
                    BodyPartDirectionType = stand.BodyPartAssignmentSlotDirectionType
                };
                datas.BodyPartStandDataList.Add(data);
            }

            var dataJson = JsonConvert.SerializeObject(datas);
            PlayerPrefs.SetString($"{key}ItemsJson", dataJson);
        }

        private void LoadTheBodyPartItem(List<CharacterBodyPartItemStand> standList, string key)
        {
            // if (gameModel.IsTutorialLevel || gameModel.ShowingLevelNumber == 2)
            //     return;

            string dataJson = PlayerPrefs.GetString($"{key}ItemsJson", string.Empty);
            if (string.IsNullOrEmpty(dataJson))
                return;
            BodyPartStandDatas datas = JsonConvert.DeserializeObject<BodyPartStandDatas>(dataJson);
            if (datas.BodyPartStandDataList.Count == 0)
                return;

            int k = 0;
            for (int i = 0; i < standList.Count; i++)
            {
                if (datas.BodyPartStandDataList.Count <= k)
                    break;
                BodyPartStandData data = datas.BodyPartStandDataList[k];
                if (data.BodyPartType == CharacterBodyPartTypes.None ||
                    standList[i].BodyPartAssignmentSlotDirectionType !=
                    data.BodyPartDirectionType)
                    continue;
                SetBodyPartStand(standList[i], GenerateBodyPart(data.BodyPartType, data.BodyPartLevel), false);
                k++;
            }
        }

        #endregion
    }

    [Serializable]
    public class BodyPartStandDatas
    {
        public List<BodyPartStandData> BodyPartStandDataList = new List<BodyPartStandData>();
    }

    [Serializable]
    public class BodyPartStandData
    {
        public CharacterBodyPartTypes BodyPartType { get; set; }

        public CharacterBodyPartDirectionTypes BodyPartDirectionType { get; set; } =
            CharacterBodyPartDirectionTypes.None;

        public int BodyPartLevel { get; set; }
    }

    public class WillBeResetStandsListHolder : MonoBehaviour
    {
        public List<CharacterBodyPartDirectionTypes> standDirectionTypes =
            new List<CharacterBodyPartDirectionTypes>();
    }
}