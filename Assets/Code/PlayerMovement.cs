using Pasta;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody = null;
    public float Speed = 1.0f;

    public void Setup(Rigidbody2D rigidbody)
    {
        _rigidbody = rigidbody;
    }

    public void Move(Vector2 dir)
    {
        Debug.Assert(_rigidbody != null);
    }
}
