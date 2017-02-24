using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerController : MonoBehaviour
{
    public GameObject player;

    TouchTracker touchTracker;
    Controller playerController;
    SpriteRenderer spriteRenderer;

    public Text counter;
    public Text time;
    public Text hs;

    float timerStart;

    public bool levelOver;
    bool timer = true;

    void Start()
    {
        // assigning components of the player object
        touchTracker = player.GetComponentInChildren<TouchTracker>();
        playerController = player.GetComponentInChildren<Controller>();
        spriteRenderer = playerController.GetComponentInChildren<SpriteRenderer>();

        // resetting the level
        counter.enabled = false;
        timerStart = 0;
        playerController.enabled = false;
        levelOver = false;

        // finding the level number for highscores
        string sceneName = SceneManager.GetActiveScene().name;
        string sceneNumber = sceneName.Substring(5, sceneName.Length - 5); // make sure this works

        // loading highschores
        if (PlayerPrefs.GetFloat("HS" + sceneNumber) != 0)
        {
            int seconds = (int)(PlayerPrefs.GetFloat("HS" + sceneNumber)) % 60;
            int minutes = (int)(PlayerPrefs.GetFloat("HS" + sceneNumber)) / 60;
            if (seconds > 9)
            {
                hs.text = "Highscore: " + minutes + ":" + seconds;
            }
            else
            {
                hs.text = "Highscore: " + minutes + ":0" + seconds;
            }
        }
        else
        {
            hs.text = "Highscore: not set";
        }

        // starting the chain of countdown coroutines
        StartCoroutine("countdown1");
    }

    void Update()
    {
        UpdateGameState();
        UpdateTimer();
    }

    void UpdateGameState()
    {
        if (levelOver)
        {
            levelOver = false;

            // if the player crossed the finish line
            if (playerController.win == true)
            {
                // finding the level number for highscores
                string sceneName = SceneManager.GetActiveScene().name;
                string sceneNumber = sceneName.Substring(5, sceneName.Length - 5);

                float timeElapsed = Time.time - timerStart;

                // highscore system
                if (PlayerPrefs.GetFloat("HS" + sceneNumber) != 0)
                {
                    if (timeElapsed < PlayerPrefs.GetFloat("HS" + sceneNumber))
                    {
                        PlayerPrefs.SetFloat("HS" + sceneNumber, timeElapsed);
                        counter.text = "New Highscore!";
                    }
                    else
                    {
                        counter.text = "Winner!";
                    }
                }
                else
                {
                    PlayerPrefs.SetFloat("HS" + sceneNumber, timeElapsed);
                    counter.text = "New Highscore!";
                }
                Debug.Log("New hs: " + PlayerPrefs.GetFloat("HS" + sceneNumber));


            }
            // if the player did not cross the finish line
            else
            {
                counter.text = "Level failed";
                playerController.GetComponentInChildren<SpriteRenderer>().enabled = false;
            }

            playerController.enabled = false;
            counter.fontSize = 40;
            counter.color = new Color(255, 255, 255, 255);

            timer = false;

            StartCoroutine("goToLevelSelect");
        }
    }

    void UpdateTimer()
    {
        // timer update code
        if (timerStart != 0 && !levelOver && timer)
        {
            int deltaTime = (int)Time.time - (int)timerStart;
            int minutes = deltaTime / 60;
            int seconds = deltaTime % 60;
            if (seconds > 9)
                time.text = minutes + ":" + seconds;
            else
                time.text = minutes + ":0" + seconds;
        }
    }

    IEnumerator goToLevelSelect()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene("levelSelect");
    }

    IEnumerator countdown1()
    {
        counter.enabled = true;
        counter.text = "3";
        yield return new WaitForSeconds(1);
        StartCoroutine("countdown2");
    }

    IEnumerator countdown2()
    {
        counter.text = "2";
        yield return new WaitForSeconds(1);
        StartCoroutine("countdown3");
    }

    IEnumerator countdown3()
    {
        counter.text = "1";
        yield return new WaitForSeconds(1);
        StartCoroutine("countdown4");
    }

    IEnumerator countdown4()
    {
        counter.text = "GO!";
        timerStart = Time.time;
        playerController.enabled = true;
        yield return new WaitForSeconds(0.5f);
        counter.GetComponent<Animation>().Play("Textfade");
    }



}
