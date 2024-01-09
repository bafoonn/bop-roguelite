using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class EnemyBase : MonoBehaviour
    {

        private Rigidbody2D rbd2d = null;

        [SerializeField] private float acceleration = 1;

        [SerializeField] private float deceleration = 1;

        [SerializeField] private float maxSpeed = 1;

        private float currentSpeed = 0;

        protected float Acceleration => acceleration;
        protected float Deceleration => deceleration;
        protected float MaxSpeed => maxSpeed;
        protected float CurrentSpeed => currentSpeed;

        protected virtual void Awake()
		{
            rbd2d = GetComponent<Rigidbody2D>();
		}

        protected virtual void Start()
		{

		}

        protected virtual void OnEnable()
		{

		}

        protected virtual void OnDisable()
        {

        }

        public virtual void Move(Vector2 direction, float deltaTime)
		{
            bool isStopping = Mathf.Approximately(direction.sqrMagnitude, 0);
			if (isStopping)
			{
                rbd2d.velocity = Vector2.zero;
                return;
			}

            bool isChangingDir = Vector2.Dot(direction, rbd2d.velocity) < 0 && !isStopping;
			if (isChangingDir)
			{
                currentSpeed -= Deceleration * deltaTime;
                rbd2d.velocity = rbd2d.velocity.normalized * currentSpeed;
            }

			else
			{
                float speedModifier = direction.magnitude;
                float increment = acceleration * speedModifier * deltaTime;
                rbd2d.velocity = direction * maxSpeed;
			}
		}


        /// <summary>
        /// Turn the enemy
        /// </summary>
        /// <param name="angle">angles per second</param>
        /// <param name="deltaTime">deltatime</param>
        public virtual void Turn(float angle, float deltaTime)
		{
            Vector2 direction = new Vector2(0, angle * deltaTime);
            Quaternion deltaRotation = Quaternion.Euler(direction);
            Quaternion targetRotation = transform.rotation * deltaRotation;
            transform.rotation = targetRotation;
		}

        
    }
}
