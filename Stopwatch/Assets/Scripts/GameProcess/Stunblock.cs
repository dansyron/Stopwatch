using UnityEngine;
using System.Collections;

public class Stunblock : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}

    void OnCollisionEnter(Collision coll)
    {
        if(coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerController>().Action = PlayerController.ActionState.IsHurt;
        }
    }
}
