using UnityEngine;
using System.Collections;

public class TimeRefill : MonoBehaviour {

    //public GameObject start;
    public GameObject target;
    public float speed;
    public float distance;

    public Vector3 startLoc;
    public Vector3 targetLoc;

    float destroyTimer = 1.0f;

    // Use this for initialization
    void Start ()
    {
        //startLoc = start.transform.position;
        target = GameObject.FindGameObjectWithTag("Clock");
        targetLoc = target.transform.position;
        targetLoc.z = targetLoc.z - distance;
        startLoc = GameObject.FindGameObjectWithTag("PlayerKnife").transform.position;
        startLoc.z = transform.position.z - distance;
        transform.position = startLoc;
	}
	
	// Update is called once per frame
	void Update ()
    {
        targetLoc = target.transform.position - new Vector3(0, 0, distance);
        transform.position = Vector3.MoveTowards(transform.position, targetLoc, speed * Time.deltaTime);
        //stop trail when it reaches target location
        if (Vector3.Distance(transform.position,targetLoc) <= 0.5f)
        {
            destroyTimer -= Time.deltaTime;
            if(destroyTimer <= 0)
                Destroy(this.gameObject);
        }
	}

    public void setStart(Vector3 startPosition)
    {
        startLoc = startPosition;
    }
    public void setTarget(Vector3 targetLocation)
    {
        targetLoc = targetLocation;
    }
}
