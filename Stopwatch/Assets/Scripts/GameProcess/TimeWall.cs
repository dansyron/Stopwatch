using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TimeWall : MonoBehaviour 
{
    #region Fields

    public GameObject timer;              //used to store a reference to the timer UI, so the current time can be used for the wall distance
    TimerUI timerUI;
    PlayerController player;             //used to store a reference to the player so it can be used to tell the wall where its going
    public float previousTimerValue = 0f; //used to store the time value from the previous frame
    private float timerValue = 0f;        //used to store the current time value to use in calculating the walls distance from the player
    private bool inCatchUp = false;       //used to chack if the wall is "catching up" to where it should be in the game
    private float timeWallWidth = 24;
    private int gameEndTimer = 0;

    #endregion

    #region Initialization

    // Use this for initialization
    void Start () 
	{
        //get a reference to the timer UI
		timer = GameObject.FindGameObjectWithTag ("TimerUI");
        timerUI = GameManager.Instance.Timer;

    }

    #endregion

    #region Public Methods

    // Update is called once per frame
    void Update () 
	{
        gameEndTimer++;

        //get the current timer value
        timerValue = timerUI.timerLength;

        //get an updated reference to the player
        player = GameManager.Instance.Player;

        //check if the current time value is greater than the previous frame, meaning time was added
        if(timerValue > previousTimerValue)
        {
            //set the catch up state to true
            inCatchUp = true;

            //update the timer to gradually move back, instead of jumping back
            timerValue = previousTimerValue + ((timerValue - previousTimerValue) / 20);

            //set the previous timer value to be the value that was just changed
            previousTimerValue = timerValue;
        }
        else
        {
            //stop the catch up state
            inCatchUp = false;
        }

        if (timerValue > 0)
        {
            //move the wall according to the player position - (width of the time wall) - timerValue(gets closer to the player as the time is decreased)
            transform.position = new Vector3(player.transform.position.x - (timeWallWidth) - (2 * timerValue), 10, 0);
        }
        else
        {
            if((gameEndTimer % 2) == 0)
            {
                if (timeWallWidth > 0)
                {
                    timeWallWidth -= .2f;
                }
            }

            //move the wall according to the player position - (width of the time wall) - timerValue(gets closer to the player as the time is decreased)
            transform.position = new Vector3(player.transform.position.x - (timeWallWidth) - timerValue, 10, 0);
        }
        //check the the catch up state isn't active
        if(!inCatchUp)
        {
            //update the previous timer value
            previousTimerValue = timerValue;
        }
    }

    #endregion
}
