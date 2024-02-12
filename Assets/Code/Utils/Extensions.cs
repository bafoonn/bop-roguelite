using System;
using System.Collections;
using UnityEngine;

namespace Pasta
{
    public static partial class Extensions
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

        public static void WaitAndRun(this MonoBehaviour mono, float time, Action action)
        {
            mono.StartCoroutine(Wait());

            IEnumerator Wait()
            {
                yield return new WaitForSeconds(time);
                action();
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


        public static bool Includes(this LayerMask layerMask, string layerName)
        {
            return layerMask.Includes(LayerMask.NameToLayer(layerName));
        }

        public static bool Includes(this LayerMask layerMask, int layer)
        {
            return layerMask == (layerMask | (1 << layer));
        }
    }
}
