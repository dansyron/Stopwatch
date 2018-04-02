using UnityEngine;
using System.Collections;

/// <summary>
/// Corrects spawn placement for onjects spawned while editing
/// </summary>
public class CorrectSpawnPlacement : MonoBehaviour 
{
    /*

    #region fields

    public float componentOffset = .5f;
    private GameObject levelController;
    private PrefabSpawn prefabSpawn;

    #endregion

    #region Start
    // Use this for initialization
    void Start () 
    {
        //getting components
        levelController = GameObject.Find("Level Designer Controller");
        prefabSpawn = levelController.GetComponent<PrefabSpawn>();
	}
    #endregion

    #region Update

    // Update is called once per frame
    void Update()
    {
        //checks if the game is in editor state
        if (prefabSpawn.switchToLevelDesigner)
        {
            //temp variable
            Vector3 tempRoundedPosition;
            //setting its posititon
            tempRoundedPosition = (new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z)));

            //Checks the current location against a rounded one. 
            //If the location is rounded down lower the block by the offset 
            //when doing the transform If the block is rounded up 
            //dont manipulate it at all
            // - (componentOffset / 2)
            //checks if any are already on a half block
            if (transform.position.y == Mathf.Round(transform.position.y) - componentOffset || transform.position.x == Mathf.Round(transform.position.x) - componentOffset)
            {
                //if they are nothing happens, just preventing rounding from messing with it
            }
            //set position if neither are rounded up
            else if (tempRoundedPosition.y < transform.position.y && tempRoundedPosition.x < transform.position.x)
            {
                //sets the position to a round location to simulate snapping
                transform.position = (new Vector3(Mathf.Round(transform.position.x) + (componentOffset / 2), Mathf.Round(transform.position.y) + (componentOffset / 2), 0));
            }
            //sets position if y is rounded up
            else if (tempRoundedPosition.y > transform.position.y && tempRoundedPosition.x < transform.position.x)
            {
                //sets the position to a round location to simulate snapping
                transform.position = (new Vector3(Mathf.Round(transform.position.x) + (componentOffset / 2), Mathf.Round(transform.position.y) - (componentOffset / 2), 0));
            }
            //sets position if x is rounded up
            else if (tempRoundedPosition.y < transform.position.y && tempRoundedPosition.x > transform.position.x)
            {
                //sets the position to a round location to simulate snapping
                transform.position = (new Vector3(Mathf.Round(transform.position.x) - (componentOffset / 2), Mathf.Round(transform.position.y) + (componentOffset / 2), 0));
            }
            //sets position if x and y are both rounded up
            else if (tempRoundedPosition.y > transform.position.y && tempRoundedPosition.x > transform.position.x)
            {
                //sets the position to a round location to simulate snapping
                transform.position = (new Vector3(Mathf.Round(transform.position.x) - (componentOffset / 2), Mathf.Round(transform.position.y) - (componentOffset / 2), 0));
            }
        }
    }

    #endregion

    #region Public Method

    // Does exactly what Update does, but can be called externally
    public void CorrectPosition ()
    {
            //temp variable
            Vector3 tempRoundedPosition;
            //setting its posititon
            tempRoundedPosition = (new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z)));

            //Checks the current location against a rounded one. 
            //If the location is rounded down lower the block by the offset 
            //when doing the transform If the block is rounded up 
            //dont manipulate it at all
            // - (componentOffset / 2)
            //checks if any are already on a half block
            if (transform.position.y == Mathf.Round(transform.position.y) - componentOffset || transform.position.x == Mathf.Round(transform.position.x) - componentOffset)
            {
                //if they are nothing happens, just preventing rounding from messing with it
            }
            //set position if neither are rounded up
            else if (tempRoundedPosition.y < transform.position.y && tempRoundedPosition.x < transform.position.x)
            {
                //sets the position to a round location to simulate snapping
                transform.position = (new Vector3(Mathf.Round(transform.position.x) + (componentOffset / 2), Mathf.Round(transform.position.y) + (componentOffset / 2), 0));
            }
            //sets position if y is rounded up
            else if (tempRoundedPosition.y > transform.position.y && tempRoundedPosition.x < transform.position.x)
            {
                //sets the position to a round location to simulate snapping
                transform.position = (new Vector3(Mathf.Round(transform.position.x) + (componentOffset / 2), Mathf.Round(transform.position.y) - (componentOffset / 2), 0));
            }
            //sets position if x is rounded up
            else if (tempRoundedPosition.y < transform.position.y && tempRoundedPosition.x > transform.position.x)
            {
                //sets the position to a round location to simulate snapping
                transform.position = (new Vector3(Mathf.Round(transform.position.x) - (componentOffset / 2), Mathf.Round(transform.position.y) + (componentOffset / 2), 0));
            }
            //sets position if x and y are both rounded up
            else if (tempRoundedPosition.y > transform.position.y && tempRoundedPosition.x > transform.position.x)
            {
                //sets the position to a round location to simulate snapping
                transform.position = (new Vector3(Mathf.Round(transform.position.x) - (componentOffset / 2), Mathf.Round(transform.position.y) - (componentOffset / 2), 0));
            }
    }

    #endregion
     */
}