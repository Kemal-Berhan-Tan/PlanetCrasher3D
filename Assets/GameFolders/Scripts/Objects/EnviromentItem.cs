using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GameFolders.Scripts.Objects
{
    public class EnviromentItem : MonoBehaviour
    {
        [SerializeField] private bool isRotate, isIdleMovementY;

        private void Start()
        {
            DoIdleEnabled();
        }

        public void DoIdleEnabled(bool isEnabled = true)
        {
            Transform meshTransform = transform.GetChild(0);
            if (!isEnabled)
            {
                DOTween.Kill(meshTransform);
                return;
            }

            float randomTime = 0;
            randomTime = Random.Range(.25f, .75f);
            if (isIdleMovementY)
            {
                meshTransform.DOMoveY(5, 3).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(randomTime);
            }

            if (isRotate)
            {
                Vector3 rot = new Vector3(meshTransform.rotation.eulerAngles.x,
                    meshTransform.rotation.eulerAngles.y - 90, meshTransform.rotation.eulerAngles.z);
                meshTransform.DOLocalRotate(rot, 5).SetLoops(-1, LoopType.Incremental)
                    .SetEase(Ease.Linear).SetDelay(randomTime);
            }
        }
    }
}