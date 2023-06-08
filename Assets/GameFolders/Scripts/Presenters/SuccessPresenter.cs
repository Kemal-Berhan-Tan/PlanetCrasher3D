using DG.Tweening;
using Framework.Core;
using Framework.UI;
using GameFolders.Scripts.Events;
using GameFolders.Scripts.Helpers;
using GameFolders.Scripts.Views;
using UnityEngine;

namespace GameFolders.Scripts.Presenters
{
    public class SuccessPresenter : BasePresenter
    {
        public override void Receive(BaseEventArgs baseEventArgs)
        {
        }

        public override void ShowView()
        {
            base.ShowView();

            Vector3 rotation = (view as SuccessView).Beam.transform.rotation.eulerAngles;
            Vector3 rot = new Vector3(rotation.x, rotation.y, rotation.z - 90);
            (view as SuccessView).Beam.transform.DOLocalRotate(rot, 3).SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear).SetId(9);
        }

        public override void HideView()
        {
            base.HideView();

            DOTween.Kill(9);
        }

        public void NextLevelButtonHandler()
        {
            BroadcastUpward(new NextLevelButtonClickedEventArgs());
        }

        public void SetReward(int reward)
        {
            ((view as SuccessView).RewardText).text = $"Reward: {reward}";
        }

        public void ActivateBossMap()
        {
            (view as SuccessView).SuccessContainer.gameObject.SetActive(false);
            (view as SuccessView).BossMapContainer.gameObject.SetActive(true);
            (view as SuccessView).BossMapContainer.transform.DOScale(Vector3.one, .75f).SetEase(Ease.Linear)
                .OnComplete(
                    () =>
                    {
                        (view as SuccessView).BossRef.transform.DOScale(Vector3.one, .75f).SetEase(Ease.OutBack)
                            .SetDelay(.5f)
                            .OnComplete(() =>
                            {
                                CoroutineController.DoAfterGivenTime(.75f, () =>
                                {
                                    (view as SuccessView).BossMapContainer.gameObject.SetActive(false);
                                    (view as SuccessView).SuccessContainer.gameObject.SetActive(true);
                                });
                            });
                    });
        }
    }
}