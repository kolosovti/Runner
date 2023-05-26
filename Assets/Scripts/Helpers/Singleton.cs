using UnityEngine;

namespace Game.Helpers
{
    public class Singleton<T> : MonoBehaviour
    {
        public static T Instance { get; private set; }

        protected virtual void Awake()
        {
            if (Instance != null)
            {
                Destroy(this);
                Debug.LogError($"Double singleton: {nameof(T)}");
                return;
            }

            Instance = GetComponent<T>();
            DontDestroyOnLoad(this);
        }
    }
}