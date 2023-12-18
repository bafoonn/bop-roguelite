using UnityEngine;

namespace Pasta
{
    /// <summary>
    /// Singleton pattern.
    /// </summary>
    /// <typeparam name="T">Type of the singleton.</typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public abstract bool PersistSceneLoad { get; }
        public static bool Exists => _current != null;

        private static T _current;
        public static T Current
        {
            get
            {
                if (_current == null)
                {
                    if (!Application.isPlaying)
                    {
                        //var obj = FindFirstObjectByType<T>();
                        //if (obj != null)
                        //{
                        //    _current = obj;
                        //    return _current;
                        //}
                        throw new System.InvalidOperationException($"Attempted to access singleton ${typeof(T).Name} in editor.");
                    }

                    T prefab = Resources.Load<T>($"Singletons/{typeof(T).Name}");

                    if (prefab)
                    {
                        _current = Instantiate(prefab);
                    }
                    else
                    {
                        var gameobject = new GameObject(typeof(T).Name);
                        _current = gameobject.AddComponent<T>();
                    }
                }

                return _current;
            }
        }

        protected void Awake()
        {
            if (_current == null)
            {
                _current = this as T;
            }

            if (_current != this)
                Destroy(gameObject);

            if (PersistSceneLoad)
            {
                DontDestroyOnLoad(gameObject);
            }

            Init();
        }

        /// <summary>
        /// Method to be used to initialize the singleton.
        /// </summary>
        protected virtual void Init()
        {
        }
    }
}
