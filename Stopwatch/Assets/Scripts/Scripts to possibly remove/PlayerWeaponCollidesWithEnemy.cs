using UnityEngine;
using System.Collections;

public class PlayerWeaponCollidesWithEnemy : MonoBehaviour 
{

#region Fields

    //variables
    private EnemyCharacter enemyScript;
    
#endregion

#region Initialization

	// Use this for initialization
	void Start ()
    {
        //gets EnemyScript Component from gameobject
        enemyScript = GetComponent<EnemyCharacter>();
	}

#endregion

#region Public Methods

	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        //if the object that collides with this game object has the tag PlayerWeapon
        //the enemyScript will set isDying to true
        if(coll.tag == "PlayerWeapon")
        {
            enemyScript.isDying = true;
            
        }
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
        //upon collision with player's weapon, destroy enemy
        if (coll.gameObject.tag == "PlayerWeapon")
        {
            Destroy(GameObject.FindGameObjectWithTag("enemy"));
        }
    }

#endregion

}
