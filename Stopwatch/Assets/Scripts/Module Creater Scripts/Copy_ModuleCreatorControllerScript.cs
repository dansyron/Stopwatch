//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using System;
//using System.Runtime.Serialization.Formatters.Binary;
//using System.IO;
//using UnityEngine.UI;

//#region Table of Contents

////-------------------------------------------------------------------------------------------------------------------------------------------------------
////
//// Table of Contents
////
////      At some point I realized that anyone new trying to learn how this script worked was going to have a bad time. There's too much in here to
////      learn it all very quickly. Therefore, the purpose if this Table of Contents and the headings at the top of every region is to expedite the
////      process of learning and finding the various elements of this script.
////
////      Key:
////          name            = Region
////          [name]          = Class declaration
////          - name          = Subregion
////          > name()        = Method              --NOTE: Methods displayed this way do not include return types or arguments
////
////
////*******************************************************************************************************************************************************
////
////      ModuleCreatorControllerScript Class
////
////          [ModuleCreatorControllerScript]
////
////          - Fields
////
////          - Initialization
////
////              > Start()
////
////          - Update
////
////              > Update()
////
////          - Public Methods
////
////              - Button Click Methods
////
////                  > SpawnEnemy()
////                  > SpawnNormalBlock()
////                  > SpawnTimeBlock()
////                  > SpawnSpeedupBlock()
////                  > SpawnOverflowBlock()
////                  > SpawnStunBlock()
////                  > OnSaveTextFileClick()
////                  > OnLoadTextFileClick()
////                  > OnAcceptSaveTextFileClick()
////                  > OnAcceptLoadTextFileClick()
////                  > OnCancelClick()
////
////              - Editor Methods
////
////                  > ResumeGameState()
////                  > CorrectPosition()
////                  > MatchModuleGridWithObjectGrid()
////                  > MatchObjectGridWithModuleGrid()
////
////              - Module Related Methods
////
////                  > WipeModule()
////
////                  - Binary Formatter Module Methods
////
////                      > SaveEmptyModuleBinary()
////                      > LoadEmptyModuleBinary()
////                      > SaveModuleAsBinary()
////                      > LoadModuleAsBinary()
////                      > SaveModuleInListBinary()
////                      > LoadModuleInListBinary()
////
////                  - Text File Module Methods
////
////                      > ParseModuleToString()
////                      > ParseStringToModule()
////                      > SaveModuleAsTextFile()
////                      > LoadModuleAsTextFile()
////
////          - Private Methods
////
////              > InstantiateUIElement() --NOTE: GameObject override
////              > InstantiateUIElement() --NOTE: InputField override
////              > InstantiateUIElement() --NOTE: Button override
////
////      Module Object Enumeration
////
////          [ModuleObject]
////
////      ModuleInfo Class
////
////          [ModuleInfo]
////
////          - Fields
////
////          - Constructors
////
////              > ModuleInfo()
////
////          - Public Methods
////
////              > SetBlock()
////              > GetGridArray()
////              > ResetModule()
////
////-------------------------------------------------------------------------------------------------------------------------------------------------------

//#endregion

//#region ModuleCreatorControllerScript Class

////-------------------------------------------------------------------------------------------------------------------------------------------------------
////
//// Region:
////      ModuleCreatorControllerScript Class
////
////
//// Subregions:
////      > Fields
////      > Initialization
////      > Update
////      > Public Methods
////      > Private Methods
////
////-------------------------------------------------------------------------------------------------------------------------------------------------------

///// <summary>
///// Module editor script that allows for the creation, destruction, saving, and loading of modules
///// </summary>
//public class Copy_ModuleCreatorControllerScript : MonoBehaviour
//{
//    #region Fields

//    //---------------------------------------------------------------------------------------------------------------------------------------------------
//    //
//    // Region:
//    //      Fields
//    //
//    //---------------------------------------------------------------------------------------------------------------------------------------------------

//    //Prefabs for the current prefabs in the game
//    public GameObject normalBlock;
//    public GameObject timeBlock;
//    public GameObject overflowBlock;
//    public GameObject stunBlock;
//    public GameObject speedupBlock;
//    public GameObject enemy;
//    public Canvas gridCanvas;
//    public bool switchToLevelDesigner = true;
//    private GameObject spawnObject;
//    // Booleans that determine which item is spawned by clicking
//    private SpawnFlag spawnFlag;

//    // GameObject array of width gridX and height gridY hold all valuable gameObjects in grid area
//    public const int gridX = Constants.MODULE_GRID_WIDTH;
//    public const int gridY = Constants.MODULE_GRID_HEIGHT;
//    GameObject[,] gridObjects;

//    // These are the bounds of the module grid
//    public const double spawnAreaUpperBound = 4;
//    public const double spawnAreaRightBound = 4.5;
//    public const double spawnAreaLowerBound = -4;
//    public const double spawnAreaLeftBound = -3.5;

//    // Current module that is in use
//    public ModuleInfo currentModuleInfo;

//    // List of ModuleInfo for ease of access
//    private List<ModuleInfo> moduleInfoList;

//    // Holds current mouse position
//    Vector3 currentMouseWorldPosition;

//    // Offset for determining correct placement of items in game world
//    public float componentOffset = .5f;

//    // The textbox that appears and prompts user for save location upon saving
//    Text saveLocationTextbox;

//    // These are the empty UI element prefabs from which other UI elements will be generated
//    public Button emptyButtonPrefab;
//    public InputField emptyInputFieldPrefab;

//    // UI elements related to module saving and loading
//    InputField fileNameInput;
//    Button acceptTextFileNameAndSave;
//    Button acceptTextFileNameAndLoad;
//    Button cancelSaveOrLoad;

//    // Constants that dictate the positioning and sizes of save load UI elements
//    const int UI_X_POSITION_OFFSET = 350;
//    const int UI_Y_POSITION_OFFSET = 45;

//    const float UI_STANDARD_HEIGHT = 30;
//    const float UI_STANDARD_WIDTH = 160;
//    const float UI_PARTIAL_WIDTH = 70;

//    // The directory that the modules will save to
//    string moduleDirectoryPath = @"C:\Program Files\Stopwatch Modules";

//    #endregion

//    #region Initialization

//    //---------------------------------------------------------------------------------------------------------------------------------------------------
//    //
//    // Region:
//    //      Initialization
//    //
//    //---------------------------------------------------------------------------------------------------------------------------------------------------

//    void Start()
//    {
//        // Initializes fields to have no values
//        currentMouseWorldPosition = new Vector3();
//        gridObjects = new GameObject[gridX, gridY];

//        currentModuleInfo = new ModuleInfo();
//        moduleInfoList = new List<ModuleInfo>();

//        // Procedurally generating much of the UI


//        // FILE NAME INPUT FIELD
//        // Creates the file name input field
//        fileNameInput = InstantiateUIElement(emptyInputFieldPrefab, gridCanvas, UI_X_POSITION_OFFSET, UI_Y_POSITION_OFFSET,
//            UI_STANDARD_WIDTH, UI_STANDARD_HEIGHT, false);

//        // Sets name
//        fileNameInput.name = "File Name Input Field";

//        // Sets the file name input field to only accept alphanumeric characters to prevent an invalid
//        // name for a file from being passed into a save method
//        fileNameInput.GetComponent<InputField>().contentType = InputField.ContentType.Alphanumeric;


//        // ACCEPT TEXT FILE NAME AND SAVE BUTTON
//        // Creates the button for accepting a file name and saving it.
//        acceptTextFileNameAndSave = InstantiateUIElement(emptyButtonPrefab, gridCanvas, UI_X_POSITION_OFFSET - 40, 0,
//            UI_PARTIAL_WIDTH, UI_STANDARD_HEIGHT, false);

//        // Sets name and the button text that is displayed
//        acceptTextFileNameAndSave.name = "Accept and Save Button - Text File";
//        acceptTextFileNameAndSave.GetComponentInChildren<Text>().text = "Accept";

//        // Sets the onClick listener to the appropriate button click method
//        acceptTextFileNameAndSave.onClick.AddListener(() => OnAcceptSaveTextFileClick());


//        // ACCEPT TEXT FILE NAME AND LOAD BUTTON
//        // Creates the button for accepting a file name and loading it.
//        acceptTextFileNameAndLoad = InstantiateUIElement(emptyButtonPrefab, gridCanvas, UI_X_POSITION_OFFSET - 40, 0,
//            UI_PARTIAL_WIDTH, UI_STANDARD_HEIGHT, false);

//        // Sets name and the button text that is displayed
//        acceptTextFileNameAndLoad.name = "Accept and Load Button - Text File";
//        acceptTextFileNameAndLoad.GetComponentInChildren<Text>().text = "Accept";

//        // Sets the onClick listener to the appropriate button click method
//        acceptTextFileNameAndLoad.onClick.AddListener(() => OnAcceptLoadTextFileClick());


//        // CANCEL SAVE OR LOAD BUTTON
//        // Creates the button for canceling both saving and loading.
//        cancelSaveOrLoad = InstantiateUIElement(emptyButtonPrefab, gridCanvas, UI_X_POSITION_OFFSET + 40, 0,
//            UI_PARTIAL_WIDTH, UI_STANDARD_HEIGHT, false);

//        // Sets name and the button text that is displayed
//        cancelSaveOrLoad.name = "Cancel Save or Load Button";
//        cancelSaveOrLoad.GetComponentInChildren<Text>().text = "Cancel";

