using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] float followSpeed = 0.25f;
    private float lerpSpeed;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private Vector3 offset;

    private GameObject player;
    private Vector3 targetPosition;

    private void Start()
    {
        targetPosition = transform.position;
        player = GameObject.Find("Body"); //FindObjectOfType<PlayerController>().gameObject;
    }

    private void FixedUpdate()
    {
        if(player != null)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = player.transform.position.z;

            Vector3 playerDirection = (player.transform.position - posNoZ);
            lerpSpeed = playerDirection.magnitude * 5f;
            targetPosition = transform.position + (playerDirection.normalized * lerpSpeed * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, targetPosition + offset, followSpeed);
        }
    }

}
