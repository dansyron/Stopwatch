
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.UI;

/// <summary>
/// SaveLoadModuleScript allows modules to be saved and loaded into ModuleInfo objects
/// that keep track of placements of objects in a given module. ModuleInfo objects
/// themselves can then be saved and loaded into and from memory outside of the scope
/// of the program, using SaveLoadModuleScript's methods.
/// </summary>
public class SaveLoadModuleScript : MonoBehaviour
{
    /*
    #region Fields

    // Current module that is in use
    public ModuleInfo currentModuleInfo;
    //public LocationOfObjects locationOfObjects;
    //public SerializeEntry serializeEntry;

    // List of ModuleInfo for ease of access
    private List<ModuleInfo> moduleInfoList;

    #endregion

    #region Initialization

    // Use this for initialization
	void Start () 
    {
        currentModuleInfo = new ModuleInfo();
        moduleInfoList = new List<ModuleInfo>();

        // COMMENT THIIS OUT IF NOT TESTING SAVE-LOAD FUNCTIONALITY
        // TestForData();

	}

    #endregion

    #region Update

    // Update is called once per frame
	void Update () 
    {

	}

    #endregion

    #region Public Methods

    // Saves module in a file with one given path.
    // Note: any saving using this method will automatically overwrite the previous save
    public void SaveEmptyModuleBinary()
    {
        // Establishes a binary formatter and creates file path that will be used for saving
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/moduleinfo.dat");

        // Creates an empty ModuleInfo object, serializes data into file destination, and closes path
        ModuleInfo moduleInfo = new ModuleInfo();
        bf.Serialize(file, moduleInfo);
        file.Close();
    }

    // Loads module that was saved using the SaveModule method
    public void LoadEmptyModuleBinary()
    {
        // Checks if data in destermined file destination exists before trying to load it
        if(File.Exists(Application.persistentDataPath + "/moduleinfo.dat"))
        {
            // Establishes binary formatter and file path used for loading
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/moduleinfo.dat", FileMode.Open);

            // Deserializes data into ModuleInfo object and closes file path
            ModuleInfo moduleInfo = (ModuleInfo)bf.Deserialize(file);
            file.Close();
        }
    }

    // Saves module in a file destination that user defines
    public void SaveModuleAsBinary(string fileExtension)
    {
        // Establishes a binary formatter and creates file path that will be used for saving
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + fileExtension + ".dat");

        // Serializes current module data into file destination and closes path
        bf.Serialize(file, currentModuleInfo);
        file.Close();
    }

    #region Brians Addition


    //// Saves module in a file destination that user defines
    //public void SaveModuleAsV2(string fileExtension)
    //{
    //    // Establishes a binary formatter and creates file path that will be used for saving
    //    BinaryFormatter bf = new BinaryFormatter();
    //    FileStream file = File.Create(Application.persistentDataPath + "/" + fileExtension + ".dat");

    //    // Serializes current module data into file destination and closes path
    //    bf.Serialize(file, serializeEntry.listOfObjects);
    //    file.Close();
    //}

    //// Loads module that was saved using the SaveModuleAsBinary method, requires same file destination
    //public  List<LocationOfObjects> LoadModuleAsV2(string fileExtension)
    //{
    //    // Checks if data in given file destination exists before trying to load it
    //    if (File.Exists(Application.persistentDataPath + "/" + fileExtension + ".dat"))
    //    {
    //        // Establishes binary formatter and file path used for loading
    //        BinaryFormatter bf = new BinaryFormatter();

    //        // Deserializes data into currentModuleInfo object and closes file path
    //        FileStream file = File.Open(Application.persistentDataPath + "/" + fileExtension + ".dat", FileMode.Open);
    //        serializeEntry.listOfObjects = (List<LocationOfObjects>)bf.Deserialize(file);
    //        file.Close();
    //    }
    //    print(serializeEntry.listOfObjects.Count);
    //    return serializeEntry.listOfObjects;
    //}
    #endregion

    // Loads module that was saved using the SaveModuleAsBinary method, requires same file destination
    public void LoadModuleAsBinary(string fileExtension)
    {
        // Checks if data in given file destination exists before trying to load it
        if (File.Exists(Application.persistentDataPath + "/" + fileExtension + ".dat"))
        {
            // Establishes binary formatter and file path used for loading
            BinaryFormatter bf = new BinaryFormatter();

            // Deserializes data into currentModuleInfo object and closes file path
            FileStream file = File.Open(Application.persistentDataPath + "/" + fileExtension + ".dat", FileMode.Open);
            currentModuleInfo = (ModuleInfo)bf.Deserialize(file);
            file.Close();
        }
    }

    // Saves module in a file destination that is procedurally generated and pulled from a list
    public void SaveModuleInListBinary()
    {
        // Establishes a binary formatter and creates file path that will be used for saving
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + moduleInfoList.Count.ToString() + ".dat");

        // Serializes current module data into file destination, adds ModuleInfo object to list, and closes path
        moduleInfoList.Add(currentModuleInfo);
        bf.Serialize(file, currentModuleInfo);
        file.Close();
    }

    // Loads module that was saved using the SaveModuleInListBinary method, requires location in list as arguement
    public void LoadModuleInListBinary(int listLocation)
    {
        // Checks if data in given file destination exists before trying to load it
        if (File.Exists(Application.persistentDataPath + "/" + listLocation.ToString() + ".dat"))
        {
            // Establishes binary formatter and file path used for loading
            BinaryFormatter bf = new BinaryFormatter();

            // Deserializes data into currentModuleInfo object and closes file path
            FileStream file = File.Open(Application.persistentDataPath + "/" + listLocation.ToString() + ".dat", FileMode.Open);
            currentModuleInfo = (ModuleInfo)bf.Deserialize(file);
            file.Close();
        }
    }

    #endregion

    #region Testing Methods

    public void TestForData()
    {
        // TEST MODULE 0
        
        print("Test Module 0:");
        
        // Sets unique components in first row of currentModuleInfo
        currentModuleInfo.SetBlock(0, 0, ModuleObject.Enemy);
        currentModuleInfo.SetBlock(0, 1, ModuleObject.Enemy);
        currentModuleInfo.SetBlock(0, 2, ModuleObject.Enemy);
        currentModuleInfo.SetBlock(0, 3, ModuleObject.Player);
        currentModuleInfo.SetBlock(0, 4, ModuleObject.Player);
        currentModuleInfo.SetBlock(0, 5, ModuleObject.Player);
        currentModuleInfo.SetBlock(0, 6, ModuleObject.Player);
        currentModuleInfo.SetBlock(0, 7, ModuleObject.Enemy);
        currentModuleInfo.SetBlock(0, 8, ModuleObject.Enemy);
        currentModuleInfo.SetBlock(0, 9, ModuleObject.Player);
        currentModuleInfo.SetBlock(0, 10, ModuleObject.Player);

        // Prints off the nodes in first row of currenModuleInfo
        for (int i = 0; i < 16; i++)
        {
            print("Current node [0 , " + i.ToString() + "]: " + currentModuleInfo.GetGridArray()[0, i].ToString());
        }

        // Saves Module 0 in file path "test0"
        SaveModuleAsBinary("test0");


        // TEST MODULE 1

        print ("Test Module 1:");

        // Sets unique components in first row of currentModuleInfo
        currentModuleInfo.SetBlock(0, 0, ModuleObject.Block);
        currentModuleInfo.SetBlock(0, 1, ModuleObject.Block);
        currentModuleInfo.SetBlock(0, 2, ModuleObject.Block);
        currentModuleInfo.SetBlock(0, 3, ModuleObject.Block);
        currentModuleInfo.SetBlock(0, 4, ModuleObject.Block);
        currentModuleInfo.SetBlock(0, 5, ModuleObject.Block);
        currentModuleInfo.SetBlock(0, 6, ModuleObject.Block);
        currentModuleInfo.SetBlock(0, 7, ModuleObject.Block);
        currentModuleInfo.SetBlock(0, 8, ModuleObject.Block);
        currentModuleInfo.SetBlock(0, 9, ModuleObject.Block);
        currentModuleInfo.SetBlock(0, 10, ModuleObject.Block);

        // Prints off the nodes in first row of currenModuleInfo
        for (int i = 0; i < 16; i++)
        {
            print("Current node [0 , " + i.ToString() + "]: " + currentModuleInfo.GetGridArray()[0, i].ToString());
        }

        // Saves Module 1 in file path name "test1"
        SaveModuleAsBinary("test1");


        // TESTING LOADING MODULE 0

        // Loads Module 0 from file path "test0"
        LoadModuleAsBinary("test0");

        print("This should be Module 0:");

        // Prints off row 0 nodes for user to compare with Module 0 printed before
        for (int i = 0; i < 16; i++)
        {
            print("Current node [0 , " + i.ToString() + "]: " + currentModuleInfo.GetGridArray()[0, i].ToString());
        }


        // TESTING LOADING MODULE 1

        // Loads Module 1 from file path "test1"
        LoadModuleAsBinary("test1");

        print("This should be Module 1:");

        // Prints off row 0 nodes for user to compare with Module 1 printed before
        for (int i = 0; i < 16; i++)
        {
            print("Current node [0 , " + i.ToString() + "]: " + currentModuleInfo.GetGridArray()[0, i].ToString());
        }

        // Resets currentModuleInfo so that it is blank again
        currentModuleInfo.ResetModule();
    }

    #endregion
}

#region Module Object Enumeration

// Enumeration of possible module objects
public enum ModuleObject
{
    Nothing, // 0
    Block,   // 1
    Enemy,   // 2
    Player   // 3
};

#endregion

#region AltWay BRIANS

//[System.Serializable]
//public class SerializeEntry
//{
//    [SerializeField]
//    public List<LocationOfObjects> listOfObjects = new List<LocationOfObjects>();
//}

///// <summary>
///// Gets the x/y/z values and an enum of an object saves it to an object
///// </summary>
//public class LocationOfObjects : SerializeEntry
//{
//    bool addToList;
//    float x, y, z;
//    int objectType;

//    /// <summary>
//    /// constructor, adds values to class
//    /// </summary>
//    /// <param name="_x">transform.x</param>
//    /// <param name="_y">transform.y</param>
//    /// <param name="_z">transform.z</param>
//    /// <param name="_objectType">type of object 0 = enemy, 1 = floor</param>
//    public LocationOfObjects(float _x, float _y, float _z, int _objectType)
//    {
//        x = _x;
//        y = _y;
//        z = _z;
//        objectType = _objectType;
//        addToList = true;
//    }

//    #region getters
//    /// <summary>
//    /// get x
//    /// </summary>
//    public float getX
//    {
//        get { return x; }
//    }
//    /// <summary>
//    /// get y
//    /// </summary>
//    public float getY
//    {
//        get { return y; }
//    }
//    /// <summary>
//    /// get z
//    /// </summary>
//    public float getZ
//    {
//        get { return z; }
//    }
//    /// <summary>
//    /// get object type
//    /// </summary>
//    public int getObjectType
//    {
//        get { return objectType; }
//    }

//    #endregion
//    void Update()
//    {
//        if (addToList)
//        {
//            listOfObjects.Add(this);
//            addToList = false;
//        }
//    }
//}

#endregion

#region ModuleInfo Class

[Serializable]
// ModuleInfo stores module information, including placement of various objects in given module.
// It is a class that is private to the SaveLoadModuleScript class.
public class ModuleInfo
{

    // Alterable size fields of module
    public const int gridWidth = 16;
    public const int gridHeight = 16;

    // Array that stores arrangement of module objects
    private ModuleObject[,] gridArray;

    
    // Initializes grid array so that every node is empty
    //gridArray = new ModuleObject[gridWidth, gridHeight];
    for (int i = 0; i < gridWidth; i++)
    {
        for (int j = 0; j < gridHeight; j++)
        {
        // Sets every node as Nothing to begin
        gridArray[i, j] = ModuleObject.Nothing;
        }
    }
    
    public ModuleInfo()
    {
        // Initializes grid array so that every node is empty
        gridArray = new ModuleObject[gridWidth, gridHeight];
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                // Sets every node as Nothing to begin
                gridArray[i, j] = ModuleObject.Nothing;
            }
        }
    }

    #region Public Methods

    // Setter method that allows nodes of the module object grid array to be changed
    public void SetBlock(int gridX, int gridY, ModuleObject moduleObject)
    {
        // Checks if the arguements for array location fall in the valid range
        if (gridX < gridWidth && gridX >= 0 && gridY < gridHeight && gridY >= 0)
        {
            // Sets node to the type of module object
            gridArray[gridX, gridY] = moduleObject;
        }
    }
    

    // Getter method that allows access to the array that stores arrangement of module objects
    public ModuleObject[,] GetGridArray()
    {
        return gridArray;
    }

    // Resets module object so that every node of the grid array has nothing
    public void ResetModule()
    {
        // Set grid array so that every node is empty
        gridArray = new ModuleObject[gridWidth, gridHeight];
        for (int i = 0; i < gridWidth; i++)
        {
            for (int j = 0; j < gridHeight; j++)
            {
                // Sets every node as nothing
                gridArray[i, j] = ModuleObject.Nothing;
            }
        }
    }

    #endregion
     */
}

