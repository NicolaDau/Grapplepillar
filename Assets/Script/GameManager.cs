using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float endTimer = 2f;

    Collectible[] collectibles;

    [SerializeField] private Image pauseMenu;
    [SerializeField] private Text endGameText;
    bool gamePaused;

    PlayerController player;
    int score = 0;
    [SerializeField] Text scoreText;
    public int scoreMultiplier()
    {
        return player.bodyParts.Count;
    }
    [SerializeField] private Text multiplierText;

    private void Start()
    {
        collectibles = FindObjectsOfType<Collectible>();
        player = FindObjectOfType<PlayerController>();
        gamePaused = false;
        endGameText.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }

        multiplierText.text = "x" + scoreMultiplier().ToString();
    }

    public void PauseGame()
    {
        switch(gamePaused)
        {
            case true:
                pauseMenu.gameObject.SetActive(false);
                Time.timeScale = 1;
                break;
            case false:
                pauseMenu.gameObject.SetActive(true);
                Time.timeScale = 0;
                break;
        }

        gamePaused = !gamePaused;
    }

    public void AddScore(int amount)
    {
        score += amount * scoreMultiplier();
        scoreText.text = score.ToString();
    }

   
    public void EndLevel(int sceneToLoad, string endText)
    {
        endGameText.gameObject.SetActive(true);
        endGameText.text = endText;
        StartCoroutine(LoadLevel(sceneToLoad));
    }
    public void EndLevel(string sceneToLoad, string endText)
    {
        endGameText.gameObject.SetActive(true);
        endGameText.text = endText;
        StartCoroutine(LoadLevel(sceneToLoad));
    }

    IEnumerator LoadLevel(int sceneToLoad)
    {
        yield return new WaitForSeconds(endTimer);
        SceneManager.LoadScene(sceneToLoad);
    }
    IEnumerator LoadLevel(string sceneToLoad)
    {
        yield return new WaitForSeconds(endTimer);
        SceneManager.LoadScene(sceneToLoad);
    }

    public Vector3 GetPlayerCentre()
    {
        return (player.head.transform.position + player.lastBodyPart.transform.position) / 2;
    }
}
