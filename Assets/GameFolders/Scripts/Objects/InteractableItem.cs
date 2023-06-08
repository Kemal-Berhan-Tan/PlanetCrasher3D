using System;
using DG.Tweening;
using FluffyUnderware.Curvy.Controllers;
using GameFolders.Scripts.Enums;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace GameFolders.Scripts.Objects
{
    public class InteractableItem : MonoBehaviour
    {
        public PowerTitles PowerTitle;
        public TMP_Text PowerTitleText;

        public InteractableItemTypes ItemType;
        public int PowerTitleValue = 5;
        [FormerlySerializedAs("DamageValue")] public int PodyPartDamageValue = 200;
        public GameObject DefObject, BrokenObject, RigObject;

        private void Start()
        {
            if (ItemType == InteractableItemTypes.Alien && PowerTitleText)
                PowerTitleText.text = PowerTitle.ToString().Replace("_", " ");

            if (ItemType == InteractableItemTypes.Coin || ItemType == InteractableItemTypes.Fuel)
                DoIdleEnabled();
        }

        public void Break()
        {
            if (DefObject && BrokenObject)
            {
                DefObject.SetActive(false);
                BrokenObject.SetActive(true);
            }

            FallToPlatformOut();
        }

        public void FallToPlatformOut()
        {
            DoIdleEnabled(false);
            if (ItemType == InteractableItemTypes.Alien && RigObject)
                RigObject.SetActive(true);
            Vector3 tempPos = Random.Range(0, 2) == 0 ? transform.right : -transform.right;
            tempPos *= Random.Range(15f, 20f);
            tempPos += transform.forward * Random.Range(5f, 10f);
            tempPos += transform.position;
            // tempPos.z *= Random.Range(1f, 1.25f);
            // tempPos.x *= Random.Range(3f, 4.5f);
            tempPos.y = -5;
            transform.DORotateQuaternion(Random.rotation, 2);
            transform.DOJump(tempPos, 10, 1, 2).OnComplete(() => { DoScaleToZero(); });
        }

        public void DoScaleToZero()
        {
            if (ItemType == InteractableItemTypes.Alien && RigObject)
                RigObject.SetActive(false);
            transform.DOScale(Vector3.zero, .5f).SetEase(Ease.Linear);
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
            // randomTime = Random.Range(.25f, .75f);
            meshTransform.DOMoveY(0.5f, 1.5f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetDelay(randomTime);
            Vector3 rot = new Vector3(meshTransform.rotation.eulerAngles.x,
                meshTransform.rotation.eulerAngles.y - 90, meshTransform.rotation.eulerAngles.z);
            meshTransform.DOLocalRotate(rot, 2).SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear).SetDelay(randomTime);
        }

        public Vector3 GetPos()
        {
            return transform.GetChild(0).transform.position;
        }
    }
}