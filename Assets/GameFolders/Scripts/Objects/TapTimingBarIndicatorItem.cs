using System;
using DG.Tweening;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Helpers;
using UnityEngine;

namespace GameFolders.Scripts.Objects
{
    public class TapTimingBarIndicatorItem : MonoBehaviour
    {
        [SerializeField] private Animator animator;

        public TapTimingBarTypes targetStopType { get; set; }
        public TapTimingBarTypes TapTimingBarType { get; private set; }

        public event Action OnTapTimingBarStoppedEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            switch (other.gameObject.layer)
            {
                case 6:
                    TapTimingBarType = TapTimingBarTypes.Green;
                    break;
                case 7:
                    TapTimingBarType = TapTimingBarTypes.LightGreen;
                    break;
                case 8:
                    TapTimingBarType = TapTimingBarTypes.Red;
                    break;
            }

            if (targetStopType == TapTimingBarType)
            {
                OnTapTimingBarStoppedEvent?.Invoke();
                CoroutineController.DoAfterGivenTime(.1f, () => DOTween.Kill(1));
            }
        }

        public TapTimingBarTypes GetTapTimingBarType()
        {
            SetMovementAnimEnabled(false);
            return TapTimingBarType;
        }

        public void SetMovementAnimEnabled(bool isEnabled)
        {
            transform.parent.gameObject.SetActive(isEnabled);
            if (!isEnabled)
            {
                DOTween.Kill(1);
                return;
            }

            int random = UnityEngine.Random.Range(0, 2);
            int anan = random == 0 ? 40 : -40;
            transform.rotation = Quaternion.Euler(0, 0, anan * -1);
            transform.DORotate(new Vector3(0, 0, anan), 2f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetId(1);

            // animator.enabled = isEnabled;
        }
    }
}