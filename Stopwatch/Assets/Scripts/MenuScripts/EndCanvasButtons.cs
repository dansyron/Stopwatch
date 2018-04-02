using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class EndCanvasButtons : MonoBehaviour {

	public void Restart()
    {
        SceneManager.LoadScene("Feature Freeze Demo Scene");
        AudioManager.instance.Restart();
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Destroy(AudioManager.instance.gameObject);
    }
}
