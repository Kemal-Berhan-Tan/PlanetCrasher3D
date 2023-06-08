/*
using System;
using System.Collections;
using Cinemachine;
using UnityEngine;

namespace Game.Scripts.Controllers
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private VirtualCameraDictionary virtualCameras;

        private static Camera _camera;
        private static CinemachineBrain _cinemachineBrain;

        public static Camera MainCamera
        {
            get
            {
                if (!_camera) _camera = Camera.main;
                return _camera;
            }
        }

        public static CinemachineBrain MainCameraBrain
        {
            get
            {
                if (!_cinemachineBrain) _cinemachineBrain = MainCamera.GetComponent<CinemachineBrain>();
                return _cinemachineBrain;
            }
        }

        private Coroutine _cameraBlendRoutine;

        public void ChangeCamera(CameraType cameraToEnable)
        {
            foreach (var cameraPair in virtualCameras)
            {
                var vCamera = cameraPair.Value;
                var cameraType = cameraPair.Key;
                vCamera.gameObject.SetActive(cameraType == cameraToEnable);
            }
        }

        public void ChangeCamera(CameraType cameraType, Transform target, Action onBlendComplete = null)
        {
            ChangeCamera(cameraType, onBlendComplete);
            SetTarget(cameraType, target);
        }

        public void ChangeCamera(CameraType cameraToEnable, Action onBlendComplete = null)
        {
            ChangeCamera(cameraToEnable);
            if (_cameraBlendRoutine != null) StopCoroutine(_cameraBlendRoutine);
            _cameraBlendRoutine = StartCoroutine(DoAfterBlend(onBlendComplete));
        }

        private void SetTarget(CameraType type, Transform target)
        {
            virtualCameras[type].Follow = target;
            virtualCameras[type].LookAt = target;
        }

        private IEnumerator DoAfterBlend(Action action)
        {
            yield return new WaitUntil((() => MainCameraBrain.IsBlending));

            yield return new WaitUntil((() => !MainCameraBrain.IsBlending));
            action?.Invoke();
        }

        [Serializable]
        private class VirtualCameraDictionary : SerializableDictionary<CameraType, CinemachineVirtualCamera>
        {
        }
    }

    public enum CameraType
    {
        StartCamera,
        FollowCamera,
        FinishCamera
    }
}
*/