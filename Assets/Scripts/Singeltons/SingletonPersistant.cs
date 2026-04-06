using UnityEngine;

namespace Singeltons
{
    public abstract class SingletonPersistant : MonoBehaviour
    {
        public static SingletonPersistant Instance { get; private set; }
        
        protected virtual void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(this);
            }
            else
            {
                Instance = this;
            }
            DontDestroyOnLoad(this);
        }
    }
}