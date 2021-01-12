using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssShaker : MonoBehaviour
{
    [SerializeField] public float speed;

    [SerializeField] GrapplingRope grapplingRope;
    Rigidbody2D rb;

    private void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(grapplingRope.isGrappling)
        {
            Vector2 force = new Vector2(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0);
            rb.AddForce(force);
        }
    }
}
