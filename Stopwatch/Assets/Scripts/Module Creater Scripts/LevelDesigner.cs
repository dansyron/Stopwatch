using UnityEngine;

/// <summary>
/// All the features people need when level editing, such as mouse movement on objects
/// </summary>
public class LevelDesigner : MonoBehaviour {

    /*
    #region fields
    //variables
    private Vector3 offset;
    private bool gameObjectTagUpdater = false;
    private DestroyParent destroyParent;
    public bool notMoving = false;
    private GameObject levelController;
    private PrefabSpawn prefabSpawn;
    #endregion

    #region start
    // Use this for initialization
    void Start()
    {
        //getting components
        levelController = GameObject.Find("Level Designer Controller");
        prefabSpawn = levelController.GetComponent<PrefabSpawn>();
        destroyParent = GetComponentInParent<DestroyParent>();
    }

    #endregion

    #region Update
    //Update is called once per frame
    void Update()
    {

        //checks if the game objects tag needs to be updated
        if (gameObjectTagUpdater)
        {
            //Updating the game objects tag
            gameObjectTagUpdater = false;
        }
    }

    #endregion

    #region Mouse Controls
    /// <summary>
    /// Does stuff when Mouse 1 is held down
    /// </summary>
    void OnMouseDown()
    {
        //sets offset for gameobject
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z));
    }

    /// <summary>
    /// Does stuff when the mouse is held down and dragged
    /// </summary>
    void OnMouseDrag()
    {
        //checks if the game is in editor state
        if (prefabSpawn.switchToLevelDesigner)
        {
            //gets current mouse positions
            Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Input.mousePosition.z);

            //get current mouse position in screen to world coords
            Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
            transform.position = curPosition;
            //sets not moving var to false
            notMoving = false;
        }
    }

    /// <summary>
    /// When mouse is released 
    /// </summary>
    void OnMouseUp()
    {
        //checks if the game is in editor state
        if (prefabSpawn.switchToLevelDesigner)
        {
            //sets variables to true
            gameObjectTagUpdater = true;
            notMoving = true;

            //drags the object being grabbed to the mouse
            transform.position = (new Vector3((transform.position.x), (transform.position.y), 0));
        }
    }

    #endregion

    #region method
    /// <summary>
    /// Only used to destroy parent destroying itself when a specific trigger happens
    /// </summary>
    public void DestroyParentMethod()
    {
        //sets destroy parent to true 
        destroyParent.destroyParentVar = true;
    }
    #endregion
    */
}
