using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] GameObject scoreText;
    Collider2D col;
    PlayerController player;
    GameManager gameManager;
    AudioManager audioManager;

    bool collected;
    private void Start()
    {
        collected = false;
        player = FindObjectOfType<PlayerController>();
        gameManager = FindObjectOfType<GameManager>();
        audioManager = FindObjectOfType<AudioManager>();
        col = gameObject.GetComponent<Collider2D>();
    }
    void FixedUpdate()
    {
        transform.parent.Rotate(Vector3.forward * Random.Range(200, 400) * Time.deltaTime);

        foreach(var bodyPart in player.bodyParts)
        {
            float dis = Vector3.Distance(bodyPart.transform.position, transform.parent.position);
            if (dis < 2f)
            {
                float speed = 15f - dis;
                speed = speed * Time.deltaTime * .5f;
                transform.parent.position = Vector3.MoveTowards(transform.parent.position, bodyPart.transform.position, speed);
            }
        }
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collected == false)
        {
            collected = true;
            col.enabled = false;
            player.AddBodyPart();
            gameManager.AddScore(50);
            audioManager.PlayAudio(audioManager.pickupCollected, 1 + 0.08f * player.bodyParts.Count);
            Instantiate(scoreText, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject);
        }
    }
}
