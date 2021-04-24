using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    Rigidbody2D rigidbody;
    [SerializeField] float maxSpeed;
    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }
    private void FixedUpdate()
    {
        if (rigidbody.velocity.magnitude > maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
        }
    }
    
}
