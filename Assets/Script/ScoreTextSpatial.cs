using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreTextSpatial : MonoBehaviour
{
    [SerializeField] float delay;
    GameManager gameManager;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameObject.GetComponentInChildren<TextMesh>().text = "+" + 50 * gameManager.scoreMultiplier() + "!";
        StartCoroutine(Die());
    }

    IEnumerator Die()
    {
        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
