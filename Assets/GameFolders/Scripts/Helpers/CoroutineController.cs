using System;
using System.Collections;
using UnityEngine;

namespace GameFolders.Scripts.Helpers
{
    public class CoroutineController : Singleton<CoroutineController>
    {
        public static void DoAfterGivenTime(float time, Action callback)
        {
            Instance.DoAfterGivenTimeLocal(time, callback);
        }

        public static void DoAfterFrame(Action callback)
        {
            Instance.DoAfterGivenTimeLocal(Time.deltaTime, callback);
        }

        public static void DoAfterFixedUpdate(Action callback)
        {
            Instance.DoAfterGivenTimeLocal(Time.fixedDeltaTime, callback);
        }

        public void DoAfterGivenTimeLocal(float time, Action callback)
        {
            StartCoroutine(ExecuteAfterTime(time, callback));
        }

        private IEnumerator ExecuteAfterTime(float time, Action callback)
        {
            yield return new WaitForSeconds(time);
            callback?.Invoke();
        }
    }
}