//        // Sets the onClick listener to the appropriate button click method
//        cancelSaveOrLoad.onClick.AddListener(() => OnCancelClick());


//        // Creates a directory in which to save modules if one does not already exist
//        if (Directory.Exists(moduleDirectoryPath) == false)
//        {
//            try
//            {
//                Directory.CreateDirectory(moduleDirectoryPath);
//            }
//            catch (Exception e)
//            {
//                print("Error: could not create file directory - " + e.ToString());
//            }
//        }


//        // Creates a blank module that can be used to start a new module in the editor
//        WipeModule();
//        SaveModuleAsTextFile("empty");
//    }

//    #endregion

//    #region Update

//    //---------------------------------------------------------------------------------------------------------------------------------------------------
//    //
//    // Region:
//    //      Update
//    //
//    //---------------------------------------------------------------------------------------------------------------------------------------------------

//    // Update is called once per frame
//    void Update()
//    {
//        //if the user clicks ` the game will pause to allow them to change the level
//        if (Input.GetKeyDown(KeyCode.BackQuote) && switchToLevelDesigner == false)
//        {
//            switchToLevelDesigner = true;
//        }

//        //switch to level designer
//        if (switchToLevelDesigner)
//        {
//            //shows the grid
//            gridCanvas.enabled = true;
//            //change the frame updates to 0
//            Time.timeScale = 0;
//            Time.fixedDeltaTime = 0;
//        }

//        // Determines mouse location in world units for accurate comparison
//        currentMouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));

//        // Checks for mouse being within grid area before allowing any objects to spawn or despawned
//        if (currentMouseWorldPosition.x < spawnAreaRightBound && currentMouseWorldPosition.x > spawnAreaLeftBound
//            && currentMouseWorldPosition.y < spawnAreaUpperBound && currentMouseWorldPosition.y > spawnAreaLowerBound)
//        {
//            // checks if the enemy needs to be spawned and the player clicks where
//            if (Input.GetMouseButtonDown(0))
//            {
//                //Switch statement that instantiates the correct object
//                switch (spawnFlag)
//                {
//                    //Enemy Spawn
//                    case SpawnFlag.Enemy:
//                        //spawns enemy and immediately snaps it to grid
//                        spawnObject = (GameObject)Instantiate(enemy, CorrectPosition(currentMouseWorldPosition), Quaternion.identity);
//                        break;
//                    //Normal Block Spawn
//                    case SpawnFlag.NormalBlock:
//                        //spawns enemy and immediately snaps it to grid
//                        spawnObject = (GameObject)Instantiate(normalBlock, CorrectPosition(currentMouseWorldPosition), Quaternion.identity);
//                        break;
//                    //Overflow Block Spawn
//                    case SpawnFlag.OverflowBlock:
//                        //spawns enemy and immediately snaps it to grid
//                        spawnObject = (GameObject)Instantiate(overflowBlock, CorrectPosition(currentMouseWorldPosition), Quaternion.identity);
//                        break;
//                    //Speed Block Spawn
//                    case SpawnFlag.SpeedupBlock:
//                        //spawns enemy and immediately snaps it to grid
//                        spawnObject = (GameObject)Instantiate(speedupBlock, CorrectPosition(currentMouseWorldPosition), Quaternion.identity);
//                        break;
//                    //Stun Block Spawn
//                    case SpawnFlag.StunBlock:
//                        //spawns enemy and immediately snaps it to grid
//                        spawnObject = (GameObject)Instantiate(stunBlock, CorrectPosition(currentMouseWorldPosition), Quaternion.identity);
//                        break;
//                    //Time Block Spawn
//                    case SpawnFlag.TimeBlock:
//                        //spawns enemy and immediately snaps it to grid
//                        spawnObject = (GameObject)Instantiate(timeBlock, CorrectPosition(currentMouseWorldPosition), Quaternion.identity);
//                        break;
//                    //No selection is set
//                    case SpawnFlag.NoSelection:
//                        //Does nothing
//                        break;
//                    //Default does nothing
//                    default:
//                        break;
//                }

//                // Converts spawned enemy's location into location in gridObjects array and adds it there
//                int tempI = (int)(((double)spawnObject.transform.position.x + 3.25) * 2);
//                int tempJ = (int)(((double)spawnObject.transform.position.y - 3.75) * -2);

//                /*
//                // FOR TESTING PURPOSES
//                print("EnemyLocal X: " + enemyLocal.transform.position.x.ToString() + "; After addition: " 
//                    + ((double)enemyLocal.transform.position.x + 3.25).ToString()
//                    + "; After Multiplication: " + (((double)enemyLocal.transform.position.x + 3.25) * 2).ToString());

//                print("EnemyLocal Y: " + enemyLocal.transform.position.y.ToString() + "; After addition: "
//                    + ((double)enemyLocal.transform.position.y - 3.75).ToString()
//                    + "; After Multiplication: " + (((double)enemyLocal.transform.position.y - 3.75) * -2).ToString());

//                print("TempI: " + tempI.ToString() + "; TempJ: " + tempJ.ToString());

//                print("Index location: " + tempI.ToString() + ", " + tempJ.ToString());
//                */

//                // If an object already exists in the spot where the user clicked, that object is detroyed before being replaced
//                if (gridObjects[tempI, tempJ] != null)
//                {
//                    Destroy(gridObjects[tempI, tempJ]);
//                }
//                gridObjects[tempI, tempJ] = spawnObject;
//                //objectToModuleConverter.AddToArray(enemyLocal);
//            }

//            // If a spot is right-clicked, removes the game object from that spot
//            if (Input.GetMouseButtonDown(1))
//            {
//                // Calculates the position of the mouse relative to the grid
//                int tempX = (int)(((double)CorrectPosition(currentMouseWorldPosition).x + 3.25) * 2);
//                int tempY = (int)(((double)CorrectPosition(currentMouseWorldPosition).y - 3.75) * -2);

//                // If the grid contains an object at that spot, it destroys it and sets that grid spot to null
//                if (gridObjects[tempX, tempY] != null)
//                {
//                    Destroy(gridObjects[tempX, tempY]);
//                    gridObjects[tempX, tempY] = null;
//                }
//            }
//        }
//    }

//    #endregion

//    #region Public Methods

//    //---------------------------------------------------------------------------------------------------------------------------------------------------
//    //
//    // Region:
//    //      Public Methods
//    //
//    //
//    // Subregions:
//    //      > Button Click Methods
//    //      > Editor Methods
//    //      > Module Related Methods
//    //      > Testing Methods
//    //
//    //---------------------------------------------------------------------------------------------------------------------------------------------------

//    #region Button Click Methods

//    //---------------------------------------------------------------------------------------------------------------------------------------------------
//    //
//    // Region:
//    //      Button Click Methods
//    //
//    //---------------------------------------------------------------------------------------------------------------------------------------------------

//    /// <summary>
//    /// Sets spawnflag to SpawnEnemy
//    /// </summary>
//    public void SpawnEnemy()
//    {
//        spawnFlag = SpawnFlag.Enemy;
//    }

//    /// <summary>
//    /// Sets spawnflag to NormalBlock
//    /// </summary>
//    public void SpawnNormalBlock()
//    {
//        spawnFlag = SpawnFlag.NormalBlock;
//    }

//    /// <summary>
//    /// Sets spawnflag to TimeBlock
//    /// </summary>
//    public void SpawnTimeBlock()
//    {
//        spawnFlag = SpawnFlag.TimeBlock;
//    }

//    /// <summary>
//    /// Sets spawnflag to OverflowBlock
//    /// </summary>
//    public void SpawnOverflowBlock()
//    {
//        spawnFlag = SpawnFlag.OverflowBlock;
//    }

//    /// <summary>
//    /// Sets spawnflag to SpeedupBlock
//    /// </summary>
//    public void SpawnSpeedupBlock()
//    {
//        spawnFlag = SpawnFlag.SpeedupBlock;
//    }

//    /// <summary>
//    /// Sets spawnflag to StunBlock
//    /// </summary>
//    public void SpawnStunBlock()
//    {
//        spawnFlag = SpawnFlag.StunBlock;
//    }

//    /// <summary>
//    /// Sets spawnflag to NoSelection
//    /// </summary>
//    public void SpawnNoSelection()
//    {
//        spawnFlag = SpawnFlag.NoSelection;
//    }
//    /// <summary>
//    /// Activates the input field for a file name, the cancel button, and the button that indicates
//    /// that the text in the input field is correct and to proceed with the save
//    /// </summary>
//    public void OnSaveTextFileClick()
//    {
//        fileNameInput.gameObject.SetActive(true);
//        acceptTextFileNameAndSave.gameObject.SetActive(true);
//        acceptTextFileNameAndLoad.gameObject.SetActive(false);
//        cancelSaveOrLoad.gameObject.SetActive(true);
//    }

//    /// <summary>
//    /// Activates the input field for a file name, the cancel button, and the button that indicates
//    /// that the text in the input field is correct and to proceed with the load
//    /// </summary>
//    public void OnLoadTextFileClick()
//    {
//        fileNameInput.gameObject.SetActive(true);
//        acceptTextFileNameAndSave.gameObject.SetActive(false);
//        acceptTextFileNameAndLoad.gameObject.SetActive(true);
//        cancelSaveOrLoad.gameObject.SetActive(true);
//    }

