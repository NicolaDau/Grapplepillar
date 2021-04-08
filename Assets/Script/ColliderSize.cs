using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderSize : MonoBehaviour
{
    PlayerController player;
    CircleCollider2D col;

    float colSize()
    {
        return 10 + 0.5f * player.bodyParts.Count;
    }
    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        col = gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        col.radius = colSize();
    }
}
