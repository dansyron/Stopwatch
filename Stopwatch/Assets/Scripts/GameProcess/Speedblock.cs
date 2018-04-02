using UnityEngine;
using System.Collections;

public class Speedblock : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnCollisionStay(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerController>().curHorSpeed += 1.2f;
        }
    }

    void OnCollisionExit(Collision coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            if (coll.gameObject.GetComponent<PlayerController>().curHorSpeed > Constants.HOR_MAX_SPEED)
            {
                coll.gameObject.GetComponent<PlayerController>().curHorSpeed = Constants.HOR_MAX_SPEED;
            }
        }
    }
}
