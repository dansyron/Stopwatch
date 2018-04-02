﻿using UnityEngine;
using System.Collections;

/// <summary>
/// EnemyScript contains some of the Enemy prefab's behavior and functionality.
/// It allows instances of Enemy to die upon being stabbed.
/// </summary>

public class EnemyScript : MonoBehaviour
{

    #region Fields

    // Enemy State Fields
    public bool isStabbed;
	public bool isDying;

    // Animation Related Fields
    int deathState = Animator.StringToHash("Death");
    Animator anim;

    // Slow Down Scale
    private float timeScale = 0.3f;

    // Audio Related Fields
    public AudioSource audioSource;
    public AudioClip dyingSound;
    public AudioClip stabbedSound;

    #endregion

    #region Initialization

    // Use this for initialization
	void Start () 
	{
		isStabbed = false;
		isDying = false;
        anim = GetComponent<Animator>();
	}

    #endregion

    #region Public Methods

    // Update is called once per frame
	void Update ()
    {
        //if isDying is true the game object will destroy itself
	    if(isDying == true)
        {
            //triggers the death animation and sets timer for despawn
            //sets the layer to dead things to stop collisions
            gameObject.layer = 9;
            anim.SetTrigger(deathState);
            StartCoroutine(deathWait());
        }
	}

    void OnCollisionEnter(Collision coll)
    {
        // if weapon collides with enemy, prints "stab" for testing purposes, sets
        // enemy into dying state, and plays stabbing sound.
        if (coll.gameObject.tag == "PlayerWeapon")
        {
            print("stab");
            isDying = true;
            audioSource.PlayOneShot(stabbedSound);
        }

    }

    //forces a wait before playing dying sound and destroying self
    IEnumerator deathWait()
    {
        yield return new WaitForSeconds(3);
        audioSource.PlayOneShot(dyingSound);
        Destroy(gameObject);
    }

    #region Slow Down Methods

    // NOTE: THIS CODE WILL CHANGE UPON LATER IMPLEMENTATION OF DELEGATES

    /// <summary>
    /// Initiates the slowdown effect for the enemy.
    /// Right now, it only scales the enemy animations to demonstrate slowdown
    /// </summary>
    public void InitiateSlowDown()
    {
        anim.speed = timeScale;

        // Prints message for testing purposes
        print("enemy slowdown initiated");
    }

    /// <summary>
    /// Ends the slowdown effect on the enemy
    /// </summary>
    public void EndSlowDown()
    {
        anim.speed = 1f;

        // Prints message for testing purposes
        print("enemy slowdown ended");
    }

    #endregion

    #endregion
}
