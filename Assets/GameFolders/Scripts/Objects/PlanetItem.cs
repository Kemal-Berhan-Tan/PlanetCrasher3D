using System.Collections.Generic;
using DG.Tweening;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Helpers;
using GameFolders.Scripts.Managers.MidLevelManagers;
using TMPro;
using UnityEngine;

namespace GameFolders.Scripts.Objects
{
    public class PlanetItem : MonoBehaviour
    {
        [SerializeField] private MeshRenderer meshRenderer;
        [SerializeField] private SphereCollider sphereCollider;
        [SerializeField] private GameObject brokenPlanetParticlesParrent;
        [SerializeField] private TMP_Text powerScoreText;
        [SerializeField] private List<Rigidbody> brokenPlanetParticles;

        private int powerScore;

        public void Break()
        {
            sphereCollider.enabled = false;
            CoroutineController.DoAfterGivenTime(.25f, () =>
            {
                powerScoreText.transform.parent.gameObject.SetActive(false);
                foreach (var particle in brokenPlanetParticles)
                {
                    particle.isKinematic = false;
                    particle.AddExplosionForce(1000, transform.position, 100);
                }

                DOTween.Kill(5);
                transform.DOMoveY(20, 1).OnComplete(() => Destroy(gameObject)).SetUpdate(UpdateType.Fixed)
                    .SetEase(Ease.Linear);
            });
        }

        public void Crack()
        {
            DOTween.Kill(5);
            meshRenderer.transform.rotation = Quaternion.identity;
            meshRenderer.enabled = false;
            brokenPlanetParticlesParrent.SetActive(true);
            VFXManager.Instance.CreateParticle(VFXTypesEnum.PlanetCrash, transform.position).transform
                .parent = transform;
        }

        public void CrackMore()
        {
            Vector3 tempVec = new Vector3(.015f, .015f, .015f);
            transform.localScale += tempVec;
            foreach (var particle in brokenPlanetParticles)
            {
                particle.transform.localScale -= tempVec;
                particle.isKinematic = true;
            }
        }

        public Tween MoveUp()
        {
            powerScoreText.transform.parent.gameObject.SetActive(false);
            DOTween.Kill(5);
            return transform.DOMoveY(20, 2f).SetEase(Ease.Linear);
        }

        public Tween StartSequence()
        {
            SetRotation(2f);
            transform.position = new Vector3(0, -70, 0);
            return transform.DOMoveY(-35, 2f);
        }

        public void SetRotation(float duration)
        {
            DOTween.Kill(5);

            Vector3 rot = new Vector3(meshRenderer.transform.rotation.eulerAngles.x,
                meshRenderer.transform.rotation.eulerAngles.y - 90, meshRenderer.transform.rotation.eulerAngles.z);
            meshRenderer.transform.DOLocalRotate(rot, duration).SetLoops(-1, LoopType.Incremental)
                .SetEase(Ease.Linear).SetId(5);
        }
    }
}