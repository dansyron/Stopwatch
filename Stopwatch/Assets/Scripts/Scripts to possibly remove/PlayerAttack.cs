using UnityEngine;
using System.Collections;

/// <summary>
/// PlayerAttack determines whether or not the Player object can initiate an attack
/// based on the state of the Player object, and initiates it at the input of the player.
/// </summary>

public class PlayerAttack : MonoBehaviour
{

    #region Fields

    // External Parameters
    public Animator animatorController;
    public PlayerHashIDs tags;
    public AudioSource audioSource;
    public AudioClip attackSound;

    // Private Fields
    private bool isGrounded;

    #endregion

    #region Initialization

    // Use this for initialization
	void Start () {
	
	}

    #endregion

    #region Public Methods

    // Update is called once per frame
	void Update () 
    {
        // inititate player attack if player is on the ground, the player is not already attacking,
        // and the player presses Right Shift.
        if (Input.GetKey(KeyCode.RightShift) && isGrounded)
        {
            if (animatorController.GetBool(tags.attackFloat) == false)
            {
                // if attack is initiated, play attack sound and change to attacking animation
                audioSource.PlayOneShot(attackSound);
                animatorController.SetBool(tags.attackFloat, true);
            }
        }
	}

    // checking and updating whether player character is on the ground
    void OnCollisionEnter(Collision coll)
    {
        // set isGrounded to true if the player is currently on the ground
        if (coll.gameObject.tag == "Floor")
        {
            isGrounded = true;
        }

    }

    void OnCollisionExit(Collision coll)
    {
        // set isGrounded to false if the player is not currently on the ground
        if (coll.gameObject.tag == "Floor")
        {
            isGrounded = false;
        }
    }

    #endregion
}
