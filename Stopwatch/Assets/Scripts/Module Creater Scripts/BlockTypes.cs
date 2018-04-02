using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class BlockTypes : MonoBehaviour {

    #region Fields
    [HeaderAttribute("MouseOver Variable Name for info")]
    [Tooltip("TimeInPhase = pass during slowdown \nTimeOutOfPhase = pass during normal speed")]
    public BlockType blockType;
    [Tooltip("Brighter blue for when block is not passable")]
    public Material timeInPhaseMat;
    [Tooltip("Darker blue for when the block is passable")]
    public Material timeOutOfPhaseMat;
    [Tooltip("Orange")]
    public Material stunBlockMat;
    [Tooltip("Yellow")]
    public Material speedBlockMat;
    [Tooltip("Green")]
    public Material jumpBlockMat;
    [Tooltip("Magenta")]
    public Material overflowBlockMat;
    [Tooltip("Black")]
    public Material defaultColorMat;

    GameObject player;
    float phaseBlockSafteyDistanceX;
    float phaseBlockSafteyDistanceY;
    float timeReward;

    TimerUI timerUI;
    #endregion

    #region Start
    // Use this for initialization
    void Start () {
        phaseBlockSafteyDistanceX = Constants.PHASE_BLOCK_SAFETY_DISTANCE_X;
        phaseBlockSafteyDistanceY = Constants.PHASE_BLOCK_SAFETY_DISTANCE_Y;

        if (SceneManager.GetActiveScene().ToString() != "Module Designer Scene")
        {
            player = GameObject.Find("New Player Model");
        }
        timerUI = GameObject.Find("TimerUI").GetComponent<TimerUI>();

        //modifies how much time is awarded per block touch
        timeReward = Constants.TIME_REWARD;
	}
    #endregion

    #region Update
    // Update is called once per frame
    void Update () {
        #region Test Cases

        //Testing if the cube can be dynamically assigned
        //6
        //if(Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    //sets to normal
        //    blockType = BlockType.Normal;
        //}
        ////1
        //if(Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    //sets to normal
        //    blockType = BlockType.Overflow;
        //}
        ////2
        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    //sets to normal
        //    blockType = BlockType.Speed;
        //}
        ////3
        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    //sets to normal
        //    blockType = BlockType.Stun;
        //}
        ////4
        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    //sets to normal
        //    blockType = BlockType.TimeInPhase;
        //}
        ////5
        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    //sets to normal
        //    blockType = BlockType.TimeOutOfPhase;
        //}

        #endregion

        //Switch statement that loads the correct type of block type for the gameobject it is attached to
        switch (blockType)
        {
            //Stun Block
            case BlockType.Stun:
                StunBlock();
                break;
            //passable in slowdown, blocks in normal speed
            case BlockType.TimeInPhase:
                TimeBlockInPhase();
                break;
            //passable in normal speed, blocks in slowdown
            case BlockType.TimeOutOfPhase:
                TimeBlockOutOfPhase();
                break;
            //Overflow Block
            case BlockType.Overflow:
                OverflowBlock();
                break;
            //Speed Block
            case BlockType.Speed:
                SpeedBlock();
                break;
            //No characteristics
            case BlockType.Normal:
                NormalBlock();
                break;
            case BlockType.WallJump:
                WallJumpBlock();
                break;
            //Default does nothing
            default:
                break;
        }
	}
    #endregion

    #region methods

    #region StunBlock
    /// <summary>
    /// Gives this object the StunBlock Capabilities
    /// </summary>
    private void StunBlock()
    {
        gameObject.GetComponent<Renderer>().material = stunBlockMat;
    }
    #endregion

    #region SpeedBlock
    /// <summary>
    /// Gives this object the SpeedBlock Capabilities
    /// </summary>
    private void SpeedBlock()
    {
        gameObject.GetComponent<Renderer>().material = speedBlockMat;
    }
    #endregion

    #region TimeBlockInPhase
    /// <summary>
    /// Gives this object the TimeBlock Capabilities
    /// </summary>
    private void TimeBlockInPhase()
    {
        //If the player exists
        if (player != null)
        {
            //if the slowdown is active
            if (timerUI.getSlowDownTime)
            {
                //remove collider and change color
                gameObject.GetComponent<Collider>().enabled = false;
                gameObject.GetComponent<Renderer>().material = timeOutOfPhaseMat;
            }
            //does nothing if the player is inside the block or near it
            if (gameObject.transform.position.x + phaseBlockSafteyDistanceX < player.transform.position.x ||
               gameObject.transform.position.x > player.transform.position.x ||
               gameObject.transform.position.y < player.transform.position.y ||
               gameObject.transform.position.y - phaseBlockSafteyDistanceY > player.transform.position.y)
            {
                //if the slowdown is not active
                if (!timerUI.getSlowDownTime)
                {
                    //keep color and collider as the normal block
                    gameObject.GetComponent<Collider>().enabled = true;
                    gameObject.GetComponent<Renderer>().material = timeInPhaseMat;
                }
            }
        }
        else
        {
            //default color
            gameObject.GetComponent<Renderer>().material = timeInPhaseMat;
        }
    }

    #endregion

    #region TimeBlockOutOfPhase
    /// <summary>
    /// Gives this object the TimeBlock Capabilities
    /// </summary>
    private void TimeBlockOutOfPhase()
    {
        //If the player exists
        if (player != null)
        {
            //does nothing if the player is inside the block or near it
            if (gameObject.transform.position.x + phaseBlockSafteyDistanceX < player.transform.position.x ||
           gameObject.transform.position.x > player.transform.position.x ||
           gameObject.transform.position.y < player.transform.position.y ||
           gameObject.transform.position.y - phaseBlockSafteyDistanceY > player.transform.position.y)
            {
                //if the slowdown is not active
                if (timerUI.getSlowDownTime)
                {
                    //keep color and collider as the normal block
                    gameObject.GetComponent<Collider>().enabled = true;
                    gameObject.GetComponent<Renderer>().material = timeInPhaseMat;
                }
            }
            if (!timerUI.getSlowDownTime)
            {
                //remove collider and change color
                gameObject.GetComponent<Collider>().enabled = false;
                gameObject.GetComponent<Renderer>().material = timeOutOfPhaseMat;
            }
        }
        else
        {
            //Default color
            gameObject.GetComponent<Renderer>().material = timeOutOfPhaseMat;
        }
    }

    #endregion

    #region Overflow
    /// <summary>
    /// Gives this object the OverflowBlock Capabilities
    /// </summary>
    private void OverflowBlock()
    {
        gameObject.GetComponent<Renderer>().material = overflowBlockMat;
    }

    #endregion

    #region NormalBlock
    /// <summary>
    /// Gives this object no Capabilities
    /// </summary>
    private void NormalBlock()
    {
        //This Does nothing, it just exists to give generic blocks a attribute
        gameObject.GetComponent<Renderer>().material = defaultColorMat;
    }

    #endregion

    #region WallJumpBlock
    /// <summary>
    /// Gives this object no Capabilities
    /// </summary>
    private void WallJumpBlock()
    {
        //This Does nothing, it just exists to give generic blocks a attribute
        gameObject.GetComponent<Renderer>().material = jumpBlockMat;
    }

    #endregion

    #region OnCollisionEnter
    /// <summary>
    /// Checks if something collided with the block
    /// </summary>
    /// <param name="coll">colliding gameobjects collider</param>
    void OnCollisionEnter(Collision coll)
    {
        //Switch statement that only does stuff if the block type is as follows
        switch (blockType)
        {
            //Stun Block
            case BlockType.Stun:
                if (coll.gameObject.tag == "Player")
                {
                    if (coll.gameObject.GetComponent<PlayerController>().Action != PlayerController.ActionState.IsDodging)
                    {
                        coll.gameObject.GetComponent<PlayerController>().Action = PlayerController.ActionState.IsHurt;
                    }
                }
                break;
            //Time Block
            case BlockType.TimeInPhase:
                break;

            case BlockType.TimeOutOfPhase:
                break;
            //Overflow Block
            case BlockType.Overflow:
                break;
            //Speed Block
            case BlockType.Speed:
                if(coll.gameObject.tag == "Player")
                {
                    timerUI.timerLength += timeReward;
                }
                break;
            //No characteristics
            case BlockType.Normal:
                break;
            //Default does nothing
            default:
                break;
        }
    }

    #endregion

    #region OnCollisionStay

    /// <summary>
    /// Checks if a collider is staying in range
    /// </summary>
    /// <param name="coll">collider component of the touching object</param>
    public void OnCollisionStay(Collision coll)
    {
        switch (blockType)
        {
            //Stun Block
            case BlockType.Stun:
                break;
            //Time Block
            case BlockType.TimeInPhase:
                break;

            case BlockType.TimeOutOfPhase:
                break;
            //Overflow Block
            case BlockType.Overflow:
                //checks of the block touches the player
                if (coll.gameObject.tag == "Player")
                {
                    //adds overflow value to the stamina bar
                    timerUI.AddStaminaAmount = Constants.OVERFLOW_BLOCK_ADD_AMOUNT;
                }
                    break;
            //Speed Block
            case BlockType.Speed:
                break;
            //No characteristics
            case BlockType.Normal:
                break;
            //Default does nothing
            default:
                break;
        }
    }

    #endregion
    #region Mouse
    /// <summary>
    /// If the mouse is over the gameObject
    /// </summary>
    public void OnMouseOver()
    {

    }
    #endregion

    #endregion

    #region Properties

    /// <summary>
    /// Set the block type to the given enum (Stun, TimeInPhase, TimeOutOfPhase, Speed, Overflow, Normal)
    /// </summary>
    public BlockType SetBlockEnumeration
    {
        set { blockType = value; }
    }

    public BlockType GetBlockEnumeration
    {
        get { return blockType; }
    }

    #endregion
}

#region Enumerations

public enum BlockType { Stun, TimeInPhase, TimeOutOfPhase, Speed, Overflow, WallJump, Normal };

#endregion
