using UnityEngine;
using System.Collections;

public class BasicInfantryAI : EnemyAIParentScript
{

    #region Fields

    /*
    public GameObject enemyWeapon;
    public float moveDelayAmount;
    Constants gameConstants;
    TempAIParentScript tempAIParentScript;
    GameObject weaponInstance;
    bool playerInAttackRange;
    //used for when the player is in the aggro range
    bool moveTowardsPlayer;
    bool isAttacking;
    bool canAttack;
    bool triggerAttack;
    bool allowMovement = false;
    bool knifeSpawnLimiter;
    //vector 3 of the direction/mag from this object towards the player
    
    //This is where the EnemyAI parent will be called

    //TEMPORARY FIELDS UNTIL ENEMYAIPARENT IS COMPLETED
    public float windUpSpeed;
    public float attackDelay;
    public bool attackBool;
     * 
     */

    GameObject weaponInstance;
    bool knifeSpawnLimiter = false;

    public float moveDelayAmount;
    public float windUpSpeed;

    float slowSpeedFactor;

    #endregion

    #region Initialization
    // Use this for initialization
    protected override void Start()
    {
        base.Start();

        //tempAIParentScript = GetComponent<TempAIParentScript>();
        moveDelayAmount = 3f;
        windUpSpeed = 1f;
        //gets the timer class from the timerUI
        //timerUI = GameObject.Find("TimerUI").GetComponent<TimerUI>();
    }

    #endregion

    #region Update
    // Update is called once per frame
    protected override void Update()
    {

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        #region StartWindUpAnimation

        base.Update();
        //If the weaponInstance exists then they are not going to collide
        if (weaponInstance != null)
        {
            Physics.IgnoreLayerCollision(this.gameObject.layer, weaponInstance.gameObject.layer, true);
        }
        //checks if the player is in the attack range
        if (triggerAttack == true)
        {
            triggerAttack = false;
            
                //then that attack windup coroutine is started
                StartCoroutine("AttackWindup");
        }
    }
    #endregion

    #endregion

    #region Triggers

    /// <summary>
    /// Stuff happens if a trigger enters the collider
    /// </summary>
    /// <param name="coll">collider from the object the entered the trigger area</param>
    protected override void OnTriggerEnter(Collider coll)
    {
        base.OnTriggerEnter(coll);
        
        if (coll.name == "New Player Model")
        {
            if (coll.gameObject.GetComponent<PlayerController>().Action == PlayerController.ActionState.IsDodging)
            {
                Physics.IgnoreLayerCollision(this.gameObject.layer, coll.gameObject.layer, true);
            }
            else
            {
                Physics.IgnoreLayerCollision(this.gameObject.layer, coll.gameObject.layer, false);
                allowMovement = false;
                triggerAttack = true;
            }
        }
    }

    /// <summary>
    /// Checks if a trigger is staying in the trigger area
    /// </summary>
    /// <param name="coll">collider of any object staying in the trigger area</param>
    protected override void OnTriggerStay(Collider coll)
    {
        base.OnTriggerStay(coll);
    }

    /// <summary>
    /// Stuff happens if a trigger exits the collider
    /// </summary>
    /// <param name="coll">collider from the object leaving the trigger range</param>
    protected override void OnTriggerExit(Collider coll)
    {
        base.OnTriggerExit(coll);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Attack Method for the enemy causing the weapon to spawn
    /// </summary>
    private void attack()
    {

        //Spawns the enemyWeapon
        weaponInstance = (GameObject)Instantiate(enemyWeapon, transform.position, transform.rotation);
        //attaches it to the right hand
        knifeSpawnLimiter = false;

        //Instantiate(enemyWeapon, new Vector3(gameObject.transform.position.x - .2f, gameObject.transform.position.y + .6f, gameObject.transform.position.z), gameObject.transform.rotation);
        StartCoroutine("MoveDelay");
    }

    #endregion
    
    #region Coroutines

    /// <summary>
    /// How long till the enemy can move after attacking
    /// </summary>
    /// <returns></returns>
    IEnumerator MoveDelay()
    {
        yield return new WaitForSeconds(moveDelayAmount);
        allowMovement = true;
        //removes the knife
        if (knifeSpawnLimiter == false)
        {
            // Destroys knife player has in hand
            Destroy(weaponInstance);

            // Allows the process of regaining a knife
            knifeSpawnLimiter = true;
        }
    }

    /// <summary>
    /// How long a attack windup is
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackWindup()
    {
        //timer before attack
        yield return new WaitForSeconds(windUpSpeed);
        //calls attack
        attack();
    }

    public void MoveSlower(float speedFactor)
    {
        base.moveSpeed *= speedFactor;
    }

    public void ReturnToRegularSpeed()
    {
        base.moveSpeed /= Constants.SLOWDOWN_RELATIVE_TIMESCALE;
    }
    //public void testDebug()
    //{
    //    Debug.Log("BasicInfantry");
    //}

    #endregion
}
