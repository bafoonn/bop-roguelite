using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pasta
{
    public class TargetDummy : MonoBehaviour, IEnemy
    {
        public Health Health { get; private set; }
        public Movement Movement { get; private set; }
        public Rigidbody2D Rigidbody { get; private set; }
        public StatusHandler Status { get; private set; }
        public MonoBehaviour Mono => this;

        public void Hit(float damage, HitType type, ICharacter source = null)
        {
            Health.TakeDamage(damage);
        }

        private void Awake()
        {
            Health = this.AddOrGetComponent<Health>();
            Movement = this.AddOrGetComponent<Movement>();
            Rigidbody = this.AddOrGetComponent<Rigidbody2D>();
            Status = this.AddOrGetComponent<StatusHandler>();
            Movement.Setup(Rigidbody);
            Status.Setup(this);
        }
    }
}
