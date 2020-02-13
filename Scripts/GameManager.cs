using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    //references
    public GameObject gameOverCanvas;
    public GameObject score;
    public GameObject getReadyImg;
    public GameObject pauseBtn;
    public Animator blackFadeAnim;

    public static Vector2 bottomLeft;

    //game states
    public static bool gameOver;
    public static bool gameHasStarted;
    public static bool gameIsPaused;

    public Text panelScore;

    public static int gameScore;
    int drawScore;


    private void Awake()
    {
        bottomLeft = Camera.main.ScreenToWorldPoint(new Vector2(0, 0));
    }

    // Start is called before the first frame update
    void Start()
    {
        gameOver = false;
        gameHasStarted = false;
        gameIsPaused = false;
    }

    public void GameHasStarted()
    {
        gameHasStarted = true;
        score.SetActive(true);
        getReadyImg.SetActive(false);
        pauseBtn.SetActive(true);
    }

    public void GameOver()
    {
        gameOver = true;
        gameScore = score.GetComponent<Score>().GetScore();
        score.SetActive(false);
        Invoke("ActivateGameOverCanvas", 1);
        pauseBtn.SetActive(false);
    }

    void ActivateGameOverCanvas()
    {
        gameOverCanvas.SetActive(true);
        AudioManager.audiomanager.Play("die");
    }


    public void OnOkBtnPressed()
    {
        AudioManager.audiomanager.Play("transition");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }

    public void OnMenuBtnPressed()
    {
        AudioManager.audiomanager.Play("transition");
        //SceneManager.LoadScene("Menu");
        blackFadeAnim.SetTrigger("fadeIn");
    }


    public void DrawScore()
    {
        if (drawScore <= gameScore)
        {
            panelScore.text = drawScore.ToString();
            drawScore++;
            Invoke("DrawScore", 0.05f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}