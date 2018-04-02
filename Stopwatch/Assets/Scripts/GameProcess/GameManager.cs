using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton class that manages the general state of the game
/// Not a script, but it is self-updating once initialized
/// </summary>
class GameManager
{
    #region Fields

    static GameManager instance;

    //GameObject objPrefab;
    //score handling
    
    public int currentScore = 0;
    public int highScore = 0;
    public Vector3 playerInitialPos;
    #endregion

    #region Constructor

    /// <summary>
    /// Private constructor
    /// </summary>
    private GameManager()
    {
        // Creates the object that calls GM's Update method
        UnityEngine.Object.DontDestroyOnLoad(new GameObject("gmUpdater", typeof(Updater)));

        GroundLayers = Constants.GROUND_LAYERS;
        EnemyLayers = Constants.ENEMY_LAYERS;
         
       // HighScore = 0;
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the singleton instance of the game manager
    /// </summary>
    public static GameManager Instance
    {
        get { return instance ?? (instance = new GameManager()); }

        // This is shorthand for:
        // get
        // {
        // 		if (instance == null)
        //		{
        //			instance = new GameManager();
        //		}
        //		return instance;
        // }

        // Allows anything in the game to access the GM with GameManager.Instance.whatever
    }

    /// <summary>
    /// The accessor for the player
    /// </summary>
    public PlayerController Player
    { get; set; }

    /// <summary>
    /// The accessor for the player attack box collider
    /// </summary>
    public CheckForAttack Attack
    { get; set; }

    /// <summary>
    /// The ground layers
    /// </summary>
    public int[] GroundLayers
    {
        get; private set;
    }

    /// <summary>
    /// The enemy layers
    /// </summary>
    public int[] EnemyLayers
    {
        get; private set;
    }

    /// <summary>
    /// The TimerUI script
    /// </summary>
    public TimerUI Timer
    { get; set; }

    public void SetHighScore(int curScore) {
        highScore = curScore;
    }

    #endregion

    #region Public Methods

    #endregion

    #region Private Methods

    /// <summary>
    /// Updates the game manager (called from updater class)
    /// </summary>
    private void Update()
    {
        //event testing test code
        //if (Input.GetKeyDown(KeyCode.G))
        //{
        //    EventManager.Instance.RegisterToEvent(EventType.PlayerDeath, TriggerWhenPlayerDies);
        //}
        //if (Input.GetKeyDown(KeyCode.H))
        //{
        //    EventManager.Instance.RegisterToEvent(EventType.PlayerDeath, AlsoTriggerWhenPlayerDies);
        //}
        //if (Input.GetKeyDown(KeyCode.J))
        //{
        //    EventManager.Instance.UnregisterFromEvent(EventType.PlayerDeath, TriggerWhenPlayerDies);
        //}
        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    EventManager.Instance.FireEvent(EventType.PlayerDeath);
        //}

        //restart the scene hotkey
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        if (Player != null)
        {
            currentScore = (int)(Player.transform.position.x - playerInitialPos.x);
        }
        // Debug.Log(currentScore);
    }

    ////used in event testing
    //private void TriggerWhenPlayerDies()
    //{
    //    Debug.Log("Screen pauses and displays loser screen");
    //}

    ////used in event testing
    //private void AlsoTriggerWhenPlayerDies()
    //{
    //    Debug.Log("Enemies handle themselves appropriately and reset");
    //}

    #endregion

    #region Updater class

    #region Public methods

    /// <summary>
    /// Loads the next scene
    /// </summary>
    /// <param name="name">Select the scene name you want to go to. If you need to add a scene it is in Constants.cs under SCENE_STRINGS</param>
    public void LoadScene(SceneName name)
    {
        SceneManager.LoadScene(Constants.SCENE_STRINGS[name]);
    }

    /// <summary>
    /// Generates a new level
    /// </summary>
    public void GenerateNewLevel()
    {

    }

    /// <summary>
    /// Loads module into current scene
    /// </summary>
    /// <param name="filePathExtension">File name of the module to pull in</param>
    public void LoadModule(string filePathExtension)
    {

    }


    public bool GameOver() {
        if (currentScore >= highScore)
        {
            highScore = currentScore;
            currentScore = 0;
            return true;
        }
        return false;
    }

    #endregion

    /// <summary>
    /// Internal class that updates the game manager
    /// </summary>
    class Updater : MonoBehaviour
    {
        // Update is called once per frame
        void Update()
        {
            Instance.Update();
        }
    }

    #endregion

}