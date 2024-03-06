using UnityEngine;

namespace Pasta
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float speed = 5f;
        public float damage = 2f;
        public LayerMask WhatLayerDestroysThis;
        private Rigidbody2D rbd2d;
        private Transform player;
        private Vector2 direction;


        private void OnEnable()
        {
            ItemAbilities.OnEvent += OnEvent;
        }

        private void OnDisable()
        {
            ItemAbilities.OnEvent -= OnEvent;
        }

        private void OnEvent(EventContext obj)
        {
            if (obj.EventType != EventActionType.OnRoomEnter) return;
            Destroy(this.gameObject);
        }


        // Start is called before the first frame update
        void Awake()
        {
            rbd2d = GetComponent<Rigidbody2D>();
            Invoke("OnDestroy", 4f);
        }

        public void Launch(Vector2 dir)
        {
            direction = dir;
        }

        // Update is called once per frame
        void Update()
        {

            //transform.Translate(Vector3.right * speed * Time.deltaTime);


        }

        private void FixedUpdate()
        {
            rbd2d.MovePosition(rbd2d.position + direction * speed * Time.fixedDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                if (collision.TryGetComponent<IHittable>(out var hittable))
                {
                    hittable.Hit(damage);
                }
            }
            else if (WhatLayerDestroysThis.Includes(collision.gameObject.layer) && collision.gameObject.tag != "Enemy")
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            Destroy(gameObject);
        }
    }
}