//    /// <summary>
//    /// Indicates that the text in the input field for file name is correct and that saving should occur
//    /// </summary>
//    public void OnAcceptSaveTextFileClick()
//    {
//        // Passes the text in the input field as the name for the file in SaveModuleAsTextFile
//        SaveModuleAsTextFile(fileNameInput.text);

//        // Makes it so that the save or load state resets back after saving
//        OnCancelClick();
//    }

//    /// <summary>
//    /// Indicates that the text in the input field for file name is correct and that loading should occur
//    /// </summary>
//    public void OnAcceptLoadTextFileClick()
//    {
//        // Passes the text in the input field as the name for the file in LoadModuleAsTextFile
//        LoadModuleAsTextFile(fileNameInput.text);

//        // Makes it so that the save or load state resets back after saving
//        OnCancelClick();
//    }

//    /// <summary>
//    /// Cancels save or load selection and disables the appropriate UI elements
//    /// </summary>
//    public void OnCancelClick()
//    {
//        // All UI elements involved with saving or loading past the initial ones are disabled
//        fileNameInput.gameObject.SetActive(false);
//        acceptTextFileNameAndSave.gameObject.SetActive(false);
//        acceptTextFileNameAndLoad.gameObject.SetActive(false);
//        cancelSaveOrLoad.gameObject.SetActive(false);
//    }

//    #endregion

//    #region Editor Methods

//    //---------------------------------------------------------------------------------------------------------------------------------------------------
//    //
//    // Region:
//    //      Editor Methods
//    //
//    //---------------------------------------------------------------------------------------------------------------------------------------------------

//    /// <summary>
//    /// Allows game to resume from level editing state
//    /// </summary>
//    public void ResumeGameState()
//    {
//        switchToLevelDesigner = false;
//        //hides the grid
//        gridCanvas.enabled = false;
//        //resuming time in the game
//        Time.timeScale = 1;
//        Time.fixedDeltaTime = 1;
//    }

//    /// <summary>
//    /// Adjusts an object's 3D position so that it snaps to a grid
//    /// </summary>
//    /// <param name="positionVector">The 3D position of the game object which will have its position adjusted</param>
//    /// <returns></returns>
//    public Vector3 CorrectPosition(Vector3 positionVector)
//    {
//        // Temporary variable that stores rounded position values
//        Vector3 tempRoundedPosition;

//        // Vector3 that will be returned
//        // Initialized to zero case vector
//        Vector3 correctedPositionVector = new Vector3(0, 0, 0);

//        //setting its posititon
//        tempRoundedPosition = (new Vector3(Mathf.Round(positionVector.x), Mathf.Round(positionVector.y), Mathf.Round(positionVector.z)));

//        //Checks the current location against a rounded one. 
//        //If the location is rounded down, lower the block by the offset 
//        //when doing the transform. If the block is rounded up, 
//        //dont manipulate it at all.

//        //checks if any are already on a half block
//        if ((positionVector.y == (Mathf.Round(positionVector.y) - componentOffset)) || (positionVector.x == (Mathf.Round(positionVector.x) - componentOffset)))
//        {
//            //if they are nothing happens, just preventing rounding from messing with it
//        }

//        //set position if neither are rounded up
//        else if (tempRoundedPosition.y < positionVector.y && tempRoundedPosition.x < positionVector.x)
//        {
//            //sets the position to a round location to simulate snapping
//            correctedPositionVector = (new Vector3(Mathf.Round(positionVector.x) + (componentOffset / 2), Mathf.Round(positionVector.y) + (componentOffset / 2), 0));
//        }

//        //sets position if y is rounded up
//        else if (tempRoundedPosition.y > positionVector.y && tempRoundedPosition.x < positionVector.x)
//        {
//            //sets the position to a round location to simulate snapping
//            correctedPositionVector = (new Vector3(Mathf.Round(positionVector.x) + (componentOffset / 2), Mathf.Round(positionVector.y) - (componentOffset / 2), 0));
//        }

//        //sets position if x is rounded up
//        else if (tempRoundedPosition.y < positionVector.y && tempRoundedPosition.x > positionVector.x)
//        {
//            //sets the position to a round location to simulate snapping
//            correctedPositionVector = (new Vector3(Mathf.Round(positionVector.x) - (componentOffset / 2), Mathf.Round(positionVector.y) + (componentOffset / 2), 0));
//        }

//        //sets position if x and y are both rounded up
//        else if (tempRoundedPosition.y > positionVector.y && tempRoundedPosition.x > positionVector.x)
//        {
//            //sets the position to a round location to simulate snapping
//            correctedPositionVector = (new Vector3(Mathf.Round(positionVector.x) - (componentOffset / 2), Mathf.Round(positionVector.y) - (componentOffset / 2), 0));
//        }

//        // Returns calculated corrected position
//        return correctedPositionVector;
//    }

//    /// <summary>
//    /// Updates current ModuleInfo grid to match gridObjects
//    /// </summary>
//    public void MatchModuleGridWithObjectGrid()
//    {
//        // Iterates through local copy of gridArray to store data in
//        for (int i = 0; i < gridX; i++)
//        {
//            for (int j = 0; j < gridY; j++)
//            {
//                // If gridObjects node contains nothing, sets module node to nothing
//                if (gridObjects[i, j] == null)
//                {
//                    currentModuleInfo.SetBlock(i, j, ModuleObject.Nothing);
//                }

//                // If gridObjects node contains an enemy, sets module node to Enemy type
//                else if (gridObjects[i, j].GetType() == enemy.GetType())
//                {
//                    currentModuleInfo.SetBlock(i, j, ModuleObject.Enemy);
//                }

//                // If gridObjects node contains a normal block, sets module node to Block type
//                else if (gridObjects[i, j].GetType() == normalBlock.GetType())
//                {
//                    currentModuleInfo.SetBlock(i, j, ModuleObject.Block);
//                }

//                // If gridObjects node contains a time block, sets module node to TimeBlock type
//                else if (gridObjects[i, j].GetType() == timeBlock.GetType())
//                {
//                    currentModuleInfo.SetBlock(i, j, ModuleObject.TimeBlock);
//                }

//                // If gridObjects node contains an overflow block, sets module node to OverflowBlock type
//                else if (gridObjects[i, j].GetType() == overflowBlock.GetType())
//                {
//                    currentModuleInfo.SetBlock(i, j, ModuleObject.OverflowBlock);
//                }

//                // If gridObjects node contains a stun block, sets module node to StunBlock type
//                else if (gridObjects[i, j].GetType() == stunBlock.GetType())
//                {
//                    currentModuleInfo.SetBlock(i, j, ModuleObject.StunBlock);
//                }

//                // If gridObjects node contains a speedup block, sets module node to SpeedupBlock type
//                else if (gridObjects[i, j].GetType() == speedupBlock.GetType())
//                {
//                    currentModuleInfo.SetBlock(i, j, ModuleObject.SpeedupBlock);
//                }
//            }
//        }
//    }

//    /// <summary>
//    /// Updates gridObjects to match current ModuleInfo grid
//    /// </summary>
//    public void MatchObjectGridWithModuleGrid()
//    {
//        // Wipes module
//        WipeModule();

//        // Pulls gridArray from currentModuleInfo into local temporary copy
//        ModuleObject[,] tempModuleArray = currentModuleInfo.GetGridArray();

//        // Iterates through local copy of gridArray to store data in
//        for (int i = 0; i < gridX; i++)
//        {
//            for (int j = 0; j < gridY; j++)
//            {
//                // If tempModuleArray node contains nothing, sets gridArray node to nothing
//                if (tempModuleArray[i, j] == ModuleObject.Nothing)
//                {
//                    gridObjects[i, j] = null;
//                }

//                else
//                {
//                    // Determines spawned enemy's location
//                    double tempI = (((double)i / 2) - 3.25);
//                    double tempJ = (((double)j / -2) + 3.75);

//                    // The object to be instantiated
//                    GameObject objectLocal;

//                    // If the object in the temporary module is an enemy, instantiate an enemy
//                    if (tempModuleArray[i, j] == ModuleObject.Enemy)
//                    {
//                        objectLocal = (GameObject)Instantiate(enemy, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
//                    }

//                    // If the object in the temporary module is a block, instantiate a normalBlock
//                    else if (tempModuleArray[i, j] == ModuleObject.Block)
//                    {
//                        objectLocal = (GameObject)Instantiate(normalBlock, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
//                    }

//                    // If the object in the temporary module is a timeBlock, instantiate a timeBlock
//                    else if (tempModuleArray[i, j] == ModuleObject.TimeBlock)
//                    {
//                        objectLocal = (GameObject)Instantiate(timeBlock, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
//                    }

//                    // If the object in the temporary module is a stunBlock, instantiate a stunBlock
//                    else if (tempModuleArray[i, j] == ModuleObject.StunBlock)
//                    {
//                        objectLocal = (GameObject)Instantiate(stunBlock, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
//                    }

//                    // If the object in the temporary module is an overflowBlock, instantiate an overflowBlock
//                    else if (tempModuleArray[i, j] == ModuleObject.OverflowBlock)
//                    {
//                        objectLocal = (GameObject)Instantiate(overflowBlock, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
//                    }

//                    // If the object in the temporary module is a speedupBlock, instantiate a speedupBlock
//                    else if (tempModuleArray[i, j] == ModuleObject.SpeedupBlock)
//                    {
//                        objectLocal = (GameObject)Instantiate(speedupBlock, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
//                    }

