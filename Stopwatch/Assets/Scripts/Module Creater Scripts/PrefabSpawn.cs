using UnityEngine;
using System.Collections;

/// <summary>
/// Overarching class for level editing, allows spawning of objects, opening level editor, and pausing of game
/// </summary>
public class PrefabSpawn : MonoBehaviour {

    /*
    #region Fields
    //Prefabs for the current prefabs in the game
    public GameObject floor;
    public GameObject enemy;
    public Canvas gridCanvas;
    public bool switchToLevelDesigner = false;

    private ObjectToModuleConverter objectToModuleConverter;
    public SaveLoadModuleScript moduleSaveLoad;
    private bool spawnEnemyFlag;
    private bool spawnFloorFlag;

    // GameObject array of width gridX and height gridY hold all valuable gameObjects in grid area
    public const int gridX = 16;
    public const int gridY = 16;
    GameObject[,] gridObjects;

    // These are the bounds of the module grid
    public const double spawnAreaUpperBound = 4;
    public const double spawnAreaRightBound = 4.5;
    public const double spawnAreaLowerBound = -4;
    public const double spawnAreaLeftBound = -3.5;

    Vector3 currentMouseWorldPosition;

    #endregion

    #region Start

    void Start()
    {
        objectToModuleConverter = GetComponent<ObjectToModuleConverter>();
        moduleSaveLoad = GetComponent<SaveLoadModuleScript>();
        currentMouseWorldPosition = new Vector3();
        gridObjects = new GameObject[gridX, gridY];
    }

    #endregion

    #region Update
    // Update is called once per frame
    void Update () {
        //if the user clicks ` the game will pause to allow them to change the level
        if (Input.GetKeyDown(KeyCode.BackQuote) && switchToLevelDesigner == false)
        {
            switchToLevelDesigner = true;
        }

        //switch to level designer
        if (switchToLevelDesigner)
        {
            //shows the grid
            gridCanvas.enabled = true;
            //change the frame updates to 0
            Time.timeScale = 0;
            Time.fixedDeltaTime = 0;
        }

        // Determines mouse location in world units for accurate comparison
        currentMouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        
        // Checks for mouse being within grid area before allowing any objects to spawn
        if (currentMouseWorldPosition.x < spawnAreaRightBound && currentMouseWorldPosition.x > spawnAreaLeftBound
            && currentMouseWorldPosition.y < spawnAreaUpperBound && currentMouseWorldPosition.y > spawnAreaLowerBound)
        {
            // checks if the enemy needs to be spawned and the player clicks where
            if (Input.GetMouseButtonDown(0) && spawnEnemyFlag)
            {
                //spawns enemy and immediately snaps it to grid
                GameObject enemyLocal = (GameObject)Instantiate(enemy, currentMouseWorldPosition, Quaternion.identity);
                enemyLocal.GetComponent<EnemySnapInLevelEditor>().CorrectPosition();

                // Converts spawned enemy's location into location in gridObjects array and adds it there
                int tempI = (int)(((double)enemyLocal.transform.position.x + 3.25) * 2);
                int tempJ = (int)(((double)enemyLocal.transform.position.y - 3.75) * -2);

                // FOR TESTING PURPOSES
                print("EnemyLocal X: " + enemyLocal.transform.position.x.ToString() + "; After addition: " 
                    + ((double)enemyLocal.transform.position.x + 3.25).ToString()
                    + "; After Multiplication: " + (((double)enemyLocal.transform.position.x + 3.25) * 2).ToString());

                print("EnemyLocal Y: " + enemyLocal.transform.position.y.ToString() + "; After addition: "
                    + ((double)enemyLocal.transform.position.y - 3.75).ToString()
                    + "; After Multiplication: " + (((double)enemyLocal.transform.position.y - 3.75) * -2).ToString());

                print("TempI: " + tempI.ToString() + "; TempJ: " + tempJ.ToString());

                print("Index location: " + tempI.ToString() + ", " + tempJ.ToString());
                

                gridObjects[tempI, tempJ] = enemyLocal;
                //objectToModuleConverter.AddToArray(enemyLocal);
            }

            //spawns a floor and immediately snaps it to grid
            if (Input.GetMouseButtonDown(0) && spawnFloorFlag)
            {
                //spawns floor
                GameObject floorLocal = (GameObject)Instantiate(floor, currentMouseWorldPosition, Quaternion.identity);
                floorLocal.GetComponent<CorrectSpawnPlacement>().CorrectPosition();

                // Converts spawned floor's location into location in gridObjects array and adds it there
                int tempI = (int)(((double)floorLocal.transform.position.x + 3.25) * 2);
                int tempJ = (int)(((double)floorLocal.transform.position.y - 3.75) * -2);

                // FOR TESTING PURPOSES
                print("EnemyLocal X: " + floorLocal.transform.position.x.ToString() + "; After addition: "
                    + ((double)floorLocal.transform.position.x + 3.25).ToString()
                    + "; After Multiplication: " + (((double)floorLocal.transform.position.x + 3.25) * 2).ToString());

                print("EnemyLocal Y: " + floorLocal.transform.position.y.ToString() + "; After addition: "
                    + ((double)floorLocal.transform.position.y - 3.75).ToString()
                    + "; After Multiplication: " + (((double)floorLocal.transform.position.y - 3.75) * -2).ToString());

                print("TempI: " + tempI.ToString() + "; TempJ: " + tempJ.ToString());

                print("Index location: " + tempI.ToString() + ", " + tempJ.ToString());
                

                gridObjects[tempI, tempJ] = floorLocal;
                //objectToModuleConverter.AddToArray(floorLocal);
            }

            //cancels the current selection 
            if (Input.GetMouseButtonDown(1))
            {
                spawnFloorFlag = false;
                spawnEnemyFlag = false;
            }
        }
    }

    #endregion

    #region Public Methods
    
    /// <summary>
    /// sets spawn enemy to true
    /// </summary>
    public void SpawnEnemy()
    {
        spawnEnemyFlag = true;
        //disables other from spawning
        spawnFloorFlag = false;
    }

    /// <summary>
    /// sets spawn floor to true
    /// </summary>
    public void SpawnFloor()
    {
        spawnFloorFlag = true;
        //disable the other from spawning
        spawnEnemyFlag = false;
    }

    /// <summary>
    /// Allows game to resume from level editing state
    /// </summary>
    public void ResumeGameState()
    {
        switchToLevelDesigner = false;
            //hides the grid
            gridCanvas.enabled = false;
            //resuming time in the game
            Time.timeScale = 1;
            Time.fixedDeltaTime = 1;
    }

    // Completely removes all enemies and floors from module
    public void WipeModule()
    {
        //// Finds all enemies and wipes them
        //Object[] allEnemies = FindObjectsOfType(enemy.GetType());
        //foreach (Object enemyObject in allEnemies)
        //{
        //    Destroy(enemyObject);
        //}

        // Finds all enemies and wipes them
        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemyObject in allEnemies)
        {
            Destroy(enemyObject);
        }

        //// Finds all floors and wipes them
        //Object[] allFloors = FindObjectsOfType(floor.GetType());
        //foreach (Object floorObject in allFloors)
        //{
        //    Destroy(floorObject);
        //}

        // Finds all floors and wipes them
        GameObject[] allFloors = GameObject.FindGameObjectsWithTag("Floor");
        foreach (GameObject floorObject in allFloors)
        {
            Destroy(floorObject);
        }


        // Goes through gridObjects array and ensures that all its nodes are null
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                gridObjects[i, j] = null;
            }
        }
    }

    // Saves module that editor is currently working on to a test location file path
    public void SaveCurrentModuleBinary()
    {
        // Iterates through local copy of gridArray to store data in
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                // If gridObjects node contains nothing, sets module node to nothing
                if (gridObjects[i, j] == null)
                {
                    moduleSaveLoad.currentModuleInfo.SetBlock(i, j, ModuleObject.Nothing);
                }

                // If gridObjects node contains an enemy, sets module node to Enemy type
                else if (gridObjects[i, j].name == "enemyLevelDesign(Clone)")
                {
                    moduleSaveLoad.currentModuleInfo.SetBlock(i, j, ModuleObject.Enemy);
                }

                // If gridObjects node contains a floor, sets module node to Block type
                else if (gridObjects[i, j].name == "floorPlaceholderLevelDesign(Clone)")
                {
                    moduleSaveLoad.currentModuleInfo.SetBlock(i, j, ModuleObject.Block);
                }
            }
        }

        // Saves current module to file location "test3"
        moduleSaveLoad.SaveModuleAsBinary("test3");

    }

    // Loads module that was saved in conjunction with method SaveCurrentModuleBinary
    public void LoadPastModuleBinary()
    {
        // Wipes module so that it is clean for loading test module
        WipeModule();

        // Loads saved mosule into moduleSaveLoad
        moduleSaveLoad.LoadModuleAsBinary("test3");

        
        // Pulls gridArray from currentModuleInfo into local temporary copy
        ModuleObject[,] tempModuleArray = moduleSaveLoad.currentModuleInfo.GetGridArray();
        
        // Iterates through local copy of gridArray to store data in
        for (int i = 0; i < gridX; i++)
        {
            for (int j = 0; j < gridY; j++)
            {
                // If tempModuleArray node contains nothing, sets gridArray node to nothing
                if (tempModuleArray[i, j] == ModuleObject.Nothing)
                {
                    gridObjects[i, j] = null;
                }

                // If tempModuleArray node contains Enemy type, spawns enemy and sets it in gridArray
                else if (tempModuleArray[i, j] == ModuleObject.Enemy)
                {
                    // Determines spawned enemy's location
                    double tempI = (((double)i / 2) - 3.25);
                    double tempJ = (((double)j / -2) + 3.75);

                    //spawns enemy and immediately snaps it to grid
                    GameObject enemyLocal = (GameObject)Instantiate(enemy, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
                    enemyLocal.GetComponent<EnemySnapInLevelEditor>().CorrectPosition();

                    // Adds enemy object to gridObjects
                    gridObjects[i, j] = enemyLocal;
                }

                // If tempModuleArray node contains Block type, spawns floor and sets it in gridArray
                else if (tempModuleArray[i, j] == ModuleObject.Block)
                {
                    // Determines spawned floor's location
                    double tempI = (((double)i / 2) - 3.25);
                    double tempJ = (((double)j / -2) + 3.75);

                    //spawns floor and immediately snaps it to grid
                    GameObject floorLocal = (GameObject)Instantiate(floor, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
                    floorLocal.GetComponent<CorrectSpawnPlacement>().CorrectPosition();

                    // Adds floor object to gridObjects
                    gridObjects[i, j] = floorLocal;
                }
            }
        }
    }

    #endregion
     */
}
