using UnityEngine;
using System.Collections;

public class EndGameScript : MonoBehaviour {

    public Canvas endingCanvas;

	void OnCollisionEnter(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            endingCanvas.transform.gameObject.SetActive(true);
            FindObjectOfType<PlayerController>().transform.gameObject.SetActive(false);
            FindObjectOfType<TimerUI>().transform.gameObject.SetActive(false);
            //FindObjectOfType<TimeWall>().transform.gameObject.SetActive(false);
        }
    }
}
