using UnityEngine;

namespace Utils
{
    public class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private T _instance;
        public T Instance => _instance;

        protected virtual void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = (T) this;
            DontDestroyOnLoad(_instance);
        }
    }
}