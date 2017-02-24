using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ButtonController : MonoBehaviour {

	public void loadLevel(string levelName)
    {
        SceneManager.LoadScene(levelName);
    }

    public void exitGame()
    {
        Application.Quit();
    }

    public void clearHighscores()
    {
        for(int i = 1; i<(SceneManager.sceneCountInBuildSettings - 2); i++)
        {
            PlayerPrefs.SetFloat("HS" + i, 0);
        }
    }
}
