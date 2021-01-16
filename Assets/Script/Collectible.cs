using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (collision.TryGetComponent(out PlayerController player))
            {
                player.IncreaseScore(1);
            }

            else collision.GetComponentInParent<PlayerController>().IncreaseScore(1);
            Destroy(gameObject);
        }
    }
}
