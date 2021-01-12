using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float endTimer = 2f;
    [SerializeField] private Text endGameText;

    private void Start()
    {
        endGameText.gameObject.SetActive(false);
    }
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
    public void EndLevel(int sceneToLoad, string endText)
    {
        endGameText.gameObject.SetActive(true);
        endGameText.text = endText;
        LoadLevel(sceneToLoad);
    }

    IEnumerator LoadLevel(int sceneToLoad)
    {
        yield return new WaitForSeconds(endTimer);
        SceneManager.LoadScene(sceneToLoad);
    }
}
