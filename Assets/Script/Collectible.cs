using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] GameObject scoreText;
    Collider2D col;
    PlayerController player;

    bool collected;
    private void Start()
    {
        collected = false;
        player = FindObjectOfType<PlayerController>();
        col = gameObject.GetComponent<Collider2D>();

        GameManager.Instance.AddCollectible(this);

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
            ParticleManager FX = FindObjectOfType<ParticleManager>();
            FX.SpawnOnce(FX.coinFX, this.gameObject.transform.position);
            GameManager.Instance.AddScore(50);
            AudioManager.Instance.PlayAudio(AudioManager.Instance.pickupCollected, 1 + 0.08f * player.bodyParts.Count);
            Instantiate(scoreText, transform.position, Quaternion.identity);
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("GameplayArea"))
        {
            Destroy(transform.parent.gameObject);
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.RemoveCollectible(this);

    }
}