//                    // If no other case, create a new, empty GameObject
//                    else
//                    {
//                        objectLocal = new GameObject();
//                    }

//                    // Corrects the position of the local object
//                    objectLocal.transform.position = CorrectPosition(objectLocal.transform.position);

//                    // Adds the local object to gridObjects
//                    gridObjects[i, j] = objectLocal;
//                }
//            }
//        }
//    }

//    #endregion

//    #region Module Related Methods

//    //---------------------------------------------------------------------------------------------------------------------------------------------------
//    //
//    // Region:
//    //      Module Related Methods
//    //
//    //
//    // Subregions:
//    //      > Binary Formatter Module Methods
//    //      > Text File Module Methods
//    //
//    //---------------------------------------------------------------------------------------------------------------------------------------------------

//    /// <summary>
//    /// Completely removes all instantiated enemies and floors from module and object grid
//    /// </summary>
//    public void WipeModule()
//    {
//        // Finds all enemies and wipes them
//        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
//        foreach (GameObject enemyObject in allEnemies)
//        {
//            Destroy(enemyObject);
//        }

//        // Finds all floors and wipes them
//        GameObject[] allFloors = GameObject.FindGameObjectsWithTag("Floor");
//        foreach (GameObject floorObject in allFloors)
//        {
//            Destroy(floorObject);
//        }

//        // Goes through gridObjects array and ensures that all its nodes are null
//        for (int i = 0; i < gridX; i++)
//        {
//            for (int j = 0; j < gridY; j++)
//            {
//                gridObjects[i, j] = null;
//            }
//        }
//    }

//    #region Binary Formatter Module Methods

//    //---------------------------------------------------------------------------------------------------------------------------------------------------
//    //
//    // Region:
//    //      Binary Formatter Module Methods
//    //
//    //---------------------------------------------------------------------------------------------------------------------------------------------------

//    /// <summary>
//    /// Saves a new, empty module in a file to one predefined path.
//    /// </summary>
//    public void SaveEmptyModuleBinary()
//    {
//        // Establishes a binary formatter and creates file path that will be used for saving
//        BinaryFormatter bf = new BinaryFormatter();
//        FileStream file = File.Create(Application.persistentDataPath + "/moduleinfo.dat");

//        // Creates an empty ModuleInfo object, serializes data into file destination, and closes path
//        ModuleInfo moduleInfo = new ModuleInfo();
//        bf.Serialize(file, moduleInfo);
//        file.Close();
//    }

//    /// <summary>
//    /// Loads empty module that was saved using the SaveEmptyModuleBinary method
//    /// </summary>
//    public void LoadEmptyModuleBinary()
//    {
//        // Checks if data in destermined file destination exists before trying to load it
//        if (File.Exists(Application.persistentDataPath + "/moduleinfo.dat"))
//        {
//            // Establishes binary formatter and file path used for loading
//            BinaryFormatter bf = new BinaryFormatter();
//            FileStream file = File.Open(Application.persistentDataPath + "/moduleinfo.dat", FileMode.Open);

//            // Deserializes data into ModuleInfo object and closes file path
//            ModuleInfo moduleInfo = (ModuleInfo)bf.Deserialize(file);
//            file.Close();
//        }
//    }

//    /// <summary>
//    /// Saves module in a file destination that user defines
//    /// </summary>
//    /// <param name="fileExtension">The user defined file path name to which the module will save</param>
//    public void SaveModuleAsBinary(string fileExtension)
//    {
//        // Establishes a binary formatter and creates file path that will be used for saving
//        BinaryFormatter bf = new BinaryFormatter();
//        FileStream file = File.Create(Application.persistentDataPath + "/" + fileExtension + ".dat");

//        // Serializes current module data into file destination and closes path
//        bf.Serialize(file, currentModuleInfo);
//        file.Close();
//    }

//    /// <summary>
//    /// Loads module that was saved using the SaveModuleAsBinary method
//    /// </summary>
//    /// <param name="fileExtension">The same user defined file path name to which a module was saved using SaveModuleAsBinary</param>
//    public void LoadModuleAsBinary(string fileExtension)
//    {
//        // Checks if data in given file destination exists before trying to load it
//        if (File.Exists(Application.persistentDataPath + "/" + fileExtension + ".dat"))
//        {
//            // Establishes binary formatter and file path used for loading
//            BinaryFormatter bf = new BinaryFormatter();

//            // Deserializes data into currentModuleInfo object and closes file path
//            FileStream file = File.Open(Application.persistentDataPath + "/" + fileExtension + ".dat", FileMode.Open);
//            currentModuleInfo = (ModuleInfo)bf.Deserialize(file);
//            file.Close();
//        }
//    }

//    /// <summary>
//    /// Saves module in a file destination that is procedurally generated and pulled from a list
//    /// </summary>
//    public void SaveModuleInListBinary()
//    {
//        // Establishes a binary formatter and creates file path that will be used for saving
//        BinaryFormatter bf = new BinaryFormatter();
//        FileStream file = File.Create(Application.persistentDataPath + "/" + moduleInfoList.Count.ToString() + ".dat");

//        // Serializes current module data into file destination, adds ModuleInfo object to list, and closes path
//        moduleInfoList.Add(currentModuleInfo);
//        bf.Serialize(file, currentModuleInfo);
//        file.Close();
//    }

//    /// <summary>
//    /// Loads module that was saved using the SaveModuleInListBinary method
//    /// </summary>
//    /// <param name="listLocation">The location in a list from which a module will be loaded</param>
//    public void LoadModuleInListBinary(int listLocation)
//    {
//        // Checks if data in given file destination exists before trying to load it
//        if (File.Exists(Application.persistentDataPath + "/" + listLocation.ToString() + ".dat"))
//        {
//            // Establishes binary formatter and file path used for loading
//            BinaryFormatter bf = new BinaryFormatter();

//            // Deserializes data into currentModuleInfo object and closes file path
//            FileStream file = File.Open(Application.persistentDataPath + "/" + listLocation.ToString() + ".dat", FileMode.Open);
//            currentModuleInfo = (ModuleInfo)bf.Deserialize(file);
//            file.Close();
//        }
//    }

//    #endregion

//    #region Text File Module Methods

//    //---------------------------------------------------------------------------------------------------------------------------------------------------
//    //
//    // Region:
//    //      Text File Module Methods
//    //
//    //---------------------------------------------------------------------------------------------------------------------------------------------------

//    /// <summary>
//    /// Transforms the information contained in a ModuleInfo object into a string and returns it
//    /// </summary>
//    /// <param name="moduleInfo">The ModuleInfo object from which the array of module objects is made into a string</param>
//    /// <returns></returns>
//    public string[] ParseModuleToString(ModuleInfo moduleInfo)
//    {
//        // The string that will eventually be returned once it contains the necessary information
//        string[] parsedModule = new string[gridX];

//        // The temporary grid array of module objects that is to be transformed into a string
//        ModuleObject[,] tempModuleGrid = moduleInfo.GetGridArray();

//        // Loops through the temporary grid array
//        for (int i = 0; i < tempModuleGrid.GetLength(0); i++)
//        {
//            for (int j = 0; j < tempModuleGrid.GetLength(1); j++)
//            {
//                // Turns the module object info into a string, adds it to the string that will be returned,
//                // and adds a comma to separate the module object types, as long as the module object is not null
//                if (tempModuleGrid[i, j] != null)
//                {
//                    parsedModule[i] += (tempModuleGrid[i, j].ToString() + ",");
//                }
//            }

//            // Adds a new line separator between rows of module object info
//            // parsedModule[i] += "\n";
//        }

//        // Returns the final result
//        return parsedModule;
//    }

//    /// <summary>
//    /// Transforms the information contained by a string that was produced by the ParseModuleToString method into
//    /// a ModuleInfo object and returns it
//    /// </summary>
//    /// <param name="moduleString">The string produced by ParseModuleToString that contains the module information</param>
//    /// <returns></returns>
//    public ModuleInfo ParseStringToModule(string[] moduleString)
//    {
//        // The ModuleInfo that will be passed back once the parsing process is done
//        ModuleInfo parsedModuleInfo = new ModuleInfo();

//        // The temporary string that will determine the type of module object to add
//        string tempType = "";

//        // "k" denotes the index of the current character in the string
//        int k = 0;

//        // Loops through the module string array the same number of time as the expected
//        // height of the module grid contained by the string.
//        // "i" will be the tracker for the grid's x value
//        for (int i = 0; i < Constants.MODULE_GRID_HEIGHT; i++)
//        {
//            // Resets the character index at the beginning of every string to 0
//            k = 0;

//            // Loops through the length of the string in the row of the string array for the
//            // expected number of module objects contained therein.
//            // "j" will be the tracker for the grid's y value
//            for (int j = 0; j < Constants.MODULE_GRID_WIDTH; j++)
//            {
//                // Resets the temporary type string
//                tempType = "";

//                // Adds the first character in module string to temporary type string until a comma appears
//                // or theend of the string is reached
//                while (moduleString[i][k] != ","[0] && k < moduleString[i].Length)
//                {
//                    tempType += moduleString[i][k];

//                    // Moves to next character in string
//                    k++;
//                }

//                // After temporary type string has had characters passed in, analyze string to determine
//                // what module object needs to be added to module

