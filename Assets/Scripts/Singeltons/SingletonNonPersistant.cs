using UnityEngine;

namespace Singeltons
{
    public abstract class SingletonNonPersistant : MonoBehaviour
    {
        public static SingletonNonPersistant Instance {get; private set;}

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
        }
    }
}