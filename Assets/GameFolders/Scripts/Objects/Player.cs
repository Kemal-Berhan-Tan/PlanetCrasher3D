using System;
using System.Collections.Generic;
using DG.Tweening;
using FluffyUnderware.Curvy.Controllers;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Helpers;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using VFXManager = GameFolders.Scripts.Managers.MidLevelManagers.VFXManager;

namespace GameFolders.Scripts.Objects
{
    public class Player : MonoBehaviour
    {
        public CapsuleCollider CapsuleCollider;
        public GameObject trailRenderer;
        public SplineController SplineController;
        public Animator animator;
        public TMP_Text PowerTitleText;
        public Transform Container;
        public ParticleSystem FallingParticle;
        [SerializeField] private ParticleSystem jetpackSmokeLeft, jetpackSmokeRight;
        [SerializeField] private TrailRenderer trailRendererLeft, trailRendererRight;
        [SerializeField] private TMP_Text totalPowerScoreText;
        [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
        [SerializeField] private GameObject leftShoes, rightShoes;
        [SerializeField] private RectTransform powerTitleBarMask;
        [SerializeField] private List<GameObject> characterMechanicalBodyParts = new List<GameObject>();
        [SerializeField] private CharacterBodyPartWeaponList characterBodyPartWeaponLists;

        private Vector3 lastPosition;

        private float lastTrargetRotationY;

        public int PowerTitleBarValue { get; set; }

        public int TotalPowerScoreShower
        {
            set
            {
                totalPowerScoreText.text = value.ToString();

                // switch (value)
                // {
                //     case int n when (n >= 0 && n <= 500):
                //         ScorePowerTitleText.text = "Normal Person";
                //         break;
                //     case int n when (n > 500 && n <= 800):
                //         ScorePowerTitleText.text = "Sidekick";
                //         break;
                //     case int n when (n > 800 && n <= 1000):
                //         ScorePowerTitleText.text = "Hero";
                //         break;
                //     case int n when (n > 1000 && n <= 1500):
                //         ScorePowerTitleText.text = "Super Hero";
                //         break;
                //     case int n when (n > 1500 && n <= 2000):
                //         ScorePowerTitleText.text = "God";
                //         break;
                // }
            }
        }

        public float TheWillBeAddRotationY { get; set; }

        public float PowerTitleBarSizeDeltaX
        {
            get { return powerTitleBarMask.sizeDelta.x; }
            set
            {
                powerTitleBarMask.sizeDelta =
                    new Vector2(value, powerTitleBarMask.sizeDelta.y);
            }
        }

        public event Action<Collider> PlayerIsOnCollidedEvent;

        public float SetFallingParticleSpeed
        {
            set
            {
                var main = FallingParticle.main;
                main.simulationSpeed = value;
            }
        }

        public void SetCharacterWeaponEnabled(CharacterBodyPartDirectionTypes directionType, int bodyPartLevel)
        {
            Vector3 particlePos = Vector3.zero;
            skinnedMeshRenderer.SetBlendShapeWeight(GetBodyPartBlendShapeIndex(directionType),
                bodyPartLevel == -1 ? 0 : 100);
            if (directionType != CharacterBodyPartDirectionTypes.None)
                foreach (var obj in characterBodyPartWeaponLists.CharacterBodyPartWeapons[(int)directionType - 1]
                             .BodyPartWeapons)
                {
                    obj.SetActive(false);
                }

            particlePos = characterMechanicalBodyParts[(int)directionType - 1].transform.position;
            switch (directionType)
            {
                case CharacterBodyPartDirectionTypes.Arm_Right:
                    particlePos.y += 3;
                    particlePos.x += -1.5f;
                    break;
                case CharacterBodyPartDirectionTypes.Arm_Left:
                    particlePos.y += 3;
                    particlePos.x += 1.5f;
                    break;
                case CharacterBodyPartDirectionTypes.Leg_Right:
                    particlePos.y -= .75f;
                    break;
                case CharacterBodyPartDirectionTypes.Leg_Left:
                    particlePos.y += .75f;
                    break;
            }

            VFXManager.Instance.CreateParticle(VFXTypesEnum.BodyPartBroked,
                particlePos);


            if (directionType == CharacterBodyPartDirectionTypes.Leg_Left)
                leftShoes.SetActive(bodyPartLevel == -1);
            if (directionType == CharacterBodyPartDirectionTypes.Leg_Right)
                rightShoes.SetActive(bodyPartLevel == -1);

            if (bodyPartLevel == -1)
            {
                characterMechanicalBodyParts[(int)directionType - 1].SetActive(false);
                return;
            }

            characterBodyPartWeaponLists.CharacterBodyPartWeapons[(int)directionType - 1]
                .BodyPartWeapons[bodyPartLevel].SetActive(true);
            characterMechanicalBodyParts[(int)directionType - 1].SetActive(true);
        }

        private int GetBodyPartBlendShapeIndex(CharacterBodyPartDirectionTypes type)
        {
            switch (type)
            {
                case CharacterBodyPartDirectionTypes.Arm_Right:
                    return 0;
                case CharacterBodyPartDirectionTypes.Arm_Left:
                    return 3;
                case CharacterBodyPartDirectionTypes.Leg_Right:
                    return 1;
                case CharacterBodyPartDirectionTypes.Leg_Left:
                    return 2;
                default:
                    return 0;
            }
        }


        public Tween FallingJump()
        {
            SetCharacterAnimation(PlayerAnimtionTypes.FallingIdle, true);
            CoroutineController.DoAfterGivenTime(1, () => FallingParticle.Play());
            return transform.DOJump(new Vector3(0, 0, 0), 35, 1, 2.5f);
        }

        public void LandDownRunnerPlatform()
        {
            SetCharacterAnimation(PlayerAnimtionTypes.FallingIdle, false);
            SetCharacterAnimation(PlayerAnimtionTypes.LandDown);
            //SetCharacterAnimation(PlayerAnimtionTypes.Run, true);
            jetpackSmokeLeft.gameObject.SetActive(true);
            jetpackSmokeRight.gameObject.SetActive(true);
            trailRendererLeft.gameObject.SetActive(true);
            trailRendererRight.gameObject.SetActive(true);
            jetpackSmokeLeft.Play();
            jetpackSmokeRight.Play();
            Container.rotation = Quaternion.Euler(-45, 0, 0);
            AirFloatEnabled();
            FallingParticle.Stop();

            totalPowerScoreText.transform.parent.parent.transform.localPosition = new Vector3(0, 1.75f, 4);
            totalPowerScoreText.transform.parent.parent.transform.localRotation = Quaternion.Euler(-45, 180, 0);
        }

        public void AirFloatEnabled(bool isEnabled = true)
        {
            if (!isEnabled)
            {
                DOTween.Kill(12);
                Container.localPosition = new Vector3(0, 0, 0);
                return;
            }

            Container.localPosition = new Vector3(0, 3, 0);
            Container.DOLocalMoveY(4.5f, 1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear).SetId(12);
        }

        public Tween StartPlanetFight()
        {
            Container.localRotation = Quaternion.Euler(0, 0, 0);
            totalPowerScoreText.transform.parent.parent.transform.localRotation = Quaternion.Euler(45, 0, 0);
            AirFloatEnabled(false);
            SetCharacterAnimation(PlayerAnimtionTypes.FallingIdle, true);
            jetpackSmokeLeft.gameObject.SetActive(false);
            jetpackSmokeRight.gameObject.SetActive(false);
            trailRendererLeft.gameObject.SetActive(false);
            trailRendererRight.gameObject.SetActive(false);

            Vector3 tempPos = transform.position + (transform.forward * 10);
            tempPos.y = -20;
            FallingParticle.Play();
            return transform.DOJump(tempPos, 30, 1, 3);
        }

        public Tween PlanetFightAction()
        {
            SetAnimSpeed(.25f);
            SetFallingParticleSpeed = .5f;
            return transform.DOMoveY(-27.25f, 10).SetEase(Ease.Linear).SetId(3);
        }

        public Tween BreakPlanet()
        {
            //return transform.DOJump(new Vector3(0, -50, 0), 20, 1, 2f);
            FallingParticle.Stop();
            return transform.DOMoveY(-27.25f, .15f).SetEase(Ease.Linear).SetId(4);
        }

        public Tween GoDefFallingPos()
        {
            SetFallingParticleSpeed = 2.5f;
            FallingParticle.Play();
            return transform.DOMoveY(-20, .25f).SetEase(Ease.Linear);
        }

        public Tween Dodge(Vector3 direction)
        {
            direction *= 10;
            direction.y = transform.position.y;
            SetFallingParticleSpeed = 2.5f;
            trailRenderer.SetActive(true);
            return transform.DOMove(direction, .75f);
        }

        public void Die(bool isRunner)
        {
            if (isRunner)
            {
                SetCharacterAnimation(PlayerAnimtionTypes.Die);
                AirFloatEnabled(false);
                // container.DOMoveY(0, .25f);
                Container.localRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                SetCharacterAnimation(PlayerAnimtionTypes.Die);
                Vector3 pos = new Vector3(transform.localPosition.x, transform.localPosition.y - 30,
                    transform.localPosition.z - 10);
                transform.DOJump(pos, 30, 1, 2.5f);
            }

            PowerTitleText.transform.parent.parent.gameObject.SetActive(false);
        }

        public void SetCharacterAnimation(PlayerAnimtionTypes type, bool isEnable)
        {
            animator.SetBool(type.ToString(), isEnable);
        }

        public void SetCharacterAnimation(PlayerAnimtionTypes type)
        {
            animator.SetTrigger(type.ToString());
        }

        public void SetAnimSpeed(float speed)
        {
            animator.speed = speed;
        }

        public void SetBoostedRunActivate(bool isActive = true)
        {
            Vector3 tempPos = Vector3.zero;
            Quaternion tempRot = quaternion.identity;

            if (isActive)
            {
                tempPos = new Vector3(-10, 10, 20);
                tempRot = Quaternion.Euler(-90, 0, 0);
                FallingParticle.Play();
                trailRendererLeft.startWidth = .75f;
                trailRendererRight.startWidth = .75f;
            }
            else
            {
                FallingParticle.Stop();
                FallingParticle.transform.parent = Container;
                tempPos = new Vector3(0, -30, 0);
                trailRendererLeft.startWidth = .2f;
                trailRendererRight.startWidth = .2f;
            }

            FallingParticle.transform.localPosition = tempPos;
            FallingParticle.transform.localRotation = tempRot;
        }

        public void JetpackParticlesEnabled(bool isEnabled = true)
        {
            if (isEnabled)
            {
                jetpackSmokeLeft.Play();
                jetpackSmokeRight.Play();
            }
            else
            {
                jetpackSmokeLeft.Stop();
                jetpackSmokeRight.Stop();
            }

            trailRendererLeft.gameObject.SetActive(isEnabled);
            trailRendererRight.gameObject.SetActive(isEnabled);
        }

        public void SetSwerveMovement(float deltaX)
        {
            SplineController.OffsetRadius += deltaX;
            SplineController.OffsetRadius = Mathf.Clamp(SplineController.OffsetRadius, -4.5f, 4.5f);


            if (deltaX == 0)
            {
                DoSmoothRotationY(0);
                return;
            }

            float y = deltaX < 0 ? 35 : -35;
            // float y = Mathf.Clamp(deltaX * 50, -40, 40);
            // Debug.Log(y);
            DoSmoothRotationY(y);
        }

        public void DoSmoothRotationY(float y)
        {
            if (lastTrargetRotationY == y)
                return;

            if (DOTween.IsTweening(10))
                DOTween.Kill(10);

            float delay = 0;
            if (y == 0)
                delay = .15f;
            lastTrargetRotationY = y;
            Container.DOLocalRotate(new Vector3(Container.transform.eulerAngles.x, y, 0), .25f).SetId(10)
                .SetDelay(delay).SetAutoKill(true);
            // DOTween.To(() => TheWillBeAddRotationY, x => TheWillBeAddRotationY = x, y, .25f).SetId(10);
        }

        public Tween SetSmoothPowerTitleBarValue(float value)
        {
            return DOTween.To(() => PowerTitleBarSizeDeltaX, x => PowerTitleBarSizeDeltaX = x, value, 0.75f).SetId(11)
                .SetEase(Ease.Linear);
        }

        private void OnTriggerEnter(Collider other)
        {
            PlayerIsOnCollidedEvent?.Invoke(other);
        }
    }

    [Serializable]
    public class CharacterBodyPartWeaponList
    {
        public List<CharacterBodyPartWeapons> CharacterBodyPartWeapons = new List<CharacterBodyPartWeapons>();
    }

    [Serializable]
    public class CharacterBodyPartWeapons
    {
        public List<GameObject> BodyPartWeapons = new List<GameObject>();
    }
}