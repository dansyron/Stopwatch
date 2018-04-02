using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttonbehaviour : MonoBehaviour {

    public Button tutorialText;
    public string sceneName;

    public Canvas mainMenu;
    public Canvas loadMenu;

	public void StartButton()
    {
        //SceneManager.LoadScene("Game Demo Scene");
        //loadMenu.transform.gameObject.SetActive(true);
        //mainMenu.transform.gameObject.SetActive(false);
        SceneManager.LoadScene("LoadingScreen");

    }

    public void ExitButton()
    {
        Application.Quit();
    }

    public void TutorialButton()
    {
        if (!tutorialText.IsActive())
        {
            tutorialText.transform.gameObject.SetActive(true);
        }
        else
        {
            tutorialText.transform.gameObject.SetActive(false);
        }
    }

    public void ChangeScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}
