using UnityEngine;
using System.Collections;

public class DodgeEffect : MonoBehaviour {

    float timer = 1.0f;
	
	// Update is called once per frame
	void Update ()
    {
        timer -= Time.deltaTime;
        if (timer <= 0.0f)
            Destroy(this.gameObject);
	}
}
