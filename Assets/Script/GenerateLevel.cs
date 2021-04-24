using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerateLevel : MonoBehaviour
{
    [SerializeField] Collider2D gameArea;

    [SerializeField] GameObject collectible;
    [SerializeField] GameObject threat;
    [SerializeField] GameObject grapplePoint;

    CircleCollider2D rangeCol;

   [SerializeField] int colAmount;
   [SerializeField] int threatAmount;
   [SerializeField] int grapplePointAmount;

    private void Start()
    {
        rangeCol = GetComponent<CircleCollider2D>();
    }


    private void Update()
    {
        transform.position = GameManager.Instance.GetPlayerCentre();

        if (GameManager.Instance.collectibles.Count < colAmount)
        {
            RandomSpawn(collectible);
        }
        if (GameManager.Instance.threats.Count < threatAmount)
        {
            RandomSpawn(threat);
        }
        if (GameManager.Instance.grapplePoints.Count < grapplePointAmount)
        {
            RandomSpawn(grapplePoint);
        }
    }
    
    void RandomSpawn(GameObject spawn)
    {
       Vector2 randomPos =  new Vector2(transform.position.x, transform.position.y) + Random.insideUnitCircle * rangeCol.radius;

        if (gameArea.bounds.Contains(randomPos))
        {
            RandomSpawn(spawn);
        }

        else
        {
            Instantiate(spawn, randomPos, Quaternion.identity);
        }
    }
}

