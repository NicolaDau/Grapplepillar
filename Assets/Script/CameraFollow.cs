using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float followSpeed = 0.25f;
    private float lerpSpeed;
    [SerializeField] private float minDistance;
    [SerializeField] private float maxDistance;
    private Vector3 targetPosition;
    [SerializeField] private Vector3 offset;
    [SerializeField] public bool targetPlayer;
    PlayerController playerController;
    private GameObject playerHead, playerLastBody;

    bool offsetting;
    [SerializeField] float offsetTimer;
    float currentOffsetTimer;

    [SerializeField] float startingSize = 18;
    float smoothTime = 0.3f;
    float yVelocity = 0.0f;
    float currentSize;
    float targetSize;

    private void Start()
    {
        targetPosition = transform.position;
        if (targetPlayer)
        {
            playerController = FindObjectOfType<PlayerController>();
            if (playerHead == null)
            {
                playerHead = playerController.head;
            }
        }

        currentOffsetTimer = offsetTimer;
    }

    private void FixedUpdate()
    {
        if (targetPlayer)
        {
            if (GameManager.Instance.playerCentreVelocity.y < 0)
            {
                // followSpeed = 0.5f + (0.0001f * Mathf.Pow(Mathf.Abs(GameManager.Instance.playerCentreVelocity.y), 2));
                offset.y = 0 - (0.0017f * Mathf.Pow(Mathf.Abs(GameManager.Instance.playerCentreVelocity.y), 2));
                    }
            /*else
            {
                if (followSpeed >= 0.5f)
                {
                    followSpeed = 0.5f;
                }
                    if (offset.y <= 0)
                {
                    offset.y = 0 + (0.02f * Mathf.Abs(GameManager.Instance.playerCentreVelocity.y));
                }
            }
            */

            playerLastBody = playerController.lastBodyPart;
            if (playerHead != null && playerLastBody != null)
            {
                Vector3 posNoZ = transform.position;
                posNoZ.z = GameManager.Instance.GetPlayerCentre().z;


                Vector3 playerDirection = (GameManager.Instance.GetPlayerCentre() - posNoZ);
                lerpSpeed = playerDirection.magnitude * 5f;
                targetPosition = transform.position + (playerDirection.normalized * lerpSpeed * Time.deltaTime) + offset;
                transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed);
            }

            
            Camera.main.orthographicSize = Mathf.SmoothDamp(Camera.main.orthographicSize, startingSize + (0.6f * playerController.bodyParts.Count), ref yVelocity, smoothTime);

        }
    }
}
