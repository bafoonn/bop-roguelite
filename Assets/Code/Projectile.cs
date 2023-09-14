using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Pasta
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        
        // Start is called before the first frame update
        void Start()
        {
            Invoke("OnDestroy", 4f);
        }

        // Update is called once per frame
        void Update()
        {
            
            transform.Translate(Vector3.right * speed * Time.deltaTime);

        }


        private void OnDestroy()
        {
            Destroy(gameObject);
        }
    }
}
