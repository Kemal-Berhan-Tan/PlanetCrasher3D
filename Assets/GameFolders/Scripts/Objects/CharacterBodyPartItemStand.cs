using System;
using System.Collections.Generic;
using DG.Tweening;
using GameFolders.Scripts.Enums;
using TMPro;
using UnityEngine;

namespace GameFolders.Scripts.Objects
{
    public class CharacterBodyPartItemStand : MonoBehaviour
    {
        [SerializeField] private bool isHighStand;
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private CharacterBodyPartDirectionTypes bodyPartAssignmentSlotDirectionType;
        [SerializeField] private TMP_Text PartText;
        [SerializeField] private List<GameObject> startObjects;

        private Material defaultMaterial;

        public CharacterBodyPartDirectionTypes BodyPartAssignmentSlotDirectionType =>
            bodyPartAssignmentSlotDirectionType;

        public CharacterBodyPartItem CurrentItem { get; set; }
        public bool IsSlotEmpty { get; private set; } = true;

        public CharacterBodyPartTypes BodyPartAssignmentSlotType { get; private set; }
        public int RemaningHP { get; set; }

        private void Awake()
        {
            if (meshRenderer)
                defaultMaterial = meshRenderer.material;

            if (bodyPartAssignmentSlotDirectionType != CharacterBodyPartDirectionTypes.None)
                BodyPartAssignmentSlotType = bodyPartAssignmentSlotDirectionType.ToString()[0] == 'L'
                    ? CharacterBodyPartTypes.Leg
                    : CharacterBodyPartTypes.Arm;
        }

        public void StandFill(CharacterBodyPartItem item, Material material)
        {
            CurrentItem = item;
            //item.transform.parent = transform;
            IsSlotEmpty = false;
            if (meshRenderer)
            {
                meshRenderer.material = material;
                //SmoothMaterialChange(material);
            }

            if (bodyPartAssignmentSlotDirectionType == CharacterBodyPartDirectionTypes.None)
            {
                PartText.text = item.BodyPartType.ToString().ToUpper();
                if (item.BodyPartLevel >= 1)
                    startObjects[item.BodyPartLevel - 1].gameObject.SetActive(true);
                item.SetStandRotation(false);
            }
            else
            {
                item.SetStandRotation(true);
            }
        }

        public void ResetItem()
        {
            RemaningHP = 0;
            IsSlotEmpty = true;
            CurrentItem = null;
            if (meshRenderer)
            {
                meshRenderer.material = defaultMaterial;
                //SmoothMaterialChange(defaultMaterial);
                PartText.text = String.Empty;
                startObjects.ForEach(x => x.gameObject.SetActive(false));
            }
        }

        public void SmoothMaterialChange(Material material)
        {
            Color color = meshRenderer.material.color;
            meshRenderer.material = material;
            meshRenderer.material.color = color;
            meshRenderer.material.DOColor(material.color, 0.5f);
        }

        public Vector3 GetItemPlacePos()
        {
            Vector3 tempPos = transform.position;
            if (!meshRenderer)
                tempPos.z += 0.25f;
            else if (isHighStand)
                tempPos.y = 49.5f;
            else
                tempPos.y = 48.8f;

            return tempPos;
        }

        // public void ImageAlphaBlink()
        // {
        //     image.DOFade(0.5f, 0.5f).SetLoops(-1, LoopType.Yoyo).SetId(0).OnKill(() => image.DOFade(1, 0));
        //     outline.enabled = true;
        // }
    }
}