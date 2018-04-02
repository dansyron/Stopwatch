using UnityEngine;
using System.Collections;

/// <summary>
/// Destroyed and prefab tagged level editor intersecting with this prefab
/// </summary>
public class DestroyLowerPrefab : MonoBehaviour {

    /*

    #region fields
    private LevelDesigner levelDesigner;
   
    //variables
    private bool gameObjectTagUpdater = false;

    #endregion

    #region Start
    // Use this for initialization
    void Start () {
        //gets components
        levelDesigner = GetComponentInParent<LevelDesigner>();
	}

    #endregion

    #region Update
    // Update is called once per frame
    void Update () {
        //checks if the game objects tag needs to be updated
	    if(gameObjectTagUpdater || levelDesigner.notMoving)
        {
            //starting tag wait co routine
            StartCoroutine(tagWait());
            gameObjectTagUpdater = false;
        }
        //If the parent is moving the trigger zone is set to untagged
        if(levelDesigner.notMoving == false)
        {
            gameObject.tag = "Untagged";
        }
	}

    #endregion

    #region Triggers
    /// <summary>
    /// Trigger on enter
    /// </summary>
    /// <param name="coll"></param>
    void OnTriggerStay(Collider coll)
    {
        //checks if the trigger it is staying in has tag level designer and it currently is not moving
        if (coll.tag == "Level Designer" && levelDesigner.notMoving)
        {
            //destroys the other gameobject
            coll.GetComponentInParent<LevelDesigner>().DestroyParentMethod();
            //tells unity that the tag needs to be updated
            gameObjectTagUpdater = true;

        }
    }

    #endregion

    #region Coroutine stuff
    /// <summary>
    /// forces a wait before updating tag to prevent prefabs from destroying eachother
    /// </summary>
    /// <returns></returns>
    IEnumerator tagWait()
    {
        //waits .2 seconds
        yield return new WaitForSeconds(.2f);
        //updates tag
        gameObject.tag = "Level Designer";
    }

    #endregion
     * 
     */
}