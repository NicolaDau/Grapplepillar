using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float endTimer = 2f;

    [SerializeField] private Image pauseMenu;
    [SerializeField] private Text endGameText;

    bool gamePaused;

    private void Start()
    {
        gamePaused = false;
        endGameText.gameObject.SetActive(false);
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

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void EndLevel(int sceneToLoad, string endText)
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
}
