using System;
using DG.Tweening;
using GameFolders.Scripts.Enums;
using Lean.Common;
using Lean.Touch;
using UnityEngine;

namespace GameFolders.Scripts.Objects
{
    public class CharacterBodyPartItem : MonoBehaviour
    {
        [SerializeField] protected LeanSelectableByFinger leanSelectableByFinger;
        [SerializeField] private LeanDragTranslateAlong leanDragTranslateAlong;
        [SerializeField] private int bodyPartLevel;
        [SerializeField] private CharacterBodyPartTypes bodyPartType;

        public int BodyPartLevel => bodyPartLevel;
        public CharacterBodyPartTypes BodyPartType => bodyPartType;

        public int BodyPartPowerScore { get; private set; }

        public event Action<CharacterBodyPartItem, bool> IsSelectingChangedEvent;

        private Transform itemMeshTransform;

        private Quaternion startRotation;

        private void Awake()
        {
            itemMeshTransform = gameObject.transform.GetChild(0);
            startRotation = itemMeshTransform.rotation;
        }

        private void Start()
        {
            leanSelectableByFinger.OnSelected.AddListener(Selected);
            leanSelectableByFinger.OnDeselected.AddListener(Deselected);
            InitializeScale();
        }

        private void Selected()
        {
            IsSelectingChangedEvent?.Invoke(this, true);
        }

        private void Deselected()
        {
            IsSelectingChangedEvent?.Invoke(this, false);
        }

        public void Initialize(LeanPlane plane, int powerScore)
        {
            leanDragTranslateAlong.ScreenDepth.Object = plane;
            BodyPartPowerScore = powerScore;
        }

        public void InitializeScale()
        {
            transform.localScale = Vector3.zero;
            transform.DOScale(Vector3.one, .5f).SetEase(Ease.OutBack);
        }

        public Tween DoMoveTargetPos(Vector3 newPos)
        {
            //leanSelectableByFinger.enabled = false;
            return transform.DOMove(newPos, 0.5f);
        }

        public void SetStandRotation(bool isRotate)
        {
            if (isRotate)
            {
                Vector3 rot = new Vector3(itemMeshTransform.rotation.eulerAngles.x,
                    itemMeshTransform.rotation.eulerAngles.y - 90, itemMeshTransform.rotation.eulerAngles.z);
                itemMeshTransform.DOLocalRotate(rot, 2).SetLoops(-1, LoopType.Incremental)
                    .SetEase(Ease.Linear);
            }
            else
            {
                DOTween.Kill(itemMeshTransform);
                itemMeshTransform.rotation = startRotation;
            }
        }

        public void SetSelectable(bool isSelectable)
        {
            leanSelectableByFinger.enabled = isSelectable;
        }

        public void ResetItem()
        {
            //leanSelectableByFinger.enabled = true;
        }
    }
}