//                // If the type string says "Block", add Block to module
//                if (tempType == ModuleObject.Block.ToString())
//                {
//                    parsedModuleInfo.SetBlock(i, j, ModuleObject.Block);
//                }

//                // If type string says "Enemy", add Enemy to module
//                else if (tempType == ModuleObject.Enemy.ToString())
//                {
//                    parsedModuleInfo.SetBlock(i, j, ModuleObject.Enemy);
//                }

//                // If nothing else, add Nothing to module
//                else
//                {
//                    parsedModuleInfo.SetBlock(i, j, ModuleObject.Nothing);
//                }

//                // Continues to next character, passing the present comma
//                k++;
//            }
//        }
//        // Returns the final result
//        return parsedModuleInfo;
//    }

//    /// <summary>
//    /// Saves current module to a text file at a user specified location
//    /// </summary>
//    /// <param name="filePathExtension">The user specified extension to the file path</param>
//    public void SaveModuleAsTextFile(string filePathExtension)
//    {
//        // Ensures file path extension isn't blank or null
//        if (filePathExtension != "" && filePathExtension != null)
//        {
//            // Updates the grid in current ModuleInfo to match gridObjects
//            MatchModuleGridWithObjectGrid();

//            // Parses current module and saves it to location
//            File.WriteAllLines((@"C:\temp\" + filePathExtension + ".txt"), ParseModuleToString(currentModuleInfo));

//            // Prints message to let user know of successful save
//            print("Saved: " + filePathExtension + ".txt");
//        }

//        // Prints error message if file path is invalid
//        else
//        {
//            print("Error: invalid file path extension in SaveModuleAsTextFile");
//        }
//    }

//    /// <summary>
//    /// Loads module information saved in a user specified location into current module
//    /// </summary>
//    /// <param name="filePathExtension">The user specified extension to the file path</param>
//    public void LoadModuleAsTextFile(string filePathExtension)
//    {
//        // Ensures file path extenion isn't blank or null and that it leads to a saved text file
//        if (filePathExtension != "" && filePathExtension != null && File.Exists(@"C:\temp\" + filePathExtension + ".txt"))
//        {
//            // Creates a temporary string to hold the text pulled in by ReadAllText
//            string[] tempModuleString = File.ReadAllLines((@"C:\temp\" + filePathExtension + ".txt"));

//            // Parses information held in the temporary string and passes it into the current module
//            currentModuleInfo = ParseStringToModule(tempModuleString);

//            // Updates gridObjects to match current ModuleInfo grid
//            MatchObjectGridWithModuleGrid();

//            // Prints message to let user know of successful load
//            print("Loaded: " + filePathExtension + ".txt");
//        }

//        // Prints error message if file path is invalid
//        else
//        {
//            print("Error: invalid file path extension in LoadModuleAsTextFile");
//        }
//    }

//    #endregion

//    #endregion

//    #region Testing Methods

//    //---------------------------------------------------------------------------------------------------------------------------------------------------
//    //
//    // Region:
//    //      Testing Methods
//    //
//    //---------------------------------------------------------------------------------------------------------------------------------------------------

//    /// <summary>
//    /// Testing method which can be used to measure efficacy of module saving and loading elements
//    /// </summary>
//    public void TestForData()
//    {
//        // TEST MODULE 0

//        print("Test Module 0:");

//        // Sets unique components in first row of currentModuleInfo
//        currentModuleInfo.SetBlock(0, 0, ModuleObject.Enemy);
//        currentModuleInfo.SetBlock(0, 1, ModuleObject.Enemy);
//        currentModuleInfo.SetBlock(0, 2, ModuleObject.Enemy);
//        currentModuleInfo.SetBlock(0, 3, ModuleObject.Block);
//        currentModuleInfo.SetBlock(0, 4, ModuleObject.Block);
//        currentModuleInfo.SetBlock(0, 5, ModuleObject.Block);
//        currentModuleInfo.SetBlock(0, 6, ModuleObject.Block);
//        currentModuleInfo.SetBlock(0, 7, ModuleObject.Enemy);
//        currentModuleInfo.SetBlock(0, 8, ModuleObject.Enemy);
//        currentModuleInfo.SetBlock(0, 9, ModuleObject.Block);
//        currentModuleInfo.SetBlock(0, 10, ModuleObject.Block);

//        // Prints off the nodes in first row of currenModuleInfo
//        for (int i = 0; i < 16; i++)
//        {
//            print("Current node [0 , " + i.ToString() + "]: " + currentModuleInfo.GetGridArray()[0, i].ToString());
//        }

//        // Saves Module 0 in file path "test0"
//        SaveModuleAsBinary("test0");


//        // TEST MODULE 1

//        print("Test Module 1:");

//        // Sets unique components in first row of currentModuleInfo
//        currentModuleInfo.SetBlock(0, 0, ModuleObject.Block);
//        currentModuleInfo.SetBlock(0, 1, ModuleObject.Block);
//        currentModuleInfo.SetBlock(0, 2, ModuleObject.Block);
//        currentModuleInfo.SetBlock(0, 3, ModuleObject.Block);
//        currentModuleInfo.SetBlock(0, 4, ModuleObject.Block);
//        currentModuleInfo.SetBlock(0, 5, ModuleObject.Block);
//        currentModuleInfo.SetBlock(0, 6, ModuleObject.Block);
//        currentModuleInfo.SetBlock(0, 7, ModuleObject.Block);
//        currentModuleInfo.SetBlock(0, 8, ModuleObject.Block);
//        currentModuleInfo.SetBlock(0, 9, ModuleObject.Block);
//        currentModuleInfo.SetBlock(0, 10, ModuleObject.Block);

//        // Prints off the nodes in first row of currenModuleInfo
//        for (int i = 0; i < 16; i++)
//        {
//            print("Current node [0 , " + i.ToString() + "]: " + currentModuleInfo.GetGridArray()[0, i].ToString());
//        }

//        // Saves Module 1 in file path name "test1"
//        SaveModuleAsBinary("test1");


//        // TESTING LOADING MODULE 0

//        // Loads Module 0 from file path "test0"
//        LoadModuleAsBinary("test0");

//        print("This should be Module 0:");

//        // Prints off row 0 nodes for user to compare with Module 0 printed before
//        for (int i = 0; i < 16; i++)
//        {
//            print("Current node [0 , " + i.ToString() + "]: " + currentModuleInfo.GetGridArray()[0, i].ToString());
//        }


//        // TESTING LOADING MODULE 1

//        // Loads Module 1 from file path "test1"
//        LoadModuleAsBinary("test1");

//        print("This should be Module 1:");

//        // Prints off row 0 nodes for user to compare with Module 1 printed before
//        for (int i = 0; i < 16; i++)
//        {
//            print("Current node [0 , " + i.ToString() + "]: " + currentModuleInfo.GetGridArray()[0, i].ToString());
//        }

//        // Resets currentModuleInfo so that it is blank again
//        currentModuleInfo.ResetModule();
//    }

//    /// <summary>
//    /// Tests the SaveModuleAsTextFile method with the test location value of "test0"
//    /// </summary>
//    public void TestSaveTextFile()
//    {
//        SaveModuleAsTextFile("test0");
//    }

//    /// <summary>
//    /// Tests the LoadModuleAsTextFile method - should be used in conjunction with TestSaveTextFile
//    /// </summary>
//    public void TestLoadTextFile()
//    {
//        LoadModuleAsTextFile("test0");
//    }

//    /// <summary>
//    /// Saves current module to one predefined path - use only for testing
//    /// </summary>
//    public void SaveCurrentModuleBinary()
//    {
//        // Updates current ModuleInfo to match gridObjects
//        MatchModuleGridWithObjectGrid();

//        // Saves current module to file location "test3"
//        SaveModuleAsBinary("test3");

//    }

//    /// <summary>
//    /// Loads module that was saved in conjunction with method SaveCurrentModuleBinary - use only for testing
//    /// </summary>
//    public void LoadPastModuleBinary()
//    {
//        // Wipes module so that it is clean for loading test module
//        WipeModule();

//        // Loads saved module
//        LoadModuleAsBinary("test3");

//        // Updates gridObjects to match current ModuleInfo
//        MatchObjectGridWithModuleGrid();
//    }

//    #endregion

//    #endregion

//    #region Private Methods

//    //---------------------------------------------------------------------------------------------------------------------------------------------------
//    //
//    // Region:
//    //      Private Methods
//    //
//    //---------------------------------------------------------------------------------------------------------------------------------------------------

//    /// <summary>
//    /// Instantiates a GameObject UI element involved with the module editor
//    /// </summary>
//    /// <param name="emptyObjectPrefabBase">The empty GameObject prefab off of which the object to instantiate will be based</param>
//    /// <param name="transformParentObject">The UI element to which the object to instantiate will be parented</param>
//    /// <param name="xPositionOffset">The difference from the position of the parent object's position's x value that the object to 
//    /// instantiate will be offset</param>
//    /// <param name="yPositionOffset">The difference from the position of the parent object's position's y value that the object to 
//    /// instantiate will be offset</param>
//    /// <param name="startsEnabled">Whether the object to instantiate starts off enabled or not</param>
//    /// <returns></returns>
//    private GameObject InstantiateUIElement(GameObject emptyObjectPrefabBase, GameObject transformParentObject,
//        int xPositionOffset, int yPositionOffset, bool startsEnabled)
//    {
//        // Instantiates the game object by copying an empty game object prefab
//        GameObject objectToInstantiate = Instantiate(emptyObjectPrefabBase);

