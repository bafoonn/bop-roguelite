using UnityEngine;

namespace Pasta
{
    public static class Extensions
    {
        public static T AddOrGetComponent<T>(this Component component) where T : Component
        {
            if (component.gameObject.TryGetComponent(out T t))
            {
                return t;
            }
            else
            {
                return component.gameObject.AddComponent<T>();
            }
        }

        public static void Activate(this GameObject gameObject)
        {
            gameObject.SetActive(true);
        }

        public static void Deactivate(this GameObject gameObject)
        {
            gameObject.SetActive(false);
        }
    }
}
