using UnityEngine;
using System.Collections;

/// <summary>
/// Destroys the parent gameobject
/// </summary>
public class DestroyParent : MonoBehaviour {

    #region fields

    //variables
    public bool destroyParentVar;

    #endregion

    #region Update
    // Update is called once per frame
    void Update () {
        //checks if needs to be destroyed
	    if(destroyParentVar)
        {
            //destroys itself
            Destroy(gameObject);
        }
	}

    #endregion
}