//        // Sets transform parent to the parent object - usually will be a canvas if the object to instantiate is a UI element
//        objectToInstantiate.transform.SetParent(transformParentObject.transform);

//        // Sets the transform of the input field to the position offset specified in the arguments passed in
//        objectToInstantiate.transform.position.Set(objectToInstantiate.transform.position.x + xPositionOffset,
//            objectToInstantiate.transform.position.y + yPositionOffset, objectToInstantiate.transform.position.z);

//        // Enables or disables the game object according to the argument passed in
//        objectToInstantiate.SetActive(startsEnabled);

//        // Returns the instantiated GameObject
//        return objectToInstantiate;
//    }

//    /// <summary>
//    /// Instantiates an InputField UI element involved with the module editor
//    /// </summary>
//    /// <param name="emptyInputFieldPrefabBase">The empty InputField prefab off of which the InputField to instantiate will be based</param>
//    /// <param name="transformParentCanvas">The canvas to which the InputField to instantiate will be parented</param>
//    /// <param name="xPositionOffset">The difference from the position of the parent canvas's position's x value that the InputField to 
//    /// instantiate will be offset</param>
//    /// <param name="yPositionOffset">The difference from the position of the parent canvas's position's y value that the InputField to 
//    /// instantiate will be offset</param>
//    /// <param name="inputFieldWidth">The height of the InputField to instantiate</param>
//    /// <param name="inputFieldHeight">The width of the InputField to instantiate</param>
//    /// <param name="startsEnabled">Whether the InputField to instantiate starts off enabled or not</param>
//    /// <returns></returns>
//    private InputField InstantiateUIElement(InputField emptyInputFieldPrefabBase, Canvas transformParentCanvas,
//        int xPositionOffset, int yPositionOffset, float inputFieldWidth, float inputFieldHeight, bool startsEnabled)
//    {
//        // Instantiates the input field by copying an empty input field prefab
//        InputField inputFieldToInstantiate = (InputField)Instantiate(emptyInputFieldPrefabBase);

//        // Sets transform parent to the parent canvas
//        inputFieldToInstantiate.transform.SetParent(transformParentCanvas.transform, false);

//        // Sets the rect transform of the input field to the position and size specified in the arguments passed in
//        inputFieldToInstantiate.transform.localPosition = new Vector3(xPositionOffset, yPositionOffset, 0);
//        inputFieldToInstantiate.transform.localScale = new Vector3(inputFieldWidth / 160f, inputFieldHeight / 30f, 1f);

//        // Enables or disables the input field according to the argument passed in
//        inputFieldToInstantiate.gameObject.SetActive(startsEnabled);

//        // Returns the instantiated InputField
//        return inputFieldToInstantiate;
//    }

//    /// <summary>
//    /// Instantiates a Button UI element involved with the module editor
//    /// </summary>
//    /// <param name="emptyButtonPrefabBase">The empty Button prefab off of which the Button to instantiate will be based</param>
//    /// <param name="transformParentCanvas">The canvas to which the Button to instantiate will be parented</param>
//    /// <param name="xPositionOffset">The difference from the position of the parent canvas's position's x value that the Button to 
//    /// instantiate will be offset</param>
//    /// <param name="yPositionOffset">The difference from the position of the parent canvas's position's y value that the Button to 
//    /// instantiate will be offset</param>
//    /// <param name="buttonWidth">The height of the Button to instantiate</param>
//    /// <param name="buttonHeight">The width of the Button to instantiate</param>
//    /// <param name="startsEnabled">Whether the Button to instantiate starts off enabled or not</param>
//    /// <returns></returns>
//    private Button InstantiateUIElement(Button emptyButtonPrefabBase, Canvas transformParentCanvas,
//        int xPositionOffset, int yPositionOffset, float buttonWidth, float buttonHeight, bool startsEnabled)
//    {
//        // Instantiates the button by copying an empty button prefab
//        Button buttonToInstantiate = Instantiate(emptyButtonPrefabBase);

//        // Sets transform parent to the parent canvas
//        buttonToInstantiate.transform.SetParent(transformParentCanvas.transform, false);

//        // Sets the rect transform of the button to the position and size specified in the arguments passed in
//        buttonToInstantiate.transform.localPosition = new Vector3(xPositionOffset, yPositionOffset, 0);
//        buttonToInstantiate.transform.localScale = new Vector3(buttonWidth / 160f, buttonHeight / 30f, 1f);

//        // Enables or disables the button according to the argument passed in
//        buttonToInstantiate.gameObject.SetActive(startsEnabled);

//        // Returns the instantiated Button
//        return buttonToInstantiate;
//    }

//    #endregion
//}

//#endregion

//#region SpawnButton Enumerations

//public enum SpawnFlag { Enemy, TimeBlock, StunBlock, SpeedupBlock, OverflowBlock, NormalBlock, NoSelection };

//#endregion

//#region Module Object Enumeration

////-------------------------------------------------------------------------------------------------------------------------------------------------------
////
//// Region:
////      Module Object Enumeration
////
////-------------------------------------------------------------------------------------------------------------------------------------------------------

///// <summary>
///// Enumeration of possible module objects
///// </summary>
//public enum ModuleObject
//{
//    Nothing,       // 0
//    Enemy,         // 1
//    Block,         // 2
//    TimeBlock,     // 3
//    StunBlock,     // 4
//    SpeedupBlock,  // 5
//    OverflowBlock  // 6
//};

//#endregion

//#region ModuleInfo Class

////-------------------------------------------------------------------------------------------------------------------------------------------------------
////
//// Region:
////      ModuleInfo Class
////
////
//// Subregions:
////      > Fields
////      > Constructors
////      > Public Methods
////
////-------------------------------------------------------------------------------------------------------------------------------------------------------

///// <summary>
///// ModuleInfo stores module information, including placement of various objects in given module.
///// </summary>
//[Serializable]
//public class ModuleInfo
//{
//    #region Fields

//    //---------------------------------------------------------------------------------------------------------------------------------------------------
//    //
//    // Region:
//    //      Fields
//    //
//    //---------------------------------------------------------------------------------------------------------------------------------------------------

//    // Alterable size fields of module
//    public const int gridWidth = Constants.MODULE_GRID_WIDTH;
//    public const int gridHeight = Constants.MODULE_GRID_HEIGHT;

//    // Array that stores arrangement of module objects
//    private ModuleObject[,] gridArray;

//    #endregion

//    #region Constructors

//    //---------------------------------------------------------------------------------------------------------------------------------------------------
//    //
//    // Region:
//    //      Constructors
//    //
//    //---------------------------------------------------------------------------------------------------------------------------------------------------

//    /// <summary>
//    /// Empty constructor
//    /// </summary>
//    public ModuleInfo()
//    {
//        // Initializes grid array so that every node is empty
//        gridArray = new ModuleObject[gridWidth, gridHeight];
//        for (int i = 0; i < gridWidth; i++)
//        {
//            for (int j = 0; j < gridHeight; j++)
//            {
//                // Sets every node as Nothing to begin
//                gridArray[i, j] = ModuleObject.Nothing;
//            }
//        }
//    }

//    #endregion

//    #region Public Methods

//    //---------------------------------------------------------------------------------------------------------------------------------------------------
//    //
//    // Region:
//    //      Public Methods
//    //
//    //---------------------------------------------------------------------------------------------------------------------------------------------------

//    /// <summary>
//    /// Setter method that allows nodes of the module object grid array to be changed
//    /// </summary>
//    /// <param name="gridX">The x location in module object array</param>
//    /// <param name="gridY">The y location in module object array</param>
//    /// <param name="moduleObject">The type of module object that the module object array will store in the node</param>
//    public void SetBlock(int gridX, int gridY, ModuleObject moduleObject)
//    {
//        // Checks if the arguements for array location fall in the valid range
//        if (gridX < gridWidth && gridX >= 0 && gridY < gridHeight && gridY >= 0)
//        {
//            // Sets node to the type of module object
//            gridArray[gridX, gridY] = moduleObject;
//        }
//    }

//    /// <summary>
//    /// Getter method that allows access to the array that stores arrangement of module objects
//    /// </summary>
//    /// <returns></returns>
//    public ModuleObject[,] GetGridArray()
//    {
//        return gridArray;
//    }

//    /// <summary>
//    /// Resets module so that every node of the grid array has nothing
//    /// </summary>
//    public void ResetModule()
//    {
//        // Set grid array so that every node is empty
//        gridArray = new ModuleObject[gridWidth, gridHeight];
//        for (int i = 0; i < gridWidth; i++)
//        {
//            for (int j = 0; j < gridHeight; j++)
//            {
//                // Sets every node as nothing
//                gridArray[i, j] = ModuleObject.Nothing;
//            }
//        }
//    }

//    #endregion
//}

//#endregion


