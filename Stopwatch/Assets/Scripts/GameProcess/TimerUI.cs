using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

/// <summary>
/// Timer UI keeps track of player's time and graphically shows that
/// information with a timer and displays time left
/// </summary>
public class TimerUI : MonoBehaviour
{

    #region Fields
    
    // Amount of time left in timer in seconds
    public float timerLength = 0f;

    // Boolean for whether timer is stopped or not
    bool timerStopped;

    // Boolean for whether the timer has been started
    bool timerStarted;

    // The text through which the amount of time left is displayed
    public Text timerUI;
    

    // Starting time (usually will be 10)
    public const float StartingTime = Constants.TIMER_INITIAL_AMOUNT;

    // The amount in degrees by which the clock hand rotates every update
    //float clockHandMovement = 0f;

    // The scale by which time is either accelerated or slowed down
    private float timeScale = 1f;

    //Flash Speed and blink variables
    public float flashSpeed = 0.2f;
    public float flashLength = 1.2f;
    bool switchFlash = false;
    bool canStartNewFlash = true;
 
    //Stamina Support
    bool slowTime = false;
    public Image staminaBar;
    float stamina;
    float staminaUsageAmount;
    float staminaFillAmount;

    // Temporary aspects of child object that need to be altered
    public Image clockTimerBar;
    //public Image clockCircle;
    PlayerController playerCharacter;
    //GameObject player;
    
    //Constansts
    float staminaDodgeDrainAmount;
    float staminaAmount;
    float slowdownAmount;
    float timerInitialAmount;
    //end game fields
    public Canvas endingCanvas;

    //score handling
    public Text scoreText;
    public Text endingCanvasScoreText;
    public Text highScoreText;

    //celebration elements
    public GameObject celebrationParticleSystem;

    //Module Instantiation script reference
    public ModuleInstantiationScript moduleScript;
    #endregion

    #region Initialization

    // use the awake method to set reference to the game manager
    void Awake()
    {
        GameManager.Instance.Timer = this;
    }

    // Use this for initialization
	void Start () 
    {
        // Initializes amount of time left to be the same as the starting time amount
	    timerLength = StartingTime;
        //player = GameObject.FindWithTag("Player");
        //playerCharacter = player.GetComponent<PlayerController>();

        // Initializes the timer so that until the level is ready, the timer has not started
        timerStarted = false;
        
        //constant initialization
        stamina = Constants.STAMINA_AMOUNT;
        staminaFillAmount = Constants.STAMINA_FILL_AMOUNT;
        staminaUsageAmount = Constants.STAMINA_USAGE_AMOUNT;
        staminaDodgeDrainAmount = Constants.STAMINA_DODGE_DRAIN_AMOUNT;
        staminaAmount = Constants.STAMINA_AMOUNT;
        slowdownAmount = Constants.SLOWDOWN_RELATIVE_TIMESCALE;
        timerInitialAmount = Constants.TIMER_INITIAL_AMOUNT;
        //highScore = GameManager.Instance.HighScore;

        //Finds the the Module Instantiation script for reference
        moduleScript = FindObjectOfType<ModuleInstantiationScript>();
    }


    #endregion

    #region Update

    // Update is called once per frame
	void Update () 
    {
        #region Testing Inputs

        ////Test If Script to check color swap for flash
        //if(Input.GetKeyDown(KeyCode.Slash))
        //{
        //    //trigger the flash
        //    FlashTimer();
        //}

        //// Time subtraction testing
        //if (Input.GetKeyDown(KeyCode.O))
        //{
        //    // Subtracts arbitrary amount of time from timer
        //    ChangeTime(-2f);
        //}

        #endregion

        Stamina();

        GameOver();
        scoreText.text = GameManager.Instance.currentScore + " meters";

        ////checks if a script is null
        //if (moduleScript !=  null)
        //{
        //    scoreText.text += "\n" + "module: " + moduleScript.GetModuleListPosition().ToString();
        //}

        //changes the speed time decreases during slow down 
        if (slowTime)
        {
        // Prevents the timer from counting down if the timer has not been officially started
            //if (timerStarted == true)
            //{
                // Reduces time left on timer by change in relative time
                ChangeTime(((Time.deltaTime * -1f) * timeScale) / slowdownAmount);
            //}
        }
        //time decreses at normal speed
        else
        {
            // Prevents the timer from counting down if the timer has not been officially started
            //if (timerStarted == true)
            //{
                // Reduces time left on timer by change in relative time
                ChangeTime((Time.deltaTime * -1f) * timeScale);
            //}
        }
        // Displays amount of time left
        WriteToString(timerLength);
	}

