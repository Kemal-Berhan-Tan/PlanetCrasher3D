using System.Collections.Generic;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Helpers;
using UnityEngine;

namespace GameFolders.Scripts.Managers.MidLevelManagers
{
    public class VFXManager : Singleton<VFXManager>
    {
        [SerializeField] private List<ParticleSystem> particleSystems;

        public ParticleSystem CreateParticle(VFXTypesEnum vfxType, Vector3 position)
        {
            ParticleSystem particle = Instantiate(particleSystems[(int)vfxType], transform);
            particle.transform.position = position;
            particle.Play();
            return particle;
        }
    }
}