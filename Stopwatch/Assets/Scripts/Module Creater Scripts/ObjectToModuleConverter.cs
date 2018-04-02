using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectToModuleConverter : MonoBehaviour
{
    //private GameObject[] allGameObjectsInLevelEditor;
    //private int i = 0;
    //private SaveLoadModuleScript saveLoadModuleScript;
    //// Use this for initialization
    //void Start()
    //{
    //    saveLoadModuleScript = GetComponent<SaveLoadModuleScript>();
    //    //258 because 16x16 = 256 + 2 for a saftey net
    //    allGameObjectsInLevelEditor = new GameObject[258];
    //}

    //// Update is called once per frame
    //void Update()
    //{

    //}

    ///// <summary>
    ///// Adds a gameObject to the array
    ///// </summary>
    ///// <param name="gameObject"></param>
    //public void AddToArray(GameObject gameObject)
    //{
    //    allGameObjectsInLevelEditor[i] = gameObject;
    //    i++;
    //}
    ///// <summary>
    ///// Function to allow Button Press
    ///// </summary>
    //public void ConvertToModule()
    //{
    //    //variables
    //    float xPosition = 0;
    //    float yPosition = 0;
    //    float zPosition = 0;
    //    int objectType = 0;

    //    //loop saving all values in gameObject
    //    foreach (GameObject gameObject in allGameObjectsInLevelEditor)
    //    {
    //        xPosition = gameObject.transform.position.x;
    //        yPosition = gameObject.transform.position.y;
    //        zPosition = gameObject.transform.position.z;

    //        //sets the value of the object to a predetermined value for storage
    //        //enemy = 0
    //        if (gameObject.name == "enemyLevelDesign(Clone)")
    //        {
    //            objectType = 0;
    //        }
    //        //floor = 1
    //        else if (gameObject.name == "floorPlaceholderLevelDesign(Clone)")
    //        {
    //            objectType = 1;
    //        }
    //        //adds this to a object in save 
    //        saveLoadModuleScript.locationOfObjects = new LocationOfObjects(xPosition, yPosition, zPosition, objectType);
    //        //saves object to a file
    //        saveLoadModuleScript.SaveModuleAsV2("LevelDesigner");

    //        //IT SUCCESFULLY GETS THIS FAR
    //    }
    //}
    //public void LoadFromModule()
    //{
    //    //variables
    //    float xPosition = 0;
    //    float yPosition = 0;
    //    float zPosition = 0;
    //    int objectType = 0;
    //    LocationOfObjects local;
    //    List<LocationOfObjects> locationOfObjects = saveLoadModuleScript.LoadModuleAsV2("LevelDesigner");
    //    foreach(LocationOfObjects localObjects in locationOfObjects)
    //    {
    //        print("Bye");
    //        local = localObjects;
    //        xPosition = local.getX;
    //        yPosition = local.getY;
    //        zPosition = local.getZ;
    //        objectType = local.getObjectType;

    //        print(xPosition);
    //        print(yPosition);
    //        print(zPosition);
    //        print(objectType);
    //    }
    //}
}