    #endregion

    #region Private Methods

    /// <summary>
    /// Game Over Function
    /// </summary>
    void GameOver()
    {
        //checks if time is 0
        if (timerLength <= 0)
        {
            if (GameManager.Instance.GameOver())
            {
                endingCanvasScoreText.text = "You Ran " + GameManager.Instance.highScore + " Meters";
                highScoreText.text = "HIGH SCORE"; 
                celebrationParticleSystem.SetActive(true);
                AudioManager.instance.SlowDownMusic(true);

                AudioManager.instance.PlaySingle(SoundEffect.GameOver);
            }
            else {
                endingCanvasScoreText.text = "You Ran " + GameManager.Instance.currentScore + " Meters";
                highScoreText.text = "";
            }
            endingCanvas.transform.gameObject.SetActive(true);
            
            FindObjectOfType<PlayerController>().transform.gameObject.SetActive(false);
            FindObjectOfType<TimerUI>().transform.gameObject.SetActive(false);
            //FindObjectOfType<TimeWall>().transform.gameObject.SetActive(false);
            
            scoreText.gameObject.SetActive(false);
            
                
            // PREVIOUS WAY OF HANDLING GAME OVER
            //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    // Writes the current timer to string
    void WriteToString(float remainingTime)
    {
        if (remainingTime <= StartingTime)
        {
            timerUI.text = " " + remainingTime.ToString("f2");
        }
        else
        {
            timerUI.text = remainingTime.ToString("f2");
        }
    }

    /// <summary>
    /// Manages the stamina aspects of the timer
    /// </summary>
    /// <param name="stamina">Max amount of stamina</param>
    private void Stamina()
    {
        //Prevents the stamina bar from ever getting about the max stamina amount
        if(stamina >= staminaAmount)
        {
            stamina = staminaAmount;
        }
        
        //fills the stamina bar image by the amount of remaing stamina / constant cause it need to be between 0-1
        staminaBar.fillAmount = ((stamina / staminaAmount));
        //has the color of the stamina bar fade to green as it depletes
        staminaBar.color = Color.Lerp(Color.green, Color.white, (float)(stamina / staminaAmount));

        //if stamins is not max and slow timer is not on then increment it
        if (stamina < staminaAmount && slowTime == false)
        {
            //increments the stamina amount by 1
            stamina += staminaFillAmount * Time.deltaTime;
        }
        
        //sets time to be slowed if player clicks L
		if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.K))
        {
            if(!AudioManager.instance.efxSource.isPlaying)
            {
                AudioManager.instance.PlaySingle(SoundEffect.TimeSlowDown);
            }

            slowTime = true;
        }

        //Checks if the key is pushed to dodge "K"
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.K))
        {
            //checks if the player has over 20 stamina
            if (stamina >= staminaDodgeDrainAmount)
            {
                //subtracts 20 stamina if they do
                stamina -= 20;
            }
        }
        //if L is clicked again slow time is turned off
		else if ((!Input.GetKey(KeyCode.DownArrow) || !Input.GetKey(KeyCode.S) || !Input.GetKey(KeyCode.K) && slowTime))
        {

            //uncomment here when this section works properly
            //if (!AudioManager.instance.efxSource.isPlaying)
            //{
            //    AudioManager.instance.PlaySingle(SoundEffect.TimeSpeedUp);
            //}

            slowTime = false;
        }

        //checks if stamina is < than [staminaAmount(100) / 20 = 5] and sets it to 0 if the player is holding down k
		if ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.K)) && stamina <= (staminaAmount / 20))
        {
            slowTime = false;
            //playerCharacter.setSlowDown = false;
            stamina = 0;
        }

        //starts slowing time
        if (slowTime && stamina > 0)
        {
            #region Timer Slowdown
            //lowers stamina as long as slowTimer is on
            stamina -= staminaUsageAmount * Time.deltaTime;

            //Below are exit paths to leave the slow time function
            //stamina is 0
			if(stamina <= 0 && ((Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.K)) == false))
            {
                //makes stamina 0 so it doesn't become negative
                stamina = 0;
                //lets methods know slowTimer is not on
                slowTime = false;
                //playerCharacter.setSlowDown = false;
            }

            #endregion
        }
        //if slowtime is false or stamina is 0 or less then unslows time
        else
        {
            slowTime = false;
            //playerCharacter.setSlowDown = false;
        }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Flashes the timer to indicate the player hit
    /// </summary>

    public void FlashTimer()
    {
        // Checks if flash is already in progress
        if (canStartNewFlash == true)
        {
            // Prevents another flash from starting while this one is in progress
            canStartNewFlash = false;
            StartCoroutine("Blink");
            StartCoroutine("WaitForBlink");
            // NOTE: the line that sets canStartNewFlash back to true is at the end of WaitForBlink
        }
    }

    /// <summary>
    /// Method by which time can be added back to or subtracted from the clock
    /// </summary>
    /// <param name="timeChangeAmount">amount to change the players time by.</param>
    public void ChangeTime(float timeChangeAmount)
    {
        //fills clock timer bar by amount of time remaining
        clockTimerBar.fillAmount = timerLength / timerInitialAmount;

        // Add amount of time to timer, adjusted by the time scale
        timerLength += timeChangeAmount;
        
        // Caps the amount of time on timer to 0 if timer would otherwise be less than 0
        if (timerLength < 0)
        {
            timerLength = 0;
        }

        // Caps the amount of time on timer to the maximum if timer would otherwise be greater than it
        //COMMENT OUT TO REMOVE TIME CAP
        //if (timerLength > Constants.TIMER_INITIAL_AMOUNT)
        //{
        //    timerLength = Constants.TIMER_INITIAL_AMOUNT;
        //}

    }

    /// <summary>
    /// Public method that allows outside elements to start the timer when level is ready
    /// </summary>
    public void StartTimer()
    {
        timerStarted = true;
    }

    /// <summary>
    /// Getter method that allows objects to see whether the timer has started counting down
    /// </summary>
    /// <returns></returns>
    public bool HasTimerStarted()
    {
        return timerStarted;
    }

    #region Slow Down Methods

    // NOTE: THIS CODE WILL BE CHANGED UPON LATER IMPLEMENTATION OF DELEGATES

    /// <summary>
    /// Changes the timescale by which the TimerUI operates when slowdown is triggered
    /// </summary>
    public void InitiateSlowDown()
    {
        timeScale = slowdownAmount;
        flashSpeed /= timeScale;
        flashLength /= timeScale;
    }

    /// <summary>
    /// Reverts timescale back to normal upon end of slowdown effect
    /// </summary>
    public void EndSlowDown()
    {
        flashSpeed *= timeScale;
        flashLength *= timeScale;
        timeScale = 1f;
    }

    #endregion

    #endregion

    #region Coroutines

    /// <summary>
    /// When called stops the blink coroutine
    /// </summary>
    void StopBlink()
    {
        StopCoroutine("Blink");
    }

    /// <summary>
    /// after the set time this will stop the called object from blinking
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitForBlink()
    {
        //does stuff after a set time
        yield return new WaitForSeconds(flashLength);
        //stops the blink coroutine
        StopBlink();
        //sets the color to the non flash color
        timerUI.color = new Color(0, 0xFF, 0xFF, 0xFF);
        //clockCircle.color = new Color(0, 0xFF, 0xFF, 0xFF);
        // Allows new flashes to be initiated
        canStartNewFlash = true;
    }

    /// <summary>
    /// Causes the called object to blink by flashing red <- -> white and then after a set timer flash to the next color.
    /// After a set time it will stop flashing.
    /// </summary>
    /// <returns></returns>
    IEnumerator Blink()
    {
        while (true)
        {
            //checks if the color is blue or red
            switch (switchFlash)
            {
                //when the blink is true
                case true:
                    //Sets the image to cyan
                    timerUI.color = new Color(0,0xFF, 0xFF, 0xFF);
                    //clockCircle.color = new Color(0, 0xFF, 0xFF, 0xFF);
                    //tells blink to switch to false
                    switchFlash = false;
                    //waits set time before leaving this statement
                    yield return new WaitForSeconds(flashSpeed);
                    break;
                
                //when the blink is false
                case false:
                    //Sets the image to red
                    //clockHand.color = new Color(0xFF, 0, 0, 0xFF);
                    timerUI.color = new Color(0xFF, 0, 0, 0xFF);
                    //clockCircle.color = new Color(0xFF, 0, 0, 0xFF);
                    switchFlash = true;
                    //waits set time before leaving this statement
                    yield return new WaitForSeconds(flashSpeed);
                    break;
            }
        }
    }
    #endregion

    #region Properties

    /// <summary>
    /// Gets what the current slowdown is set to
    /// </summary>
    public bool getSlowDownTime
    {
        get { return slowTime; }
    }

    /// <summary>
    /// Adds stamina to the timer
    /// </summary>
    public float AddStaminaAmount
    {
        set { stamina += value; }
    }
    #endregion
}
