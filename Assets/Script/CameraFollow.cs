using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float followSpeed = 0.25f;
    private float lerpSpeed;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    [SerializeField] private Vector3 offset;
    [SerializeField] bool targetPlayer;
    PlayerController playerController;
    GameManager gameManager;
    private GameObject playerHead, playerLastBody;
    private Vector3 targetPosition;

    [SerializeField] float startingSize = 18;
    float smoothTime = 0.3f;
    float yVelocity = 0.0f;
    float currentSize;
    float targetSize;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        targetPosition = transform.position;
        if (targetPlayer)
        {
            playerController = FindObjectOfType<PlayerController>();
            if (playerHead == null)
            {
                playerHead = playerController.head;
            }
        }
    }

    private void FixedUpdate()
    {
        if (targetPlayer)
        {
            playerLastBody = playerController.lastBodyPart;
            if (playerHead != null && playerLastBody != null)
            {
                Vector3 posNoZ = transform.position;
                posNoZ.z = gameManager.GetPlayerCentre().z;


                Vector3 playerDirection = (gameManager.GetPlayerCentre() - posNoZ);
                lerpSpeed = playerDirection.magnitude * 5f;
                targetPosition = transform.position + (playerDirection.normalized * lerpSpeed * Time.deltaTime);
                transform.position = Vector3.Lerp(transform.position, targetPosition + offset, followSpeed);
            }

            
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, startingSize + (0.6f * playerController.bodyParts.Count), ref yVelocity, smoothTime);

        }
    }
}
