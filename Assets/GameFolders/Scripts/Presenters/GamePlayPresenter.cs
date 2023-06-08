using DG.Tweening;
using Framework.Core;
using Framework.UI;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Views;
using UnityEngine;

namespace GameFolders.Scripts.Presenters
{
    public class GamePlayPresenter : BasePresenter
    {
        private float RoadMapSizeDeltaY
        {
            get { return (view as GamePlayView).RoadMapMask.sizeDelta.y; }
            set
            {
                (view as GamePlayView).RoadMapMask.sizeDelta =
                    new Vector2((view as GamePlayView).RoadMapMask.sizeDelta.x, value);
            }
        }

        private float FuelBarSizeDeltaY
        {
            get { return (view as GamePlayView).fuelBarMask.sizeDelta.y; }
            set
            {
                (view as GamePlayView).fuelBarMask.sizeDelta =
                    new Vector2((view as GamePlayView).fuelBarMask.sizeDelta.x, value);
            }
        }

        private float RoadMapContentPosY
        {
            get { return (view as GamePlayView).Content.localPosition.y; }
            set
            {
                (view as GamePlayView).Content.localPosition =
                    new Vector2((view as GamePlayView).Content.localPosition.x, value);
            }
        }

        public override void Receive(BaseEventArgs baseEventArgs)
        {
        }

        // public void SetCurrentMoney(int currentMoney)
        // {
        //     (view as GamePlayView).currentMoneyText.text = $"{AbbrevationUtility.AbbreviateNumber(currentMoney)}";
        // }

        public void SetCurrentShowingLevelNumber(int levelNumber)
        {
            (view as GamePlayView).currentShowingLevelText.text = $"LEVEL {levelNumber}";
        }

        public void NextLevelToRoadMap(int levelId)
        {
            (view as GamePlayView).Container.DOScale(Vector3.one, .75f).SetEase(Ease.OutBack).OnComplete
            (() =>
            {
                //(view as GamePlayView).RoadmapCloseButtonHandler.interactable = true;
                //(view as GamePlayView).Container.gameObject.SetActive(true);
                float tempY = RoadMapContentPosY - (150 * levelId);
                DOTween.To(() => RoadMapContentPosY, x => RoadMapContentPosY = x, tempY, 3 + (levelId * .25f))
                    .SetEase(Ease.OutBack);
                DOTween.To(() => RoadMapSizeDeltaY, x => RoadMapSizeDeltaY = x,
                    (view as GamePlayView).planetsMaskYPositions[levelId], 2 + (levelId * .25f)).OnComplete(() =>
                    CloseRoadMapButtonHandler());
            });
        }

        public void CloseRoadMapButtonHandler()
        {
            //(view as GamePlayView).RoadmapCloseButtonHandler.interactable = false;
            (view as GamePlayView).Container.DOScale(Vector3.zero, .5f).SetEase(Ease.InBack);
            BroadcastUpward(new StartTheGamePlayEventArgs());
        }

        // public void SetTurnoverMoney(int turnoverMoney)
        // {
        //     (view as GamePlayView).TurnoverMoney.text = $"${AbbrevationUtility.AbbreviateNumber(turnoverMoney)}";
        // }

        public void OnSettingsButtonClickHandler()
        {
            BroadcastUpward(new SettingsButtonClickedEventArgs());
        }

        public void OnRestartButtonClickHandler()
        {
            BroadcastUpward(new RestartLevelButtonClickedEventArgs());
        }

        public void UpdateFuelBar(int fuelAmount)
        {
            // if (fuelAmount == 0)
            // {
            //     FuelBarSizeDeltaY = 0;
            //     return;
            // }

            float amount = fuelAmount * 11;
            DOTween.To(x => FuelBarSizeDeltaY = x, FuelBarSizeDeltaY, amount, 1f).SetId(2);
        }

        public void FuelBarFull()
        {
            DOTween.Kill(2);
            FuelBarSizeDeltaY = 160;
        }
    }
}