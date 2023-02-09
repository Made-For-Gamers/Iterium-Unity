using UnityEngine;

namespace Iterium
{
    //Base singleton class

    public class Singleton<T> : MonoBehaviour where T : Component
    {
        private static T _instance;
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                    {

                        GameObject newGo = new GameObject();
                        _instance = newGo.AddComponent<T>();
                    }
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (_instance == null)
            {
                DontDestroyOnLoad(gameObject);
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}