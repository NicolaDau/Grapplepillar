using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private float endTimer = 2f;

   public List<Collectible> collectibles = new List<Collectible>();
   public List<Threat> threats = new List<Threat>();
   public List<GrapplePoint> grapplePoints = new List<GrapplePoint>();

    [SerializeField] private Image pauseMenu;
    [SerializeField] private Text endGameText;
    bool gamePaused;

    Vector3 previousCentrePosition;
    public Vector3 playerCentreVelocity;

    PlayerController player;
    int score = 0;
    [SerializeField] Text scoreText;
    public int scoreMultiplier()
    {
        return player.bodyParts.Count;
    }
    [SerializeField] private Text multiplierText;

    public static GameManager Instance = null;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    { 

        player = FindObjectOfType<PlayerController>();
        gamePaused = false;
        endGameText.gameObject.SetActive(false);
        Time.timeScale = 1;
    }


    public void AddCollectible(Collectible collectible)
    {
        collectibles.Add(collectible);
    }

    public void AddThreat(Threat threat)
    {
        threats.Add(threat);
    }

    public void AddGrapplePoint(GrapplePoint grapplePoint)
    {
        grapplePoints.Add(grapplePoint);
    }

    public void RemoveCollectible(Collectible collectible)
    {
        collectibles.Remove(collectible);
    }

    public void RemoveThreat(Threat threat)
    {
        threats.Remove(threat);
    }

    public void RemoveGrapplePoint(GrapplePoint grapplePoint)
    {
        grapplePoints.Remove(grapplePoint);
    }

    void FixedUpdate()
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


        playerCentreVelocity = (GetPlayerCentre() - previousCentrePosition) / Time.deltaTime;
        previousCentrePosition = GetPlayerCentre();

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
