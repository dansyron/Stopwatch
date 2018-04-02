using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class ModuleInstantiationScript : MonoBehaviour
{
    #region Fields

    // Keeps track of modules and current module position
    List<ModuleInfo> moduleList;
    int moduleListPosition = 0;

    // The player character object in the scene
    public GameObject playerCharacter;

    public GameObject mainBlock;
    public GameObject enemyPrefab;

    public GameObject topBlock;

    public GameObject background;

    // Floats that specify parameters involved in module instantiation
    float moduleUnitLength = 16f;
    float initialPlayerPosition = 0f;
    float playerSector = 0f;
    float moduleXDisplacement = 0f;
    float moduleYDisplacement = 7f;

    // Bool that prevents multiple modules from being instantiated simultaneously
    bool instantiateNewModule = false;

    string currentFilePath;

    string resourceModuleFolderName;

    #endregion

    #region Initialization

    // Use this for initialization
    void Start ()
    {

        currentFilePath = Application.persistentDataPath;

        resourceModuleFolderName = "Module Text Files";

        moduleList = new List<ModuleInfo>();

        LoadAllModulesInResources();

        // Instantiating initial modules and values

        initialPlayerPosition = playerCharacter.transform.position.x;

        moduleListPosition = 4;

        InstantiateModule(moduleList[0], initialPlayerPosition, moduleYDisplacement);
        InstantiateModule(moduleList[1], (initialPlayerPosition + moduleUnitLength), moduleYDisplacement);
        InstantiateModule(moduleList[2], (initialPlayerPosition + (moduleUnitLength * 2)), moduleYDisplacement);
        InstantiateModule(moduleList[3], (initialPlayerPosition + (moduleUnitLength * 3)), moduleYDisplacement);

        Instantiate(background);
	}

    #endregion

    #region Update

    // Update is called once per frame
	void Update () 
    {
        // Checks to see if section needs updating
        UpdateSection();

        // If a module instantiation is necessary, instantiates new module
        if (instantiateNewModule == true)
        {
            instantiateNewModule = false;

            InstantiateModule(moduleList[moduleListPosition], moduleXDisplacement, moduleYDisplacement);

            // Prevents the iterator for the module list to go outside of bounds
            if (moduleListPosition < (moduleList.Count - 1))
            {
                moduleListPosition += 1;
            }
            else
            {
                moduleListPosition = 0;
            }

            GameObject[] tempObjects = GameObject.FindGameObjectsWithTag("Floor");
            foreach (GameObject tempObject in tempObjects)
            {
                if (tempObject.transform.position.x < (playerCharacter.transform.position.x - 25))
                {
                    Destroy(tempObject);
                }
            }

            GameObject[] tempEnemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject tempEnemy in tempEnemies)
            {
                if (tempEnemy.transform.position.x < (playerCharacter.transform.position.x - 25))
                {
                    Destroy(tempEnemy);
                }
            }
        }

    }

    #endregion

    #region Public Methods

    public void AddModuleInfo(ModuleInfo moduleInfo)
    {
        // Adds the module info to the list
        moduleList.Add(moduleInfo);
    }

    public void AddModuleFromTextFile(string filePathExtension)
    {
        // Creates a temporary module info that will be added to the list
        ModuleInfo tempModuleInfo = new ModuleInfo();

        // Ensures file path extenion isn't blank or null and that it leads to a saved text file
        if (filePathExtension != "" && filePathExtension != null && File.Exists(currentFilePath + @"\" + filePathExtension + ".txt"))
        {
            // Creates a temporary string to hold the text pulled in by ReadAllText
            string[] tempModuleString = File.ReadAllLines((currentFilePath + @"\" + filePathExtension + ".txt"));

            // Parses information held in the temporary string and passes it into the current module
            tempModuleInfo = ParseStringToModuleInfo(tempModuleString);

            // Adds the temporary module info to the list
            moduleList.Add(tempModuleInfo);

            // Prints message to let user know of successful load
            print("Loaded: " + filePathExtension + ".txt");
        }

        // Prints error message if file path is invalid
        else
        {
            print("Error: invalid file path extension " + currentFilePath + " in AddModuleFromTextFile");
        }
    }

    public void AddModuleFromTextFileFull(string filePathName)
    {
        // Creates a temporary module info that will be added to the list
        ModuleInfo tempModuleInfo = new ModuleInfo();

        // Ensures file path extenion isn't blank or null and that it leads to a saved text file
        if (filePathName != "" && filePathName != null && File.Exists(filePathName))
        {
            // Creates a temporary string to hold the text pulled in by ReadAllText
            string[] tempModuleString = File.ReadAllLines((filePathName));

            // Parses information held in the temporary string and passes it into the current module
            tempModuleInfo = ParseStringToModuleInfo(tempModuleString);

            // Adds the temporary module info to the list
            moduleList.Add(tempModuleInfo);

            // Prints message to let user know of successful load
            print("Loaded: " + filePathName);
        }

        // Prints error message if file path is invalid
        else
        {
            print("Error: invalid file path extension " + filePathName + " in AddModuleFromTextFileFull");
        }
    }

    public void LoadAllModulesInDirectory(string filePathName)
    {
        // Checks for whether the directory exists
        if (filePathName != "" && filePathName != null && Directory.Exists(filePathName))
        {
            foreach (string textFile in Directory.GetFiles(filePathName, @"*.txt", SearchOption.TopDirectoryOnly))
            {
                print(textFile);
                AddModuleFromTextFileFull(textFile);
            }
        }
    }

    public void LoadAllModulesInResources()
    {
        foreach (TextAsset textFile in Resources.LoadAll<TextAsset>(resourceModuleFolderName))
        {
            moduleList.Add(ParseStringToModuleInfo(textFile.text.Split("\n"[0])));
        }
    }

    public void InstantiateModule(ModuleInfo tempModuleInfo, float xDisplacement, float yDisplacement)
    {
        // Pulls gridArray from currentModuleInfo into local temporary copy
        ModuleObject[,] tempModuleArray = tempModuleInfo.GetGridArray();

        // Instantiates a top block that acts as a ceiling
        Instantiate(topBlock, new Vector3((float)(xDisplacement + 4), (float)(yDisplacement + 7.75), 0f), Quaternion.identity);

        // Iterates through local copy of gridArray to store data in
        for (int i = 0; i < Constants.MODULE_GRID_WIDTH; i++)
        {
            for (int j = 0; j < Constants.MODULE_GRID_HEIGHT; j++)
            {
                // Calculates spawned object's location
                double tempI = (((double)i) - 3.25);
                double tempJ = (((double)j * -1) + 6.75);

                float objI = (float)tempI + xDisplacement;
                float objJ = (float)tempJ + yDisplacement;

                // Moves a block of the appropriate kind to the specified location
                switch (tempModuleArray[i, j])
                {
                    // If tempModuleArray node contains nothing, does nothing
                    case ModuleObject.Nothing:
                        break;
                    // If tempModuleArray node contains Enemy type, spawns enemy
                    case ModuleObject.Enemy:
                        //spawns enemy and immediately snaps it to grid
                        Instantiate(enemyPrefab, new Vector3(objI, objJ, 0f), Quaternion.identity);
                        break;

                    // If not either of those, must be a block
                    // Moves blocks as necessary

                    case ModuleObject.Normal:
                        MoveBlock(BlockType.Normal, objI, objJ);
                        break;
                    case ModuleObject.Stun:
                        MoveBlock(BlockType.Stun, objI, objJ);
                        break;
                    case ModuleObject.Speedup:
                        MoveBlock(BlockType.Speed, objI, objJ);
                        break;
                    case ModuleObject.Overflow:
                        MoveBlock(BlockType.Overflow, objI, objJ);
                        break;
                    case ModuleObject.WallJump:
                        MoveBlock(BlockType.WallJump, objI, objJ);
                        break;
                    case ModuleObject.InPhase:
                        MoveBlock(BlockType.TimeInPhase, objI, objJ);
                        break;
                    case ModuleObject.OutOfPhase:
                        MoveBlock(BlockType.TimeOutOfPhase, objI, objJ);
                        break;

                    // In the default case, does nothing
                    default:
                        break;
                } 
            }
        }
    }

    public void InstantiateModule(GameObject moduleObject, float xDisplacement, float yDisplacement)
    {
        Instantiate(moduleObject, new Vector3(xDisplacement, yDisplacement, 0f), Quaternion.identity);
    }

    public int GetModuleListPosition()
    {
        return (moduleListPosition - 4);
    }

    #endregion

    #region Private Methods

    private ModuleInfo ParseStringToModuleInfo(string[] moduleString)
    {
        // The ModuleInfo that will be passed back once the parsing process is done
        ModuleInfo parsedModuleInfo = new ModuleInfo();

        // The temporary string that will determine the type of module object to add
        string tempType = "";

        // "k" denotes the index of the current character in the string
        int k = 0;

        // Loops through the module string array the same number of time as the expected
        // height of the module grid contained by the string.
        // "i" will be the tracker for the grid's x value
        for (int i = 0; i < Constants.MODULE_GRID_HEIGHT; i++)
        {
            // Resets the character index at the beginning of every string to 0
            k = 0;

            // Loops through the length of the string in the row of the string array for the
            // expected number of module objects contained therein.
            // "j" will be the tracker for the grid's y value
            for (int j = 0; j < Constants.MODULE_GRID_WIDTH; j++)
            {
                // Resets the temporary type string
                tempType = "";

                // Adds the first character in module string to temporary type string until a comma appears
                // or theend of the string is reached
                while (moduleString[i][k] != ","[0] && k < moduleString[i].Length)
                {
                    tempType += moduleString[i][k];

                    // Moves to next character in string
                    k++;
                }

                // After temporary type string has had characters passed in, analyze string to determine
                // what module object needs to be added to module

                // If the type string says "Normal", add Normal to module
                if (tempType == ModuleObject.Normal.ToString())
                {
                    parsedModuleInfo.SetBlock(i, j, ModuleObject.Normal);
                }

                // If the type string says "Stun", add Stun to module
                else if (tempType == ModuleObject.Stun.ToString())
                {
                    parsedModuleInfo.SetBlock(i, j, ModuleObject.Stun);
                }

                // If the type string says "Speedup", add Speedup to module
                else if (tempType == ModuleObject.Speedup.ToString())
                {
                    parsedModuleInfo.SetBlock(i, j, ModuleObject.Speedup);
                }

                // If the type string says "Overflow", add Overflow to module
                else if (tempType == ModuleObject.Overflow.ToString())
                {
                    parsedModuleInfo.SetBlock(i, j, ModuleObject.Overflow);
                }

                // If the type string says "WallJump", add WallJump to module
                else if (tempType == ModuleObject.WallJump.ToString())
                {
                    parsedModuleInfo.SetBlock(i, j, ModuleObject.WallJump);
                }

                // If the type string says "InPhase", add InPhase to module
                else if (tempType == ModuleObject.InPhase.ToString())
                {
                    parsedModuleInfo.SetBlock(i, j, ModuleObject.InPhase);
                }

                // If the type string says "OutOfPhase", add OutOfPhase to module
                else if (tempType == ModuleObject.OutOfPhase.ToString())
                {
                    parsedModuleInfo.SetBlock(i, j, ModuleObject.OutOfPhase);
                }

                // If type string says "Enemy", add Enemy to module
                else if (tempType == ModuleObject.Enemy.ToString())
                {
                    parsedModuleInfo.SetBlock(i, j, ModuleObject.Enemy);
                }

                // If nothing else, add Nothing to module
                else
                {
                    parsedModuleInfo.SetBlock(i, j, ModuleObject.Nothing);
                }

                // Continues to next character, passing the present comma
                k++;
            }
        }
        // Returns the final result
        return parsedModuleInfo;
    }

    private void UpdateSection()
    {
        // Increases the sector number if player is sufficiently far enough
        if (playerCharacter.transform.position.x >= (initialPlayerPosition + ((playerSector + 1) * moduleUnitLength)))
        {
            playerSector += 1;
            instantiateNewModule = true;

            // Updates the x value of the module displacement
            moduleXDisplacement = initialPlayerPosition + ((playerSector + 3) * moduleUnitLength);

            // Prints message for testing purposes
            //print("Section " + playerSector.ToString());
        }
    }

    private void MoveBlock(BlockType blockType, float xLocation, float yLocation)
    {
        /*
        switch (blockType)
        {
            // If the block type to move is normal, use the normal block list
            case BlockType.Normal:
                // Repositions the appropriate block to the specified location
                normalBlockList[normIter].transform.position = new Vector3(xLocation, yLocation, 0f);
                // Increases the value of the appropriate iterator by 1
                normIter++;
                // Prevents the iterator from exceeding the bounds of the list
                if (normIter >= normalBlockList.Count)
                {
                    normIter = 0;
                }
                break;

            // If the block type to move is stun, use the stun block list
            case BlockType.Stun:
                // Repositions the appropriate block to the specified location
                stunBlockList[stunIter].transform.position = new Vector3(xLocation, yLocation, 0f);
                // Increases the value of the appropriate iterator by 1
                stunIter++;
                // Prevents the iterator from exceeding the bounds of the list
                if (stunIter >= stunBlockList.Count)
                {
                    stunIter = 0;
                }
                break;

            // If the block type to move is speedup, use the speedup block list
            case BlockType.Speed:
                // Repositions the appropriate block to the specified location
                speedupBlockList[speedIter].transform.position = new Vector3(xLocation, yLocation, 0f);
                // Increases the value of the appropriate iterator by 1
                speedIter++;
                // Prevents the iterator from exceeding the bounds of the list
                if (speedIter >= speedupBlockList.Count)
                {
                    speedIter = 0;
                }
                break;

            // If the block type to move is overflow, use the overflow block list
            case BlockType.Overflow:
                // Repositions the appropriate block to the specified location
                overflowBlockList[overIter].transform.position = new Vector3(xLocation, yLocation, 0f);
                // Increases the value of the appropriate iterator by 1
                overIter++;
                // Prevents the iterator from exceeding the bounds of the list
                if (overIter >= overflowBlockList.Count)
                {
                    overIter = 0;
                }
                break;

            // If the block type to move is walljump, use the walljump block list
            case BlockType.WallJump:
                // Repositions the appropriate block to the specified location
                walljumpBlockList[wallIter].transform.position = new Vector3(xLocation, yLocation, 0f);
                // Increases the value of the appropriate iterator by 1
                wallIter++;
                // Prevents the iterator from exceeding the bounds of the list
                if (wallIter >= walljumpBlockList.Count)
                {
                    wallIter = 0;
                }
                break;

            // If the block type to move is time in phase, use the time in phase block list
            case BlockType.TimeInPhase:
                // Repositions the appropriate block to the specified location
                timeInPhaseBlockList[inPhaseIter].transform.position = new Vector3(xLocation, yLocation, 0f);
                // Increases the value of the appropriate iterator by 1
                inPhaseIter++;
                // Prevents the iterator from exceeding the bounds of the list
                if (inPhaseIter >= timeInPhaseBlockList.Count)
                {
                    inPhaseIter = 0;
                }
                break;

            // If the block type to move is time out of phase, use the time out of phase block list
            case BlockType.TimeOutOfPhase:
                // Repositions the appropriate block to the specified location
                timeOutOfPhaseBlockList[outPhaseIter].transform.position = new Vector3(xLocation, yLocation, 0f);
                // Increases the value of the appropriate iterator by 1
                outPhaseIter++;
                // Prevents the iterator from exceeding the bounds of the list
                if (outPhaseIter >= timeOutOfPhaseBlockList.Count)
                {
                    outPhaseIter = 0;
                }
                break;

            // In the default case, does nothing
            default:
                break;
        }
         */ 

        GameObject tempBlock = (GameObject)Instantiate(mainBlock, new Vector3(xLocation, yLocation, 0f), Quaternion.identity);
        tempBlock.GetComponent<BlockTypes>().SetBlockEnumeration = blockType;

    }

    #endregion
}