#region Module Block Relocation Code
    //void Update()
    //{
    //    // Finds all floors
    //    GameObject[] allFloors = GameObject.FindGameObjectsWithTag("Floor");
    //    // Finds all enemies
    //    GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
    //    //if the user clicks ` the game will pause to allow them to change the level
    //    if (Input.GetKeyDown(KeyCode.BackQuote) && switchToLevelDesigner == false)
    //    {
    //        switchToLevelDesigner = true;
    //    }

    //    //switch to level designer
    //    if (switchToLevelDesigner)
    //    {
    //        //shows the grid
    //        gridCanvas.enabled = true;
    //        //change the frame updates to 0
    //        Time.timeScale = 0;
    //        Time.fixedDeltaTime = 0;
    //    }

    //    // Determines mouse location in world units for accurate comparison
    //    currentMouseWorldPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
        
    //    // Checks for mouse being within grid area before allowing any objects to spawn or despawned
    //    if (currentMouseWorldPosition.x < spawnAreaRightBound && currentMouseWorldPosition.x > spawnAreaLeftBound
    //       && currentMouseWorldPosition.y < spawnAreaUpperBound && currentMouseWorldPosition.y > spawnAreaLowerBound)
    //    {
    //        // checks if the enemy needs to be spawned and the player clicks where
    //        if (Input.GetMouseButtonDown(0))
    //        {
                
    //            //if statement that checks for the element number in the 
    //            if (k < (gridX * gridY) ||
    //                u < (gridX * gridY))
    //            {
    //                //checks if there are more any more blocks to move before it uses ones already placed to finish block
    //                if (k == allFloors.Length)
    //                {
    //                    //resets array counter so it is not trying to hit values over the array size
    //                    k = 0;
    //                }
    //                //checks if there are more any more enemies to move before it uses ones already placed to finish block
    //                if (u == allEnemies.Length)
    //                {
    //                    //resets array counter so it is not trying to hit values over the array size
    //                    u = 0;
    //                }

    //                //If statement that checks if a block or an enemy needs to spawn
    //                if (spawnFlag == SpawnFlag.Enemy)
    //                {
    //                    //spawns enemy
    //                    allEnemies[u].transform.position = CorrectPosition(currentMouseWorldPosition);
    //                    //this make basic_infantry(clone) = basic_infantry
    //                    allEnemies[u].name = enemy.name;
    //                    int tempK = (int)(((double)allEnemies[u].transform.position.x + 3.25) * 2);
    //                    int tempL = (int)(((double)allEnemies[u].transform.position.y - 3.75) * -2);
    //                    gridObjects[tempK, tempL] = allEnemies[u];
    //                    // Converts spawned enemy's location into location in gridObjects array and adds it there

    //                }
    //                //Needs to not spawn an enemy
    //                else
    //                {
    //                    //moving a block from the reserve to the module
    //                    allFloors[k].transform.position = CorrectPosition(currentMouseWorldPosition);
    //                    //changing name from mainBlock(Clone) to mainBlock
    //                    allFloors[k].name = mainBlock.name;
    //                    int tempI = (int)(((double)allFloors[k].transform.position.x + 3.25) * 2);
    //                    int tempJ = (int)(((double)allFloors[k].transform.position.y - 3.75) * -2);
    //                    gridObjects[tempI, tempJ] = allFloors[k];
    //                    //objectLocal = (GameObject)Instantiate(mainBlock, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
    //                }
    //                //Switch statement that instantiates the correct object
    //                switch (spawnFlag)
    //                {
    //                    //Normal Block Spawn
    //                    case SpawnFlag.NormalBlock:
    //                        //spawns a normal block
    //                        allFloors[k].GetComponent<BlockTypes>().SetBlockEnumeration = BlockType.Normal;
    //                        break;
    //                    //Overflow Block Spawn
    //                    case SpawnFlag.OverflowBlock:
    //                        //spawns a overflow block
    //                        allFloors[k].GetComponent<BlockTypes>().SetBlockEnumeration = BlockType.Overflow;
    //                        break;
    //                    //Speed Block Spawn
    //                    case SpawnFlag.SpeedupBlock:
    //                        //spawns a speed up block
    //                        allFloors[k].GetComponent<BlockTypes>().SetBlockEnumeration = BlockType.Speed;
    //                        break;
    //                    //Stun Block Spawn
    //                    case SpawnFlag.StunBlock:
    //                        //spawns a stun block
    //                        allFloors[k].GetComponent<BlockTypes>().SetBlockEnumeration = BlockType.Stun;
    //                        break;
    //                    case SpawnFlag.WallJumpBlock:
    //                        //spawns a stun block
    //                        allFloors[k].GetComponent<BlockTypes>().SetBlockEnumeration = BlockType.WallJump;
    //                        break;
    //                    //Time Block Spawn
    //                    case SpawnFlag.TimeBlockInPhase:
    //                        //spawns a time block that is passable in the slowdown phase
    //                        allFloors[k].GetComponent<BlockTypes>().SetBlockEnumeration = BlockType.TimeInPhase;
    //                        break;
    //                    case SpawnFlag.TimeBlockOutOfPhase:
    //                        //spawns a time block that is passable in the normal speed phase
    //                        allFloors[k].GetComponent<BlockTypes>().SetBlockEnumeration = BlockType.TimeOutOfPhase;
    //                        break;
    //                    //Default does nothing
    //                    default:
    //                        break;
    //                }

    //                // If an object already exists in the spot where the user clicked, that object is detroyed before being replaced
    //                //if (gridObjects[tempI, tempJ] != null)
    //                //{
    //                //    Destroy(gridObjects[tempI, tempJ]);
    //                //}
    //                //gridObjects[tempI, tempJ] = allEnemies[u];
    //                //gridObjects[tempI, tempJ] = allFloors[k];
    //                /*
    //                // FOR TESTING PURPOSES
    //                print("EnemyLocal X: " + enemyLocal.transform.position.x.ToString() + "; After addition: " 
    //                    + ((double)enemyLocal.transform.position.x + 3.25).ToString()
    //                    + "; After Multiplication: " + (((double)enemyLocal.transform.position.x + 3.25) * 2).ToString());

    //                print("EnemyLocal Y: " + enemyLocal.transform.position.y.ToString() + "; After addition: "
    //                    + ((double)enemyLocal.transform.position.y - 3.75).ToString()
    //                    + "; After Multiplication: " + (((double)enemyLocal.transform.position.y - 3.75) * -2).ToString());

    //                print("TempI: " + tempI.ToString() + "; TempJ: " + tempJ.ToString());

    //                print("Index location: " + tempI.ToString() + ", " + tempJ.ToString());
    //                */



    //                //                    gridObjects[tempI, tempJ] = spawnObject;
    //                //objectToModuleConverter.AddToArray(enemyLocal);
                    
                    
    //                k++;
    //                u++;
    //            }
    //        }
    //        // If a spot is right-clicked, removes the game object from that spot
    //        if (Input.GetMouseButton(1))
    //        {
    //            // Calculates the position of the mouse relative to the grid
    //            int tempX = (int)(((double)CorrectPosition(currentMouseWorldPosition).x + 3.25) * 2);
    //            int tempY = (int)(((double)CorrectPosition(currentMouseWorldPosition).y - 3.75) * -2);

    //            // If the grid contains an object at that spot, it destroys it and sets that grid spot to null
    //            if (gridObjects[tempX, tempY] != null)
    //            {
    //                if (gridObjects[tempX, tempY].gameObject.tag == "Enemy")
    //                {
    //                    gridObjects[tempX, tempY].transform.position = enemyReserve;
    //                    gridObjects[tempX, tempY].GetComponent<BoxCollider>().enabled = false;
    //                    gridObjects[tempX, tempY].GetComponent<CapsuleCollider>().enabled = false;
    //                    gridObjects[tempX, tempY] = null;
    //                }
    //                else if (gridObjects[tempX, tempY].gameObject.tag == "Floor")
    //                {
    //                    gridObjects[tempX, tempY].transform.position = floorReserve;
    //                    gridObjects[tempX, tempY].GetComponent<BoxCollider>().enabled = false;
    //                    gridObjects[tempX, tempY] = null;
    //                }
    //            }
    //        }
    //        //assigns the floor their components if they are not in reserve
    //        foreach (GameObject floorObject in allFloors)
    //        {
    //            floorObject.gameObject.GetComponent<BoxCollider>().enabled = true;
    //        }

    //        foreach (GameObject enemyObject in allEnemies)
    //        {
    //            //assigns the enemy their components if they are not in reserve
    //            if (enemyObject.transform.position != enemyReserve)
    //            {
    //                //turns on the colliders/ scripts/ gravity
    //                enemyObject.gameObject.GetComponent<BoxCollider>().enabled = true;
    //                enemyObject.gameObject.GetComponent<CapsuleCollider>().enabled = true;
    //                enemyObject.gameObject.GetComponent<EnemyCharacter>().enabled = true;
    //                enemyObject.gameObject.GetComponent<BasicInfantryAI>().enabled = true;
    //                enemyObject.gameObject.GetComponent<Rigidbody>().useGravity = true;
    //            }
    //        }
    //    }
    //}

#endregion

#region New MatchObjectGridWithModuleGrid
//public void MatchObjectGridWithModuleGrid()
//    {
//        // Wipes module
//        WipeModule();

//        //int k = 0;
//        // Pulls gridArray from currentModuleInfo into local temporary copy
//        ModuleObject[,] tempModuleArray = currentModuleInfo.GetGridArray();

//        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
//        //Iterates through the the array removing any enemy not at the reserve point
//        for (int y = allEnemies.Length - 1; y >= 0; y--)
//        {
//            //checks if the object is not located at (100,100,100) interestingly, the enemy location
//            //updates itself after teh first enemy is placed to the correct enemyReserve location, however
//            // due to the first one throwing it off, need to have it check both locations every time
//            if (allEnemies[y].gameObject.transform.position != enemyReserve)
//            {
//                allEnemies[y] = allEnemies[y + 1];
//            }
//        }


