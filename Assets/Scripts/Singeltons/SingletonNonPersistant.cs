using UnityEngine;

namespace Singeltons
{
    public abstract class SingletonNonPersistant<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance {get; protected set;}

        protected virtual void Awake()
        {
            print("awake in wavemanager id: " + gameObject.GetInstanceID());
            if (Instance != null && Instance != this)
            {
                print("instance duplicate, destroting id: " + gameObject.GetInstanceID());
                DestroyImmediate(this);
            }
            else
            {
                print("new instance, id: " + gameObject.GetInstanceID());
                Instance = this as T;
            }
        }
    }
}