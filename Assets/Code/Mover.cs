using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    private Rigidbody2D _rigidbody = null;
    private Queue<Motion> _motionsToRemove = new Queue<Motion>();
    private HashSet<Motion> _motions = new HashSet<Motion>();

    public void Setup(Rigidbody2D rigidbody)
    {
        _rigidbody = rigidbody;
    }

    public bool AddMotion(Motion motion) => _motions.Add(motion);
    public void RemoveMotion(Motion motion) => _motionsToRemove.Enqueue(motion);

    private void FixedUpdate()
    {
        while (_motionsToRemove.Count > 0)
        {
            _motions.Remove(_motionsToRemove.Dequeue());
        }

        Vector2 velocity = Vector2.zero;
        foreach (Motion motion in _motions)
        {
            if (motion.Type == Motion.MotionType.Single)
            {
                _motionsToRemove.Enqueue(motion);
            }

            if (!motion.Enabled)
            {
                continue;
            }


            velocity += motion.Direction * motion.Modifier;
        }
        _rigidbody.velocity = velocity;
    }
}