//        // Finds all floors and wipes them
//        GameObject[] allFloors = GameObject.FindGameObjectsWithTag("Floor");
//        //Iterates through the the array
//        for (int p = allFloors.Length - 1; p >= 0; p--)
//        {
//            //checks if the object is not located at (5,0,0)
//            if (allFloors[p].gameObject.transform.position != floorReserve)
//            {
//                //moves the list down if an element needs to be removed
//                allFloors[p] = allFloors[p + 1];
//            }
//        }
//        // Iterates through local copy of gridArray to store data in
//        for (int i = 0; i < gridX; i++)
//        {
//            for (int j = 0; j < gridY; j++)
//            {
//                // If tempModuleArray node contains nothing, sets gridArray node to nothing
//                if (tempModuleArray[i, j] == ModuleObject.Nothing)
//                {
//                    gridObjects[i, j] = null;
//                }

//                else
//                {
//                    // Determines spawned enemy's location
//                    double tempI = (((double)i / 2) - 3.25);
//                    double tempJ = (((double)j / -2) + 3.75);

//                    // The object to be instantiated
//                    GameObject objectLocal;

//                    // If the object in the temporary module is an enemy, instantiate an enemy
//                    if (tempModuleArray[i, j] == ModuleObject.Enemy)
//                    {
//                        objectLocal = (GameObject)Instantiate(enemy, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
//                        //changing name from basic_infantry(Clone) to basic_infantry
//                        objectLocal.name = enemy.name;
//                    }

//                    if (k < (gridX * gridY))
//                    {

//                        // If the object in the temporary module is a block, instantiate a normalBlock
//                        if (tempModuleArray[i, j] == ModuleObject.Normal)
//                        {
//                            //moving a block from the reserve to the module
//                            allFloors[k].transform.position = new Vector3((float)tempI, (float)tempJ, 0f);
//                            //changing name from mainBlock(Clone) to mainBlock
//                            allFloors[k].name = mainBlock.name;
//                            //objectLocal = (GameObject)Instantiate(mainBlock, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
//                            //sets enum of block to Normal
//                            allFloors[k].GetComponent<BlockTypes>().SetBlockEnumeration = BlockType.Normal;

//                        }

//                        // If the object in the temporary module is a timeBlock, instantiate a timeBlockInPhase
//                        else if (tempModuleArray[i, j] == ModuleObject.InPhase)
//                        {
//                            //moving a block from the reserve to the module
//                            allFloors[k].transform.position = new Vector3((float)tempI, (float)tempJ, 0f);
//                            //changing name from mainBlock(Clone) to mainBlock
//                            allFloors[k].name = mainBlock.name;
//                            //objectLocal = (GameObject)Instantiate(mainBlock, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
//                            //sets enum of block to Normal
//                            allFloors[k].GetComponent<BlockTypes>().SetBlockEnumeration = BlockType.TimeInPhase;
//                        }

//                        // If the object in the temporary module is a timeBlock, instantiate a timeBlockOutOfPhase
//                        else if (tempModuleArray[i, j] == ModuleObject.OutOfPhase)
//                        {

//                            //moving a block from the reserve to the module
//                            allFloors[k].transform.position = new Vector3((float)tempI, (float)tempJ, 0f);
//                            //changing name from mainBlock(Clone) to mainBlock
//                            allFloors[k].name = mainBlock.name;
//                            //objectLocal = (GameObject)Instantiate(mainBlock, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
//                            //sets enum of block to Normal
//                            allFloors[k].GetComponent<BlockTypes>().SetBlockEnumeration = BlockType.TimeOutOfPhase;
//                        }

//                        // If the object in the temporary module is a stunBlock, instantiate a stunBlock
//                        else if (tempModuleArray[i, j] == ModuleObject.Stun)
//                        {
//                            //moving a block from the reserve to the module
//                            allFloors[k].transform.position = new Vector3((float)tempI, (float)tempJ, 0f);
//                            //changing name from mainBlock(Clone) to mainBlock
//                            allFloors[k].name = mainBlock.name;
//                            //objectLocal = (GameObject)Instantiate(mainBlock, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
//                            //sets enum of block to Normal
//                            allFloors[k].GetComponent<BlockTypes>().SetBlockEnumeration = BlockType.Stun;
//                        }

//                        // If the object in the temporary module is an overflowBlock, instantiate an overflowBlock
//                        else if (tempModuleArray[i, j] == ModuleObject.Overflow)
//                        {
//                            //moving a block from the reserve to the module
//                            allFloors[k].transform.position = new Vector3((float)tempI, (float)tempJ, 0f);
//                            //changing name from mainBlock(Clone) to mainBlock
//                            allFloors[k].name = mainBlock.name;
//                            //objectLocal = (GameObject)Instantiate(mainBlock, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
//                            //sets enum of block to Normal
//                            allFloors[k].GetComponent<BlockTypes>().SetBlockEnumeration = BlockType.Overflow;
//                        }

//                        // If the object in the temporary module is a speedupBlock, instantiate a speedupBlock
//                        else if (tempModuleArray[i, j] == ModuleObject.Speedup)
//                        {
//                            //moving a block from the reserve to the module
//                            allFloors[k].transform.position = new Vector3((float)tempI, (float)tempJ, 0f);
//                            //changing name from mainBlock(Clone) to mainBlock
//                            allFloors[k].name = mainBlock.name;
//                            //objectLocal = (GameObject)Instantiate(mainBlock, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
//                            //sets enum of block to Normal
//                            allFloors[k].GetComponent<BlockTypes>().SetBlockEnumeration = BlockType.Speed;
//                        }

//                        // If the object in the temporary module is a wall jump, instantiate a wallJumpBlock
//                        else if (tempModuleArray[i, j] == ModuleObject.WallJump)
//                        {
//                            //moving a block from the reserve to the module
//                            allFloors[k].transform.position = new Vector3((float)tempI, (float)tempJ, 0f);
//                            //changing name from mainBlock(Clone) to mainBlock
//                            allFloors[k].name = mainBlock.name;
//                            //objectLocal = (GameObject)Instantiate(mainBlock, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
//                            //sets enum of block to Normal
//                            allFloors[k].GetComponent<BlockTypes>().SetBlockEnumeration = BlockType.WallJump;
//                        }

//                        // If no other case, create a new, empty GameObject
//                        else
//                        {
//                            //***********
//                            //IMPORTANT**
//                            //***********
//                            //This is needed to make the script work, however it infinitly a new 
//                            //GameObject every time it is called, if we could remove it that would be great
//                            allFloors[k] = new GameObject();
//                        }

//                        //spawns floor and immediately snaps it to grid
//                        //GameObject floorLocal = (GameObject)Instantiate(mainBlock, new Vector3((float)tempI, (float)tempJ, 0f), Quaternion.identity);
//                        //floorLocal.transform.position = CorrectPosition(floorLocal.transform.position);
//                        //objectLocal.transform.position = CorrectPosition(objectLocal.transform.position);
//                        //Loops through allFloors activating all the colliders
//                        foreach (GameObject floor in allFloors)
//                        {
//                            floor.GetComponent<BoxCollider>().enabled = true;
//                        }
//                        //Loops through allEnemies activating all the colliders
//                        foreach (GameObject enemy in allEnemies)
//                        {
//                            enemy.GetComponent<BoxCollider>().enabled = true;
//                            enemy.GetComponent<CapsuleCollider>().enabled = true;
//                            enemy.gameObject.GetComponent<EnemyCharacter>().enabled = true;
//                            enemy.gameObject.GetComponent<BasicInfantryAI>().enabled = true;
//                        }

//                        // Adds the local object to gridObjects
//                        //gridObjects[i, j] = objectLocal;
//                    }
//                }
//                k++;
//            }
//        }
//    }
#endregion

#region New WipeModule

//public void WipeModule()
//    {
//        // Finds all enemies and wipes them
//        GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
//        foreach (GameObject enemyObject in allEnemies)
//        {
//            //turns off the colliders/ scripts/ gravity
//            enemyObject.gameObject.GetComponent<BoxCollider>().enabled = false;
//            enemyObject.gameObject.GetComponent<CapsuleCollider>().enabled = false;
//            enemyObject.gameObject.GetComponent<EnemyCharacter>().enabled = false;
//            enemyObject.gameObject.GetComponent<BasicInfantryAI>().enabled = false;
//            enemyObject.gameObject.GetComponent<Rigidbody>().useGravity = false;
//            enemyObject.transform.position = enemyReserve;
//        }

//        // Finds all floors and wipes them
//        GameObject[] allFloors = GameObject.FindGameObjectsWithTag("Floor");
//        foreach (GameObject floorObject in allFloors)
//        {
//            //Destroy(floorObject);
//            //moves all the objects to a certain spot after disabling their colliders(to help processing)
//            floorObject.gameObject.GetComponent<BoxCollider>().enabled = false;
//            floorObject.transform.position = floorReserve;
//        }
        
//        // Goes through gridObjects array and ensures that all its nodes are null
//        for (int i = 0; i < gridX; i++)
//        {
//            for (int j = 0; j < gridY; j++)
//            {
//                gridObjects[i, j] = null;
//            }
//        }
//    }

#endregion
