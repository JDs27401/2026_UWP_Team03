using Unity.VisualScripting;
using UnityEngine;

namespace Singeltons
{
    public abstract class SingletonPersistant<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; private set; }
        
        protected virtual void Awake()
        {
            //print("this singleton id: "+gameObject.GetEntityId());
            //print("this instance id: "+ (Instance != null ? Instance.GetEntityId() : "null"));
            if (Instance != null && Instance != this)
            {
                //print("Duplicate instance found!");
                //print("deleting id: "+gameObject.GetEntityId());
                DestroyImmediate(gameObject);
                return;
            }
            //print("Singleton Instantiated");
            Instance = this as T;
            DontDestroyOnLoad(gameObject);
        }
    }
}