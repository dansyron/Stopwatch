using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingSceneScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

        SceneManager.LoadScene("Feature Freeze Demo Scene");

	}

    void Update()
    {
        if (isActiveAndEnabled)
        {
            SceneManager.LoadScene("Feature Freeze Demo Scene");
        }
    }
}
