using UnityEngine;

namespace Singeltons
{
    public abstract class SingletonPersistant<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }
        
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this as T;
            }
            DontDestroyOnLoad(this);
        }
    }
}