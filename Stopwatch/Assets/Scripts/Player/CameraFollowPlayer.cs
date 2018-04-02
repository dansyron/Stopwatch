/**
 * Author: O'Shea Brown
 * Date: 21 September 2016
 * email: oshea.brown@gmail.com
 * 
 * This class will allow the camera to track a gameObject along the x-axis and y-axis. 
 */

using UnityEngine;
using System.Collections;


public class CameraFollowPlayer : MonoBehaviour {

#region Fields
	
    public GameObject player;
	float interpVelocity;
	public Vector3 offset;
	Vector3 targetPosition;
	Vector3 position;
	const float MINY = 3.64f;
	const float MAXY = 9.64f;

#endregion
	
#region Initialization
	
    // Use this for initialization
	void Start ()
    {
		targetPosition = transform.position;
	}
	
#endregion
		
#region Public Methods	
	
	void Update ()
    {
        // move the camera along the x-axis according to the players position and the camera offset
        //transform.position = new Vector3(player.transform.position.x + 5, 7, -18); //+ cameraOffset;
		//transform.position = new Vector3(player.transform.position.x + 5, player.transform.position.y, -18);

		// smoothly move the camera along the x and y axis to follow the player
		Vector3 positionNoZ = transform.position;
		positionNoZ.z = player.transform.position.z;
		positionNoZ.x -= 4;

		Vector3 targetDirection = (player.transform.position - positionNoZ);

		interpVelocity = targetDirection.magnitude * 7f;

		targetPosition = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime); 

		// store the position in a temporary vector to clamp Y values
		position = Vector3.Lerp( transform.position, targetPosition + offset, 0.5f);
		position.y = Mathf.Clamp (position.y, MINY, MAXY);

		transform.position = position;
	}
	
#endregion
}

