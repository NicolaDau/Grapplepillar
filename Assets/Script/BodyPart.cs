using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyPart : MonoBehaviour
{
    Rigidbody2D rigidbody;
    SpringJoint2D spring;
    [SerializeField]Vector3 originalScale;
    [SerializeField] GameObject sprite;
    [SerializeField] float maxSpeed;
    private void Awake()
    {
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
        spring = gameObject.GetComponent<SpringJoint2D>();
            }
    private void FixedUpdate()
    {
        if (rigidbody.velocity.magnitude > maxSpeed)
        {
            rigidbody.velocity = rigidbody.velocity.normalized * maxSpeed;
        }
        if (this.gameObject.name != "Head")
        {
            Vector3 diff = spring.connectedBody.gameObject.transform.position - transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
            ///spring.distance = 0.8f + 0.0008f * Mathf.Pow(Mathf.Abs(GameManager.Instance.playerCentreVelocity.x) + Mathf.Abs(GameManager.Instance.playerCentreVelocity.y), 2);
        }
        //sprite.transform.localScale = new Vector3 (originalScale.x - 0.0002f * Mathf.Pow(Mathf.Abs(GameManager.Instance.playerCentreVelocity.x) + Mathf.Abs(GameManager.Instance.playerCentreVelocity.y), 2), originalScale.y + 0.0002f * Mathf.Pow(Mathf.Abs(GameManager.Instance.playerCentreVelocity.x) + Mathf.Abs(GameManager.Instance.playerCentreVelocity.y), 2), originalScale.z);
    }
    
}
