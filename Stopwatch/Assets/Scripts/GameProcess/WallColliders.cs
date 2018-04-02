using UnityEngine;
using System.Collections;

public class WallColliders : MonoBehaviour {

#region Fields
	
    //This will be used to determine which side of the wall the player is hitting.
    //The wall object is an empty parent object with 2 wall child objects placed next to each other
    //to create 1 wall object. Depending on which child is hit will allow us to determine the side
    //of the wall the player is hitting.
    public BoxCollider leftCollider;
    public BoxCollider rightCollider;

#endregion
	
#region Initialization
	
	// Use this for initialization
	void Start () {
	
	}

#endregion
	
#region Public Methods
	
	// Update is called once per frame
	void Update () {
	
	}
	
#endregion	
}
