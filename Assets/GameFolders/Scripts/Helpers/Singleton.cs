using UnityEngine;

namespace GameFolders.Scripts.Helpers
{
    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance)
                    return _instance;
                _instance = FindObjectOfType<T>();
                if (_instance)
                    return _instance;
                Debug.LogError($"Singleton type {nameof(T)} not found! New instance created and returned! Be careful.");
                _instance = new GameObject().AddComponent<T>();
                _instance.gameObject.name = $"{nameof(T)}_Instance";
                return _instance;
            }
        }
    }
}
