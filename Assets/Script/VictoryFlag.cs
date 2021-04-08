using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class VictoryFlag : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] string sceneToLoad;
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(sceneToLoad == "")
            {
                gameManager.EndLevel(SceneManager.GetActiveScene().buildIndex + 1, "Level Complete!");

            }
            else
            {
                gameManager.EndLevel(sceneToLoad, "Level Complete!");

            }
        }
    }
}
