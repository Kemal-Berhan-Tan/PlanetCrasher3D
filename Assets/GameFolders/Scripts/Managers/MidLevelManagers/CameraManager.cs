using Cinemachine;
using DG.Tweening;
using Framework.Core;
using GameFolders.Scripts.Enums;
using GameFolders.Scripts.Events;
using UnityEngine;
using UnityEngine.Serialization;

namespace GameFolders.Scripts.Managers.MidLevelManagers
{
    public class CameraManager : BaseManager
    {
        [SerializeField] private CinemachineBrain cinemachineBrain;

        [SerializeField]
        private CinemachineVirtualCamera defCam, fallingCam, levelEndTransitionCam, fightCam, runnerCam;

        private CinemachineVirtualCamera currentCam;

        public override void Receive(BaseEventArgs baseEventArgs)
        {
            switch (baseEventArgs)
            {
                case CameraTransisitonEventArgs cameraTransisitonEventArgs:
                    if (cameraTransisitonEventArgs.Pos != Vector3.zero)
                        ChangeCameraPos(cameraTransisitonEventArgs.CameraType, cameraTransisitonEventArgs.Pos,
                            cameraTransisitonEventArgs.Rot);
                    else
                        CameraTransition(cameraTransisitonEventArgs.CameraType, cameraTransisitonEventArgs.IsSmooth,
                            cameraTransisitonEventArgs.SmoothDuration);
                    break;
                case CameraShakeEventArgs cameraShakeEventArgs:
                    currentCam.transform.DOShakePosition(0.5f, 5, 20);
                    break;
            }
        }

        private void CameraTransition(CameraTransitionTypesEnum type, bool isSmooth, float smoothDuration)
        {
            if (isSmooth)
                cinemachineBrain.m_DefaultBlend =
                    new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.EaseInOut,
                        smoothDuration == -1 ? 2 : smoothDuration);
            else
                cinemachineBrain.m_DefaultBlend =
                    new CinemachineBlendDefinition(CinemachineBlendDefinition.Style.Cut, 0f);

            CloseAllCameras();
            switch (type)
            {
                case CameraTransitionTypesEnum.MainCamera:
                    defCam.gameObject.SetActive(true);
                    currentCam = defCam;
                    break;
                case CameraTransitionTypesEnum.LevelEndTransition:
                    levelEndTransitionCam.gameObject.SetActive(true);
                    currentCam = levelEndTransitionCam;
                    break;
                case CameraTransitionTypesEnum.FallingCamera:
                    fallingCam.gameObject.SetActive(true);
                    currentCam = fallingCam;
                    break;
                case CameraTransitionTypesEnum.FightCamera:
                    fightCam.gameObject.SetActive(true);
                    currentCam = fightCam;
                    break;
                case CameraTransitionTypesEnum.RunnerCamera:
                    runnerCam.gameObject.SetActive(true);
                    currentCam = runnerCam;
                    break;
            }
        }

        private void ChangeCameraPos(CameraTransitionTypesEnum type, Vector3 pos, Quaternion rot)
        {
            CinemachineVirtualCamera tempCam = null;
            switch (type)
            {
                case CameraTransitionTypesEnum.MainCamera:
                    tempCam = defCam;
                    break;
                case CameraTransitionTypesEnum.LevelEndTransition:
                    tempCam = levelEndTransitionCam;
                    break;
                case CameraTransitionTypesEnum.FallingCamera:
                    tempCam = fallingCam;
                    break;
                case CameraTransitionTypesEnum.FightCamera:
                    tempCam = fightCam;
                    break;
                case CameraTransitionTypesEnum.RunnerCamera:
                    tempCam = runnerCam;
                    break;
            }

            if (rot == Quaternion.identity)
                tempCam.transform.rotation = rot;

            tempCam.ForceCameraPosition(pos, rot);
        }

        private void CloseAllCameras()
        {
            defCam.gameObject.SetActive(false);
            fallingCam.gameObject.SetActive(false);
            fightCam.gameObject.SetActive(false);
            runnerCam.gameObject.SetActive(false);
        }
    }
}