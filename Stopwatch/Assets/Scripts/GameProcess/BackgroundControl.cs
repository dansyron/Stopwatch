using UnityEngine;
using System.Collections;

public class BackgroundControl : MonoBehaviour {

    public GameObject[] buildings;
    public GameObject[] background;
    public GameObject[] fogs;
    public Camera cameraObject;

    void Start()
    {
        cameraObject = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

	// Update is called once per frame
	void Update ()
    {
        foreach (GameObject building in buildings)
        {
            if (building.transform.position.x < cameraObject.transform.position.x - 150)
            {
                building.transform.position = new Vector3(building.transform.position.x+260,building.transform.position.y, building.transform.position.z);
            }
        }
        foreach (GameObject back in background)
        {
            if (back.transform.position.x < cameraObject.transform.position.x - 150)
            {
                back.transform.position = new Vector3(back.transform.position.x + 260, back.transform.position.y, back.transform.position.z);
            }
        }
        foreach (GameObject fog in fogs)
        {
            if (fog.transform.position.x < cameraObject.transform.position.x - 150)
            {
                fog.transform.position = new Vector3(fog.transform.position.x + 260, fog.transform.position.y, fog.transform.position.z);
            }
        }
    }
}
